using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.EntityFrameworkCore;
using StudyAndOrder.Core.Data;
using StudyAndOrder.Core.Models;

namespace StudyAndOrder.Wpf.ViewModels
{
    public class EquipmentSelectionVm : BaseViewModel
    {
        public Equipment Equipment { get; }

        private bool _isSelected;
        public bool IsSelected
        {
            get => _isSelected;
            set => SetProperty(ref _isSelected, value);
        }

        public EquipmentSelectionVm(Equipment equipment)
        {
            Equipment = equipment;
        }
    }

    public class IngoingMaterialLineVm : BaseViewModel
    {
        private string _ingoingMaterial = string.Empty;
        public string IngoingMaterial
        {
            get => _ingoingMaterial;
            set => SetProperty(ref _ingoingMaterial, value);
        }

        private string _batchNumber = string.Empty;
        public string BatchNumber
        {
            get => _batchNumber;
            set => SetProperty(ref _batchNumber, value);
        }

        private string _amount = string.Empty;
        public string Amount
        {
            get => _amount;
            set => SetProperty(ref _amount, value);
        }
    }

    public class OrderDetailsViewModel : BaseViewModel
    {
        private readonly AppDbContext _db;

        public event Action? BackToOrdersRequested;

        public OrderDetailsViewModel(AppDbContext db)
        {
            _db = db;

            AddIngoingMaterialCommand = new RelayCommand(_ => IngoingLines.Add(new IngoingMaterialLineVm()));

            RemoveIngoingMaterialCommand = new RelayCommand(line =>
            {
                if (line is IngoingMaterialLineVm vm)
                    IngoingLines.Remove(vm);
            });

            SubmitCommand = new RelayCommand(async _ => await SubmitAsync(), _ => true);

            CreatedNewOrderCommand = new RelayCommand(async _ => await CreateNewOrderAsync());
            CancelledOrderCommand = new RelayCommand(async _ => await CancelCurrentOrderAsync());

            // Next/Back commands
            BackOrderCommand = new RelayCommand(async _ => await GoToBackOrderAsync(), _ => CanGoBack);
            NextOrderCommand = new RelayCommand(async _ => await GoToNextOrderAsync(), _ => CanGoNext);
        }

        // Context
        private int _currentStudyDbId;
        public int CurrentStudyDbId
        {
            get => _currentStudyDbId;
            set => SetProperty(ref _currentStudyDbId, value);
        }

        private string _currentStudyCode = "";
        public string CurrentStudyCode
        {
            get => _currentStudyCode;
            set => SetProperty(ref _currentStudyCode, value);
        }

        private int _currentOrderDbId;
        public int CurrentOrderDbId
        {
            get => _currentOrderDbId;
            set => SetProperty(ref _currentOrderDbId, value);
        }

        private string _currentOrderNumber = "";
        public string CurrentOrderNumber
        {
            get => _currentOrderNumber;
            set => SetProperty(ref _currentOrderNumber, value);
        }

        public void SetContext(int studyDbId, string studyCode, int orderDbId, string orderNumber)
        {
            CurrentStudyDbId = studyDbId;
            CurrentStudyCode = studyCode;
            CurrentOrderDbId = orderDbId;
            CurrentOrderNumber = orderNumber;

            _ = LoadAsync();
        }

        // Commands
        public ICommand AddIngoingMaterialCommand { get; }
        public ICommand RemoveIngoingMaterialCommand { get; }

        public ICommand SubmitCommand { get; }
        public ICommand CreatedNewOrderCommand { get; }
        public ICommand CancelledOrderCommand { get; }

        // Next/Back state
        private bool _canGoBack;
        public bool CanGoBack
        {
            get => _canGoBack;
            set => SetProperty(ref _canGoBack, value);
        }

        private bool _canGoNext;
        public bool CanGoNext
        {
            get => _canGoNext;
            set => SetProperty(ref _canGoNext, value);
        }

        public ICommand BackOrderCommand { get; }
        public ICommand NextOrderCommand { get; }

