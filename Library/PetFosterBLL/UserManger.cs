using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Oracle.ManagedDataAccess.Client;
using PetFoster.Model;
using static PetFoster.Model.PetData;
using PetFoster.DAL;
using System.IO;
using System.Text.RegularExpressions;

namespace PetFoster.BLL
{
    public class UserManager
    {
        public static string user = "\"C##PET\"";
        public static string pwd = "campus";
        public static string db = "localhost:1521/orcl";
        private static string conStr = "User Id=" + user + ";Password=" + pwd + ";Data Source=" + db + ";"; // 替换为实际的数据库连接字符串
        static bool IsValidStatus(string status)
        {
            // 读取配置文件并加载状态
            List<string> statuses = new List<string>();
            string configFile = "resource/account_status.txt";
            string[] lines = File.ReadAllLines(configFile);
            statuses.AddRange(lines);
            if (statuses.Contains(status))
                return true;
            else
            {
                throw new Exception(status + "状态不合法！");
            }
        }
        public static void ShowUserProfile(int Limitrow = -1, string Orderby = null)
        {
            DataTable dt = UserServer.UserInfo(Limitrow, Orderby);
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
        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="user">用户信息</param>
        /// <returns>是否登陆成功</returns>
        public static bool Login(USER2Row user)
        {
            bool con = false;
            using (OracleConnection connection = new OracleConnection(conStr))
            {
                // 连接对象将在 using 块结束时自动关闭和释放资源
                // 在此块中执行数据操作
                connection.Open();
                OracleCommand command = connection.CreateCommand();
                User Candidate=UserServer.GetUser(user.USER_ID,user.PASSWORD,true);
                if(Candidate.Account_Status=="Banned")
                    Console.WriteLine("用户已经被封禁");
                else if (Candidate.User_ID == "-1")
                    Console.WriteLine("UID不存在！");
                else if (Candidate.Password != user.PASSWORD)
                    Console.WriteLine("密码错误!");
                else
                    Console.WriteLine($"恭喜你，{Candidate.User_Name},登陆成功!");
                connection.Close();
            }

            return con;
        }
        private static bool IsValidAddress(string address)
        {
            // 读取配置文件并加载地址
            List<string> addresses = new List<string>();
            string configFile = "../../Resources/addresses.txt";
            string[] lines = File.ReadAllLines(configFile);
            addresses.AddRange(lines);
            if (addresses.Contains(address))
                return true;
            else
            {
                throw new Exception(address + "地址不合法！");
            }
        }
        private static bool ValidatePhoneNumber(string phoneNumber)
        {
            string pattern = @"^\d{3}-\d{4}-\d{4}$|^\d{11}$|^\d{3} \d{4} \d{4}$";
            bool isValid = Regex.IsMatch(phoneNumber, pattern);
            return isValid;
        }
        private static bool ValidatePassword(string password)
        {
            bool hasMinimumLength = password.Length >= 10;
            bool hasDigit = Regex.IsMatch(password, @"\d");
            bool hasLowerCase = Regex.IsMatch(password, @"[a-z]");
            bool hasUpperCase = Regex.IsMatch(password, @"[A-Z]");
            bool hasSpecialCharacter = Regex.IsMatch(password, @"[!@#$%^&*()]");

            bool isValid = hasMinimumLength && hasDigit && hasLowerCase && hasUpperCase && hasSpecialCharacter;
            return isValid;
        }
        private static bool ValidRegistration(string Username, string pwd, string phoneNumber, string Address = "Beijing")
        {
            if (!IsValidAddress(Address))
            {
                Console.WriteLine("请输入有效的地址!");
                return false;
            }else if (!ValidatePhoneNumber(phoneNumber))
            {
                Console.WriteLine("请输入有效的电话号码!");
                return false;
            }else if (!ValidatePassword(pwd))
            {
                Console.WriteLine("密码位数为8~16位，必须包含大小写，数字与特殊字符!");
                return false;
            }
            else if(Username.Length>20)
            {
                Console.WriteLine("用户名不得大于20位!");
                return false;
            }
            return true;
            
        }
        /// <summary>
        /// 校验信息并注册
        /// </summary>
        /// <param name="Username">用户名</param>
        /// <param name="pwd">密码</param>
        /// <param name="phoneNumber">手机号</param>
        /// <param name="Address">地址</param>
        public static bool Register(string Username, string pwd, string phoneNumber, string Address = "Beijing")
        {
            // 添加新行
            bool valid = ValidRegistration(Username, pwd, phoneNumber, Address);
            if (!valid) { return false; }
            string UID=UserServer.InsertUser(Username, pwd, phoneNumber, Address);
            //注册时的其他操作，如验证码等等.....
            Console.WriteLine($"你好，{Username},您已经注册成功，你的UID是{UID}");
            return true;
        }
        public static void Unregister(decimal UID)
        {
            bool rows=UserServer.DeleteUser(UID.ToString());
            if(rows)
            {
                Console.WriteLine($"{UID},您已经注销成功!");
            }else
                Console.WriteLine($"不存在UID位{UID}的用户");
        }
        //以下是更改个人信息部分
        //修改密码
        /// <summary>
        /// 封禁或解禁账户
        /// </summary>
        /// <param name="UID">用户的ID</param>
        /// <param name="flag">true为封禁，否则是解禁</param>
        public static void Ban(decimal UID,bool flag=true)
        {
            User user=UserServer.GetUser(UID.ToString(), "0",true);
            if(flag) 
                UserServer.UpdateUser(UID.ToString(), user.User_Name, user.Password,user.Phone_Number, user.Address, "Banned");
            else
                UserServer.UpdateUser(UID.ToString(), user.User_Name, user.Password, user.Phone_Number, user.Address, "In Good Standing");
        }
        static int RemainingTime = 5;
        static bool Waiting = false;
        static CountdownTimer countdownTimer;
        public static void ChangePassword(decimal UID,string Password,string NewPassword) {
            User candidate= UserServer.GetUser(UID.ToString(), Password,true);
            if(Waiting&& countdownTimer.GetTimeRemaining().Ticks>0)
            {
                TimeSpan timeRemaining = countdownTimer.GetTimeRemaining();
                Console.WriteLine($"Time remaining: {timeRemaining.Hours} hours, {timeRemaining.Minutes} minutes, {timeRemaining.Seconds} seconds");
            }else if(Waiting && countdownTimer.GetTimeRemaining().Ticks <= 0)
            {
                Waiting = false;
                RemainingTime = 5;
            }
            if (candidate.Password != Password && --RemainingTime > 0)
            {
                Console.WriteLine($"密码不正确,还有{RemainingTime}次机会，共计5次机会");
            }else if (RemainingTime == 0)
            {
                DateTime targetTime = DateTime.Now.AddMinutes(180);  // 假设倒计时目标时间为当前时间的10分钟后
                countdownTimer = new CountdownTimer(targetTime);
                Waiting = true;
            }else if(candidate.Password == Password)
            {
                if (!ValidatePassword(NewPassword))
                {
                    Console.WriteLine("密码长度必须为8~16位，同时包含大小写，数字，特殊字符！");
                    return;
                }
                UserServer.UpdateUser(UID.ToString(), candidate.User_Name, NewPassword, candidate.Phone_Number,candidate.Address,candidate.Account_Status);
                Console.WriteLine($"{candidate.User_Name},你好！密码已成功修改，请不要忘记密码");
            }

        }
    }
    public class CountdownTimer
    {
        private DateTime targetTime;

        public CountdownTimer(DateTime targetTime)
        {
            this.targetTime = targetTime;
        }

        public TimeSpan GetTimeRemaining()
        {
            DateTime currentTime = DateTime.Now;
            TimeSpan timeRemaining = targetTime - currentTime;
            return (timeRemaining.Ticks > 0) ? timeRemaining : TimeSpan.Zero;
        }
    }
}
