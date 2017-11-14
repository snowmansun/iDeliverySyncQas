using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Security.Cryptography;




namespace eBest.Mobile.SyncLogic
{
    public sealed class Sync_V2
    {
        private IDbConnection con = null;
        private IDbCommand command = null;
        private StringBuilder sb = new StringBuilder();
        private int OrgId;
        private string UserCode;

        public Sync_V2()
        {
            con = new SqlConnection(ConfigurationManager.ConnectionStrings["CloudingSFA"].ConnectionString);
        }

        #region SyncLogic Members

        /// <summary>
        /// 同步验证
        /// </summary>
        /// <param name="name"></param>
        /// <param name="Password"></param>
        /// <returns></returns>
        public bool Authorization(string UserName, string Password)
        {
            bool flag = false;

            try
            {
                command = con.CreateCommand();
                MD5 md5 = System.Security.Cryptography.MD5.Create();
                byte[] dataToHash = (new UTF8Encoding()).GetBytes(Password);
                byte[] hashvalue = (new MD5CryptoServiceProvider()).ComputeHash(dataToHash);
                string value = string.Empty;
                foreach (byte b in hashvalue)
                {
                    value += b.ToString("X2");
                }

                sb.Clear();
                //sb.AppendFormat("EXECUTE dbo.SyncDown_Sys_Driver '{0}','{1}'", UserName, value);
                //command.CommandText = sb.ToString();
                string sql = string.Format(@"SELECT u.UserCode,ISNULL(p.OrgId,0) OrgId,IsLock,IsValid
                                            FROM MD_User u inner join MD_Person p on u.UserCode=p.UserCode
                                            WHERE u.UserCode='{0}' AND Password='{1}'"
                                    , UserName, value);
                command.CommandText = sql;
                command.CommandTimeout = 180;
                con.Open();

                using (IDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        //user = new UserEntity();
                        //user.CustomerID = reader["CustomerID"].ToString();
                        //user.OrgUnitId = int.Parse(reader["OrgUnitId"].ToString());
                        //user.UserName = reader["UserName"].ToString();
                        //user.Device = reader["Device"].ToString();
                        //user.Status = reader["Status"].ToString();
                        //user.DeleteFlag = Convert.ToBoolean(reader["DeleteFlag"].ToString());

                        UserCode = UserName;
                        OrgId = int.Parse(reader["OrgId"].ToString());
                        flag = true;
                    }
                }


            }
            catch (Exception ex)
            {
                flag = false;
                throw ex;
            }
            finally
            {
                if (con != null)
                {
                    con.Close();
                }
                if (command != null)
                {
                    command.Dispose();
                    command = null;
                }
            }
            return flag;
        }

        #endregion

        #region Download

        public String Download_MD_Product(String LastTime)
        {
            sb.Clear();
            sb.Append("EXEC SyncDown_MD_Product ");
            sb.AppendFormat(" '{0}',", UserCode);
            sb.AppendFormat(" '{0}',", OrgId);
            sb.AppendFormat(" '{0}'", LastTime);
            return sb.ToString();
        }

        public String Download_MD_ProductUOM(String LastTime)
        {
            sb.Clear();
            sb.Append("EXEC SyncDown_MD_ProductUOM ");
            sb.AppendFormat(" '{0}',", UserCode);
            sb.AppendFormat(" '{0}',", OrgId);
            sb.AppendFormat(" '{0}'", LastTime);
            return sb.ToString();
        }

        public String Download_DSD_M_TruckCheckList(String LastTime)
        {
            sb.Clear();
            sb.Append("EXEC SyncDown_DSD_M_TruckCheckList ");
            sb.AppendFormat(" '{0}',", UserCode);
            sb.AppendFormat(" '{0}'", LastTime);
            return sb.ToString();
        }

        public String Download_MD_Dictionary(String LastTime)
        {
            sb.Clear();
            sb.Append("EXEC SyncDown_MD_Dictionary ");
            sb.AppendFormat(" '{0}'", LastTime);
            return sb.ToString();
        }

