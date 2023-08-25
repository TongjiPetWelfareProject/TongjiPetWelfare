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
using static Glue.Controllers.RegisterController;

namespace Glue.Controllers
{
    [Route("api")]
    [ApiController]
    public class ForumController : Controller
    {
        public class PostModel
        {
            public int post_id { get; set; }
        }

        [HttpGet("forum")]
        public IActionResult Post()
        {
            List<ForumPost> postlist = ForumPostManager.GetAllPosts();
            Console.WriteLine("收到论坛请求");
            return Ok(postlist);
        }

        [HttpPost("post")]
        public IActionResult PostInfo([FromBody] PostModel postModel)
        {
            List<ForumPost> post = ForumPostManager.ShowPost(postModel.post_id);
            Console.WriteLine("收到帖子请求"+ postModel.post_id);
            return Ok(post);
        }
    }
}
