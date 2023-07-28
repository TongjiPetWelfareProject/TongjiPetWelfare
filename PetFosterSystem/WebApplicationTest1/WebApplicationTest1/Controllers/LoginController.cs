﻿using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860
using PetFoster.BLL;
using Oracle.ManagedDataAccess.Client;
using System.Data;
using System.Data.OracleClient;
using PetFoster.Model;
using PetFoster.DAL;
namespace WebApplicationTest1
{

    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        public static string user = "\"C##PET\"";
        public static string pwd = "campus";
        public static string db = "localhost:1521/orcl";
        private static string conStr = "User Id=" + user + ";Password=" + pwd + ";Data Source=" + db + ";"; // 替换为实际的数据库连接字符串
        //private readonly UserManager _userManager;

        //public LoginController()

        // GET: api/<LoginController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<LoginController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<LoginController>
        /*[HttpPost]
        public void Post([FromBody] string value)
        {
        }*/
        [HttpPost]
        public IActionResult Login([FromBody] LoginModel loginModel)
        {
            string username = loginModel.Username;
            string password = loginModel.Password;
                Console.WriteLine(username + " " + password);
            IActionResult respond = Ok();
            using (OracleConnection oracle = new OracleConnection(conStr))
            {
                oracle.Open();/*
                if (UserManager.Login(username, password))
                {
                    respond = Ok();
                }
                else
                {
                    respond = BadRequest();
                }*/
                oracle.Close();
            }
            return respond;
            /*
            OracleConnection oracle=new OracleConnection(conStr);
            oracle.Open();
            if (UserManager.Login(username,password)) {
                return Ok();
            }
            return BadRequest();*/
        }

        // PUT api/<LoginController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<LoginController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}