        // Dropdowns / options
        public ObservableCollection<Material> MaterialsList { get; } = new();
        public ObservableCollection<EquipmentSelectionVm> EquipmentOptions { get; } = new();

        // Produceret materiale (1..1)
        private string _producedMaterialNumber = "";
        public string ProducedMaterialNumber
        {
            get => _producedMaterialNumber;
            set => SetProperty(ref _producedMaterialNumber, value);
        }

        private string _expectedOutcome = "";
        public string ExpectedOutcome
        {
            get => _expectedOutcome;
            set => SetProperty(ref _expectedOutcome, value);
        }

        // Ingoing lines (0..n)
        public ObservableCollection<IngoingMaterialLineVm> IngoingLines { get; } = new();

        private async Task LoadAsync()
        {
            ProducedMaterialNumber = "";
            ExpectedOutcome = "";
            IngoingLines.Clear();
            MaterialsList.Clear();
            EquipmentOptions.Clear();

            // lookup data
            var materials = await _db.Materials
                .OrderBy(m => m.MaterialNumber)
                .ToListAsync();

            foreach (var m in materials)
                MaterialsList.Add(m);

            var equipments = await _db.Equipments
                .OrderBy(e => e.EquipmentId)
                .ToListAsync();

            foreach (var e in equipments)
                EquipmentOptions.Add(new EquipmentSelectionVm(e));

            // load order
            var order = await _db.Orders
                .Include(o => o.ProducedMaterial)
                    .ThenInclude(pm => pm!.Equipments)
                .Include(o => o.IngoingMaterials)
                .FirstOrDefaultAsync(o => o.Id == CurrentOrderDbId && o.StudyId == CurrentStudyDbId);

            if (order == null)
            {
                CanGoBack = false;
                CanGoNext = false;
                return;
            }

            if (order.ProducedMaterial != null)
            {
                ProducedMaterialNumber = order.ProducedMaterial.MaterialNumber;
                ExpectedOutcome = order.ProducedMaterial.ExpectedOutcome;

                var equipmentIds = order.ProducedMaterial.Equipments?
                    .Select(x => x.EquipmentId)
                    .ToHashSet() ?? new System.Collections.Generic.HashSet<string>();

                foreach (var sel in EquipmentOptions)
                    sel.IsSelected = equipmentIds.Contains(sel.Equipment.EquipmentId);
            }

            if (order.IngoingMaterials != null)
            {
                foreach (var line in order.IngoingMaterials)
                {
                    IngoingLines.Add(new IngoingMaterialLineVm
                    {
                        IngoingMaterial = line.MaterialNumber,
                        BatchNumber = line.BatchNumber,
                        Amount = line.Amount
                    });
                }
            }

            // set Next/Back flags (OrderNumber = B)
            await UpdateNavigationFlagsAsync();
        }

        private async Task UpdateNavigationFlagsAsync()
        {
            var ordersInStudy = await _db.Orders
                .Where(o => o.StudyId == CurrentStudyDbId)
                .OrderBy(o => o.OrderNumber) // B: OrderNumber rækkefølge
                .Select(o => new { o.Id })
                .ToListAsync();

            var index = ordersInStudy.FindIndex(o => o.Id == CurrentOrderDbId);

            CanGoBack = index > 0;
            CanGoNext = index >= 0 && index < ordersInStudy.Count - 1;
        }

        private async Task GoToBackOrderAsync()
        {
            if (!CanGoBack) return;

            var ordersInStudy = await _db.Orders
                .Where(o => o.StudyId == CurrentStudyDbId)
                .OrderBy(o => o.OrderNumber) // B
                .ToListAsync();

            var index = ordersInStudy.FindIndex(o => o.Id == CurrentOrderDbId);
            if (index <= 0) return;

            var target = ordersInStudy[index - 1];
            SetContext(CurrentStudyDbId, CurrentStudyCode, target.Id, target.OrderNumber);
        }

