using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Objects;
using System.Data.OleDb;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DMS_WEB.Models;

namespace DMS_WEB.Controllers
{
    public class AccountStatusBulkCheckController : Controller
    {
        //
        // GET: /AccountStatusBulkCheck/

        public ActionResult FileUpload()
        {

            Session["sess_bulk_account_list"] = null; 

            List<SET_CLIENTS_GET_Result> client_list = new List<SET_CLIENTS_GET_Result>();

            using (DBEntities db = new DBEntities())
            {
                client_list = db.SET_CLIENTS_GET(null, true).ToList();
            }

            if (client_list == null)
            {
                client_list = new List<SET_CLIENTS_GET_Result>();
            }

            ViewBag.CLIENT_NO = new SelectList(client_list, "CLIENT_NO", "CLIENT_NAME");
            return View();
        }

        [HttpPost]
        public ActionResult FileUpload(FormCollection frm)
        {
            short CLIENT_NO = short.Parse(frm["CLIENT_NO"].ToString());

            string UploadPath = ConfigurationManager.AppSettings["UploadPath"];

            List<SET_CLIENTS_GET_Result> client_list = new List<SET_CLIENTS_GET_Result>();
            List<SET_ACCOUNT_STATUS_CHECK_BULK_Result> accountStatus_list = new List<SET_ACCOUNT_STATUS_CHECK_BULK_Result>();

            using (DBEntities db = new DBEntities())
            {
                client_list = db.SET_CLIENTS_GET(null, true).ToList();
            }

            if (client_list == null)
            {
                client_list = new List<SET_CLIENTS_GET_Result>();
            }

            ViewBag.CLIENT_NO = new SelectList(client_list, "CLIENT_NO", "CLIENT_NAME", CLIENT_NO);

            DataSet ds = new DataSet();

            HttpPostedFileBase file = Request.Files["txtFile"];

            if (file.ContentLength > 0)
            {
                string fileExtension = System.IO.Path.GetExtension(file.FileName);

                if (fileExtension == ".xls" || fileExtension == ".xlsx")
                {
                    string fileLocation = Server.MapPath(UploadPath + file.FileName) + file.FileName;
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
                }

                ArrayList myArrayList = new ArrayList();

                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    myArrayList.Add(ds.Tables[0].Rows[i][0].ToString());
                }

                Session["sess_bulk_account_list"] = myArrayList;
             
                using (DBEntities db = new DBEntities())
                {
                    string myString = String.Join(",", myArrayList.ToArray());

                    accountStatus_list = db.SET_ACCOUNT_STATUS_CHECK_BULK(CLIENT_NO, myString).ToList();

                    Session["sess_account_status_list"] = accountStatus_list;
                }

                if (accountStatus_list == null)
                {
                    accountStatus_list = new List<SET_ACCOUNT_STATUS_CHECK_BULK_Result>();
                }
            }
            return View("List", accountStatus_list);

        }

        public ActionResult ShowBulkDetails(short TRN_TYPE_NO = 0)
        {
            
            ArrayList myArrayList = new ArrayList();
            myArrayList = (ArrayList) Session["sess_bulk_account_list"];

            List<SET_ACCOUNT_STATUS_CHECK_BULK_BY_TRN_TYPE_NO_Result> accountStatusListByTrnTypeNo = new List<SET_ACCOUNT_STATUS_CHECK_BULK_BY_TRN_TYPE_NO_Result>();

            using (DBEntities db = new DBEntities())
            {
                string myString = String.Join(",", myArrayList.ToArray());
                accountStatusListByTrnTypeNo = db.SET_ACCOUNT_STATUS_CHECK_BULK_BY_TRN_TYPE_NO(TRN_TYPE_NO, myString).ToList();
            }

            if (accountStatusListByTrnTypeNo == null)
            {
                accountStatusListByTrnTypeNo = new List<SET_ACCOUNT_STATUS_CHECK_BULK_BY_TRN_TYPE_NO_Result>();
            }

            return View("ShowBulkDetails", accountStatusListByTrnTypeNo);
        }


        public ActionResult AccountStatusList()
        {

            List<SET_ACCOUNT_STATUS_CHECK_BULK_Result> accountStatusList_Bulk = new List<SET_ACCOUNT_STATUS_CHECK_BULK_Result>();

            accountStatusList_Bulk =
                Session["sess_account_status_list"] as List<SET_ACCOUNT_STATUS_CHECK_BULK_Result>;

            return View("AccountListByTRNType", accountStatusList_Bulk);
        }

    }
}
