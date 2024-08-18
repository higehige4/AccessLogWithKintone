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
    public class LicenseListModel : AbskintoneModel
    {
        [kintoneItem(isUpload = false, isKey = true)]
        public override string record_id { get; set; }
        
        [kintoneItem()]
        public string NUMBER { get; set; }
        
        [kintoneItem()]
        public string NAME { get; set; }

        [kintoneItem()]
        public DateTime LICENSE_START{ get; set; }

        [kintoneItem()]
        public DateTime LICENSE_END { get; set; }

        [kintoneItem()]
        public string LICENSE_NUMBER { get; set; }

        [kintoneItem()]
        public string IDENTIFICATION { get; set; }

        [kintoneItem()]
        public string GROUP{ get; set; }


        private string _app = ConfigurationManager.AppSettings["AppID_LicenseList"];
        public override string app
        {
            get { return _app; }
        }
    }
}
