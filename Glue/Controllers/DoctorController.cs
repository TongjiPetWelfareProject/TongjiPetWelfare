using Microsoft.AspNetCore.Mvc;
using PetFoster.BLL;
using System.Data;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Glue.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DoctorController : ControllerBase
    {
        public class Doctor
        {
            public string? id { get; set; }
            public string? name { get; set; }
            public string? phone { get; set; }
            public string? workingHours { get; set; }
            public string? salary { get; set; }
        }
        private List<Doctor> ConvertDataTableToVetList(DataTable dataTable)
        {
            List<Doctor> vetList = new List<Doctor>();

            foreach(DataRow row in dataTable.Rows)
            {
                // 使用PadLeft方法确保始终有两位数
                string startHourString = (row["working_start_hr"]?.ToString() ?? string.Empty).PadLeft(2, '0');
                string startMinuteString = (row["working_start_min"]?.ToString() ?? string.Empty).PadLeft(2, '0');
                string endHourString = (row["working_end_hr"]?.ToString() ?? string.Empty).PadLeft(2, '0');
                string endMinuteString = (row["working_end_min"]?.ToString() ?? string.Empty).PadLeft(2, '0');

                var doctor = new Doctor
                {
                    id = row["vet_id"].ToString(),
                    name = row["vet_name"].ToString(),
                    phone = row["phone_number"].ToString(),
                    workingHours = $"{startHourString}:{startMinuteString}-{endHourString}:{endMinuteString}",
                    salary = row["salary"].ToString()
                };
                vetList.Add(doctor);
            }
            return vetList;
        }
        
        // GET: api/<DoctorController>
        [HttpGet]
        public IEnumerable<Doctor> Get()
        {
            DataTable vetProfiles = VetManager.ShowVetProfile();
            List<Doctor> vets = ConvertDataTableToVetList(vetProfiles);
            return vets;
        }
        /*
        // GET api/<DoctorController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }
        
        // POST api/<DoctorController>
        [HttpPost]
        public IEnumerable<Doctor> Post()
        {
            
        }
        
        // PUT api/<DoctorController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<DoctorController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
        */
    }
}
