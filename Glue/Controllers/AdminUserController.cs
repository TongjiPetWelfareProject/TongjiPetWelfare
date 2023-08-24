using Microsoft.AspNetCore.Mvc;
using PetFoster.BLL;
using System.Data;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Glue.Controllers
{
    [Route("api")]
    [ApiController]
    public class AdminUserController : ControllerBase
    {
        public class UserModel
        {
            public string? id { get; set; }
            public string? username { get; set; }
            public string? phone { get; set; }
            public string? address { get; set; }
        }
        
        // GET: api/<AdminUserController>
        [HttpGet("tableuser")]
        public IActionResult Get()
        {
            try
            {
                DataTable dt = UserManager.ShowUserProfile();
                List<UserModel> UsersList = new List<UserModel>();

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    UserModel UserItem = new UserModel();
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        if (dt.Columns[j].ColumnName.ToLower() == "user_id")
                        {
                            UserItem.id = dt.Rows[i][j].ToString();
                        }
                        else if (dt.Columns[j].ColumnName.ToLower() == "user_name")
                        {
                            UserItem.username = dt.Rows[i][j].ToString();
                        }
                        else if (dt.Columns[j].ColumnName.ToLower() == "phone_number")
                        {
                            UserItem.phone = dt.Rows[i][j].ToString();
                        }
                        else if (dt.Columns[j].ColumnName.ToLower() == "address")
                        {
                            UserItem.address = dt.Rows[i][j].ToString();
                        }
                    }
                    UsersList.Add(UserItem);
                }
                //jsonstring = "{\"data\":" + JsonSerializer.Serialize(PetNamesList) + "}";
                //Console.WriteLine(jsonstring);
                return Ok(UsersList);
            }
            catch (Exception ex)
            {
                //Console.WriteLine(ex.Message);
                return StatusCode(500, ex.Message);
            }
        }

        /*
        // GET api/<AdminUserController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }
        */
        // POST api/<AdminUserController>
        [HttpPost("ban")]
        public IActionResult Post([FromBody] int uid)
        {
            try
            {
                string message = UserManager.Ban(uid);
                return Ok(message);
            }
            catch(Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        /*
        // PUT api/<AdminUserController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<AdminUserController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
        */
    }
}
