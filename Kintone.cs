using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp3
{
    class Kintone
    {
        private List<GroupModel> groups;

        Kintone()
        {
            groups = GroupModel.FindAll<GroupModel>();

        }

        public List<GroupModel> GetAll()
        {

            return groups;
        }


    }
}
