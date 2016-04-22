using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Net.Mime;
using System.Web;
using System.Web.Mvc;
using DMS_WEB.Models;

namespace DMS_WEB.Controllers
{
    public class RequisitionController : BaseController
    {
        //
        // GET: /Requisition/

        public ActionResult Index()
        {
            this.LoadCommonJsonData();
            return View();
        }

        [HttpPost]
        public ActionResult Save(FormCollection frm)
        {
            short CLIENT_NO = short.Parse(frm["CLIENT_NO"].ToString());
            short DEPT_NO = short.Parse(frm["DEPT_NO"].ToString());
            short REQ_TYPE_NO = short.Parse(frm["REQ_TYPE_NO"].ToString());
            //short REQUESTER_NO = short.Parse(frm["REQUESTER_NO"].ToString());
            string REQUEST_DATE = frm["REQUEST_DATE"].ToString();
            string REMARKS = frm["REMARKS"].ToString();
            

            string UploadPath = ConfigurationManager.AppSettings["UploadPath"];

            List<GET_BOX_COUNT_FOR_REQUISITION_MISMATCH_CHECK_Result> listForBoxRequisition = new List<GET_BOX_COUNT_FOR_REQUISITION_MISMATCH_CHECK_Result>();
            List<GET_ACCOUNT_COUNT_FOR_REQUISITION_MISMATCH_CHECK_Result> listForAccountRequisition = new List<GET_ACCOUNT_COUNT_FOR_REQUISITION_MISMATCH_CHECK_Result>();
            List<GET_ACCOUNT_DOC_LIST_FOR_REQUISITION_MISMATCH_CHECK_Result> listForAccountDocRequisition = new List<GET_ACCOUNT_DOC_LIST_FOR_REQUISITION_MISMATCH_CHECK_Result>();
            List<GET_CHALLAN_COUNT_FOR_REQUISITION_MISMATCH_CHECK_Result>   listForChallanRequisition = new List<GET_CHALLAN_COUNT_FOR_REQUISITION_MISMATCH_CHECK_Result>();




            this.LoadSecurity();
            this.LoadCommonJsonData();

            DataSet ds = new DataSet();

            HttpPostedFileBase file = Request.Files["txtFile"];

            if (file.ContentLength > 0)
            {
                string fileExtension = System.IO.Path.GetExtension(file.FileName);

                if (fileExtension == ".xls" || fileExtension == ".xlsx")
                {
                    string fileLocation = Server.MapPath(UploadPath + file.FileName);
                    if (System.IO.File.Exists(fileLocation))
                    {
                        System.IO.File.Delete(fileLocation);
                    }
                    file.SaveAs(fileLocation);
                    string excelConnectionString = string.Empty;
                    excelConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" +
                                            fileLocation + ";Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=2\"";
                    //connection String for xls file format.
                    if (fileExtension == ".xls")
                    {
                        excelConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" +
                                                fileLocation + ";Extended Properties=\"Excel 8.0;HDR=Yes;IMEX=2\"";
                    }
                    //connection String for xlsx file format.
                    else if (fileExtension == ".xlsx")
                    {
                        excelConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" +
                                                fileLocation + ";Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=2\"";
                    }

                    //Create Connection to Excel work book and add oledb namespace
                    OleDbConnection excelConnection = new OleDbConnection(excelConnectionString);
                    excelConnection.Open();
                    DataTable dt = new DataTable();

                    dt = excelConnection.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                    if (dt == null)
                    {
                        return null;
                    }

                    String[] excelSheets = new String[dt.Rows.Count];
                    int t = 0;

                    //excel data saves in temp file here.
                    foreach (DataRow row in dt.Rows)
                    {
                        excelSheets[t] = row["TABLE_NAME"].ToString();
                        t++;
                    }
                    OleDbConnection excelConnection1 = new OleDbConnection(excelConnectionString);


                    string query = string.Format("Select * from [{0}]", excelSheets[0]);
                    using (OleDbDataAdapter dataAdapter = new OleDbDataAdapter(query, excelConnection1))
                    {
                        dataAdapter.Fill(ds);
                    }

                    excelConnection.Close();
                }

                ArrayList myArrayList = new ArrayList();

                if (REQ_TYPE_NO == 1)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        myArrayList.Add(ds.Tables[0].Rows[i][3].ToString());
                    }
                }
                else if (REQ_TYPE_NO == 2)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        myArrayList.Add(ds.Tables[0].Rows[i][1].ToString());
                    }
                }
                else if (REQ_TYPE_NO == 4)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        myArrayList.Add(ds.Tables[0].Rows[i][0].ToString());
                    }


                }
                else if (REQ_TYPE_NO == 5)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        myArrayList.Add(ds.Tables[0].Rows[i][0].ToString());
                        myArrayList.Add(ds.Tables[0].Rows[i][2].ToString());

                        //using (DBEntities db = new DBEntities())
                        //{
                        //    listForAccountDocRequisition =
                        //        db.GET_ACCOUNT_DOC_LIST_FOR_REQUISITION_MISMATCH_CHECK(CLIENT_NO, REQ_TYPE_NO,
                        //            ds.Tables[0].Rows[i][0].ToString(), ds.Tables[0].Rows[i][2].ToString()).ToList();
                        //}
                    }
                }

                //for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                //{
                //    myArrayList.Add(ds.Tables[0].Rows[i][0].ToString());
                //}


                using (DBEntities db = new DBEntities())
                {
                    string myString = String.Join(",", myArrayList.ToArray());

                    listForBoxRequisition = db.GET_BOX_COUNT_FOR_REQUISITION_MISMATCH_CHECK(CLIENT_NO, REQ_TYPE_NO, myString).ToList();
                    listForAccountRequisition =
                                       db.GET_ACCOUNT_COUNT_FOR_REQUISITION_MISMATCH_CHECK(CLIENT_NO, REQ_TYPE_NO, myString).ToList();
                    listForChallanRequisition =
                        db.GET_CHALLAN_COUNT_FOR_REQUISITION_MISMATCH_CHECK(CLIENT_NO, REQ_TYPE_NO, myString).ToList();


                    Session["Value_list"] = myArrayList;
                    Session["list_For_Box_Requisition"] = listForBoxRequisition;
                    Session["list_For_Account_Requisition"] = listForAccountRequisition;
                    Session["list_For_Account_Doc_Requisition"] = listForAccountDocRequisition;
                    Session["list_For_Challan_Requisition"] = listForChallanRequisition;
                    Session["CLIENT_NO"] = CLIENT_NO;
                    Session["DEPT_NO"] = DEPT_NO;
                    Session["REQ_TYPE_NO"] = REQ_TYPE_NO;
                    Session["REQUEST_DATE"] = REQUEST_DATE;
                    Session["UPLOAD_FILE_NAME"] = file.FileName;
                    Session["FILE_EXT"] = fileExtension;
                    //Session["REQUESTER_NO"] = REQUESTER_NO;
                    Session["REMARKS"] = REMARKS;
                }
            }

            if (REQ_TYPE_NO == 1)
            {
                return View("ChallanList", listForChallanRequisition);
            }
            else if (REQ_TYPE_NO == 2)
            {
                return View("BoxList", listForBoxRequisition);
            }

            else if (REQ_TYPE_NO == 4)
            {
                return View("AccountList", listForAccountRequisition);
            }
            else if (REQ_TYPE_NO == 5)
            {
                return View("AccountDocList", listForAccountDocRequisition);
            }

            return View("Index");
        }

        public ActionResult SaveList()
        {
            this.LoadSecurity();
            this.LoadCommonJsonData();

            short CLIENT_NO = short.Parse(Session["CLIENT_NO"].ToString());
            short DEPT_NO = short.Parse(Session["DEPT_NO"].ToString());
            short REQ_TYPE_NO = short.Parse(Session["REQ_TYPE_NO"].ToString());
            string REQUEST_DATE = Session["REQUEST_DATE"].ToString();
            string FILE_NAME = Session["UPLOAD_FILE_NAME"].ToString();
            string FILE_EXT = Session["FILE_EXT"].ToString();
            string REMARKS = Session["REMARKS"].ToString();

            ArrayList myArrayList = new ArrayList();

            //string myString = Session["Value_list"].ToString();
            //string[] values = myString.Split(',');

            //foreach (string s in values)
            //{
            //    myArrayList.Add(s);
            //}

            foreach (string ID in Session["Value_list"] as ArrayList)
            {
                myArrayList.Add(ID);
            }


            using (DBEntities db = new DBEntities())
            {
                string myString = String.Join(",", myArrayList.ToArray());
                db.SEC_CLIENT_USER_REQUISITION_AND_REQUISITION_DETAILS_INSERT(CLIENT_NO, DEPT_NO, Convert.ToByte(REQ_TYPE_NO),
                   Convert.ToDateTime(REQUEST_DATE), FILE_NAME, FILE_EXT, null, REMARKS, myString, this.USER_NO, this.LOGON_NO);
            }

            return View("Index");
        }


        public ActionResult ShowManualDetails(short TRN_TYPE_NO = 0)
        {
            ArrayList myArrayList = new ArrayList();

            foreach (string ID in Session["Value_list"] as ArrayList)
            {
                myArrayList.Add(ID);
            }

            string myStringList = String.Join(",", myArrayList.ToArray());

            int CLIENT_NO = Convert.ToInt32(Session["CLIENT_NO"].ToString());
            int REQ_TYPE_NO = Convert.ToInt32(Session["REQ_TYPE_NO"].ToString());

            List<GET_BOX_LIST_FOR_REQUISITION_MISMATCH_CHECK_Result> listForClientBoxRequisition = new List<GET_BOX_LIST_FOR_REQUISITION_MISMATCH_CHECK_Result>();
            List<GET_ACCOUNT_LIST_FOR_REQUISITION_MISMATCH_CHECK_Result> listForClientAccountRequisition = new List<GET_ACCOUNT_LIST_FOR_REQUISITION_MISMATCH_CHECK_Result>();
            List<GET_CHALLAN_LIST_FOR_REQUISITION_MISMATCH_CHECK_Result> listForClientChallanRequisition = new List<GET_CHALLAN_LIST_FOR_REQUISITION_MISMATCH_CHECK_Result>();



            using (DBEntities db = new DBEntities())
            {
                listForClientBoxRequisition = db.GET_BOX_LIST_FOR_REQUISITION_MISMATCH_CHECK(CLIENT_NO, TRN_TYPE_NO, myStringList).ToList();
                listForClientAccountRequisition = db.GET_ACCOUNT_LIST_FOR_REQUISITION_MISMATCH_CHECK(CLIENT_NO, TRN_TYPE_NO, myStringList).ToList();
                listForClientChallanRequisition = db.GET_CHALLAN_LIST_FOR_REQUISITION_MISMATCH_CHECK(CLIENT_NO, TRN_TYPE_NO, myStringList).ToList();
            }

            if (listForClientChallanRequisition == null)
            {
                listForClientChallanRequisition = new List<GET_CHALLAN_LIST_FOR_REQUISITION_MISMATCH_CHECK_Result>();
            }

            if (listForClientBoxRequisition == null)
            {
                listForClientBoxRequisition = new List<GET_BOX_LIST_FOR_REQUISITION_MISMATCH_CHECK_Result>();
            }

            if (listForClientAccountRequisition == null)
            {
                listForClientAccountRequisition = new List<GET_ACCOUNT_LIST_FOR_REQUISITION_MISMATCH_CHECK_Result>();
            }

            if (REQ_TYPE_NO == 1)
            {
                return View("ShowChallanDetails", listForClientChallanRequisition);
            }
            else if (REQ_TYPE_NO == 2)
            {
                return View("ShowBoxDetails", listForClientBoxRequisition);
            }
            else if (REQ_TYPE_NO == 4)
            {
                return View("ShowAccountDetails", listForClientAccountRequisition);

            }
            return View();
        }

        public ActionResult StatusList()
        {
            int REQ_TYPE_NO = Convert.ToInt32(Session["REQ_TYPE_NO"].ToString());

            List<GET_CHALLAN_COUNT_FOR_REQUISITION_MISMATCH_CHECK_Result> ChallanStatusList = new List<GET_CHALLAN_COUNT_FOR_REQUISITION_MISMATCH_CHECK_Result>();
            List<GET_BOX_COUNT_FOR_REQUISITION_MISMATCH_CHECK_Result> BoxStatusList = new List<GET_BOX_COUNT_FOR_REQUISITION_MISMATCH_CHECK_Result>();
            List<GET_ACCOUNT_COUNT_FOR_REQUISITION_MISMATCH_CHECK_Result> AccountStatusList = new List<GET_ACCOUNT_COUNT_FOR_REQUISITION_MISMATCH_CHECK_Result>();


            ChallanStatusList =
                Session["list_For_Challan_Requisition"] as List<GET_CHALLAN_COUNT_FOR_REQUISITION_MISMATCH_CHECK_Result>;
            BoxStatusList =
                Session["list_For_Box_Requisition"] as List<GET_BOX_COUNT_FOR_REQUISITION_MISMATCH_CHECK_Result>;

            AccountStatusList =
               Session["list_For_Account_Requisition"] as List<GET_ACCOUNT_COUNT_FOR_REQUISITION_MISMATCH_CHECK_Result>;

            if (REQ_TYPE_NO == 1)
            {
                return View("ChallanStatusListByTRNType", ChallanStatusList);
            }
            else if (REQ_TYPE_NO == 2)
            {
                return View("BoxStatusListByTRNType", BoxStatusList);
            }
            else if (REQ_TYPE_NO == 4)
            {
                return View("AccountStatusListByTRNType", AccountStatusList);  
            }
            return View();
        }
    }
}
