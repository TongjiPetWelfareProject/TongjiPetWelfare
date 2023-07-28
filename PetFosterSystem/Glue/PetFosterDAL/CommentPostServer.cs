﻿using Oracle.ManagedDataAccess.Client;
using PetFoster.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetFoster.DAL
{
    public class CommentPostServer
    {
        public static string user = "\"C##PET\"";
        public static string pwd = "campus";
        public static string db = "localhost:1521/orcl";
        private static string conStr = "User Id=" + user + ";Password=" + pwd + ";Data Source=" + db + ";"; // 替换为实际的数据库连接字符串
        /// <summary>
        /// 注意SQL中EXTRACT的用法，这是给管理员看的，个人点赞是GetLikePetEntry...
        /// </summary>
        /// <param name="Limitrows"></param>
        /// <param name="Orderby"></param>
        /// <returns></returns>
        public static DataTable CommentPostInfo(decimal Limitrows = -1, string Orderby = null, string UID = "-1", string PID = "-1")
        {
            DataTable dataTable = new DataTable();
            using (OracleConnection connection = new OracleConnection(conStr))
            {
                connection.Open();
                string query = "";
                if (UID == "-1" && PID == "-1")
                {
                    query = "SELECT user_id,post_id,TO_CHAR(comment_time,'YYYY-MM-DD') as comment_date, TO_CHAR(comment_time,'HH24:MI:SS')as commented_time,comment_contents  FROM comment_post";
                }
                else if (PID != "-1" && UID == "-1")
                {
                    query = $"SELECT user_id,post_id,TO_CHAR(comment_time,'YYYY-MM-DD') as comment_date, TO_CHAR(comment_time,'HH24:MI:SS')as commented_time,comment_contents  FROM comment_post where Post_ID={PID}";
                }
                else if (PID == "-1" && UID != "-1")
                {
                    query = $"SELECT user_id,post_id,TO_CHAR(comment_time,'YYYY-MM-DD') as comment_date, TO_CHAR(comment_time,'HH24:MI:SS')as commented_time,comment_contents  FROM comment_post where User_ID={UID}";
                }
                else
                {
                    query = $"SELECT user_id,post_id,TO_CHAR(comment_time,'YYYY-MM-DD') as comment_date, TO_CHAR(comment_time,'HH24:MI:SS')as commented_time,comment_contents  FROM comment_post where Post_ID={PID} and User_ID={UID}";
                }
                if (Limitrows > 0)
                    query += $" where rownum<={Limitrows} ";
                if ((Orderby) != null)
                    query += $" order by {Orderby} desc";

                OracleCommand command = new OracleCommand(query, connection);

                OracleDataAdapter adapter = new OracleDataAdapter(command);

                adapter.Fill(dataTable);

                connection.Close();
            }

            Console.ReadLine();
            return dataTable;
        }
        /// <summary>
        /// 点赞
        /// </summary>
        /// <param name="UID"></param>
        /// <param name="PID"></param>
        public static void InsertCommentPost(string UID, string PID, string content)
        {
            // 添加新行
            try
            {
                using (OracleConnection connection = new OracleConnection(conStr))
                {
                    connection.Open();
                    OracleCommand command = connection.CreateCommand();
                    command.CommandType = CommandType.Text;
                    command.CommandText = "INSERT INTO comment_post (user_id, post_id,comment_contents) " +
                        "VALUES (:user_id,:post_id,:comment_contents)";
                    command.Parameters.Clear();
                    command.Parameters.Add("user_id", OracleDbType.Varchar2, UID, ParameterDirection.Input);
                    command.Parameters.Add("post_id", OracleDbType.Varchar2, PID, ParameterDirection.Input);
                    command.Parameters.Add("comment_contents", OracleDbType.Varchar2, content, ParameterDirection.Input);

                    try
                    {
                        command.ExecuteNonQuery();
                        Console.WriteLine($"{UID}给{PID}在{DateTime.Now}评论");

                    }
                    catch (OracleException ex)
                    {
                        Console.WriteLine("不存在的用户或宠物");
                    }
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                // 处理异常
                Console.WriteLine(ex.ToString());
            }
        }
        /// <summary>
        /// 只有存在条目才能删除
        /// </summary>
        /// <param name="UID"></param>
        /// <param name="PID"></param>
        /// <returns></returns>
        public static void DeleteCommentPost(string UID, string PID)
        {
            using (OracleConnection connection = new OracleConnection(conStr))
            {
                // 执行删除操作
                connection.Open();
                OracleCommand command = connection.CreateCommand();
                command.CommandType = CommandType.Text;
                command.CommandText = "delete from comment_post where Post_ID= :Post_ID and User_ID=:User_ID";
                command.Parameters.Clear();
                command.Parameters.Add("Post_ID", OracleDbType.Varchar2, PID, ParameterDirection.Input);
                command.Parameters.Add("User_ID", OracleDbType.Varchar2, UID, ParameterDirection.Input);
                try
                {
                    command.ExecuteNonQuery();
                    Console.WriteLine($"{UID}给{PID}的点赞已取消");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"不存在{UID}给{PID}的点赞");
                }
            }
        }
    }
}