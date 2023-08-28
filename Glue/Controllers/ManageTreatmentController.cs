using Microsoft.AspNetCore.Mvc;
using PetFoster.BLL;
using System.Data;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Glue.Controllers
{
    [Route("api")]
    [ApiController]
    public class ManageTreatmentController : ControllerBase
    {
        // GET: api/<ManageTreatmentController>
        [HttpGet("treatlist")]
        public IActionResult GetTreatList()
        {
            try
            {
                DataTable dt = AppointmentManager.ShowApplies();

                string jsondata = ConvertTools.DataTableToJson(dt);
                Console.WriteLine(jsondata);
                return Ok(jsondata);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(500, ex.Message);
            }
        }
        /*
        // GET api/<ManageTreatmentController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<ManageTreatmentController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<ManageTreatmentController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<ManageTreatmentController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
        */
    }
}
