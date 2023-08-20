using Microsoft.AspNetCore.Mvc;

using PetFoster.BLL;
using Oracle.ManagedDataAccess.Client;
using System.Data;
using System.Data.OracleClient;
using PetFoster.DAL;
using PetFoster.Model;
using System.Text.Json;
using System.Text;
using System.Security.Cryptography;
using System;
using System.Numerics;
using Glue.PetFoster.Model;

namespace Glue.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ForumController : Controller
    {
        [HttpGet]
        public IActionResult Post()
        {
            List<ForumPost> postlist = ForumPostManager.GetAllPosts();
            Console.WriteLine("收到帖子请求");
            return Ok(postlist);
        }
    }
}
