using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DMS_WEB.Models {

    #region Search Model
    
    public partial class SearchObjectBase {
        private int _START_INDEX;
        public int START_INDEX {
            get { return this._START_INDEX; }
            set { this._START_INDEX = value; }
        }

        private int _END_INDEX;
        public int END_INDEX {
            get { return this._END_INDEX; }
            set { this._END_INDEX = value; }
        }

        private int _PAGE_NUMBER;
        public int PAGE_NUMBER {
            get { return this._PAGE_NUMBER; }
            set { this._PAGE_NUMBER = value; }
        }

        public SearchObjectBase() {
            this._START_INDEX = 1;
            this._END_INDEX = 10;
            this._PAGE_NUMBER = 1;
        }


        public long? TotalRecords { get; set; }
    }

    public partial class DepartmentSearch : SearchObjectBase {
        public short? DEPT_NO { get; set; }
        public short? CLIENT_NO { get; set; }
        public string DEPT_NAME { get; set; }
    }

    public partial class TransmittalSearch : SearchObjectBase {
        public long? TRANSMIT_NO { get; set; }
        public byte? TRN_TYPE_NO { get; set; }
        public short? CLIENT_NO { get; set; }
        public short? DEPT_NO { get; set; }
        public string TRANSMIT_ID { get; set; }

    }

    public partial class ChallanSearch : SearchObjectBase {
        public int? CHALLAN_NO { get; set; }
        public short? CLIENT_NO { get; set; }
        public short? DEPT_NO { get; set; }
        public string CHALLAN_ID { get; set; }
        public byte? TRN_TYPE_NO { get; set; }
        public byte? REQ_TYPE_NO { get; set; }
        public long? TRANSMIT_NO { get; set; }
        public bool? IS_OLD_DATA { get; set; }

    }

    public partial class ClientBoxesSearch : SearchObjectBase {
        public int? BOX_NO { get; set; }        
        public short? CLIENT_NO { get; set; }
        public short? DEPT_NO { get; set; }
        public string CLIENT_BOX_ID { get; set; }
        public byte? PROD_TYPE_NO { get; set; }
        public byte? TRN_TYPE_NO { get; set; }
        public byte? REQ_TYPE_NO { get; set; }
        public long? TRANSMIT_NO { get; set; }
        public bool? IS_OLD_DATA { get; set; }
        public int? CHALLAN_NO { get; set; }
        public int? RECALL_BOX_NO { get; set; }
    }

    public partial class RecallBoxSearch : SearchObjectBase {
        public int? RECALL_BOX_NO { get; set; }
        public short? CLIENT_NO { get; set; }
        public short? DEPT_NO { get; set; }
        public string RECALL_BOX_ID { get; set; }
    }

    public partial class ClientBatchSearch : SearchObjectBase {
        public int? BATCH_NO { get; set; }
        public string BATCH_ID { get; set; }
        public int? BOX_NO { get; set; }
        public string CLIENT_BOX_ID { get; set; }
        public short? CLIENT_NO { get; set; }
        public DateTime? ARCHIVE_DATE { get; set; }
        public byte? TRN_TYPE_NO { get; set; }
        public byte? REQ_TYPE_NO { get; set; }
        public long? TRANSMIT_NO { get; set; }
        public bool? IS_OLD_DATA { get; set; }
    }

    public partial class ClientAccountSearch : SearchObjectBase { 
        public long? ACC_NO { get; set; }
        public short? CLIENT_NO { get; set; }
        public short? DEPT_NO { get; set; }
        public string CLIENT_BOX_ID { get; set; }
        public int? BATCH_NO { get; set; }
        public string BATCH_ID { get; set; }
        public DateTime? ARCHIVE_DATE { get; set; }
        public string ACC_ID { get; set; }
        public byte? TRN_TYPE_NO { get; set; }
        public byte? REQ_TYPE_NO { get; set; }
        public long? TRANSMIT_NO { get; set; }
        public bool? IS_OLD_DATA { get; set; }

    }

    public partial class AccountStatusSearch : SearchObjectBase
    {
        public short? CLIENT_NO { get; set; }
        public short? REQ_TYPE_NO { get; set; }
        public string ACC_ID { get; set; }
    }

    public  partial class  UserSearch : SearchObjectBase
    {
        public  int? USER_NO { get; set; }
        public short? USER_TYPE_NO { get; set; }
        public  string LOGIN_ID { get; set; }
        public string LOGIN_PASSWORD { get; set; }
        public bool? IS_ACTIVE { get; set; }
        public bool? IS_LOCKED { get; set; }
        public short? CLIENT_NO { get; set; }
    }
    #endregion

    #region View Model 

    public partial class AccountDocumentViewModel {
        public SET_CLIENT_ACCOUNTS_GET_DETAILS_Result account_info { get; set; }
        public SET_CLIENT_ACC_DOCS_GET_Result document_info { get; set; }
        public List<SET_CLIENT_ACC_DOCS_GET_Result> document_list { get; set; }
    }

    public partial class AccountTransmitalHistoryViewModel
    {
        public SET_CLIENT_ACCOUNTS_GET_DETAILS_Result account_info { get; set; }
        public List<GET_ACCOUNT_TRANSMIT_HISTORY_Result> transmit_history_list { get; set; }
    }

    public partial class TransmitalDetailsViewModel
    {
        public List<GET_CLIENT_BOX_TRANSMIT_DETAIL_BY_TRANSMIT_ID_Result> client_box_transmit_history_details { get; set; }
        public List<GET_CLIENT_ACCOUNT_TRANSMIT_DETAIL_BY_TRANSMIT_ID_Result> client_account_transmit_history_details { get; set; }
        public List<GET_CLIENT_ACCOUNT_DOC_TRANSMIT_DETAIL_BY_TRANSMIT_ID_Result> client_account_document_transmit_history_details { get; set; }
    }

    public partial class AccountDocumentTransmitalHistoryViewModel
    {
        public SET_CLIENT_ACC_DOCS_GET_DETAILS_Result account_doc_info { get; set; }
        public List<GET_ACCOUNT_DOC_TRANSMIT_HISTORY_Result> transmit_history_list { get; set; }
    }

    public partial class ClientBoxTransmitalHistoryViewModel
    {
        public SET_CLIENT_DEPT_BOXES_GET_Result client_box_info { get; set; }
        public List<GET_CLIENT_BOX_TRANSMIT_HISTORY_Result> transmit_history_list { get; set; }
    }

    public partial class AddClientBoxViewModel
    {
        public SET_CLIENT_DEPT_RECALL_BOXES_GET_DETAILS_Result recall_box_info { get; set; }
        public SET_CLIENT_DEPT_BOXES_GET_Result recall_box_dept_info { get; set; }
        public List<SET_CLIENT_DEPT_BOXES_GET_Result> recall_box_dept_list { get; set; }
    }

    public partial class AccountAttachmentViewModel {
        public SET_CLIENT_ACCOUNTS_GET_DETAILS_Result account_info { get; set; }
        public SET_CLIENT_ACC_DOCS_GET_Result document_info { get; set; }
        public SET_CLIENT_ATTACHS_GET_Result attach_info { get; set; }
        public List<SET_CLIENT_ATTACHS_GET_Result> attach_list { get; set; }
    }

    public partial class Account_Document_Attach_ViewModel {
        public SET_CLIENT_ACCOUNTS_GET_DETAILS_Result account_info { get; set; }
        public List<SET_CLIENT_ACC_DOCS_GET_Result> document_list { get; set; }
        public List<SET_CLIENT_ATTACHS_GET_Result> attach_list { get; set; }
    }

    #endregion

    #region Entry Model
    public partial class ChangePasswordModel {
        public string LOGIN_ID { get; set; }
        public string OLD_PASSWORD { get; set; }
        public string NEW_PASSWORD { get; set; }
        public string RETYPE_PASSWORD { get; set; } 
    }
    #endregion
}