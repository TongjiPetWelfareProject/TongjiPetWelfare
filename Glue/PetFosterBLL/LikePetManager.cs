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
            foreach (DataColumn column in dt.Columns)
            {
                Console.Write("{0,-15}", column.ColumnName);
            }
            Console.WriteLine();

            foreach (DataRow row in dt.Rows)
            {
                foreach (var item in row.ItemArray)
                {
                    Console.Write("{0,-15}", item);
                }
                Console.WriteLine();
            }
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
    }
}
