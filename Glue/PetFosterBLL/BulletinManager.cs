using PetFoster.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetFoster.BLL
{
    public class BulletinManager
    {
        //搜索公告 最新
        public static void ShowBulletins(string text)
        {
            DataTable dt = BulletinServer.BulletinInfo(text);
            //调试用
            foreach (DataColumn column in dt.Columns)
            {
                Console.Write("{0,-20}", column.ColumnName);
            }
            Console.WriteLine();

            foreach (DataRow row in dt.Rows)
            {
                foreach (var item in row.ItemArray)
                {
                    Console.Write("{0,-20}", item);
                }
                Console.WriteLine();
            }
        }
        //公告详细界面
        public static void ShowBulletinVerbose(string BID)
        {
            PetFoster.Model.Bulletin target = BulletinServer.SelectBulletin(BID);
        }
    }
}
