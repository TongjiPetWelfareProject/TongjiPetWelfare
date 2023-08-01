using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860
using PetFoster.BLL;
using Oracle.ManagedDataAccess.Client;
using System.Data;
using System.Data.OracleClient;
using PetFoster.DAL;
using PetFoster.Model;
using System.Text.Json;

namespace WebApplicationTest1
{

    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        public class LoginModel
        {
            public string Username { get; set; }
            public string Password { get; set; }

            public LoginModel()
            {
                Username = string.Empty;
                Password = string.Empty;
            }

        }


        [HttpPost]
        public IActionResult Login([FromBody] LoginModel loginModel)
        {
            string username = loginModel.Username;
            string password = loginModel.Password;
            Console.WriteLine(username + " " + password);
            User candidate = UserManager.Login(username, password);
            /*
            using (OracleConnection oracle = new OracleConnection(conStr))
            {
                oracle.Open();
                if (UserManager.Login(username, password))
                {
                    respond = Ok();
                }
                else
                {
                    respond = BadRequest();
                }
                oracle.Close();
            }
            */
            //string message = JsonHelper.GetErrorMessage("login", status);
            //Console.WriteLine(message);

            if (candidate.Password != password)
            {
                return Unauthorized("密码错误，请重新输入");
            }
            else if (candidate.Account_Status == "Banned")
            {
                return Unauthorized("账号已被封禁，请等待解禁");
            }
            else if (candidate.User_ID == "-1")
            {
                return Unauthorized("用户不存在");
            }
            else
            {

                var responseData = new
                {
                    data = new
                    {
                        User_ID = candidate.User_ID,
                        User_Name = candidate.User_Name,
                        Password = candidate.Password,
                        Phone_Number = candidate.Phone_Number,
                        Address = candidate.Address,
                        Role = candidate.Role,
                        Account_Status = candidate.Account_Status
                    }
                };
                string responseJson = JsonSerializer.Serialize(responseData);
                return Ok(responseJson);
            }

            
}
        // PUT api/<LoginController>/5
        /*
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<LoginController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
        */
    }
}
