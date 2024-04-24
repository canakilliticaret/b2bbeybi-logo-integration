using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INT.BeybiB2B.Model.Response
{
    public class PaymentDTO
    {
        public int Id { get; set; }
        public string OrderNo { get; set; }
        public string AccountId { get; set; }
        public string CariKod { get; set; }
        public string CariName { get; set; }
        public int BankInfoId { get; set; }
        public string PayTypeName { get; set; }
        public int InsNum { get; set; }
        public int InsPlus { get; set; }
        public decimal InsPer { get; set; }
        public decimal Amount { get; set; }
        public decimal BasicAmount { get; set; }
        public string Status { get; set; }
        public DateTime PayDate { get; set; }
        public string IpAddress { get; set; }
        public string Plasiyer { get; set; }
        public string CCNo { get; set; }
        public string CCNm { get; set; }
        public string Note { get; set; }
        public string PayType { get; set; }
        public string PaymentResponse { get; set; }
        public string PaymentId { get; set; }
        public string ConversationId { get; set; }
    }
}
