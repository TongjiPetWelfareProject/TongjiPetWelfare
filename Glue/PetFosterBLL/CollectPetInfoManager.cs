using Glue.PetFoster.BLL;
using PetFoster.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetFoster.BLL
{
    public class CollectPetInfoManager
    {
        public static DataTable ShowCollectPetInfo(int Limitrow = -1, string Orderby = null)
        {
            DataTable dt = CollectPetInfoServer.CollectPetInfo(Limitrow, Orderby);
            //调试用
            Util.DebugTable(dt);
            return dt;
        }
    }
}
