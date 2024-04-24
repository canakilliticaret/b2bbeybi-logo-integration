using INT.BeybiB2B.DataAccess;
using INT.BeybiB2B.Helpers;
using INT.BeybiB2B.Model;
using INT.BeybiB2B.Model.Response;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Runtime.InteropServices;
using System.ServiceProcess;
using System.Timers;
using UnityObjects;
using UnityObjects.Model.Response;

namespace B2BBeybiInt
{
    public partial class Service1 : ServiceBase
    {
        public Service1()
        {
            InitializeComponent();
        }

        //private static UnityApplication instance;
        //Timer timer = new Timer();
        protected override void OnStart(string[] args)
        {
            //System.Diagnostics.Debugger.Launch();
            Logger.LogInfo("Just Start");
            //Start();
            timer1.Interval = Convert.ToInt32(ServisCalismaPeriyodu) * 1000;//60000 1 dakika//1000 1 saniye
            timer1.Start();
        }

        protected override void OnStop()
        {
            //instance.Disconnect();
            if (Logo != null)
            {
                Logo.Disconnect();
                Logo = null;
                GC.Collect();
            }

            Logger.LogInfo("Logo Entegrasyon Servisi Durduruldu.");
        }

        //private void Start()
        //{
        //    timer.Elapsed += new ElapsedEventHandler(IntegrationStart);
        //    timer.Interval = 15000;
        //    timer.Enabled = true;
        //}

        public string logoPrefix = "", LogoUsername = "", LogoUserPassword = "", FirmIdStr = "";
        CurrencyRate rate = new CurrencyRate();
        //bool EntStat = false;
        //DateTime createDate = new DateTime();
        //private void IntegrationStart(object source, ElapsedEventArgs e)
        //{
        //    try
        //    {
        //        if (!EntStat)
        //        {
        //            instance = new UnityObjects.UnityApplication();
        //            EntStat = true;
        //            logoPrefix = "" + DBClassLocal.Default.ExecuteScalarWithParams("select Value from ConfigurationParameter where [Key] = 'LogoTablePrefix'");

        //            LogoUsername = ConfigurationManager.AppSettings["LogoUsername"];
        //            LogoUserPassword = ConfigurationManager.AppSettings["LogoUserPassword"];
        //            FirmIdStr = ConfigurationManager.AppSettings["FirmId"];
        //            //if (instance == null || createDate.AddHours(1) < DateTime.Now)
        //            //{
        //            //instance = (UnityApplication)Activator.CreateInstance(Marshal.GetTypeFromCLSID(new Guid("72DB412A-6BF5-4920-A002-2AAC679951DF")));
        //            //createDate = DateTime.Now;
        //            //}

        //            int firmId = 0;
        //            Int32.TryParse(FirmIdStr, out firmId);

        //            if (!instance.LoggedIn)
        //            {
        //                instance.Login(LogoUsername, LogoUserPassword, firmId, 0);
        //            }

        //            rate = GetRate();
        //            OrderIntegration();
        //            DBClassLocal.Default.ExecuteWithParams("update Basket set IntegrationStatus = 0 where IntegrationStatus = 2");
        //            PaymentIntegration();
        //            EntStat = false;
        //            instance.Disconnect();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        MailHelper.SendMail("entegrasyon@akilliticaretim.com", "destek@akilliticaret.com", "melih.utku.diker@orionyazilim.com", "", "Entegrasyon Çalıştırılamadı", "Beybi Lobject, Detay : " + ex.Message, true);

        //        Logger.LogInfo("--  Message -->" + (object)ex.Message + " -- StackTrace  --> " + ex.StackTrace + " --  Source --> IntegrationStart class:" + ex.Source + " -- InnerException -->" + (string)(object)ex.InnerException);
        //    }
        //}

