using Microsoft.AspNetCore.Mvc;
using PetFoster.BLL;
using System.Data;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Glue.Controllers
{
    [Route("api")]
    [ApiController]
    public class ManagePetController : ControllerBase
    {
        // GET: api/<ManagePetController>
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
            //foreach (DataRow row in dt.Rows)
            //{
            //    Dictionary<string, object> rowData = new Dictionary<string, object>();
            //    foreach (DataColumn column in dt.Columns)
            //    {
            //        if (column.ColumnName != "AVARTAR")
            //        {
            //            rowData[column.ColumnName] = row[column];
            //        }
            //    }
            //    byte[] avatarBytes = (byte[])row["AVATAR"];
            //    string base64Avatar = Convert.ToBase64String(avatarBytes);
            //    rowData["AVATAR"] = base64Avatar;
            //    dataRows.Add(rowData);
            //}
            string json = ConvertTools.DataTableToJson(dt);
            return Content(json, "application/json");
        }
        /*
        // GET api/<ManagePetController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<ManagePetController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<ManagePetController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<ManagePetController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }*/
    }
}
