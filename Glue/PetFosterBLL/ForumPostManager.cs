using PetFoster.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.IO;
using static System.Net.Mime.MediaTypeNames;

namespace PetFoster.BLL
{
    public class Posts
    {
        string contents;
        List<byte[]> images;
        public Posts(string s, List<byte[]> imgs)
        {
            contents = s;
             images= imgs ;
        }
    };
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
        /// <param name="paths">图片路径</param>
        public static void PublishPost(string UID,string contents,List<string> paths)
        {
            //更新帖子
            int FID = ForumPostServer.InsertPost(UID, contents);
            //上传图片（最多五张）
            foreach(var path in paths)
            {
                PostImagesServer.InsertImage(FID.ToString(), path);
            }
        }
        /// <summary>
        /// 获得帖子的图片和内容，图片以二进制形式存储，需要类型转换
        /// </summary>
        /// <param name="UID"></param>
        /// <returns></returns>
        public static List<Posts> GetPosts(string UID)
        {
            List<string> FID = ForumPostServer.GetPosts(UID);
            List<Posts> posts = new List<Posts>();
            foreach (var sFID in FID)
            {
                //获得图片与帖子内容
                string content=ForumPostServer.GetContent(sFID);
                List<byte[]> image = PostImagesServer.GetImages(Convert.ToInt32(sFID));
                Posts tmp = new Posts(content,image);
                posts.Add(tmp);
            }
            return posts;
        }
    }
}
