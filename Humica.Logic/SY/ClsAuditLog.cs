using Humica.EF.MD;
using Humica.EF;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Humica.EF.Models.SY;

namespace Humica.Logic
{
    public class ClsAuditLog
    {
        public static void AuditLog(string ScreenId, string keyValue, string ObjName, object TableOld, object TableNew)
        {
            //activate audit log
            var listName = new List<string>();
            if (!string.IsNullOrEmpty(ObjName))
                listName.Add(ObjName);
            else listName.Add("GeneralData");


            //H------------------
            var listDix = new List<Dictionary<string, string>>();
            Dictionary<string, string> dataDixOld = TableOld.GetType().GetProperties().ToDictionary(x => x.Name, x => x.GetValue(TableOld)?.ToString() ?? "");
            Dictionary<string, string> dataDixNew = TableNew.GetType().GetProperties().ToDictionary(x => x.Name, x => x.GetValue(TableNew)?.ToString() ?? "");
            listDix.Add(dataDixOld);
            listDix.Add(dataDixNew);
            activeAuditLog(ScreenId, keyValue, SYActionBehavior.EDIT.ToString(), listName, listDix);
        }
        public static void activeAuditLog(string screenid, string keyValue, string action, List<string> listObjName, List<Dictionary<string, string>> dataDixHeader,
            List<Dictionary<string, string>> dataDix1 = null,
            List<Dictionary<string, string>> dataDix2 = null,
            List<Dictionary<string, string>> dataDix3 = null,
            List<Dictionary<string, string>> dataDix4 = null,
            List<Dictionary<string, string>> dataDix5 = null)
        {
            SMSystemEntity DB = new SMSystemEntity();
            SYScreenDevelop objScreen = DB.SYScreenDevelops.Find(screenid);
            if (objScreen == null)
            {
                objScreen = new SYScreenDevelop();
            }
            if (listObjName == null)
            {
                listObjName = new List<string>();
            }
            DateTime changeDate = DateTime.Now;
            //if (objScreen.IsActivateAuditLog == true)
            //{
                if (dataDixHeader.Count == 2)
                {
                    if (listObjName[0] != null)
                    {
                        Dictionary<string, string> oldHeader = dataDixHeader.First();
                        Dictionary<string, string> newHeader = dataDixHeader.Last();
                        foreach (KeyValuePair<string, string> key in oldHeader)
                        {
                            List<KeyValuePair<string, string>> checkNew = newHeader.Where(w => w.Key == key.Key).ToList();
                            if (checkNew.Count > 0)
                            {
                                if (ClsInformation.IsNumeric(checkNew.First().Value) && ClsInformation.IsNumeric(key.Value))
                                {
                                    var newNum = Convert.ToDecimal(checkNew.First().Value);
                                    var oldNum = Convert.ToDecimal(key.Value);
                                    if (newNum != oldNum)
                                    {
                                        ExEventAuditLog objNew = new ExEventAuditLog();
                                        objNew.Company = SYSConstant.DEFAULT_PLANT;
                                        objNew.ScreenID = screenid;
                                        objNew.DateChange = changeDate;
                                        objNew.ObjectName = listObjName[0];
                                        objNew.FieldName = key.Key;
                                        objNew.OldValue = key.Value;
                                        objNew.UserName = SYSession.getSessionUser().UserName;
                                        objNew.Action = action;
                                        objNew.KeyValue = keyValue;
                                        if (checkNew.Count > 0)
                                        {
                                            objNew.NewValue = checkNew.First().Value;
                                        }
                                        DB.ExEventAuditLogs.Add(objNew);
                                    }
                                }
                                else
                                {
                                    if (!checkNew.First().Value.Equals(key.Value))
                                    {
                                        ExEventAuditLog objNew = new ExEventAuditLog();
                                        objNew.Company = SYConstant.DEFAULT_PLANT;
                                        objNew.ScreenID = screenid;
                                        objNew.DateChange = changeDate;
                                        objNew.ObjectName = listObjName[0];
                                        objNew.FieldName = key.Key;
                                        objNew.OldValue = key.Value;
                                        objNew.UserName = SYSession.getSessionUser().UserName;
                                        objNew.Action = action;
                                        objNew.KeyValue = keyValue;
                                        if (checkNew.Count > 0)
                                        {
                                            objNew.NewValue = checkNew.First().Value;
                                        }
                                        DB.ExEventAuditLogs.Add(objNew);
                                    }
                                }

                            }
                        }
                        DB.SaveChanges();
                    }
                }
                //data dix 1
                if (dataDix1 != null)
                {
                    if (dataDix1.Count == 2)
                    {
                        if (listObjName[1] != null)
                        {
                            DB = new SMSystemEntity();
                            Dictionary<string, string> oldHeader = dataDix1.First();
                            Dictionary<string, string> newHeader = dataDix1.Last();
                            foreach (KeyValuePair<string, string> key in oldHeader)
                            {
                                List<KeyValuePair<string, string>> checkNew = newHeader.Where(w => w.Key == key.Key).ToList();
                                if (checkNew.First().Value != key.Value)
                                {
                                    ExEventAuditLog objNew = new ExEventAuditLog();
                                    objNew.Company = SYConstant.DEFAULT_PLANT;
                                    objNew.ScreenID = screenid;
                                    objNew.DateChange = changeDate;
                                    objNew.ObjectName = listObjName[1];
                                    objNew.FieldName = key.Key;
                                    objNew.OldValue = key.Value;
                                    objNew.UserName = SYSession.getSessionUser().UserName;
                                    objNew.Action = action;
                                    objNew.KeyValue = keyValue;
                                    if (checkNew.Count > 0)
                                    {
                                        objNew.NewValue = checkNew.First().Value;
                                    }
                                    DB.ExEventAuditLogs.Add(objNew);
                                }
                            }
                            DB.SaveChanges();
                        }
                    }
                }
                //data dix 2
                if (dataDix2 != null)
                {
                    if (dataDix2.Count == 2)
                    {
                        if (listObjName[2] != null)
                        {
                            DB = new SMSystemEntity();
                            Dictionary<string, string> oldHeader = dataDix2.First();
                            Dictionary<string, string> newHeader = dataDix2.Last();
                            foreach (KeyValuePair<string, string> key in oldHeader)
                            {
                                List<KeyValuePair<string, string>> checkNew = newHeader.Where(w => w.Key == key.Key).ToList();
                                if (checkNew.First().Value != key.Value)
                                {
                                    ExEventAuditLog objNew = new ExEventAuditLog();
                                    objNew.Company = SYConstant.DEFAULT_PLANT;
                                    objNew.ScreenID = screenid;
                                    objNew.DateChange = changeDate;
                                    objNew.ObjectName = listObjName[2];
                                    objNew.FieldName = key.Key;
                                    objNew.OldValue = key.Value;
                                    objNew.UserName = SYSession.getSessionUser().UserName;
                                    objNew.Action = action;
                                    objNew.KeyValue = keyValue;
                                    if (checkNew.Count > 0)
                                    {
                                        objNew.NewValue = checkNew.First().Value;
                                    }
                                    DB.ExEventAuditLogs.Add(objNew);
                                }
                            }
                            DB.SaveChanges();
                        }
                    }
                }
                //data dix 3
                if (dataDix3 != null)
                {
                    if (dataDix3.Count == 2)
                    {
                        if (listObjName[3] != null)
                        {
                            DB = new SMSystemEntity();
                            Dictionary<string, string> oldHeader = dataDix3.First();
                            Dictionary<string, string> newHeader = dataDix3.Last();
                            foreach (KeyValuePair<string, string> key in oldHeader)
                            {
                                List<KeyValuePair<string, string>> checkNew = newHeader.Where(w => w.Key == key.Key).ToList();
                                if (checkNew.First().Value != key.Value)
                                {
                                    ExEventAuditLog objNew = new ExEventAuditLog();
                                    objNew.Company = SYConstant.DEFAULT_PLANT;
                                    objNew.ScreenID = screenid;
                                    objNew.DateChange = changeDate;
                                    objNew.ObjectName = listObjName[3];
                                    objNew.FieldName = key.Key;
                                    objNew.OldValue = key.Value;
                                    objNew.UserName = SYSession.getSessionUser().UserName;
                                    objNew.Action = action;
                                    objNew.KeyValue = keyValue;
                                    if (checkNew.Count > 0)
                                    {
                                        objNew.NewValue = checkNew.First().Value;
                                    }
                                    DB.ExEventAuditLogs.Add(objNew);
                                }
                            }
                            DB.SaveChanges();
                        }
                    }

                }
                //data dix 4
                if (dataDix4 != null)
                {
                    if (dataDix4.Count == 2)
                    {
                        if (listObjName[4] != null)
                        {
                            DB = new SMSystemEntity();
                            Dictionary<string, string> oldHeader = dataDix4.First();
                            Dictionary<string, string> newHeader = dataDix4.Last();
                            foreach (KeyValuePair<string, string> key in oldHeader)
                            {
                                List<KeyValuePair<string, string>> checkNew = newHeader.Where(w => w.Key == key.Key).ToList();
                                if (checkNew.First().Value != key.Value)
                                {
                                    ExEventAuditLog objNew = new ExEventAuditLog();
                                    objNew.Company = SYConstant.DEFAULT_PLANT;
                                    objNew.ScreenID = screenid;
                                    objNew.DateChange = changeDate;
                                    objNew.ObjectName = listObjName[4];
                                    objNew.FieldName = key.Key;
                                    objNew.OldValue = key.Value;
                                    objNew.UserName = SYSession.getSessionUser().UserName;
                                    objNew.Action = action;
                                    objNew.KeyValue = keyValue;
                                    if (checkNew.Count > 0)
                                    {
                                        objNew.NewValue = checkNew.First().Value;
                                    }
                                    DB.ExEventAuditLogs.Add(objNew);
                                }
                            }
                            DB.SaveChanges();
                        }
                    }

                }
                //data dix 5
                if (dataDix5 != null)
                {
                    if (dataDix5.Count == 2)
                    {
                        if (listObjName[5] != null)
                        {
                            DB = new SMSystemEntity();
                            Dictionary<string, string> oldHeader = dataDix5.First();
                            Dictionary<string, string> newHeader = dataDix5.Last();
                            foreach (KeyValuePair<string, string> key in oldHeader)
                            {

                                List<KeyValuePair<string, string>> checkNew = newHeader.Where(w => w.Key == key.Key).ToList();
                                if (checkNew.First().Value != key.Value)
                                {
                                    ExEventAuditLog objNew = new ExEventAuditLog();
                                    objNew.Company = SYConstant.DEFAULT_PLANT;
                                    objNew.ScreenID = screenid;
                                    objNew.DateChange = changeDate;
                                    objNew.ObjectName = listObjName[5];
                                    objNew.FieldName = key.Key;
                                    objNew.OldValue = key.Value;
                                    objNew.UserName = SYSession.getSessionUser().UserName;
                                    objNew.Action = action;
                                    objNew.KeyValue = keyValue;
                                    if (checkNew.Count > 0)
                                    {
                                        objNew.NewValue = checkNew.First().Value;
                                    }
                                    DB.ExEventAuditLogs.Add(objNew);
                                }
                            }
                            DB.SaveChanges();
                        }

                    }

                }
            //}

        }
    }
}
