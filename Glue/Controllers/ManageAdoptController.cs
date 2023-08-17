using Microsoft.AspNetCore.Mvc;
using PetFoster.BLL;
using System.Data;
using System.Text.Json;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Glue.Controllers
{
    [Route("api")]
    [ApiController]
    public class ManageAdoptController : ControllerBase
    {

        public class AdoptionRecord
        {
            public string date { get; set; }
            public string petId { get; set; }
            public string userId { get; set; }
            public string reason { get; set; }
            public string censor_status { get; set; }
            public AdoptionRecord()
            {
                date = "";
                petId = "";
                userId = "";
                reason = "";
                censor_status = "";
            }
        }

        // GET: api/<ManageAdoptController>
        [HttpGet("manage-adopt")]
        public IActionResult Get()
        {
            int censorstate = 0; //默认未审核
            try
            {
                DataTable dt = AdoptApplyManager.ShowCensorAdopt();
                List<AdoptionRecord> RecordList = new List<AdoptionRecord>();

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    AdoptionRecord RecordItem = new AdoptionRecord();
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        if (dt.Columns[j].ColumnName.ToLower() == "apply_date")
                        {
                            RecordItem.date = dt.Rows[i][j].ToString();
                        }
                        else if (dt.Columns[j].ColumnName.ToLower() == "pet_id")
                        {
                            RecordItem.petId = dt.Rows[i][j].ToString();
                        }
                        else if (dt.Columns[j].ColumnName.ToLower() == "adopter_id")
                        {
                            RecordItem.userId = dt.Rows[i][j].ToString();
                        }
                        else if (dt.Columns[j].ColumnName.ToLower() == "reason")
                        {
                            RecordItem.reason = dt.Rows[i][j].ToString();
                        }
                    }
                    RecordItem.censor_status = JsonHelper.GetErrorMessage("censor_state", censorstate);
                    RecordList.Add(RecordItem);
                }

                string jsondata = JsonSerializer.Serialize(RecordList);
                Console.WriteLine(jsondata);

                return Ok(jsondata);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        /*

        // GET api/<ManageAdoptController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<ManageAdoptController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<ManageAdoptController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<ManageAdoptController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
        */
    }
}
