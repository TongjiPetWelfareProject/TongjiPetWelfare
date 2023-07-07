using Microsoft.AspNetCore.Mvc;
using PetFoster.BLL;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Glue.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegisterController : ControllerBase
    {
        public class RegisterModel
        {
            public string Username { get; set; }
            public string Password { get; set; }
            public string PhoneNumber { get; set; }
            public string City { get; set; }

            public RegisterModel()
            {
                Username = String.Empty;
                Password = String.Empty;
                PhoneNumber = String.Empty;
                City = String.Empty;
            }
        }
        /*
        // GET: api/<RegisterController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<RegisterController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }
        */
        // POST api/<RegisterController>
        [HttpPost]
        public IActionResult Register([FromBody] RegisterModel registerModel)
        {
            string username = registerModel.Username;
            string password = registerModel.Password;
            string phoneNumber = registerModel.PhoneNumber;
            string city = registerModel.City;

            IActionResult respond;

            if (UserManager.Register(username,password,phoneNumber,city))
            {
                respond = Ok();
            }
            else
            {
                respond = BadRequest();
            }
            return respond;
        }
        /*
        // PUT api/<RegisterController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<RegisterController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
        */
    }
}
