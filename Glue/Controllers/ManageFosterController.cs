using Microsoft.AspNetCore.Mvc;
using PetFoster.BLL;
using System.Data;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Glue.Controllers
{
    [Route("api")]
    [ApiController]
    public class ManageFosterController : ControllerBase
    {
        public class FosterRecord
        {
            public string date;
            public string id;
            public string petId;
            public string userId;
            public int days;
            public string censor_status;
            public FosterRecord()
            {
                date = "";
                id = "";
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
            
            return Ok(RecordList);
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
