using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnityObjects.Model.Response
{
    public class OrderDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int ItemCount { get; set; }
        public int UserId { get; set; }
        public int AccountId { get; set; }
        public bool IsDefault { get; set; }

        public string OrderNumber { get; set; }

        public DateTime OrderDate { get; set; }

        public string OrderDateString
        {
            get { return OrderDate.ToString("dd.MM.yyyy"); }
        }

        public double GrossAmount { get; set; }

        public double TotalDiscount { get; set; }

        public double SubTotal
        {
            get { return GrossAmount - TotalDiscount; }
        }

        public double TotalVat { get; set; }
        public double Total
        {
            get { return SubTotal + TotalVat; }
        }

        public double TotalManual { get; set; }

        public string FullName { get; set; }

        public string Mobile { get; set; }

        public bool? IsPendingOrder { get; set; }

        public string Description { get; set; }
        public int PriceType { get; set; }

        public int FromB2B { get; set; }
    }
}
