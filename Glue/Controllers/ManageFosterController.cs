using Microsoft.AspNetCore.Mvc;
using PetFoster.BLL;
using System.Data;
using System.Text.Json;
//using Microsoft.AspNetCore.JsonPatch;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Glue.Controllers
{
    [Route("api")]
    [ApiController]
    public class ManageFosterController : ControllerBase
    {
        public class FosterRecord
        {
            public string date { get; set; }
            public string petId { get; set; }
            public string userId { get; set; }
            public int days { get; set; }
            public string censor_status { get; set; }
            public FosterRecord()
            {
                date = "";
                petId = "";
                userId = "";
                days = 0;
                censor_status = "";
            }
        }

        // GET: api/<ManageFosterController>
        
        [HttpGet("manage-foster")]
        public IActionResult Get()
        {
            string censorStr;
            DataTable dt = FosterManager.CensorFoster(out censorStr);
            List<FosterRecord> RecordList = new List<FosterRecord>();

            for(int i=0; i < dt.Rows.Count; i++)
            {
                FosterRecord RecordItem = new FosterRecord();
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    if (dt.Columns[j].ColumnName.ToLower() == "duration")
                    {
                        RecordItem.days = Convert.ToInt32(dt.Rows[i][j]);
                    }
                    else if (dt.Columns[j].ColumnName.ToLower() == "fosterer")
                    {
                        RecordItem.userId = dt.Rows[i][j].ToString();
                    }
                    else if (dt.Columns[j].ColumnName.ToLower() == "pet_id")
                    {
                        RecordItem.petId = dt.Rows[i][j].ToString();
                    }
                    else if (dt.Columns[j].ColumnName.ToLower() == "startdate")
                    {
                        RecordItem.date = dt.Rows[i][j].ToString();
                    }
                }
                RecordItem.censor_status = censorStr;
                RecordList.Add(RecordItem);
            }
            /*
            foreach (FosterRecord Record in RecordList)
            {
                Console.WriteLine(Record.date+Record.petId+Record.userId+Record.days.ToString()+Record.censor_status);
            }
            */
            string jsondata = JsonSerializer.Serialize(RecordList);
            Console.WriteLine(jsondata);

            return Ok(jsondata);
        }
        /*
        // PATCH: api/<ManageFosterController>
        [HttpPatch("manage-foster-update")]
        public IActionResult PatchFosterRecord([FromBody] JsonPatchDocument<FosterRecord> patchDoc)
        {

        }

        /*

        // GET api/<ManageFosterController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<ManageFosterController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<ManageFosterController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<ManageFosterController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }

        */
    }
}
