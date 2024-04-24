using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnityObjects.Model.Response
{
    public class OrderItemDTO
    {
        public int Id { get; set; }
        public int ProductId { get; set; }

        public int BasketId { get; set; }
        public DateTime CreateDate { get; set; }
        public string CreateDateString
        {
            get { return CreateDate.ToString("dd/MM/yyyy"); }
        }
        public double VatRatio { get; set; }
        public string VatRatioStr { get; set; }
        public bool VatIncluded { get; set; }
        public double Discount { get; set; }
        public double PaymentDiscount { get; set; }
        public double UserDiscount { get; set; }
        public double DiscountRatio { get; set; }
        public double PaymentDiscountRatio { get; set; }
        public double UserDiscountRatio { get; set; }
        public double CargoFee { get; set; }

        public string StockCode { get; set; }
        public string Name { get; set; }
        public string ImageUrl { get; set; }
        public string ThumbUrl { get; set; }
        public double Amount { get; set; }
        public double Amount1 { get; set; }
        public string UnitName { get; set; }
        public double UnitPrice { get; set; }
        public string UnitPriceStr
        {
            get
            {
                return UnitPrice.ToString("N2");
            }
        }
        public double UnitPrice2 { get; set; }
        public string UnitPrice2Str
        {
            get
            {
                return UnitPrice2.ToString("N2");
            }
        }
        public int CurrencyType { get; set; }
        public double Vat { get; set; }
        public double GrandTotal { get; set; }
        public string GrandTotalStr
        {
            get
            {
                return GrandTotal.ToString("N4");
            }
        }
        public double GrandTotal2 { get; set; }
        public string GrandTotal2Str
        {
            get
            {
                return GrandTotal2.ToString("N4");
            }
        }
        public int UnitType { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }
        public int PriceType { get; set; }
        public string PriceTypeStr { get; set; }
        public string Description { get; set; }
        public double? UnitFactor { get; set; }
        public string DefaultUnit { get; set; }
        public string Unit { get; set; }
        public string ProductGroup { get; set; }
    }
}
