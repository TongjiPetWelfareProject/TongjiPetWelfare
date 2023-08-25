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
        public static int GetCollectNums(string PID)
        {
            return CollectPetInfoServer._GetCollectNums(PID);
        }
        public static void GiveACollect(string UID, string PID, bool is_give)
        {
            bool dt = CollectPetInfoServer.GetCollectPetInfoEntry(UID, PID);
            //调试用
            if (!dt && is_give)
            {
                CollectPetInfoServer.InsertCollectPetInfo(UID, PID);
                Console.WriteLine($"{UID} gives a collect to {PID}."); // 输出收藏信息
            }
            else if(dt && !is_give)
            {
                CollectPetInfoServer.DeleteCollectPetInfo(UID, PID);
                Console.WriteLine($"{UID} undo a collect to {PID}."); // 输出收藏信息
            }
            else if(!dt && !is_give)
            {
                throw new Exception("无效操作：没有收藏过，无法取消");
            }
            else if(dt && is_give)
            {
                throw new Exception("无效操作：已经收藏了，不能再收藏");
            }
            else
            {
                throw new Exception("无效操作");
            }
        }
    }

}
