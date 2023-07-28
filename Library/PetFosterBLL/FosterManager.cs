using PetFoster.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetFoster.BLL
{
    public class FosterManager
    {
        /// <summary>
        /// 展示未审核/已审核/未通过界面
        /// </summary>
        /// <param name="censorstate">0为未审核，1为未通过审核，2为通过审核</param>
        /// <param name="Limitrow"></param>
        /// <param name="Orderby"></param>
        public static void CensorFoster(int censorstate=0,int Limitrow = -1, string Orderby = null)
        {
            string censorStr=JsonHelper.GetErrorMessage("censor_status",censorstate);

            DataTable dt = FosterServer.FosterInfo(censorStr,Limitrow, Orderby);
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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="UID">用户登录后获得潜在UID</param>
        /// <param name="Petname">宠物名</param>
        /// <param name="Breed">品种</param>
        /// <param name="size">大小</param>
        /// <param name="dateTime">日期</param>
        /// <param name="duration">寄养时长</param>
        /// <param name="remark">备注</param>
        /// <returns></returns>
        public static int ApplyFoster(string UID, string Petname,string Breed,string size, DateTime dateTime, int duration, string remark)
        {
            //0.该宠物不存在，则需要注册
            string PID=PetFoster.DAL.PetServer.InsertPet(Petname, Breed, size, DateTime.MinValue);
            //1. 判断foster项是否符合要求
            if(!FosterServer.NoOverlapping(UID, PID, dateTime, duration))
                FosterServer.InsertFoster(UID, PID, dateTime, duration, remark);//申请抚养初步通过
            else
            {
                    Console.WriteLine($"{UID}已经在{dateTime.Date}~{dateTime.AddDays(duration).Date}时间段寄养foster该宠物{PID}");
                    return 1;
            }
            //3. 分配房间
            short storey, compartment;
            RoomServer.AllocateRoom(out storey, out compartment);
            RoomServer.UpdateRoom(storey, compartment, false,"Y");
            //4. 在accommodate中登记入住
            AccommodateServer.InsertAccommodate(UID, PID, storey, compartment);
            return 0;
        }
        /// <summary>
        /// 旧有宠物的申请
        /// </summary>
        /// <param name="UID"></param>
        /// <param name="PID"></param>
        /// <param name="Breed"></param>
        /// <param name="size"></param>
        /// <param name="dateTime"></param>
        /// <param name="duration"></param>
        /// <param name="remark"></param>
        /// <returns></returns>
        public static int ApplyFoster(string UID, int PID, string Breed, string size, DateTime dateTime, int duration, string remark)
        {
            //0.该宠物不存在，则需要注册
            Model.Pet test=PetFoster.DAL.PetServer.SelectPet(PID.ToString());
            if (test.Pet_ID == "-1")
            {
                Console.WriteLine($"PID为{PID}的宠物不存在");
                return 2;
            }
            //1. 判断foster项是否符合要求
            if (!FosterServer.NoOverlapping(UID, PID.ToString(), dateTime, duration))
                FosterServer.InsertFoster(UID, PID.ToString(), dateTime, duration, remark);//申请抚养初步通过
            else
            {
                Console.WriteLine($"{UID}已经在{dateTime.Date}~{dateTime.AddDays(duration).Date}时间段寄养foster该宠物{PID}");
                return 1;
            }
            //3. 分配房间
            short storey, compartment;
            RoomServer.AllocateRoom(out storey, out compartment);
            RoomServer.UpdateRoom(storey, compartment, false, "Y");
            //4. 在accommodate中登记入住
            AccommodateServer.InsertAccommodate(UID, PID.ToString(), storey, compartment);
            return 0;
        }
        /// <summary>
        /// 审核
        /// </summary>
        /// <param name="UID"></param>
        /// <param name="PID"></param>
        /// <param name="date"></param>
        /// <param name="censorcode">1表示审核未通过，2表示通过，3表示过期，默认为未审核</param>
        public static void Censorship(string UID, int PID, DateTime date,int censorcode)
        {
            string msg = censorcode == 1 ? "aborted" : "legitimate";
            switch (censorcode)
            {
                case 1:
                    msg = "aborted";
                    break;
                case 2:
                    msg = "legitimate";
                    break;
                case 3:
                    msg = "outdated";
                    break;
                default:
                    msg = "to be censored";
                    break;
            }
            FosterServer.UpdateFosterEntry(UID, PID.ToString(), date,msg);
        }
    }   
}
