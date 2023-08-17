using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PetFoster.DAL;
namespace PetFoster.BLL
{
    public class AdoptApplyManager
    {
        public static void ShowCensorAdopt(int censorstate = 0, int Limitrow = -1, string Orderby = null)
        {
            string censorStr = JsonHelper.GetErrorMessage("censor_state", censorstate);

            DataTable dt =AdoptApplyServer.AdoptInfo(censorStr, Limitrow, Orderby);
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
        public static int ApplyAdopt(string UID, string PID,bool gender, bool pet_exp, bool long_term_care,
            bool w_to_treat, decimal d_care_h, string P_Caregiver, decimal f_popul, bool be_children, bool accept_vis)
        {
            AdoptApplyServer.InsertAdoptApply(UID,PID,gender,pet_exp,long_term_care,w_to_treat,
            d_care_h, P_Caregiver,f_popul,be_children,accept_vis);
            return 0;
        }
        //审核通过
        public static int CensorAdopt(string UID,int pid,out int errcode)
        {
            if (pid == -1)
            {
                Console.WriteLine($"宠物已经全被领养或寄养（正在申请寄养）走了!");
                errcode = 1;//如上
                return -1;
            }else if (pid == -2)
            {
                Console.WriteLine($"审核过程异常!");
                errcode = 2;
                return -1;
            }
            //2.将此宠物交给主人领养
            int err = 0;
            AdoptServer.InsertAdopt(UID, pid.ToString(),out err);
            if (err == -1)
            {
                Console.WriteLine($"不存在该申请人!");
                errcode = 3;
                return -1;
            }
            //3.审核状态为通过
            AdoptApplyServer.UpdateAdoptEntry(UID, "legitimate");
            errcode = 0;//正常运行
            return pid;
        }
    }
}