        public String Download_MD_Person(String LastTime)
        {
            sb.Clear();
            sb.Append("EXEC SyncDown_MD_Person ");
            sb.AppendFormat(" '{0}',", UserCode);
            sb.AppendFormat(" '{0}'", LastTime);
            return sb.ToString();
        }

        public String Download_MD_Account(String LastTime)
        {
            sb.Clear();
            sb.Append("EXEC SyncDown_MD_Account ");
            sb.AppendFormat(" '{0}',", UserCode);
            sb.AppendFormat(" '{0}'", LastTime);
            return sb.ToString();
        }

        public String Download_MD_Account47(String LastTime)
        {
            sb.Clear();
            sb.Append("EXEC SyncDown_MD_Account47 ");
            sb.AppendFormat(" '{0}',", UserCode);
            sb.AppendFormat(" '{0}'", LastTime);
            return sb.ToString();
        }    

        public String Download_MD_Account(String LastTime,string arg)
        {
            sb.Clear();
            sb.Append("EXEC SyncDown_MD_Account_Query ");
            sb.AppendFormat(" '{0}',", UserCode);
            sb.AppendFormat(" '{0}',", LastTime);
            sb.AppendFormat(" '{0}'", arg);
            return sb.ToString();
        }

        public String Download_MD_Account47(String LastTime, string arg)
        {
            sb.Clear();
            sb.Append("EXEC SyncDown_MD_Account_Query ");
            sb.AppendFormat(" '{0}',", UserCode);
            sb.AppendFormat(" '{0}',", LastTime);
            sb.AppendFormat(" '{0}'", arg);
            return sb.ToString();
        }    
        public String Download_MD_AccountAR(String LastTime)
        {
            sb.Clear();
            sb.Append("EXEC SyncDown_MD_AccountAR ");
            sb.AppendFormat(" '{0}',", UserCode);
            sb.AppendFormat(" '{0}'", LastTime);
            return sb.ToString();
        }

        public String Download_MD_AccountAR(String LastTime,string arg)
        {
            sb.Clear();
            sb.Append("EXEC SyncDown_MD_AccountAR_Query ");
            sb.AppendFormat(" '{0}',", UserCode);
            sb.AppendFormat(" '{0}',", LastTime);
            sb.AppendFormat(" '{0}'", arg);
            return sb.ToString();
        }

        public String Download_MD_Contact(String LastTime)
        {
            sb.Clear();
            sb.Append("EXEC SyncDown_MD_Contact ");
            sb.AppendFormat(" '{0}',", UserCode);
            sb.AppendFormat(" '{0}'", LastTime);
            return sb.ToString();
        }

        public String Download_MD_Contact(String LastTime,string arg)
        {
            sb.Clear();
            sb.Append("EXEC SyncDown_MD_Contact_Query ");
            sb.AppendFormat(" '{0}',", UserCode);
            sb.AppendFormat(" '{0}',", LastTime);
            sb.AppendFormat(" '{0}'", arg);
            return sb.ToString();
        }

        public String Download_DSD_M_ShipmentHeader(String LastTime)
        {
            sb.Clear();
            sb.Append("EXEC SyncDown_DSD_M_ShipmentHeader ");
            sb.AppendFormat(" '{0}',", UserCode);
            sb.AppendFormat(" '{0}'", LastTime);
            return sb.ToString();
        }

        public String Download_DSD_M_ShipmentItem(String LastTime)
        {
            sb.Clear();
            sb.Append("EXEC SyncDown_DSD_M_ShipmentItem ");
            sb.AppendFormat(" '{0}',", UserCode);
            sb.AppendFormat(" '{0}'", LastTime);
            return sb.ToString();
        }

        public String Download_DSD_M_ShipmentHelper(String LastTime)
        {
            sb.Clear();
            sb.Append("EXEC SyncDown_DSD_M_ShipmentHelper ");
            sb.AppendFormat(" '{0}',", UserCode);
            sb.AppendFormat(" '{0}'", LastTime);
            return sb.ToString();
        }

