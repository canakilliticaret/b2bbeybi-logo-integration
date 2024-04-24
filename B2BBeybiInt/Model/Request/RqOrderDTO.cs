using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnityObjects.Model.Request
{
    public class RqOrderDTO
    {
        public string DateStart { get; set; }

        public string DateEnd { get; set; }

        public int OrderStatus { get; set; }

        public string OrderSource { get; set; }

        public int PaymentType { get; set; }
    }
}
