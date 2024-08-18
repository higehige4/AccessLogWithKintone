using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using kintoneDotNET.API;
using kintoneDotNET.API.Types;
using System.Configuration;

namespace WindowsFormsApp3
{
    public class InoutListModel : AbskintoneModel
    {
        [kintoneItem(isUpload = false, isKey = true)]
        public override string record_id { get; set; }

        [kintoneItem()]
        public string GROUP { get; set; }

        [kintoneItem()]
        public string NUMBER { get; set; }
        
        [kintoneItem()]
        public string NAME { get; set; }

        [kintoneItem()]
        public string REASON { get; set; }

        [kintoneItem()]
        public DateTime IN_DATE { get; set; }

        [kintoneItem(FieldType = "TIME")]
        public DateTime IN_TIME { get; set; }

        [kintoneItem()]
        public DateTime OUT_DATE { get; set; }

        [kintoneItem(FieldType = "TIME")]
        public DateTime OUT_TIME { get; set; }

        [kintoneItem(FieldType = "TIME")]
        public DateTime REST_START { get; set; }

        [kintoneItem(FieldType = "TIME")]
        public DateTime RSET_END { get; set; }

        [kintoneItem(FieldType = "TIME")]
        public DateTime RSET_TIME { get; set; }

        [kintoneItem()]
        public string CHECK { get; set; }

        private string _app = ConfigurationManager.AppSettings["AppID_InoutList"];
        public override string app
        {
            get { return _app; }
        }
    }
}
