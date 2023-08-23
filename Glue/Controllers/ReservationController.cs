using Microsoft.AspNetCore.Mvc;
using PetFoster.BLL;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Glue.Controllers
{
    [Route("api")]
    [ApiController]
    public class ReservationController : ControllerBase
    {
        public class AppointmentData
        {
            public string? name { get; set; }
            public string? kind { get; set; }
            public string? date1 { get; set; }
            public string? desc { get; set; }
            public string? selectedDoctorID { get; set; }
            public string? userId { get; set; }
        }
        /*
        // GET: api/<MedicalController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<MedicalController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }
        */

        // POST api/<MedicalController>
        [HttpPost("submitAppointment")]
        public IActionResult Post([FromBody] AppointmentData appointment_data)
        {
            if(appointment_data == null)
            {
                return BadRequest("Invalid Data");
            }
            if(appointment_data.userId == null)
            {
                return BadRequest("未登录");
            }
            if(appointment_data.name == null)
            {
                return BadRequest("empty pet name");
            }
            if(appointment_data.selectedDoctorID == null)
            {
                return BadRequest("empty doctor");
            }
            DateTime? date = ConvertTools.StringConvertToDate(appointment_data.date1);
            if (date == null)
            {
                return BadRequest("Failed to parse the date.");
            }
            string desc;
            if(appointment_data.desc == null)
            {
                desc = "";
            }
            else
            {
                desc = appointment_data.desc;
            }
            try
            {
                AppointmentManager.Appointment(appointment_data.userId,
                    appointment_data.name, "dog",
                    appointment_data.selectedDoctorID,
                    (DateTime)date,
                    desc);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
            return Ok();
        }

        /*
        // PUT api/<MedicalController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<MedicalController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
        */
    }
}
