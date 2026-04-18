using System;

namespace StudyAndOrder.Wpf.ViewModels
{
    public class OrdersRowVm
    {
        public string Study_ID { get; set; } = "";
        public string Order_number { get; set; } = "";
        public string Facility { get; set; } = "";
        public string Material_Number { get; set; } = "";
        public string Process_Order_Type { get; set; } = "";
        public DateTime From_Date { get; set; }
        public DateTime To_Date { get; set; }

        // Ingoing material (1 række pr. ingoing line)
        public string Ingoing_Material { get; set; } = "";
        public string Batch_Number { get; set; } = "";
        public string Amount { get; set; } = "";

        public string Expected_Outcome { get; set; } = "";
        public string Email { get; set; } = "";
        public string Equipment { get; set; } = "";

        public bool Data_Equipment_Validation { get; set; }
        public bool Sampling_Stock { get; set; }

        public string WBS { get; set; } = "";
        public string Cost_Center { get; set; } = "";
        public DateTime Creation_Date { get; set; }
    }
}