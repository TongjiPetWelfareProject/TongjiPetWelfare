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
    public class LikePetManager
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Limitrow"></param>
        /// <param name="Orderby"></param>
        public static DataTable ShowLikePet(int Limitrow = -1, string Orderby = null)
        {
            DataTable dt = LikePetServer.LikePetInfo(Limitrow, Orderby);
            //调试用
            Util.DebugTable(dt);
            return dt;
        }
        public static void GiveALike(string UID, string PID)
        {
            bool dt = LikePetServer.GetLikePetEntry(UID, PID);
            //调试用
            if (!dt)
            {
                LikePetServer.InsertLikePet(UID, PID);
                Console.WriteLine($"{UID} gives a like to {PID}."); // 输出点赞信息
            }
            else
            {
                LikePetServer.DeleteLikePet(UID, PID);
                Console.WriteLine($"{UID} undo a like to {PID}."); // 输出点赞信息
            }
        }
        public static int IfLike(string UID, string PID)
        {
            bool dt = LikePetServer.GetLikePetEntry(UID, PID);
            if (!dt)
            {
                return -1;//未点赞
            }
            else
            {
                return 1;//已点赞
            }
        }
    }
}