        public String Download_DSD_M_ShipmentFinance(String LastTime)
        {
            sb.Clear();
            sb.Append("EXEC SyncDown_DSD_M_ShipmentFinance ");
            sb.AppendFormat(" '{0}',", UserCode);
            sb.AppendFormat(" '{0}'", LastTime);
            return sb.ToString();
        }

        public String Download_DSD_T_ShipmentFinance(String LastTime)
        {
            sb.Clear();
            sb.Append("EXEC SyncDown_DSD_T_ShipmentFinance ");
            sb.AppendFormat(" '{0}',", UserCode);
            sb.AppendFormat(" '{0}'", LastTime);
            return sb.ToString();
        }

        public String Download_DSD_T_ShipmentFinance60(String LastTime)
        {
            sb.Clear();
            sb.Append("EXEC SyncDown_DSD_T_ShipmentFinance60 ");
            sb.AppendFormat(" '{0}',", UserCode);
            sb.AppendFormat(" '{0}'", LastTime);
            return sb.ToString();
        }
        

        public String Download_DSD_M_DeliveryHeader(String LastTime)
        {
            sb.Clear();
            sb.Append("EXEC SyncDown_DSD_M_DeliveryHeader ");
            sb.AppendFormat(" '{0}',", UserCode);
            sb.AppendFormat(" '{0}'", LastTime);
            return sb.ToString();
        }

        public String Download_DSD_M_DeliveryItem(String LastTime)
        {
            sb.Clear();
            sb.Append("EXEC SyncDown_DSD_M_DeliveryItem ");
            sb.AppendFormat(" '{0}',", UserCode);
            sb.AppendFormat(" '{0}'", LastTime);
            return sb.ToString();
        }

        public String Download_DSD_T_DeliveryHeader(String LastTime)
        {
            sb.Clear();
            sb.Append("EXEC SyncDown_DSD_T_DeliveryHeader ");
            sb.AppendFormat(" '{0}',", UserCode);
            sb.AppendFormat(" '{0}'", LastTime);
            return sb.ToString();
        }

        public String Download_DSD_T_DeliveryItem(String LastTime)
        {
            sb.Clear();
            sb.Append("EXEC SyncDown_DSD_T_DeliveryItem ");
            sb.AppendFormat(" '{0}',", UserCode);
            sb.AppendFormat(" '{0}'", LastTime);
            return sb.ToString();
        }

        public String Download_DSD_M_SystemConfig(String LastTime)
        {
            sb.Clear();
            sb.Append("EXEC SyncDown_DSD_M_SystemConfig ");
            sb.AppendFormat(" '{0}',", OrgId);
            sb.AppendFormat(" '{0}'", LastTime);
            return sb.ToString();
        }
        public string Download_DSD_Truck(String LastTime)
        {
            sb.Clear();
            sb.Append("EXEC SyncDown_DSD_M_Truck ");
            sb.AppendFormat(" '{0}',", UserCode);
            sb.AppendFormat(" '{0}'", LastTime);
            return sb.ToString();
        }
        public string Download_DSD_DayTimeTracking(String LastTime)
        {
            sb.Clear();
            sb.Append("EXEC SyncDown_DSD_M_DayTimeTracking ");
            sb.AppendFormat(" '{0}',", UserCode);
            sb.AppendFormat(" '{0}'", LastTime);
            return sb.ToString();
        }
        public string Download_DSD_T_TruckCheckResult(String LastTime)
        {
            sb.Clear();
            sb.Append("EXEC SyncDown_DSD_T_TruckCheckResult ");
            sb.AppendFormat(" '{0}',", UserCode);
            sb.AppendFormat(" '{0}'", LastTime);
            return sb.ToString();
        }

        public string Download_DSD_M_Truck(String LastTime)
        {
            sb.Clear();
            sb.Append("EXEC SyncDown_DSD_M_Truck ");
            sb.AppendFormat(" '{0}',", UserCode);
            sb.AppendFormat(" '{0}'", LastTime);
            return sb.ToString();
        }

