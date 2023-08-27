using Microsoft.AspNetCore.Mvc;
using PetFoster.BLL;
using PetFoster.Model;
using System.Data;
using System.Text;
using Newtonsoft.Json;

namespace Glue.Controllers
{
    [Route("api")]
    [ApiController]
    public class AdminPetController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet("petlist")]
        public IActionResult Get()
        {
            DataTable dt = PetManager.ShowPetProfile();
            //List<Dictionary<string, object>> dataRows = new List<Dictionary<string, object>>();
            foreach (DataRow row in dt.Rows)
            {

                foreach (DataColumn column in dt.Columns)
                {

                    if (column.ColumnName == "VACCINE")
                    {
                        row[column.ColumnName] = row[column.ColumnName].ToString() == "Y" ? "已接种" : "未接种";
                    }
                    else if (column.ColumnName == "HEALTH_STATE")
                    {
                        row[column.ColumnName] = JsonHelper.TranslateToCn(row[column.ColumnName].ToString(), "health_state");
                    }
                    else if (column.ColumnName == "SEX")
                    {
                        row[column.ColumnName] = row[column.ColumnName].ToString() == "F" ? "女" : "男";
                    }
                    else if (column.ColumnName == "SPECIES")
                    {
                        row[column.ColumnName] = row[column.ColumnName].ToString() == "dog" ? "狗" : "猫";
                    }
                    else if (column.ColumnName == "STATUS")
                    {
                        column.ColumnName = "SOURCE";
                    }
                }
            }
            string json = DataTableToJson(dt);
            return Content(json, "application/json");
        }
        private string DataTableToJson(DataTable table)
        {
            var jsonString = new StringBuilder();

            if (table.Rows.Count > 0)
            {
                jsonString.Append("[");
                for (int i = 0; i < table.Rows.Count; i++)
                {
                    jsonString.Append("{");

                    for (int j = 0; j < table.Columns.Count; j++)
                    {
                        jsonString.AppendFormat("\"{0}\":\"{1}\"",
                            table.Columns[j].ColumnName,
                            table.Rows[i][j]);

                        if (j < table.Columns.Count - 1)
                        {
                            jsonString.Append(",");
                        }
                    }

                    jsonString.Append("}");
                    if (i < table.Rows.Count - 1)
                    {
                        jsonString.Append(",");
                    }
                }
                jsonString.Append("]");
            }

            return jsonString.ToString();
        }
    }
}
