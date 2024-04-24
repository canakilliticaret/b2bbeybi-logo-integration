using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INT.BeybiB2B.Model
{
    public class CurrencyRate
    {
        public int Id { get; set; }
        public int CurrencyType { get; set; }
        public double CurrencyRate1 { get; set; }
        public System.DateTime RateDate { get; set; }
        public double CurrencyRate2 { get; set; }
        //public double CurrencyRateFinal => (CurrencyRate2.HasValue ? CurrencyRate2.Value : CurrencyRate1);
    }
}