        public string Download_DSD_T_TruckStock(String LastTime)
        {
            sb.Clear();
            sb.Append("EXEC SyncDown_DSD_T_TruckStock ");
            sb.AppendFormat(" '{0}',", UserCode);
            sb.AppendFormat(" '{0}'", LastTime);
            return sb.ToString();
        }

        public string Download_DSD_T_TruckStockTracking(String LastTime)
        {
            sb.Clear();
            sb.Append("EXEC SyncDown_DSD_T_TruckStockTracking ");
            sb.AppendFormat(" '{0}',", UserCode);
            sb.AppendFormat(" '{0}'", LastTime);
            return sb.ToString();
        }

        public string Download_DSD_T_ShipmentHeader(String LastTime)
        {
            sb.Clear();
            sb.Append("EXEC SyncDown_DSD_T_ShipmentHeader ");
            sb.AppendFormat(" '{0}',", UserCode);
            sb.AppendFormat(" '{0}'", LastTime);
            return sb.ToString();
        }

        public string Download_DSD_T_ShipmentItem(String LastTime)
        {
            sb.Clear();
            sb.Append("EXEC SyncDown_DSD_T_ShipmentItem ");
            sb.AppendFormat(" '{0}',", UserCode);
            sb.AppendFormat(" '{0}'", LastTime);
            return sb.ToString();
        }

        public string Download_DSD_T_ShipmentHelper(String LastTime)
        {
            sb.Clear();
            sb.Append("EXEC SyncDown_DSD_T_ShipmentHelper ");
            sb.AppendFormat(" '{0}',", UserCode);
            sb.AppendFormat(" '{0}'", LastTime);
            return sb.ToString();
        }

        public string Download_DSD_T_DayTimeTracking(String LastTime)
        {
            sb.Clear();
            sb.Append("EXEC SyncDown_DSD_T_DayTimeTracking ");
            sb.AppendFormat(" '{0}',", UserCode);
            sb.AppendFormat(" '{0}'", LastTime);
            return sb.ToString();
        }

        public string Download_DSD_T_Visit(String LastTime)
        {
            sb.Clear();
            sb.Append("EXEC SyncDown_DSD_T_Visit ");
            sb.AppendFormat(" '{0}',", UserCode);
            sb.AppendFormat(" '{0}'", LastTime);
            return sb.ToString();
        }

        public string Download_DSD_T_DeliveryBilling(String LastTime)
        {
            sb.Clear();
            sb.Append("EXEC SyncDown_DSD_T_DeliveryBilling ");
            sb.AppendFormat(" '{0}',", UserCode);
            sb.AppendFormat(" '{0}'", LastTime);
            return sb.ToString();
        }

        public string Download_DSD_M_ShipmentVanSalesRoute(String LastTime)
        {
            sb.Clear();
            sb.Append("EXEC SyncDown_DSD_M_ShipmentVanSalesRoute ");
            sb.AppendFormat(" '{0}',", UserCode);
            sb.AppendFormat(" '{0}'", LastTime);
            return sb.ToString();
        }

        public string Download_DSD_M_ShipmentVanSalesRoute20(String LastTime)
        {
            sb.Clear();
            sb.Append("EXEC SyncDown_DSD_M_ShipmentVanSalesRoute20 ");
            sb.AppendFormat(" '{0}',", UserCode);
            sb.AppendFormat(" '{0}'", LastTime);
            return sb.ToString();
        }
        public string Download_DSD_T_ARInvoice(String LastTime)
        {
            sb.Clear();
            sb.Append("EXEC SyncDown_DSD_T_ARInvoice ");
            sb.AppendFormat(" '{0}',", UserCode);
            sb.AppendFormat(" '{0}'", LastTime);
            return sb.ToString();
        }

