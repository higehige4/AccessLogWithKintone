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
    public class GroupModel : AbskintoneModel
    {
        [kintoneItem(isUpload = false, isKey = true)]
        public override string record_id { get; set; }

        [kintoneItem()]
        public string 課 { get; set; } 

        private string _app = ConfigurationManager.AppSettings["AppID_Group"];
        public override string app
        {
            get { return _app; }
        }
    }
}
