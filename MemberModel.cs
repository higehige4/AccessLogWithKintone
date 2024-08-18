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
    public class MemberModel : AbskintoneModel
    {
        [kintoneItem(isUpload = false, isKey = true)]
        public override string record_id { get; set; }

        [kintoneItem()]
        public string GROUP { get; set; }

        [kintoneItem()]
        public string NAME { get; set; }

        [kintoneItem()]
        public string NUMBER { get; set; }


        private string _app = ConfigurationManager.AppSettings["AppID_Member"];
        public override string app
        {
            get { return _app; }
        }
    }
}