        public string Download_DSD_T_ARCollection(String LastTime)
        {
            sb.Clear();
            sb.Append("EXEC SyncDown_DSD_T_ARCollection ");
            sb.AppendFormat(" '{0}',", UserCode);
            sb.AppendFormat(" '{0}'", LastTime);
            return sb.ToString();
        }

        public string Download_DSD_T_Payment(String LastTime)
        {
            sb.Clear();
            sb.Append("EXEC SyncDown_DSD_T_Payment ");
            sb.AppendFormat(" '{0}',", UserCode);
            sb.AppendFormat(" '{0}'", LastTime);
            return sb.ToString();
        }

        public string Download_DSD_T_Order(string LastTime)
        {
            sb.Clear();
            sb.Append("EXEC SyncDown_DSD_T_Order ");
            sb.AppendFormat(" '{0}',", UserCode);
            sb.AppendFormat(" '{0}'", LastTime);
            return sb.ToString();
        }

        public string Download_DSD_T_OrderItem(string LastTime)
        {
            sb.Clear();
            sb.Append("EXEC SyncDown_DSD_T_OrderItem ");
            sb.AppendFormat(" '{0}',", UserCode);
            sb.AppendFormat(" '{0}'", LastTime);
            return sb.ToString();
        }
        #endregion

        #region Upload

        public string Upload_DSD_T_DayTimeTracking()
        {
            sb.Clear();
            sb.Append(string.Format("EXEC Upload_DSD_T_DayTimeTracking @1,@2,@3,@4,@5,@6,@7,@8,@9,@10,'{0}'",UserCode));
            return sb.ToString();
        }

        public string Upload_DSD_T_TruckCheckResult()
        {
            sb.Clear();
            sb.Append(string.Format("EXEC Upload_DSD_T_TruckCheckResult '{0}',@1,@2,@3,@4,@5 ", UserCode));
            return sb.ToString();
        }

        public string Upload_DSD_T_ShipmentHelper()
        {
            sb.Clear();
            sb.Append(string.Format("EXEC Upload_DSD_T_ShipmentHelper '{0}',@1,@2,@3,@4 ", UserCode));
            return sb.ToString();
        }

        public string Upload_DSD_T_ShipmentFinance()
        {
            sb.Clear();
            sb.Append(string.Format("EXEC Upload_DSD_T_ShipmentFinance '{0}',@1,@2,@3,@4,@5", UserCode));
            return sb.ToString();
        }

        public string Upload_DSD_T_ShipmentFinance60()
        {
            sb.Clear();
            sb.Append(string.Format("EXEC Upload_DSD_T_ShipmentFinance60 '{0}',@1,@2,@3,@4,@5,@6", UserCode));
            return sb.ToString();
        }

        public string Upload_DSD_T_TruckStock()
        {
            sb.Clear();
            sb.Append(string.Format("EXEC Upload_DSD_T_TruckStock '{0}',@1,@2,@3,@4,@5,@6,@7", UserCode));
            return sb.ToString();
        }

        public string Upload_DSD_T_TruckStockTracking()
        {
            sb.Clear();
            sb.Append(string.Format("EXEC Upload_DSD_T_TruckStockTracking '{0}',@1,@2,@3,@4,@5,@6,@7,@8,@9,@10,@11", UserCode));
            return sb.ToString();
        }

