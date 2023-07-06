using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;

namespace PetFoster.BLL
{
    public class JsonHelper
    {
        public static string GetConnection()
        {
            string user = JsonHelper.PreConfig("managerID");
            string pwd = JsonHelper.PreConfig("managerPwd");
            string db = JsonHelper.PreConfig("dataSource");
            string conStr = "User Id=" + user + ";Password=" + pwd + ";Data Source=" + db + ";";
            return conStr;
    }
        public static string PreConfig(string attr)
        {
            var directory = System.AppContext.BaseDirectory.Split(Path.DirectorySeparatorChar);
            directory[0] += '/';
            var slice = new ArraySegment<string>(directory, 0, directory.Length - 4);
            var path = Path.Combine(slice.ToArray());
            //根据项目路径设置配置文件路径
            string filePath = Path.Combine(path, "config.json");
            string jsonFromFile = File.ReadAllText(filePath);
            Dictionary<string, string> configFromFile = JsonSerializer.Deserialize<Dictionary<string, string>>(jsonFromFile);
            return configFromFile[attr];
        }
        /// <summary>
        /// 翻译attr属性的对应值为英文
        /// </summary>
        /// <param name="chineseValue"></param>
        /// <param name="attr"></param>
        /// <returns></returns>
        public static string TranslateToEn(string chineseValue,string attr)
        {
            var directory = System.AppContext.BaseDirectory.Split(Path.DirectorySeparatorChar);
            directory[0] += '/';
            var slice = new ArraySegment<string>(directory, 0, directory.Length - 4);
            var path = Path.Combine(slice.ToArray());
            string filePath = Path.Combine(path, "values.json");
            string jsonFromFile = File.ReadAllText(filePath);

            // 解析JSON字符串
            string json = File.ReadAllText(filePath);
            // 解析JSON字符串
            JsonDocument doc = JsonDocument.Parse(json);

            // 获取provinces数组
            JsonElement root = doc.RootElement;
            // 获取 "status" 属性下的数据
            JsonElement statusData = root.GetProperty(attr);

            // 获取特定状态的英文名
            foreach(var item in statusData.EnumerateObject())
            {
                if(item.Name==chineseValue)
                    return item.Value.ToString();
            }
            return null;
        }
        public static string TranslateToCn(string enValue, string attr)
        {
            var directory = System.AppContext.BaseDirectory.Split(Path.DirectorySeparatorChar);
            directory[0] += '/';
            var slice = new ArraySegment<string>(directory, 0, directory.Length - 4);
            var path = Path.Combine(slice.ToArray());
            string filePath = Path.Combine(path, "values.json");
            string jsonFromFile = File.ReadAllText(filePath);

            // 解析JSON字符串
            string json = File.ReadAllText(filePath);
            // 解析JSON字符串
            JsonDocument doc = JsonDocument.Parse(json);

            // 获取provinces数组
            JsonElement root = doc.RootElement;
            // 获取 "status" 属性下的数据
            JsonElement statusData = root.GetProperty(attr);

            // 获取特定状态的英文名
            foreach (var item in statusData.EnumerateObject())
            {
                if (item.Value.ToString() == enValue)
                    return item.Name.ToString();
            }
            return null;
        }
        public static string TranslateAddr(string input)
        {

            string pattern = @"^(.*?)(省|市|自治区|特别行政区)(.*)$";
            Regex regex = new Regex(pattern);
            Match match = regex.Match(input);
            string[] region = new string[3];
            string[] res=new string[2];
            if (match.Success)
            {
                region[0] = match.Groups[1].Value;
                region[1] = match.Groups[2].Value;
                region[2] = match.Groups[3].Value;
                res[1] = TranslateToEn(region[0] + region[1], "provinces");
                if (res[1]==null)return null;
                res[0] = TranslateToEn(region[2], res[1]);
                if (res[0]==null)return null;
            }
            else
            {
                Console.WriteLine("无法分割字符串");
            }
            return res[0]+','+res[1];
        }

    }
}
