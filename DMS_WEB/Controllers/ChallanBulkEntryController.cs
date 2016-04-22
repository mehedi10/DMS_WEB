using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DMS_WEB.Models;

namespace DMS_WEB.Controllers
{
    public class ChallanBulkEntryController : BaseController
    {
        //
        // GET: /ChallanBulkEntry/

        public ActionResult FileUpload()
        {
            this.LoadCommonJsonData();
            this.LoadSecurity();
            return View();
        }

        [HttpPost]
        public ActionResult FileUpload(FormCollection frm)
        {
            short CLIENT_NO = short.Parse(frm["CLIENT_NO"].ToString());
            short DEPT_NO = short.Parse(frm["DEPT_NO"].ToString());
            string CHALLAN_ID = frm["CHALLAN_ID"];
            DateTime CHALLAN_DATE = Convert.ToDateTime(frm["CHALLAN_DATE"].ToString());

            string UploadPath = ConfigurationManager.AppSettings["UploadPath"];

            this.LoadSecurity();
            this.LoadCommonJsonData();
            //List<SET_CLIENTS_GET_Result> client_list = new List<SET_CLIENTS_GET_Result>();

            //using (DBEntities db = new DBEntities())
            //{
            //    client_list = db.SET_CLIENTS_GET(null, true).ToList();
            //}

            //if (client_list == null)
            //{
            //    client_list = new List<SET_CLIENTS_GET_Result>();
            //}

            //ViewBag.CLIENT_NO = new SelectList(client_list, "CLIENT_NO", "CLIENT_NAME", CLIENT_NO);

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

                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    //String V = ds.Tables[0].Rows[i][0].ToString();

                    //String V2 = ds.Tables[0].Rows[i][2].ToString();
                    //String V3 = ds.Tables[0].Rows[i][3].ToString();
                    //String V4 = ds.Tables[0].Rows[i][4].ToString();
                    //String V5 = ds.Tables[0].Rows[i][5].ToString();
                    //String V6 = ds.Tables[0].Rows[i][6].ToString();
                    //String V7 = ds.Tables[0].Rows[i][7].ToString();
                    //String V8 = ds.Tables[0].Rows[i][8].ToString();
                    //String V9 = ds.Tables[0].Rows[i][9].ToString();
                    //String V1 = ds.Tables[0].Rows[i][1].ToString();

                    //DateTime REC_DATE = Convert.ToDateTime(ds.Tables[0].Rows[i][1]);

                    using (DBEntities db = new DBEntities())
                    {
                        db.SET_CHALLAN_BULK_INSERT(CLIENT_NO, DEPT_NO, ds.Tables[0].Rows[i][2].ToString(),
                            ds.Tables[0].Rows[i][3].ToString(),
                            ds.Tables[0].Rows[i][4].ToString(), ds.Tables[0].Rows[i][5].ToString(),
                            ds.Tables[0].Rows[i][6].ToString(), ds.Tables[0].Rows[i][7].ToString(),
                            ds.Tables[0].Rows[i][8].ToString(), CHALLAN_ID,
                            Convert.ToDateTime(ds.Tables[0 ].Rows[i][1]), CHALLAN_DATE
                            , this.USER_NO, this.LOGON_NO);
                    }
                }


            }
            return View();
        }

    }
}
