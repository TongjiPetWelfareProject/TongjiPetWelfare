using PetFoster.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetFoster.BLL
{
    public class CommentPostManager
    {
        public static void ShowCommentPost(int Limitrow = -1, string Orderby = null)
        {
            DataTable dt = CommentPostServer.CommentPostInfo(Limitrow, Orderby);
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
        public static void GiveACommentPost(string UID, string PID, string content)
        {
            //调试用
            CommentPostServer.InsertCommentPost(UID, PID, content);
            Console.WriteLine($"{UID} gives a comment that reads {content} to {PID}."); // 输出点赞信息
        }
        public static void UndoACommentPost(string UID, string PID, DateTime dateTime)
        {
            //调试用
            //CommentPostServer.DeleteCommentPost(UID, PID, dateTime);
            Console.WriteLine($"{UID} undo a post comment to {PID}."); // 输出点赞信息
        }
    }

}
