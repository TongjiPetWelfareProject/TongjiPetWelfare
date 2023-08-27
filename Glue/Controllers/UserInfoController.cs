using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860
using PetFoster.BLL;
using Oracle.ManagedDataAccess.Client;
using System.Data;
using System.Data.OracleClient;
using PetFoster.DAL;
using PetFoster.Model;
using System.Text.Json;
using System.Text;
using System.Security.Cryptography;
using Newtonsoft.Json;

namespace WebApplicationTest1
{

    [Route("api")]
    [ApiController]
    public class UserInfoController : ControllerBase
    {
        public class UserInfoModel
        {
            public string user_id { get; set; }

            public UserInfoModel()
            {
                user_id = string.Empty;
            }

        }
        [HttpPost("userinfo")]
        public IActionResult GetUserInfo([FromBody] UserInfoModel userinfoModel)
        {
            int likenum = UserManager.GetLikeNum(userinfoModel.user_id);
            int readnum = UserManager.GetReadNum(userinfoModel.user_id);

            var userInfo = new
            {
                Likes = likenum,
                Reads = readnum
            };

            return Ok(userInfo);
        }
        [HttpPost("userpostcomment")]
        public IActionResult GetUserPostComment([FromBody] UserInfoModel userinfoModel)
        {
            List<PostComment> usercomment = CommentPostManager.ShowUIDComment(userinfoModel.user_id);
            return Ok(usercomment);
        }
    }
}
