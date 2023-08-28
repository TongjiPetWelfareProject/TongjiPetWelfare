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
        */
        public class PetModel
        {
            public string? id { get; set; }
            public string? petname { get; set; }
            public string? breed { get; set; }
            public string? size { get; set; }
            public int age { get; set; }
            public string? sex { get; set; }
            public string? popularity { get; set; }
            public string? health { get; set; }
            public string? vaccine { get; set; }
            public string? from { get; set; }
        }
        
        // POST api/<ManagePetController>
        [HttpPost("add-pet")]
        public IActionResult AddPet([FromBody] PetModel pet)
        {
            // 这个函数用来接受添加员工请求，由于前端输入的是工作时长，而后端需要的是工作起始时间，这个地方你们斟酌一下
            if (pet == null)
            {
                return BadRequest("Empty Data.");
            }
            if (pet.petname == null)
            {
                return BadRequest("Empty Pet Name.");
            }
            if(pet.breed == null)
            {
                return BadRequest("Empty Pet Specis.");
            }
            if(pet.size == null)
            {
                return BadRequest("Empty Pet Size.");
            }
            else
            {
                pet.size = pet.size.ToLower();
                if(pet.size != "small" && pet.size != "medium" && pet.size != "large")
                {
                    return BadRequest("Invalid Pet Size.");
                }
            }
            string health = "Vibrant";
            if(!string.IsNullOrEmpty(pet.health))
            {
                if((health = PetManager.GetHealth(pet.health)) == null)
                {
                    return BadRequest("Invalid Health State.");
                }
            }
            bool vaccine;
            if(pet.vaccine == "已经接种")
            {
                vaccine = true;
            }
            else if(pet.vaccine =="未接种")
            {
                vaccine = false;
            }
            else
            {
                return BadRequest("Invalid Vaccine Status.");
            }
            /*
            if (!ConvertTools.ConvertHourStringToDouble(employee.workingHours, out double hours))
            {
                return BadRequest("Invalid Working Hours Format.");
            }*/
            try
            {
                PetManager.RegisterPet(pet.petname, pet.breed, pet.size, pet.age, null, health, vaccine);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // POST api/<ManagePetController>/5
        [HttpPost("edited-pet")]
        public IActionResult Post(int employeeId, [FromBody] PetModel pet)
        {
            if (pet == null)
            {
                return BadRequest("Empty Data.");
            }
            if (pet.id == null || !int.TryParse(pet.id, out int pid))
            {
                return BadRequest("Invalid Pet Id.");
            }
            string? health;
            if(!string.IsNullOrEmpty(pet.health))
            {
                if ((health = PetManager.GetHealth(pet.health)) == null)
                {
                    return BadRequest("Invalid Health State.");
                }
            }
            else
            {
                health = null;
            }
            bool? vaccine;
            if (pet.vaccine == "已经接种")
            {
                vaccine = true;
            }
            else if (pet.vaccine == "未接种")
            {
                vaccine = false;
            }
            else
            {
                return BadRequest("Invalid Vaccine Status.");
            }
            try
            {
                PetManager.UpdatePetInfo(pet.id, health, vaccine);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        /*
        // DELETE api/<ManagePetController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }*/
    }
}
