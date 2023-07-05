using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetFoster.DAL
{
    public class ForumPostServer
    {
        public static string user = "\"C##PET\"";
        public static string pwd = "campus";
        public static string db = "localhost:1521/orcl";
        private static string conStr = "User Id=" + user + ";Password=" + pwd + ";Data Source=" + db + ";"; // 替换为实际的数据库连接字符串
        /// <summary>
        /// 展示所有帖子的详细信息，通过Named Param.（问ChatGPT）实现
        /// </summary>
        /// <param name="Limitrows"></param>
        /// <param name="Orderby"></param>
        /// <param name="beingcensored">true表示只显示未审核帖子，否则显示全部帖子</param>
        /// <returns></returns>
        public static DataTable UncensoredForum(decimal Limitrows = -1, string Orderby = null,bool beingcensored=true)
        {
            DataTable dataTable = new DataTable();
            using (OracleConnection connection = new OracleConnection(conStr))
            {
                connection.Open();

                string query = "SELECT * FROM forum_posts ";
                if (Limitrows > 0)
                    query += $" where rownum<={Limitrows} ";
                else if (beingcensored)
                    query += $" where censored='N'";
                if (Limitrows >0&& beingcensored)
                    query += $" and censored='N'";
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
        /// 
        /// </summary>
        /// <param name="FID">帖子的post_id</param>
        /// <param name="censored">true表示审核通过</param>
        public static void UpdateForum(string FID,string post_contents=null,bool censored=false)
        {
            // 更改信息
            try
            {
                using (OracleConnection connection = new OracleConnection(conStr))
                {
                    connection.Open();
                    OracleCommand command = connection.CreateCommand();
                    command.CommandType = CommandType.Text;
                    command.Parameters.Clear();
                    if (post_contents != null)
                    {
                        command.CommandText = "UPDATE forum_posts SET post_contents=:post_contents,post_time=CURRENT_TIMESTAMP" +
                            " where post_id= :post_id";
                        command.Parameters.Add("post_contents", OracleDbType.Varchar2, post_contents, ParameterDirection.Input);
                    }
                    if(censored)
                        command.CommandText = "UPDATE forum_posts SET censored='Y'" +
                            " where post_id= :post_id";
                    command.Parameters.Add("post_id", OracleDbType.Varchar2, FID, ParameterDirection.Input);
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch (OracleException ex)
                    {
                        Console.WriteLine("错误码" + ex.ErrorCode.ToString());

                        throw;
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
        /// 注意在删除后，还需要用UserManager.Banned函数将相应用户“Warning Issued”，
        /// UID可以通过本类的GetUID获取
        /// </summary>
        /// <param name="FID">要删除的帖子ID</param>
        /// <returns>是否删除成功</returns>
        public static bool DeleteForum(string FID)
        {
            using (OracleConnection connection = new OracleConnection(conStr))
            {
                // 执行删除操作
                connection.Open();
                OracleCommand command = connection.CreateCommand();
                command.CommandType = CommandType.Text;
                command.CommandText = "delete from forum_posts where Post_ID= :Post_ID";
                command.Parameters.Clear();
                command.Parameters.Add("Post_ID", OracleDbType.Varchar2, FID, ParameterDirection.Input);
                try
                {
                    command.ExecuteNonQuery();
                    connection.Close();
                    return true;
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
        }
        public static string GetUID(string FID)
        {
            string getUserIdQuery = "SELECT User_ID FROM forum_posts WHERE Post_ID = :Post_ID";

            // 获取被删除行的 UserID
            using (OracleConnection connection = new OracleConnection(conStr))
            {
                using (OracleCommand getUserIdCommand = new OracleCommand(getUserIdQuery, connection))
                {
                    getUserIdCommand.Parameters.Add(":Post_ID", FID);

                    // 执行查询并获取结果
                    string deletedUserID = getUserIdCommand.ExecuteScalar()?.ToString();
                    return deletedUserID;
                }
            }
        }
    }
}
