using Glue.PetFoster.BLL;
using PetFoster.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetFoster.BLL
{
    public class EmployeeManager
    {
        public static DataTable ShowProfile(int Limitrow = -1, string Orderby = null)
        {
            DataTable dt = EmployeeServer.EmployeeInfo(Limitrow, Orderby);
            //调试用
            Console.WriteLine("开始显示员工列表");
            return dt;
        }
    }
}
