using PetFoster.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetFoster.BLL
{
    public class ForumPostManager
    {
        public static void ShowForumProfile(int Limitrow = -1, string Orderby = null)
        {
            DataTable dt = ForumPostServer.UncensoredForum(Limitrow, Orderby);
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
        }
        public static void UpdateForumProfile(string FID)
        {
            int id;
            if (int.TryParse(FID, out id))
            {
                // FID 全为数字，执行相应的逻辑
                ForumPostServer.UpdateForum(FID);
            }
            else
            {
                // FID 不全为数字，执行相应的错误处理逻辑
                Console.WriteLine("FID is not a valid numeric value.");
            }
        }
        public static void DeleteForumProfile(string FID)
        {
            int id;
            int uid;
            if (int.TryParse(FID, out id))
            {
                // FID 全为数字，执行相应的逻辑
                uid = Convert.ToInt32(ForumPostServer.GetUID(FID));
                ForumPostServer.DeleteForum(FID);
                UserManager.Ban(uid, "Warning Issued");
                Console.WriteLine("Forum profile with FID " + FID + " has been deleted.");
                Console.WriteLine("User with UID " + uid + " has been warned.");
            }
            else
            {
                // FID 不全为数字，执行相应的错误处理逻辑
                Console.WriteLine("FID is not a valid numeric value.");
            }
        }
        /// <summary>
        /// 发布帖子待审阅
        /// </summary>
        /// <param name="UID"></param>
        /// <param name="contents"></param>
        /// <param name="paths"></param>
        public static void PublishPost(string UID,string contents,List<string> paths)
        {
            int FID = ForumPostServer.InsertPost(UID, contents);
            foreach(var path in paths)
            {
                PostImagesServer.InsertImage(FID.ToString(), path);
            }
        }
       
    }
}
