using Glue.PetFoster.Model;
using Microsoft.Extensions.Configuration;
using Oracle.ManagedDataAccess.Client;
using PetFoster.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetFoster.DAL
{
    public class ForumPostServer
    {
        public static string conStr = AccommodateServer.conStr;
        public static DataTable SelectPost(string PID)
        {
            DataTable dataTable = new DataTable();
            string query = $"SELECT * from verbosepost where post_id={PID}";
            return DBHelper.ShowInfo(query);
        }
        /// <summary>
        /// 展示所有帖子的详细信息，用于管理员端审核,注意用post_id区分发帖者的ID，并进行相应的封号等操作，而不是user_name
        /// </summary>
        /// <param name="Limitrows"></param>
        /// <param name="Orderby"></param>
        /// <param name="beingcensored">true表示只显示未审核帖子，否则显示全部帖子</param>
        /// <returns></returns>
        public static DataTable UncensoredForum(decimal Limitrows = -1, string Orderby = null,bool beingcensored=true)
        {
            DataTable dataTable = new DataTable();
            string query = "SELECT post_id,user_name,post_contents,post_time FROM forum_posts" +
                    " natural join user2 ";
            if (Limitrows > 0)
                query += $" where rownum<={Limitrows} ";
            else if (beingcensored)
                query += $" where censored='N'";
            if (Limitrows > 0 && beingcensored)
                query += $" and censored='N'";
            if ((Orderby) != null)
                query += $" order by {Orderby} desc";
            return DBHelper.ShowInfo(query);
        }
        public static int InsertPost(string UID, string heading,string contents)
        {
            try
            {
                using (OracleConnection connection = new OracleConnection(conStr))
                {
                    connection.Open();
                    OracleCommand command = connection.CreateCommand();
                    command.CommandType = CommandType.Text;
                    command.CommandText = "INSERT INTO forum_posts (post_id, user_id,heading, post_contents)" +
                        $"VALUES (post_id_seq.NEXTVAL,{UID},'{heading}','{contents}')";
                    try
                    {
                        command.ExecuteNonQuery();
                        command.CommandText = "SELECT post_id_seq.CURRVAL FROM DUAL";
                        int PostID = Convert.ToInt32(command.ExecuteScalar());
                        return PostID;
                    }
                    catch (OracleException ex)
                    {
                        Console.WriteLine("错误码" + ex.ErrorCode.ToString());
                        return -1;
                    }

                }
            }
            catch (Exception ex)
            {
                // 处理异常
                Console.WriteLine(ex.ToString());
                return -1;
            }
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
        public static void ReadForum(string FID)
        {
            // 更改信息
            try
            {
                using (OracleConnection connection = new OracleConnection(conStr))
                {
                    connection.Open();
                    OracleCommand command = connection.CreateCommand();
                    command.CommandType = CommandType.Text;
                    command.CommandText = "UPDATE forum_posts SET read_count=read_count+1" +
                            $" where post_id={FID}";
                    try
                    {
                        command.ExecuteNonQuery();
                        Console.WriteLine($"{FID}的阅读量+1!");
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
        public static string GetContent(string FID)
        {
            string getContentQuery = $"SELECT post_contents FROM forum_posts WHERE Post_ID = {FID}";

            // 获取对应的帖子内容
            using (OracleConnection connection = new OracleConnection(conStr))
            {
                using (OracleCommand getContentCommand = new OracleCommand(getContentQuery, connection))
                {
                    connection.Open();
                    return getContentCommand.ExecuteScalar()?.ToString();
                }
            }
        }
        public static List<string> GetPosts(string UID)
        {
            List<string> postIDs = new List<string>();
            string getUserIdQuery = $"SELECT Post_ID FROM forum_posts WHERE User_ID = {UID}";

            using (OracleConnection connection = new OracleConnection(conStr))
            {
                using (OracleCommand getUserIdCommand = new OracleCommand(getUserIdQuery, connection))
                {
                    connection.Open();

                    using (OracleDataReader reader = getUserIdCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string postID = reader["Post_ID"].ToString();
                            postIDs.Add(postID);
                        }
                    }
                }
            }

            return postIDs;
        }

        public static List<ForumPost> GetAllPosts()
        {
            List<ForumPost> posts = new List<ForumPost>();

            using (OracleConnection connection = new OracleConnection(conStr))
            {
                connection.Open();

                string query = "SELECT * FROM forum_posts";

                OracleCommand command = new OracleCommand(query, connection);

                using (OracleDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        ForumPost forumpost = new ForumPost
                        {
                            PostId = reader["post_id"].ToString(),
                            UserId = reader["user_id"].ToString(),
                            Post_time = Convert.ToDateTime(reader["post_time"]),
                            Content = reader["post_contents"].ToString(),
                            ReadCount = Convert.ToInt32(reader["read_count"]),
                            LikeNum = Convert.ToInt32(reader["like_num"]),
                            CommentNum = Convert.ToInt32(reader["comment_num"]),
                            Heading = reader["heading"].ToString()
                        };

                        posts.Add(forumpost);
                    }
                }

                connection.Close();
            }

            return posts;
        }
    }
}