        public string Upload_DSD_T_ShipmentHeader()
        {
            sb.Clear();
            sb.Append(string.Format(@"EXEC Upload_DSD_T_ShipmentHeader @1,@2,@3,@4,@5,@6,@7,@8,@9,@10,@11,@12
                                        ,@13,@14,@15,@16,@17,@18,@19,@20,@21,@22,@23,@24,@25,@26,@27,@28,@29,@30,'{0}'", UserCode));
            return sb.ToString();
        }
        public string Upload_DSD_T_ShipmentItem()
        {
            sb.Clear();
            sb.Append(string.Format(@"EXEC Upload_DSD_T_ShipmentItem @1,@2,@3,@4,@5,@6,@7,@8,'{0}'", UserCode));
            return sb.ToString();
        }

        public string Upload_DSD_T_DeliveryHeader()
        {
            sb.Clear();
            sb.Append(string.Format(@"EXEC Upload_DSD_T_DeliveryHeader '{0}',@1,@2,@3,@4,@5,
                                                                            @6,@7,@8,@9,@10,
                                                                            @11,@12,@13,@14,@15,
                                                                            @16,@17,@18,@19,@20,
                                                                            @21,@22,@23,@24,@25,
                                                                            @26,@27,@28,@29,@30,@31", UserCode));
            return sb.ToString();
        }

        public string Upload_DSD_T_DeliveryItem()
        {
            sb.Clear();
            sb.Append(string.Format(@"EXEC Upload_DSD_T_DeliveryItem '{0}',@1,@2,@3,@4,@5,
                                                                            @6,@7,@8,@9,@10,
                                                                            @11,@12,@13,@14", UserCode));
            return sb.ToString();
        }

        public string Upload_DSD_T_DeliveryBilling()
        {
            sb.Clear();
            sb.Append(string.Format(@"EXEC Upload_DSD_T_DeliveryBilling '{0}',@1,@2,@3,@4,@5,
                                                                            @6,@7,@8,@9", UserCode));
            return sb.ToString();
        }

        public string Upload_DSD_T_Visit()
        {
            sb.Clear();
            sb.Append(string.Format(@"EXEC Upload_DSD_T_Visit '{0}',@1,@2,@3,@4,@5,
                                                                    @6,@7,@8,@9,@10", UserCode));
            return sb.ToString();
        }
        
        public string Upload_DSD_T_ARInvoice()
        {
            sb.Clear();
            sb.Append(string.Format(@"EXEC Upload_DSD_T_ARInvoice '{0}',@1,@2,@3,@4,@5,
                                                                    @6,@7,@8,@9", UserCode));
            return sb.ToString();
        }

        public string Upload_DSD_T_ARCollection()
        {
            sb.Clear();
            sb.Append(string.Format(@"EXEC Upload_DSD_T_ARCollection '{0}',@1,@2,@3,@4,@5,
                                                                    @6,@7,@8,@9,@10,
                                                                    @11,@12,@13,@14,@15,
                                                                    @16,@17,@18", UserCode));
            return sb.ToString();
        }

        public string Upload_DSD_T_Payment()
        {
            sb.Clear();
            sb.Append(string.Format(@"EXEC Upload_DSD_T_Payment '{0}',@1,@2,@3,@4,@5,
                                                                    @6,@7,@8,@9,@10,
                                                                    @11", UserCode));
            return sb.ToString();
        }

        public string Upload_App_Log()
        {
            sb.Clear();
            sb.Append(string.Format(@"EXEC Upload_App_Log '{0}',@1,@2,@3,@4,@5,@6,@7", UserCode));
            return sb.ToString();
        }

        public string Upload_DSD_M_ShipmentVanSalesRoute20()
        {
            sb.Clear();
            sb.Append(string.Format(@"EXEC Upload_DSD_M_ShipmentVanSalesRoute20 '{0}',@1,@2,@3,@4", UserCode));
            return sb.ToString();
        }

        public string Upload_DSD_T_Order()
        {
            sb.Clear();
            sb.Append(string.Format(@"EXEC Upload_DSD_T_Order '{0}',@1,@2,@3,@4,@5,
                                                                    @6,@7,@8,@9,@10,
                                                                    @11,@12,@13,@14,@15,
                                                                    @16,@17,@18,@19,@20,
                                                                    @21", UserCode));
            return sb.ToString();
        }

        public string Upload_DSD_T_OrderItem()
        {
            sb.Clear();
            sb.Append(string.Format(@"EXEC Upload_DSD_T_OrderItem '{0}',@1,@2,@3,@4,@5,
                                                                    @6,@7,@8,@9,@10,@11", UserCode));
            return sb.ToString();
        }
        #endregion
    }
}
