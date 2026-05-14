using System;
using System.Windows.Input;
using Microsoft.EntityFrameworkCore;
using StudyAndOrder.Core.Models;
using StudyAndOrder.Core.Enums;
using StudyAndOrder.Core.Data;
using System.Linq;

namespace StudyAndOrder.Wpf.ViewModels



{
    public class StudyInformationViewModel : BaseViewModel
    {
        private readonly AppDbContext _db;

        // MainViewModel lytter til dette og skifter skærm
        public event Action<int, string, int, string>? OrderDetailsRequested;

        // Inputs (bindings fra UI)
        private string _email = string.Empty;
        public string Email { get => _email; set => SetProperty(ref _email, value); }

        private string _wbs = string.Empty;
        public string WBS { get => _wbs; set => SetProperty(ref _wbs, value); }

        private string _costCenter = string.Empty;
        public string CostCenter { get => _costCenter; set => SetProperty(ref _costCenter, value); }

        private string _facility = "Alpha";
        public string Facility { get => _facility; set => SetProperty(ref _facility, value); }

        private DateTime _fromDate = DateTime.Today;
        public DateTime FromDate { get => _fromDate; set => SetProperty(ref _fromDate, value); }

        private DateTime _toDate = DateTime.Today;
        public DateTime ToDate { get => _toDate; set => SetProperty(ref _toDate, value); }

        private ProcessOrderType _processOrderType = ProcessOrderType.TechnicalQuality;
        public ProcessOrderType ProcessOrderType { get => _processOrderType; set => SetProperty(ref _processOrderType, value); }

        private bool _dataEquipmentValidation;
        public bool DataEquipmentValidation { get => _dataEquipmentValidation; set => SetProperty(ref _dataEquipmentValidation, value); }

        private bool _samplingStock;
        public bool SamplingStock { get => _samplingStock; set => SetProperty(ref _samplingStock, value); }

        // ComboBox values
        public Array ProcessOrderTypeValues => Enum.GetValues(typeof(ProcessOrderType));

        // Command knap i UI
        public ICommand CreatedOrderCommand { get; }

        public StudyInformationViewModel(AppDbContext db)
        {
            _db = db;

            CreatedOrderCommand = new RelayCommand(async _ => await CreateStudyAndFirstOrderAsync());
        }

        private async System.Threading.Tasks.Task CreateStudyAndFirstOrderAsync()
        {
            var studyIdCode = "ST-" + Guid.NewGuid().ToString("N")[..8].ToUpperInvariant();
            var orderNumber = "ORD-" + Guid.NewGuid().ToString("N")[..8].ToUpperInvariant();

            // 1) Hent en eksisterende Material (default) så FK'en kan opfyldes
            var defaultMaterial = await _db.Materials
                .OrderBy(m => m.MaterialNumber)
                .FirstOrDefaultAsync();

            if (defaultMaterial == null)
                throw new InvalidOperationException("Ingen Materials findes i SOMS_Db. Seed mangler.");

            var study = new Study
            {
                StudyId = studyIdCode,
                Email = Email,
                WBS = WBS,
                CostCenter = CostCenter,
                Facility = Facility,
                FromDate = FromDate,
                ToDate = ToDate,
                ProcessOrderType = ProcessOrderType,
                DataEquipmentValidation = DataEquipmentValidation,
                SamplingStock = SamplingStock,
            };

            var order = new Order
            {
                OrderNumber = orderNumber,
                CreationDate = DateTime.Now,
                Study = study,
                ProducedMaterial = new OrderProducedMaterialLine
                {
                    MaterialId = defaultMaterial.Id,     // ✅ vigtigt pga. FK + NOT NULL
                    ExpectedOutcome = string.Empty
                }
            };

            study.Orders.Add(order);
            _db.Studies.Add(study);

            await _db.SaveChangesAsync();

            OrderDetailsRequested?.Invoke(study.Id, study.StudyId, order.Id, order.OrderNumber);
        }
        public void Reset()
        {
            Email = string.Empty;
            WBS = string.Empty;
            CostCenter = string.Empty;

            Facility = "Alpha";          // samme default 
            FromDate = DateTime.Today;
            ToDate = DateTime.Today;

            ProcessOrderType = ProcessOrderType.TechnicalQuality;
            DataEquipmentValidation = false;
            SamplingStock = false;
        }

    }

}