using Microsoft.AspNetCore.Mvc;
using PetFoster.BLL;
using PetFoster.Model;
using System.Data;
using System.Text;
using Newtonsoft.Json;

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
            public bool gender { get; set; }
            public decimal pet_exp { get; set; }
            public bool long_term_care { get; set; }
            public bool w_to_treat { get; set; }
            public decimal d_care_h { get; set; }
            public string? P_caregiver { get; set; }
            public decimal f_popul { get; set; }
            public bool be_children { get; set; }
            public bool accept_vis { get; set; }
        }

        // GET: api/<PetAdoptController>
        [HttpGet("pet-adopt-list")]
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

        public class PetWithoutAvartar
        {
            public string Pet_ID { get; set; }
            public string Pet_Name { get; set; }
            public string Species { get; set; }
            public DateTime birthdate { get; set; }
            public string Health_State { get; set; }
            public string Vaccine { get; set; }
            public decimal Read_Num { get; set; }
            public decimal Like_Num { get; set; }
            public decimal Collect_Num { get; set; }
            public PetWithoutAvartar()
            {
                Pet_ID = "-1";
                Pet_Name = "宠物已注销";
            }
            public PetWithoutAvartar(PetData.PETRow prow)
            {
                Pet_ID = prow.PET_ID;
                Pet_Name = prow.PET_NAME;
                Species = prow.BREED;
                birthdate = prow.BIRTHDATE;
                Health_State = prow.HEALTH_STATE;
                Vaccine = prow.VACCINE;
                Read_Num = prow.READ_NUM;
                Like_Num = prow.LIKE_NUM;
                Collect_Num = prow.COLLECT_NUM;
            }
        }
        public class Pet2WithoutAvartar
        {
            public Pet2WithoutAvartar()
            {

            }
            public PetWithoutAvartar original_pet;
            public int Popularity;
            public bool sex;
            public string Psize;//宠物大小
            public int Comment_Num;
            public Pet2.Comment[] comments;
        }

        public class Pet2WithHaveLiked: Pet2WithoutAvartar
        {
            public bool have_liked;
            public bool have_collected;
        }
        public class DetailsRequest
        {
            public int userId;
        }
        // GET api/<PetAdoptController>
        [HttpGet("pet-details/{petId}")]
        public IActionResult Get(int petId,[FromBody] DetailsRequest request)
        {
            try
            {
                Pet2 pet = AdoptApplyManager.RetrievePet(petId);
                Pet2WithHaveLiked pet2_temp = new Pet2WithHaveLiked();
                PetWithoutAvartar pet_temp = new PetWithoutAvartar();
                pet2_temp.comments = new Pet2.Comment[pet.comments.Length];
                for (int i = 0; i < pet.comments.Length; i++)
                {
                    pet2_temp.comments[i] = new Pet2.Comment(
                        pet.comments[i].comment_contents,
                        pet.comments[i].comment_time
                    );
                }
                pet2_temp.Comment_Num = pet.Comment_Num;
                pet2_temp.Psize = pet.Psize;
                pet2_temp.sex = pet.sex;
                pet2_temp.Popularity = pet.Popularity;
                pet_temp.Pet_ID = pet.original_pet.Pet_ID;
                pet_temp.Pet_Name = pet.original_pet.Pet_Name;
                pet_temp.Species = pet.original_pet.Species;
                pet_temp.birthdate = pet.original_pet.birthdate;
                pet_temp.Health_State = pet.original_pet.Health_State;
                pet_temp.Vaccine = pet.original_pet.Vaccine;
                pet_temp.Read_Num = pet.original_pet.Read_Num;
                pet_temp.Like_Num = pet.original_pet.Like_Num;
                pet_temp.Collect_Num = pet.original_pet.Collect_Num;
                pet2_temp.original_pet = pet_temp;

                // newly added to show whether user has liked or collected this pet
                try
                {
                    pet2_temp.have_liked = LikePetManager.HaveUserLiked(request.userId.ToString(), petId.ToString());
                    pet2_temp.have_collected = CollectPetInfoManager.HaveUserCollected(request.userId.ToString(), petId.ToString());
                }
                catch (Exception ex)
                {
                    pet2_temp.have_liked = false;
                    pet2_temp.have_collected = false;
                }
                try
                {
                    string jsondata = Newtonsoft.Json.JsonConvert.SerializeObject(pet2_temp);
                    Console.WriteLine(jsondata);
                    return Ok(jsondata);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return BadRequest("无法转换为Json");
                }

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
            if(adopt_table.user == null)
            {
                return BadRequest("Invalid userId.");
            }
            if(adopt_table.pet == null)
            {
                return BadRequest("Invalid petId.");
            }

            //bool gender = (adopt_table.gender == "M") ? true : false;
            bool pet_exp = (adopt_table.pet_exp > 0) ? true : false;
            //bool long_term_care = (adopt_table.long_term_care == "Y") ? true : false;
            //bool w_to_treat = (adopt_table.w_to_treat == "Y") ? true : false;
            //bool be_children = (adopt_table.be_children == "Y") ? true : false;
            //bool accept_vis = (adopt_table.accept_vis == "Y") ? true : false;

            try
            {
                if (AdoptApplyManager.ApplyAbuseOrNot(adopt_table.user, adopt_table.pet))
                    return BadRequest("重复申请领养该重复/该宠物领养热度过高");
                AdoptApplyManager.ApplyAdopt(adopt_table.user, adopt_table.pet,
                    adopt_table.gender, pet_exp, adopt_table.long_term_care,
                    adopt_table.w_to_treat, adopt_table.d_care_h, adopt_table.P_caregiver,
                    adopt_table.f_popul, adopt_table.be_children, adopt_table.accept_vis);
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
