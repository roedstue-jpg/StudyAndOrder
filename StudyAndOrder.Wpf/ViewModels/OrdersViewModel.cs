using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.EntityFrameworkCore;
using StudyAndOrder.Core.Data;

namespace StudyAndOrder.Wpf.ViewModels
{
    public class OrdersViewModel : BaseViewModel
    {
        private readonly AppDbContext _db;

        public ObservableCollection<OrdersRowVm> Rows { get; } = new();

        public event Action? StudyInformationRequested;

        public ICommand CreatedStudyCommand { get; }

        public OrdersViewModel(AppDbContext db)
        {
            _db = db;

            CreatedStudyCommand = new RelayCommand(_ =>
            {
                StudyInformationRequested?.Invoke();
            });

            _ = LoadAsync();
        }

        private async Task LoadAsync()
        {
            Rows.Clear();

            var studies = await _db.Studies
                .Include(s => s.Orders)
                    .ThenInclude(o => o.ProducedMaterial)
                        .ThenInclude(p => p.Equipments)
                .Include(s => s.Orders)
                    .ThenInclude(o => o.IngoingMaterials)
                .ToListAsync();

            foreach (var study in studies)
            {
                foreach (var order in study.Orders)
                {
                    var produced = order.ProducedMaterial;

                    var materialNumber = produced?.MaterialNumber ?? "";
                    var expectedOutcome = produced?.ExpectedOutcome ?? "";

                    var equipmentText = (produced?.Equipments == null || !produced.Equipments.Any())
                        ? ""
                        : string.Join(", ", produced!.Equipments.Select(e => e.EquipmentId));

                    var ing = order.IngoingMaterials?.ToList() ?? new();

                    if (ing.Count == 0)
                    {
                        Rows.Add(new OrdersRowVm
                        {
                            Study_ID = study.StudyId,
                            Order_number = order.OrderNumber,
                            Facility = study.Facility,
                            Material_Number = materialNumber,
                            Process_Order_Type = study.ProcessOrderType.ToString(),
                            From_Date = study.FromDate,
                            To_Date = study.ToDate,

                            Ingoing_Material = "",
                            Batch_Number = "",
                            Amount = "",

                            Expected_Outcome = expectedOutcome,
                            Email = study.Email,
                            Equipment = equipmentText,

                            Data_Equipment_Validation = study.DataEquipmentValidation,
                            Sampling_Stock = study.SamplingStock,

                            WBS = study.WBS,
                            Cost_Center = study.CostCenter,
                            Creation_Date = order.CreationDate
                        });
                    }
                    else
                    {
                        foreach (var line in ing)
                        {
                            Rows.Add(new OrdersRowVm
                            {
                                Study_ID = study.StudyId,
                                Order_number = order.OrderNumber,
                                Facility = study.Facility,
                                Material_Number = materialNumber,
                                Process_Order_Type = study.ProcessOrderType.ToString(),
                                From_Date = study.FromDate,
                                To_Date = study.ToDate,

                                Ingoing_Material = line.MaterialNumber,
                                Batch_Number = line.BatchNumber,
                                Amount = line.Amount,

                                Expected_Outcome = expectedOutcome,
                                Email = study.Email,
                                Equipment = equipmentText,

                                Data_Equipment_Validation = study.DataEquipmentValidation,
                                Sampling_Stock = study.SamplingStock,

                                WBS = study.WBS,
                                Cost_Center = study.CostCenter,
                                Creation_Date = order.CreationDate
                            });
                        }
                    }
                }
            }
        }
    }
}