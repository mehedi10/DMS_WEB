using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DMS_WEB.Models {
    public partial class TRN_CHALLANS {
        public string IsHDR { get; set; }
    }


    public partial class SET_RECALL_BOXES {
        private string _RECALL_BOX_LOC;
        public string RECALL_BOX_LOC {
            get {
                return this.LOC_CLIENT_ID + this.LOC_DEPT_ID + this.LOC_ARCH_ID + this.LOC_FLOOR_ID + this.LOC_CHAMBER_ID + this.LOC_ROW_ID + this.LOC_ROW_FACE_ID + this.LOC_BAY_ID + this.LOC_LEVEL_ID + this.LOC_SEQ_ID;
            }
            set {
                this._RECALL_BOX_LOC = value;
            }
        }
    }


    public partial class SET_CLIENT_ATTACHS_GET_Result {
        public short? CLIENT_NO { get; set; } 
        public short? DEPT_NO { get; set; } 
    }
}