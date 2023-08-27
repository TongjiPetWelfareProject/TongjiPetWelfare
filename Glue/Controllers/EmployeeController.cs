using Microsoft.AspNetCore.Mvc;
using PetFoster.BLL;
using System.Data;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Glue.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        public class EmployeeModel
        {
            public string? id { get; set; }
            public string? name { get; set; }
            public string? phone {get; set; }
            public string? responsibility { get; set; }
            public string? workingHours { get; set; }
            public string? salary { get; set; }
        }
        // GET: api/<EmployeeController>
        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                DataTable dt = EmployeeManager.ShowProfile();
                List<EmployeeModel> EmployeesList = new List<EmployeeModel>();

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    EmployeeModel EmployeeItem = new EmployeeModel();
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        if (dt.Columns[j].ColumnName.ToLower() == "employee_id")
                        {
                            EmployeeItem.id = dt.Rows[i][j].ToString();
                        }
                        else if (dt.Columns[j].ColumnName.ToLower() == "employee_name")
                        {
                            EmployeeItem.name = dt.Rows[i][j].ToString();
                        }
                        else if (dt.Columns[j].ColumnName.ToLower() == "phone_number")
                        {
                            EmployeeItem.phone = dt.Rows[i][j].ToString();
                        }
                        else if(dt.Columns[j].ColumnName.ToLower() == "duty")
                        {
                            
                            EmployeeItem.responsibility = JsonHelper.TranslateToCn(dt.Rows[i][j].ToString(), "duties");
                        }
                        else if(dt.Columns[j].ColumnName.ToLower() == "salary")
                        {
                            EmployeeItem.salary = dt.Rows[i][j].ToString()+"￥";
                        }
                        else if(dt.Columns[j].ColumnName.ToLower() == "working_hours")
                        {
                            EmployeeItem.workingHours = dt.Rows[i][j].ToString()+"小时";
                        }
                    }
                    EmployeesList.Add(EmployeeItem);
                }
                //jsonstring = "{\"data\":" + JsonSerializer.Serialize(PetNamesList) + "}";
                //Console.WriteLine(jsonstring);
                return Ok(EmployeesList);
            }
            catch (Exception ex)
            {
                //Console.WriteLine(ex.Message);
                return StatusCode(500, ex.Message);
            }

        }

        [HttpPost("addemployee")]
        public IActionResult AddEmployee()
        {
            // 这个函数用来接受添加员工请求，由于前端输入的是工作时长，而后端需要的是工作起始时间，这个地方你们斟酌一下
            return Ok();
        }

        /*
        // GET api/<EmployeeController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<EmployeeController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }
        
        // PUT api/<EmployeeController>/5
        [HttpPut("{employeeid}")]
        public IActionResult Put(int employeeId, [FromBody] EmployeeModel employee)
        {

        }

        // DELETE api/<EmployeeController>/5
        [HttpDelete("{employeeId}")]
        public IActionResult Delete(int employeeId)
        {
            return StatusCode(405, "Method Not Allowed");
        }
        */
    }
}