        private async Task GoToNextOrderAsync()
        {
            if (!CanGoNext) return;

            var ordersInStudy = await _db.Orders
                .Where(o => o.StudyId == CurrentStudyDbId)
                .OrderBy(o => o.OrderNumber) // B
                .ToListAsync();

            var index = ordersInStudy.FindIndex(o => o.Id == CurrentOrderDbId);
            if (index < 0 || index >= ordersInStudy.Count - 1) return;

            var target = ordersInStudy[index + 1];
            SetContext(CurrentStudyDbId, CurrentStudyCode, target.Id, target.OrderNumber);
        }

        private async Task SubmitAsync()
        {
            var order = await _db.Orders
                .Include(o => o.ProducedMaterial)
                    .ThenInclude(pm => pm!.Equipments)
                .Include(o => o.IngoingMaterials)
                .FirstOrDefaultAsync(o => o.Id == CurrentOrderDbId && o.StudyId == CurrentStudyDbId);

            if (order == null)
                return;

            if (order.ProducedMaterial == null)
            {
                order.ProducedMaterial = new OrderProducedMaterialLine { OrderId = order.Id };
            }

            order.ProducedMaterial.MaterialNumber = ProducedMaterialNumber;
            order.ProducedMaterial.ExpectedOutcome = ExpectedOutcome;

            order.ProducedMaterial.Equipments.Clear();

            if (!string.IsNullOrWhiteSpace(SelectedEquipmentId))
            {
                var eq = EquipmentOptions.FirstOrDefault(x => x.Equipment.EquipmentId == SelectedEquipmentId)?.Equipment;
                if (eq != null)
                    order.ProducedMaterial.Equipments.Add(eq);
            }

            order.IngoingMaterials.Clear();
            foreach (var line in IngoingLines)
            {
                if (string.IsNullOrWhiteSpace(line.IngoingMaterial))
                    continue;

                order.IngoingMaterials.Add(new IngoingMaterial
                {
                    OrderId = order.Id,
                    MaterialNumber = line.IngoingMaterial,
                    BatchNumber = line.BatchNumber ?? "",
                    Amount = line.Amount ?? ""
                });
            }

            await _db.SaveChangesAsync();
            BackToOrdersRequested?.Invoke();
        }

        private async Task CreateNewOrderAsync()
        {
            var newOrderNumber = "ORD-" + Guid.NewGuid().ToString("N")[..8].ToUpperInvariant();

            var order = new Order
            {
                StudyId = CurrentStudyDbId,
                OrderNumber = newOrderNumber,
                CreationDate = DateTime.Now,
                ProducedMaterial = new OrderProducedMaterialLine()
            };

            _db.Orders.Add(order);
            await _db.SaveChangesAsync();

            SetContext(CurrentStudyDbId, CurrentStudyCode, order.Id, order.OrderNumber);
        }

        private string _selectedEquipmentId = "";
        public string SelectedEquipmentId
        {
            get => _selectedEquipmentId;
            set => SetProperty(ref _selectedEquipmentId, value);
        }

        private async Task CancelCurrentOrderAsync()
        {
            var ordersInStudy = await _db.Orders
                .Where(o => o.StudyId == CurrentStudyDbId)
                .OrderBy(o => o.CreationDate)
                .ToListAsync();

            var current = ordersInStudy.FirstOrDefault(o => o.Id == CurrentOrderDbId);
            if (current == null)
                return;

            var otherOrder = ordersInStudy.FirstOrDefault(o => o.Id != CurrentOrderDbId);

            _db.Orders.Remove(current);
            await _db.SaveChangesAsync();

            if (otherOrder != null)
            {
                // (B was chosen for Next/Back; Cancel uses "first other" behavior)
                SetContext(CurrentStudyDbId, CurrentStudyCode, otherOrder.Id, otherOrder.OrderNumber);
            }
            else
            {
                var study = await _db.Studies.FirstOrDefaultAsync(s => s.Id == CurrentStudyDbId);
                if (study != null)
                {
                    _db.Studies.Remove(study);
                    await _db.SaveChangesAsync();
                }

                BackToOrdersRequested?.Invoke();
            }
        }
    }
}