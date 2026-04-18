using System;
using System.Windows.Input;
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
            // Generér Study_ID og Order_number automatisk
            var studyIdCode = "ST-" + Guid.NewGuid().ToString("N")[..8].ToUpperInvariant();
            var orderNumber = "ORD-" + Guid.NewGuid().ToString("N")[..8].ToUpperInvariant();

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
                    MaterialNumber = string.Empty,
                    ExpectedOutcome = string.Empty
                    // Equipments udfylder vi senere i OrderDetailsScreen
                }
            };

            study.Orders.Add(order);

            _db.Studies.Add(study);
            await _db.SaveChangesAsync();

            // study.Id og order.Id er nu udfyldt
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