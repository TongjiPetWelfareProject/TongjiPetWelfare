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
            public string user_name { get; set; }
            public string phone { get; set; }
            public string province { get; set; }
            public string city { get; set; }
            public UserInfoModel()
            {
                user_id = string.Empty;
                user_name = string.Empty;
                phone = string.Empty;
                province = string.Empty;
                city = string.Empty;
            }

        }
        [HttpPost("editinfo")]
        public IActionResult EditUserInfo([FromBody] UserInfoModel userinfoModel)
        {
            try
            {
                UserServer.UpdateUser2(userinfoModel.user_id, userinfoModel.user_name,
                    userinfoModel.phone, userinfoModel.city+","+userinfoModel.province );
                return Ok();
            }
            catch
            {
                return BadRequest("更改个人信息失败！");
            }
            
        }
        [HttpPost("userinfo")]
        public IActionResult GetUserInfo([FromBody] UserInfoModel userinfoModel)
        {
            int likenum = UserManager.GetLikeNum(userinfoModel.user_id);
            int readnum = UserManager.GetReadNum(userinfoModel.user_id);
            string user_name = UserServer.GetName(userinfoModel.user_id);
            string phone = UserServer.GetPhone(userinfoModel.user_id);
            string address = UserServer.GetAddress(userinfoModel.user_id);

            var userInfo = new
            {
                User_name = user_name, 
                Phone = phone,
                Address = JsonHelper.TranslateBackToChinese(address),
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
        [HttpPost("userpostsend")]
        public IActionResult GetUserPostSend([FromBody] UserInfoModel userinfoModel)
        {
            DataTable userposts = ForumPostManager.ShowUIDPosts(userinfoModel.user_id);
            string json = DataTableToJson(userposts);
            return Content(json, "application/json");
        }
        [HttpPost("usercollectpet")]
        public IActionResult GetUserCollectPet([FromBody] UserInfoModel userinfoModel)
        {
            DataTable collectpets = CollectPetInfoServer.GetCollectPetInfos(userinfoModel.user_id);
            string json = DataTableToJson(collectpets);
            return Content(json, "application/json");
        }
        [HttpPost("userpostlike")]
        public IActionResult GetUserLikePost([FromBody] UserInfoModel userinfoModel)
        {
            DataTable likedposts = LikePostServer.GetLikePosts(userinfoModel.user_id);
            string json = DataTableToJson(likedposts);
            return Content(json, "application/json");
        }
        [HttpPost("usercommentpet")]
        public IActionResult GetUserCommentPet([FromBody] UserInfoModel userinfoModel)
        {
            DataTable collectpets = CommentPetServer.GetCommentPets(userinfoModel.user_id);
            string json = DataTableToJson(collectpets);
            return Content(json, "application/json");
        }
        [HttpPost("useradoptpet")]
        public IActionResult GetUserAdoptPet([FromBody] UserInfoModel userinfoModel)
        {
            DataTable adoptedpets = AdoptApplyServer.GetAdoptPets(userinfoModel.user_id);
            string json = DataTableToJson(adoptedpets);
            return Content(json, "application/json");
        }
        [HttpPost("userfosterpet")]
        public IActionResult GetUserFosterPet([FromBody] UserInfoModel userinfoModel)
        {
            DataTable adoptedpets = FosterServer.GetFosterPets(userinfoModel.user_id);
            string json = DataTableToJson(adoptedpets);
            return Content(json, "application/json");
        }
        [HttpPost("userlikepet")]
        public IActionResult GetUserLikePet([FromBody] UserInfoModel userinfoModel)
        {
            DataTable collectpets = LikePetServer.GetLikePet(userinfoModel.user_id);
            string json = DataTableToJson(collectpets);
            return Content(json, "application/json");
        }
        [HttpPost("userdonation")]
        public IActionResult GetUserDonation([FromBody] UserInfoModel userinfoModel)
        {
            DataTable donation = DonationManager.DonateIDsForUser(userinfoModel.user_id);
            string json = DataTableToJson(donation);
            return Content(json, "application/json");
        }
        [HttpPost("usermedical")]
        public IActionResult GetUserMedical([FromBody] UserInfoModel userinfoModel)
        {
            DataTable medical = AppointmentManager.GetUserAppointment(userinfoModel.user_id);
            string json = DataTableToJson(medical);
            return Content(json, "application/json");
        }
        private string DataTableToJson(DataTable table)
        {
            var jsonString = new StringBuilder();

            if (table.Rows.Count > 0)
            {
                jsonString.Append("[");
                for (int i = 0; i < table.Rows.Count; i++)
                {
                    jsonString.Append("{");

                    for (int j = 0; j < table.Columns.Count; j++)
                    {
                        jsonString.AppendFormat("\"{0}\":\"{1}\"",
                            table.Columns[j].ColumnName,
                            table.Rows[i][j]);

                        if (j < table.Columns.Count - 1)
                        {
                            jsonString.Append(",");
                        }
                    }

                    jsonString.Append("}");
                    if (i < table.Rows.Count - 1)
                    {
                        jsonString.Append(",");
                    }
                }
                jsonString.Append("]");
            }

            return jsonString.ToString();
        }
    }
}