        public static CurrencyRate GetRate()
        {
            var now = DateTime.Now;
            var date = new DateTime(now.Year, now.Month, now.Day);
            try
            {
                var rate = new CurrencyRate();
                //string query = GetRateQuery(currencytype);
                //if (currencytype == 2)
                //{
                var query = "SELECT TOP(1) RATES1 AS Rate FROM [dbo].[L_DAILYEXCHANGES] WHERE CRTYPE = {0} AND EDATE BETWEEN '{1}' AND '{2}' ORDER BY LREF DESC";
                var table = DBClass.Default.Select(string.Format(query, "1", DateTime.Now.AddDays(-2).ToString("yyyy-MM-dd HH:mm:ss"), DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));

                double curRate1 = 0;
                double.TryParse(table.Rows[0]["Rate"].ToString(), out curRate1);

                rate.CurrencyRate1 = table == null ? 1 : curRate1;
                //Rate = 
                //}
                //else if (currencytype == 1)
                //{
                query = "SELECT TOP(1) RATES1 AS Rate FROM [dbo].[L_DAILYEXCHANGES] WHERE CRTYPE = {0} AND EDATE BETWEEN '{1}' AND '{2}' ORDER BY LREF DESC";
                table = DBClass.Default.Select(string.Format(query, "20", DateTime.Now.AddDays(-2).ToString("yyyy-MM-dd HH:mm:ss"), DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));

                double curRate2 = 0;
                double.TryParse(table.Rows[0]["Rate"].ToString(), out curRate2);

                rate.CurrencyRate2 = table == null ? 1 : curRate2;

                return rate;
            }
            catch (Exception ex)
            {
                //LogHelper.Error(ex, "LogoStrategy", "GetRate");
            }
            return new CurrencyRate
            {
                CurrencyType = 1,
                CurrencyRate1 = 1,
                CurrencyRate2 = 1,
                RateDate = date,
            };
        }

