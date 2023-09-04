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
                Username = string.Empty;
                Password = string.Empty;
                PhoneNumber = string.Empty;
                City = string.Empty;
            }
        }
        [HttpPost]
        public IActionResult Register([FromBody] RegisterModel registerModel)
        {
            string username = registerModel.Username;
            string password = registerModel.Password;
            string phoneNumber = registerModel.PhoneNumber;
            string city = registerModel.City;
            string? UID;

            IActionResult respond;

            Console.WriteLine(username + " " + password + " " + phoneNumber + " " + city);
            int status = UserManager.Register(out UID, username, password, phoneNumber, city);
            string message;
            if (status == 4)
            {
                message = $"你好，{username},您已经注册成功，你的UID是{UID}";
                respond = Ok(message);
            }
            else
            {
                message = JsonHelper.GetErrorMessage("register", status);
                respond = Unauthorized(message);
            }
            return respond;
        }
    }
}
