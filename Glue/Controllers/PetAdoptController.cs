using Microsoft.AspNetCore.Mvc;
using PetFoster.BLL;
using PetFoster.Model;
using System.Data;
using System.Text;
using System.Text.Json;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Glue.Controllers
{
    [Route("api")]
    [ApiController]
    public class PetAdoptController : ControllerBase
    {
        public class AdoptData
        {
            public string? user { get; set; }
            public string? pet { get; set; }
            public string? gender { get; set; }
            public string? pet_exp { get; set; }
            public string? long_term_care { get; set; }
            public string? w_to_treat { get; set; }
            public decimal d_care_h { get; set; }
            public string? P_caregiver { get; set; }
            public decimal f_popul { get; set; }
            public string? be_children { get; set; }
            public string? accept_vis { get; set; }
        }

        // GET: api/<PetAdoptController>
        [HttpGet("petlist")]
        public IActionResult Get()
        {
            DataTable dt = AdoptManager.ShowPetProfile();
            //List<Dictionary<string, object>> dataRows = new List<Dictionary<string, object>>();

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
            string json = DataTableToJson(dt);
            return Content(json, "application/json");
            /*
            try
            {
                DataTable dt = AdoptManager.ShowPetProfile();
                List<Pet> PetList = new List<Pet>();

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Pet PetItem = new Pet((PetData.PETRow);
                    
                    PetList.Add(PetItem);
                }

                string jsondata = JsonSerializer.Serialize(PetList);

                return Ok(jsondata);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
            */
        }

        // GET api/<PetAdoptController>
        [HttpGet("pet-details")]
        public IActionResult Get(int petId)
        {
            try
            {
                Pet2 pet = AdoptApplyManager.RetrievePet(petId);
                return Ok(pet);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // POST api/<PetAdoptController>
        [HttpPost("pet-adopt")]
        public IActionResult Post([FromBody] AdoptData adopt_table)
        {
            if (adopt_table == null)
            {
                return BadRequest("Invalid data.");
            }

            bool gender = (adopt_table.gender == "M") ? true : false;
            bool pet_exp = (adopt_table.pet_exp == "Y") ? true : false;
            bool long_term_care = (adopt_table.long_term_care == "Y") ? true : false;
            bool w_to_treat = (adopt_table.w_to_treat == "Y") ? true : false;
            bool be_children = (adopt_table.be_children == "Y") ? true : false;
            bool accept_vis = (adopt_table.accept_vis == "Y") ? true : false;

            try
            {
                AdoptApplyManager.ApplyAdopt(adopt_table.user, adopt_table.pet,
                    gender, pet_exp, long_term_care,
                    w_to_treat, adopt_table.d_care_h, adopt_table.P_caregiver,
                    adopt_table.f_popul, be_children, accept_vis);
                return Ok();
            }
            catch (Exception ex)
            {
                return NotFound("不存在的用户或宠物");
            }
        }

        /*
        // PUT api/<PetAdoptController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<PetAdoptController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
        */

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