        private void OrderIntegration()
        {
            List<OrderDTO> ordersa = "select *  from Basket where BasketStatus = 3 and IntegrationStatus = 0".BindListLocal<OrderDTO>();

            try
            {
                foreach (var order in ordersa)
                {
                    if (order != null)
                    {
                        DBClassLocal.Default.ExecuteWithParams("update Basket set IntegrationStatus = 2 where Id = @prm0", order.Id);
                        Logger.LogInfo("Order - " + order.OrderNumber);

                        //string tempOrderNumber = "";
                        //int count = 0;
                        //do
                        //{
                        //    Random rnd = new Random();
                        //    string str = rnd.Next(1, 8).ToString();
                        //    for (int i = 1; i < 8; i++)
                        //    {
                        //        int sayi = rnd.Next(0, 8);
                        //        str += sayi.ToString();
                        //    }
                        //    tempOrderNumber = str;

                        //    count = Convert.ToInt32(DBClassLocal.Default.ExecuteScalarWithParams("select Count(*) from Basket where OrderNumber = @prm0", tempOrderNumber).ToString());
                        //} while (count > 0);

                        //order.OrderNumber = tempOrderNumber;
                        UserDTO usersa = "select * from Account where Id = @prm0".BindObjectLocal<UserDTO>(order.AccountId);

                        string GroupCode1 = "" + DBClassLocal.Default.ExecuteScalarWithParams("select GroupCode1 from [User] where Id = @prm0", order.UserId);

                        int source = 0;

                        List<OrderItemDTO> orderitems = "select * from BasketItem where BasketId = @prm0".BindListLocal<OrderItemDTO>(order.Id);

                        foreach (var orderitem in orderitems)
                        {
                            string kod1 = "" + DBClassLocal.Default.ExecuteScalarWithParams("SELECT KOD_1 from Product WHERE Id =@prm0 ", orderitem.ProductId);

                            if (kod1.Contains("URT"))
                            {
                                source = 2;
                            }
                        }


                        string projectCodeRef = DBClass.Default.ExecuteScalarWithParams("select * from LG_219_PROJECT where  CODE  like 'CASH RECEİPT FRO'") + "";
                        string IsEfatura = DBClass.Default.ExecuteScalarWithParams("select top 1 ACCEPTEINV from LG_219_CLCARD where CODE like @prm0", usersa.Username) + "";

                        #region SiparişDetay
                        char c = '0';
                        string strOrderId = Convert.ToInt32(order.Id).ToString().PadLeft(8, c);
                        string sipNoID = "";
                        try
                        {
                            DateTime dateTime = order.OrderDate;

                            string dt = dateTime.ToString("dd.MM.yyyy");
                            string dtWS = dateTime.ToString("dd.MM.yyyy HH:mm:ss");
                            string dts = dateTime.ToString("HH");
                            string dtd = dateTime.ToString("mm");
                            string dtms = dateTime.ToString("ss");

                            double netTutar = Convert.ToDouble(order.SubTotal);

                            Data data = Logo.NewDataObject(DataObjectType.doSalesOrderSlip);
                            data.New();

                           

                            string specode = "" + DBClass.Default.ExecuteScalarWithParams("select SPECODE from " + logoPrefix + "_CLCARD WHERE CODE = @prm0", usersa.Username);

                            int IsDbs = Convert.ToInt32(DBClassLocal.Default.ExecuteScalarWithParams("select count(*) from PayType where Id = @prm0 and PaymentCode like 'DBS%'", order.PriceType));
                            string PaymentName = DBClassLocal.Default.ExecuteScalarWithParams("select top 1 [Name] from PayType where Id = @prm0", order.PriceType) + "";
                            string PaymentLOGICALREF = DBClass.Default.ExecuteScalarWithParams("select LOGICALREF from LG_219_PAYPLANS where CODE = @prm0", PaymentName) + "";

                            string parentUserCode = DBClassLocal.Default.ExecuteScalarWithParams("select Username from Account where Id = ( select  ParentUserId from Account where Username = @prm0)", usersa.Username) + "";

                            data.DataFields.FieldByName("NUMBER").Value = "~";
                            data.DataFields.FieldByName("DATE").Value = dt;
                            data.DataFields.FieldByName("TIME").Value = GenerateTime(Convert.ToDateTime(dtWS));
                            data.DataFields.FieldByName("DOC_NUMBER").Value = specode;
                            data.DataFields.FieldByName("AUXIL_CODE").Value = IsDbs > 0 ? "DBS" : "";
                            data.DataFields.FieldByName("ARP_CODE").Value = usersa.Username;
                            data.DataFields.FieldByName("GL_CODE").Value = usersa.Username;
                            data.DataFields.FieldByName("TRADING_GRP").Value = IsDbs > 0 ? "S600.03" : "S600.01";
                            data.DataFields.FieldByName("SOURCE_WH").Value = source;
                            data.DataFields.FieldByName("SOURCE_COST_GRP").Value = source;
                            data.DataFields.FieldByName("ORDER_STATUS").Value = "1";
                            data.DataFields.FieldByName("PROJECTREF").Value = projectCodeRef;
                            data.DataFields.FieldByName("AFFECT_RISK").Value = IsDbs > 0 ? "0" : "1";
                            data.DataFields.FieldByName("UPDCURR").Value = "0";
                            data.DataFields.FieldByName("UPD_CURR").Value = "1";
                            data.DataFields.FieldByName("TOTAL_VAT").Value = order.TotalVat;
                            data.DataFields.FieldByName("TOTAL_GROSS").Value = Convert.ToDouble(order.Total);
                            data.DataFields.FieldByName("TOTAL_NET").Value = netTutar;
                            data.DataFields.FieldByName("RC_RATE").Value = rate.CurrencyRate1;
                            data.DataFields.FieldByName("PAYMENT_CODE").Value = PaymentName;
                            data.DataFields.FieldByName("PAYDEFREF").Value = PaymentLOGICALREF;
                            data.DataFields.FieldByName("SALESMAN_CODE").Value = parentUserCode;

                            data.DataFields.FieldByName("CREATED_BY").Value = 1;
                            data.DataFields.FieldByName("NOTES1").Value = order.Description + "";
                            data.DataFields.FieldByName("NOTES2").Value = order.OrderNumber + "";
                            data.DataFields.FieldByName("DATE_CREATED").Value = dt;
                            data.DataFields.FieldByName("HOUR_CREATED").Value = dts;
                            data.DataFields.FieldByName("MIN_CREATED").Value = dtd;
                            data.DataFields.FieldByName("SEC_CREATED").Value = dtms;
                            data.DataFields.FieldByName("CURRSEL_TOTAL").Value = 1;
                            //data.DataFields.FieldByName("DATA_REFERENCE").Value = 1;
                            //data.DataFields.FieldByName("EINVOICE").Value = IsEfatura == "1" ? 1 : 2;
                            data.DataFields.FieldByName("CURRSEL_DETAILS").Value = 4;
                            data.DataFields.FieldByName("FC_STATUS_NAME").Value = "ONAYLANDI";

                            Lines lines = data.DataFields.FieldByName("TRANSACTIONS").Lines;


                            if (IsEfatura == "1")
                            {
                                data.DataFields.FieldByName("EINVOICE").Value = 1;
                                data.DataFields.FieldByName("EINVOICE_PROFILEID").Value = 2;
                                data.DataFields.FieldByName("EINSTEAD_OF_DISPATCH").Value = 1;
                            }
                            else
                            {
                                data.DataFields.FieldByName("EINVOICE").Value = 2;
                                data.DataFields.FieldByName("EARCHIVEDETR_SENDMOD").Value = 2;
                                data.DataFields.FieldByName("EARCHIVEDETR_INTPAYMENTTYPE").Value = 4;
                                data.DataFields.FieldByName("EARCHIVEDETR_INSTEADOFDESP").Value = 0;
                                data.DataFields.FieldByName("EARCHIVEDETR_INTSALESADDR").Value = "www.beybi.com.tr";
                                data.DataFields.FieldByName("EARCHIVEDETR_INTPAYMENTDATE").Value = Convert.ToDateTime(dateTime).ToShortDateString();
                            }

                            List<OrderItemDTO> ordersItemsa = "select * from BasketItem where BasketId = @prm0".BindListLocal<OrderItemDTO>(order.Id);

                            foreach (var orderItem in ordersItemsa)
                            {
                                string CurrencyTypeStr = DBClassLocal.Default.ExecuteScalarWithParams("select CurrencyType from Product  where Id = @prm0", orderItem.ProductId) + "";

                                double cur = 1;
                                if (CurrencyTypeStr == "2")
                                {
                                    cur = rate.CurrencyRate1;
                                }


                                if (lines.AppendLine())
                                {
                                    orderItem.StockCode = "" + DBClassLocal.Default.ExecuteScalarWithParams("select StockCode from  Product where  Id = @prm0", orderItem.ProductId);
                                    double Listprice2 = 0;
                                    string Listprice2Str = "" + DBClassLocal.Default.ExecuteScalarWithParams("select ListPrice2 from  Product where  Id = @prm0", orderItem.ProductId);
                                    double.TryParse(Listprice2Str, out Listprice2);

                                    string IsCurr = "" + DBClassLocal.Default.ExecuteScalarWithParams("select CurrencyType from  Product where  Id = @prm0", orderItem.ProductId);
                                    orderItem.VatRatioStr = "" + DBClassLocal.Default.ExecuteScalarWithParams("select Tax1 from  Product where  Id = @prm0", orderItem.ProductId);
                                    string UnitName = "" + DBClassLocal.Default.ExecuteScalarWithParams(@"select top 1 PU.Name from ProductUnitAmounts PUA
                                            join ProductUnit PU on PUA.ProductUnitId = PU.Id
                                            where ProductId = @prm0 and Status = 1 and IsDefault = 1", orderItem.ProductId);

                                    string StockCode = "" + DBClass.Default.ExecuteScalarWithParams("select top 1 CODE from LG_219_ITEMS  where PRODUCERCODE = @prm0", orderItem.StockCode);

                                    double VatRatio = 0;
                                    double.TryParse(orderItem.VatRatioStr, out VatRatio);

                                    lines[lines.Count - 1].FieldByName("TYPE").Value = 0;
                                    lines[lines.Count - 1].FieldByName("MASTER_CODE").Value = StockCode;
                                    lines[lines.Count - 1].FieldByName("QUANTITY").Value = (object)orderItem.Amount1;
                                    lines[lines.Count - 1].FieldByName("ORG_QUANTITY").Value = (object)orderItem.Amount1;

                                    double netTutarDetayStr = Convert.ToDouble((orderItem.UnitPrice * cur * orderItem.Amount1));

                                    if (IsCurr != "0" && IsCurr != "160")
                                    {
                                        lines[lines.Count - 1].FieldByName("CURR_PRICE").Value = 1;
                                        lines[lines.Count - 1].FieldByName("PC_PRICE").Value = orderItem.UnitPrice;
                                        lines[lines.Count - 1].FieldByName("EDT_CURR").Value = 1;
                                        lines[lines.Count - 1].FieldByName("EDT_PRICE").Value = orderItem.UnitPrice;

                                        if (Listprice2 > 0)
                                        {
                                            cur = Listprice2;
                                        }
                                        lines[lines.Count - 1].FieldByName("PR_RATE").Value = cur;
                                    }
                                    netTutarDetayStr = Convert.ToDouble((orderItem.UnitPrice * cur * orderItem.Amount1));
                                    lines[lines.Count - 1].FieldByName("RC_XRATE").Value = cur;

                                    lines[lines.Count - 1].FieldByName("PRICE").Value = orderItem.UnitPrice * cur;
                                    lines[lines.Count - 1].FieldByName("ORG_PRICE").Value = orderItem.UnitPrice * cur;
                                    lines[lines.Count - 1].FieldByName("UNIT_CODE").Value = UnitName;
                                    lines[lines.Count - 1].FieldByName("VAT_RATE").Value = VatRatio;
                                    lines[lines.Count - 1].FieldByName("VAT_AMOUNT").Value = ((orderItem.UnitPrice * cur) * ((100 + VatRatio) / 100)) - (orderItem.UnitPrice * cur);
                                    lines[lines.Count - 1].FieldByName("TRANS_DESCRIPTION").Value = "";


                                    string GL_CODE2 = "391.01.01.0080";
                                    if (orderItem.VatRatio == 18)
                                    {
                                        GL_CODE2 = "391.01.01.0180";
                                    }
                                    lines[lines.Count - 1].FieldByName("GL_CODE2").Value = GL_CODE2;

                                    lines[lines.Count - 1].FieldByName("DUE_DATE").Value = order.OrderDateString;
                                    lines[lines.Count - 1].FieldByName("ORG_DUE_DATE").Value = order.OrderDateString;

                                    lines[lines.Count - 1].FieldByName("SALESMAN_CODE").Value = parentUserCode;

                                    lines[lines.Count - 1].FieldByName("AFFECT_RISK").Value = IsDbs > 0 ? "0" : "1";

                                    lines[lines.Count - 1].FieldByName("SOURCE_WH").Value = source;
                                    lines[lines.Count - 1].FieldByName("SOURCE_COST_GRP").Value = source;

                                    lines[lines.Count - 1].FieldByName("TOTAL").Value = netTutarDetayStr;
                                    lines[lines.Count - 1].FieldByName("TOTAL_NET").Value = netTutarDetayStr;
                                    lines[lines.Count - 1].FieldByName("VAT_BASE").Value = netTutarDetayStr;
                                    //lines[lines.Count - 1].FieldByName("DATA_REFERENCE").Value = 1;
                                    lines[lines.Count - 1].FieldByName("MULTI_ADD_TAX").Value = 0;
                                }
                            }
                            data.ApplyCampaign();
                            data.FillAccCodes();

                            for (int i = 0; i < lines.Count; i++)
                            {
                                lines[i].FieldByName("OHP_CODE1").Value = GroupCode1 == "S" ? "6.00.01.2008" : "6.00.01.1008";
                                lines[i].FieldByName("OHP_CODE2").Value = GroupCode1 == "S" ? "6.00.01.2008" : "6.00.01.1008";
                            }

                            if (data.HasDataExtensions && IsDbs > 0)
                            {
                                var newOrderSlipExt = data.DataExtensions;
                                newOrderSlipExt[0].Fields.FieldByName("DBS_KONTROLU").Value = "1";
                            }

                            ValidateErrors validateErrors = data.ValidateErrors;
                            bool flag = data.Post();

                            sipNoID = data.DataFields.FieldByName("NUMBER").Value.ToString();

                            if (!flag)
                            {
                                string message = "";
                                for (int index = 0; index < validateErrors.Count; ++index)
                                {
                                    message += string.Format("{0} - {1}; \n", (object)validateErrors[index].ID, (object)validateErrors[index].Error);
                                    Exception ex = new Exception(message);
                                }

                                Logger.LogInfo("Order Error - " + message);
                                MailHelper.SendMail("entegrasyon@akilliticaretim.com", "destek@akilliticaret.com", "kerem.celik@orionyazilim.com; yunus.ayten@beybi.com.tr; sertac.akkaya@elorholding.com", "", "Fatura Oluşturulmadı", "Beybi Lobject, Detay : " + message + " \n Sipariş No : " + order.Name, true);
                                DBClassLocal.Default.ExecuteWithParams("update Basket set IntegrationStatus = 0 where Id = @prm0", order.Id);
                            }
                            else
                            {
                                Logger.LogInfo("Order Success - " + sipNoID);
                                DBClassLocal.Default.ExecuteWithParams("update Basket set IntegrationNumber = @prm0, [BasketStatus] = 5, IntegrationStatus = 1, IntegrationDate = GETDATE() where Id = @prm1", sipNoID, order.Id);
                            }

                        }
                        catch (Exception ex)
                        {
                            MailHelper.SendMail("entegrasyon@akilliticaretim.com", "destek@akilliticaret.com", "kerem.celik@orionyazilim.com; yunus.ayten@beybi.com.tr; sertac.akkaya@elorholding.com", "", "Fatura Oluşturulmadı", "Beybi Lobject, Detay : " + ex.Message + " \n Sipariş No : " + order.Name, true);
                            Logger.LogInfo("--  Message -->" + (object)ex.Message + " -- StackTrace  --> " + ex.StackTrace + " --  Source --> ProductIntegration class:" + ex.Source + " -- InnerException -->" + (string)(object)ex.InnerException);
                        }
                    }
                    #endregion
                }
            }
            catch (Exception ex)
            {
                MailHelper.SendMail("entegrasyon@akilliticaretim.com", "destek@akilliticaret.com", "kerem.celik@orionyazilim.com; yunus.ayten@beybi.com.tr; sertac.akkaya@elorholding.com", "", "Entegrasyon Çalıştırılamadı", "Beybi Lobject, Detay : " + ex.Message, true);

                Logger.LogInfo("--  Message -->" + (object)ex.Message + " -- StackTrace  --> " + ex.StackTrace + " --  Source --> ProductIntegration class:" + ex.Source + " -- InnerException -->" + (string)(object)ex.InnerException);
            }
        }
        private string FindPaymentCodeByInstallmentNumber(int InstallmentNum)
        {
            switch (InstallmentNum)
            {
                case 1:
                    return "K.KARTI";
                case 2:
                    return "K.KARTI 2 TAKSIT";
                case 3:
                    return "K.KARTI 3 TAKSIT";
                case 4:
                    return "K.KARTI 4 TAKSIT";
                case 5:
                    return "K.KARTI 5 TAKSIT";
                case 6:
                    return "K.KARTI 6 TAKSIT";
                case 9:
                    return "K.KARTI 9 TAKSIT";
                default:
                    return "K.KARTI";
            }

        }
        private void PaymentIntegration()
        {
            List<PaymentDTO> payments = "select * from Payment where IntegrationStatus = 0  and Status=1".BindListLocal<PaymentDTO>();

            int firmId = 0;
            Int32.TryParse(FirmIdStr, out firmId);

            try
            {
                if (payments != null)
                {
                    foreach (var pay in payments)
                    {
                        try
                        {
                            Logger.LogInfo("Just Start - " + pay.OrderNo);

                            string GroupCode1 = "" + DBClassLocal.Default.ExecuteScalarWithParams("select GroupCode1 from [User] where AccountId = @prm0", pay.AccountId);

                            DateTime dateTime = pay.PayDate;

                            string dt = dateTime.ToString("dd.MM.yyyy");
                            string dts = dateTime.ToString("HH");
                            string dtd = dateTime.ToString("mm");
                            string dtms = dateTime.ToString("ss");

                            Data data = Logo.NewDataObject(DataObjectType.doARAPVoucher);
                            data.New();

                            data.DataFields.FieldByName("NUMBER").Value = "IYZ" + pay.OrderNo.Substring(0, 10);
                            data.DataFields.FieldByName("DATE").Value = dt;
                            data.DataFields.FieldByName("TYPE").Value = 70;
                            data.DataFields.FieldByName("NOTES1").Value = pay.InsNum + " TAKSİT - Cari Hesap Ödeme";
                            data.DataFields.FieldByName("NOTES2").Value = "PayId: " + pay.OrderNo.Substring(0, 10) + " - TrnId: " + pay.OrderNo;
                            data.DataFields.FieldByName("TOTAL_CREDIT").Value = pay.Amount;
                            //data.DataFields.FieldByName("RC_TOTAL_CREDIT").Value = (double)pay.Amount / rate.CurrencyRate1;
                            data.DataFields.FieldByName("DATE_CREATED").Value = dt;
                            data.DataFields.FieldByName("HOUR_CREATED").Value = dts;
                            data.DataFields.FieldByName("MIN_CREATED").Value = dtd;
                            data.DataFields.FieldByName("SEC_CREATED").Value = dtms;
                            data.DataFields.FieldByName("GL_POSTED").Value = 1;
                            data.DataFields.FieldByName("GL_CODE").Value = pay.CariKod;
                            data.DataFields.FieldByName("OHP_CODE").Value = "8.01.01.999";
                            //data.DataFields.FieldByName("ACCFICHEREF").Value = ;
                            data.DataFields.FieldByName("CURRSEL_TOTALS").Value = pay.InsNum;
                            data.DataFields.FieldByName("ARP_CODE").Value = pay.CariKod;
                            data.DataFields.FieldByName("TIME").Value = GenerateTime(Convert.ToDateTime(dt));
                            data.DataFields.FieldByName("BANKACC_CODE").Value = "001.01001";
                            data.DataFields.FieldByName("BNGL_CODE").Value = "108.01.01.0001";
                            data.DataFields.FieldByName("BNOHP_CODE").Value = "8.01.01.999";
                            data.DataFields.FieldByName("AFFECT_RISK").Value = "1";

                            Lines lines = data.DataFields.FieldByName("TRANSACTIONS").Lines;

                            if (lines.AppendLine())
                            {
                                lines[0].FieldByName("ARP_CODE").Value = pay.CariKod;
                                lines[0].FieldByName("GL_CODE1").Value = pay.CariKod;
                                lines[0].FieldByName("GL_CODE2").Value = "108.01.01.0001";
                                lines[0].FieldByName("OHP_CODE2").Value = "8.00.01.1013";
                                lines[0].FieldByName("AUXIL_CODE").Value = pay.InsNum + " TAKSİT";
                                lines[0].FieldByName("TRANNO").Value = "IYZ" + pay.OrderNo;
                                lines[0].FieldByName("DOC_NUMBER").Value = "Cari Hesaba Ödeme";
                                lines[0].FieldByName("DESCRIPTION").Value = "";
                                lines[0].FieldByName("CREDIT").Value = (double)pay.Amount;
                                //lines[0].FieldByName("TC_XRATE").Value = 1;
                                //lines[0].FieldByName("TC_AMOUNT").Value = pay.Amount;
                                lines[0].FieldByName("RC_XRATE").Value = rate.CurrencyRate1;
                                lines[0].FieldByName("RC_AMOUNT").Value = (double)pay.Amount / rate.CurrencyRate1;
                                lines[0].FieldByName("PAYMENT_CODE").Value = FindPaymentCodeByInstallmentNumber(pay.InsNum);
                                //lines[0].FieldByName("DATE_MODIFIED").Value = dt;
                                //lines[0].FieldByName("HOUR_MODIFIED").Value = dts;
                                //lines[0].FieldByName("MIN_MODIFIED").Value = dtd;
                                //lines[0].FieldByName("SEC_MODIFIED").Value = dtms;
                                lines[0].FieldByName("CREDIT_CARD_NO").Value = pay.CCNo;
                                lines[0].FieldByName("PAYMENT_CODE").Value = FindPaymentCodeByInstallmentNumber(pay.InsNum);
                                //lines[0].FieldByName("MONTH").Value = ;
                                //lines[0].FieldByName("YEAR").Value = ;
                                lines[0].FieldByName("AFFECT_RISK").Value = 1;
                                lines[0].FieldByName("BANKACC_CODE").Value = "001.01001";
                                lines[0].FieldByName("BANK_GL_CODE").Value = "108.01.01.0001";
                                lines[0].FieldByName("BANK_OHP_CODE").Value = "8.01.01.999";
                                lines[0].FieldByName("DISTRIBUTION_TYPE_FNO").Value = 0;
                            }

                            data.ApplyCampaign();
                            data.FillAccCodes();

                            lines[0].FieldByName("OHP_CODE1").Value = GroupCode1 == "S" ? "6.00.01.2008" : "6.00.01.1008";

                            string xxxXML = "";

                            data.ExportToXmlStr("ARP_VOUCHERS", out xxxXML);

                            ValidateErrors validateErrors = data.ValidateErrors;
                            bool flag = data.Post();

                            if (!flag)
                            {
                                String EMsg = "";
                                if (data.ErrorCode > 0) EMsg = data.ErrorCode.ToString() + "-" + data.ErrorDesc;
                                if (data.ValidateErrors.Count > 0)
                                {
                                    for (int sayac = 0; sayac < data.ValidateErrors.Count; sayac++)
                                    { EMsg += "[" + Convert.ToString(data.ValidateErrors[sayac].ID) + "] " + data.ValidateErrors[sayac].Error; }
                                }

                                string message = "";
                                for (int index = 0; index < validateErrors.Count; ++index)
                                {
                                    message += string.Format("{0} - {1}; \n", (object)validateErrors[index].ID, (object)validateErrors[index].Error);
                                    Exception ex = new Exception(message);
                                }

                                Logger.LogInfo("Payment Error - " + message);
                                MailHelper.SendMail("entegrasyon@akilliticaretim.com", "destek@akilliticaret.com", "kerem.celik@orionyazilim.com; yunus.ayten@beybi.com.tr; sertac.akkaya@elorholding.com", "", "Fatura Oluşturulmadı", "Detay : " + message + " \n Ödeme No : " + pay.OrderNo.Substring(0, 10), true);
                            }
                            else
                            {
                                Logger.LogInfo("Payment Success - " + pay.OrderNo);
                                DBClassLocal.Default.ExecuteWithParams("update Payment set IntegrationStatus = 1 where Id = @prm0", pay.Id);
                            }
                        }
                        catch (Exception ex)
                        {
                            MailHelper.SendMail("entegrasyon@akilliticaretim.com", "destek@akilliticaret.com", "kerem.celik@orionyazilim.com; yunus.ayten@beybi.com.tr; sertac.akkaya@elorholding.com", "", "Fatura Oluşturulmadı", "Detay : " + ex.Message + " \n Sipariş No : " + pay.OrderNo, true);
                            Logger.LogInfo("--  Message -->" + (object)ex.Message + " -- StackTrace  --> " + ex.StackTrace + " --  Source --> ProductIntegration class:" + ex.Source + " -- InnerException -->" + (string)(object)ex.InnerException);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MailHelper.SendMail("entegrasyon@akilliticaretim.com", "destek@akilliticaret.com", "kerem.celik@orionyazilim.com; yunus.ayten@beybi.com.tr; sertac.akkaya@elorholding.com", "", "Entegrasyon Çalıştırılamadı", "Detay : " + ex.Message, true);

                Logger.LogInfo("--  Message -->" + (object)ex.Message + " -- StackTrace  --> " + ex.StackTrace + " --  Source --> ProductIntegration class:" + ex.Source + " -- InnerException -->" + (string)(object)ex.InnerException);
            }

        }

        public static long GenerateTime(DateTime date)
        {
            return (long)(date.Millisecond + date.Second * 256 + date.Minute * 65536 + 16777216 * date.Hour);
        }

        public Boolean islemdemi = false;
        private void timer1_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (!islemdemi)
            {
                try
                {
                    islemdemi = true;

                    IntegrationStart();

                    Logger.LogInfo("Aktarım işlemleri tamamlandı.");
                    islemdemi = false;
                }
                catch (Exception ex)
                {
                    Logger.LogInfo("timer1_Elapsed Hata Oluştu : " + ex.Message);
                    islemdemi = false;
                }
            }
        }

        private void IntegrationStart()
        {
            try
            {
                LogoUsername = ConfigurationManager.AppSettings["LogoUsername"];
                LogoUserPassword = ConfigurationManager.AppSettings["LogoUserPassword"];
                FirmIdStr = ConfigurationManager.AppSettings["FirmId"];

                int firmId = 0;
                Int32.TryParse(FirmIdStr, out firmId);

                if (LogoErisim(LogoUsername, LogoUserPassword, firmId, 1))
                {
                    logoPrefix = "" + DBClassLocal.Default.ExecuteScalarWithParams("select Value from ConfigurationParameter where [Key] = 'LogoTablePrefix'");
                    rate = GetRate();
                    OrderIntegration();
                    DBClassLocal.Default.ExecuteWithParams("update Basket set IntegrationStatus = 0 where IntegrationStatus = 2");
                    PaymentIntegration();
                }
            }
            catch (Exception ex)
            {
                MailHelper.SendMail("entegrasyon@akilliticaretim.com", "destek@akilliticaret.com", "kerem.celik@orionyazilim.com; yunus.ayten@beybi.com.tr; sertac.akkaya@elorholding.com", "", "Entegrasyon Çalıştırılamadı", "Beybi Lobject, Detay : " + ex.Message, true);

                Logger.LogInfo("--  Message -->" + (object)ex.Message + " -- StackTrace  --> " + ex.StackTrace + " --  Source --> IntegrationStart class:" + ex.Source + " -- InnerException -->" + (string)(object)ex.InnerException);
            }
        }

        public UnityApplication Logo;
        public Boolean boolErisimVar = false;
        string ServisCalismaPeriyodu = ConfigurationSettings.AppSettings["ServisCalismaPeriyodu"];
        int maxKBSize = Convert.ToInt32(ConfigurationSettings.AppSettings["maxKBSize"]);

        public Boolean LogoErisim(String KullaniciAdi, String Sifre, int Firma, int Donem)
        {
            try
            {
                if (Logo == null)
                {
                    Logo = new UnityApplication();
                    boolErisimVar = false;
                }
                else
                {
                    if (Logo.GetMemUsageKB() > maxKBSize)
                    {
                        Logo.Disconnect();
                        Logo = null;
                        System.GC.Collect();

                        Logo = new UnityObjects.UnityApplication();
                        boolErisimVar = false;
                    }
                }
                if (!boolErisimVar)
                {
                    if (!Logo.Login(KullaniciAdi, Sifre, Firma, Donem))
                    {
                        boolErisimVar = false;
                    }
                    else
                    {
                        boolErisimVar = true;
                    }
                }
            }
            catch (Exception exLogoErisim)
            {
                Logger.LogInfo("LOGO bağlantısında hata oluştu. Açıklaması : " + exLogoErisim);
                Logo = null;
                System.GC.Collect();
            }
            return boolErisimVar;
        }
    }
}
