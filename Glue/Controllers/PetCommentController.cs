using Glue.PetFoster.Model;
using Microsoft.AspNetCore.Mvc;
using PetFoster.BLL;
using PetFoster.DAL;
using PetFoster.Model;

namespace Glue.Controllers
{
    [Route("api")]
    [ApiController]
    public class PetCommentController : Controller
    {
        public class PetModel
        {
            public string? pet_id { get; set; }
            public string? user_id { get; set; }
            public string? pet_title { get; set; }
            public string? pet_content { get; set; }
            public string? added_comment { get; set; }
            public DateTime comment_time { get; set; }

        }

        [HttpGet("forum")]
        public IActionResult Post()
        {
            List<ForumPost> postlist = ForumPostManager.GetAllPosts();
            Console.WriteLine("收到论坛请求");
            return Ok(postlist);
        }

        [HttpPost("post")]
        public IActionResult PostInfo([FromBody] PetModel petModel)
        {
            List<ForumPost> post = ForumPostManager.ShowPost(petModel.pet_id);
            ForumPostServer.ReadForum(petModel.pet_id);
            Console.WriteLine("收到帖子请求：" + petModel.pet_id);
            return Ok(post);
        }

        [HttpPost("petcontent")]
        public IActionResult PostContent([FromBody] PetModel petModel)
        {
            string acstatus = UserServer.GetStatus(petModel.user_id);
            string role = UserServer.GetRole(petModel.user_id);
            if (acstatus == "Warning Issued" || acstatus == "Banned" || acstatus == "Appealing"
                || acstatus == "Probation")
                return BadRequest("您的账号活动异常，无法发布帖子");
            int status = ForumPostManager.PublishPost(petModel.user_id, petModel.pet_title, petModel.pet_content);
            if (role == "Admin")
                ForumPostManager.CensorPost(status.ToString(), false);
            Console.WriteLine("收到发帖请求：" + petModel.pet_id);
            if (status != -1)
                return Ok(0);
            else
                return BadRequest(-1);
        }

        [HttpPost("postcomment")]
        public IActionResult PostComment([FromBody] PetModel petModel)
        {
            List<PostComment> AllComment = CommentPostManager.ShowPIDPost(petModel.pet_id);
            Console.WriteLine("收到帖子评论列表请求：" + petModel.pet_id);
            return Ok(AllComment);
        }

        [HttpPost("addcomment")]
        public IActionResult AddComment([FromBody] PetModel petModel)
        {
            Console.WriteLine("收到帖子评论请求,帖子ID：" + petModel.pet_id + "评论内容：" + petModel.added_comment + "评论人ID：" + petModel.user_id);
            int status = CommentPostManager.GiveACommentPost(petModel.user_id, petModel.pet_id, petModel.added_comment);
            if (status != -1)
                return Ok("评论成功");
            return BadRequest(-1);
        }
        [HttpPost("likepost")]
        public IActionResult LikePet([FromBody] PetModel petModel)
        {
            Console.WriteLine("收到点赞帖子请求,帖子ID：" + petModel.pet_id + "点赞人ID：" + petModel.user_id);
            LikePetManager.GiveALike(petModel.user_id, petModel.pet_id);
            return Ok("点赞或取消成功");
        }
        [HttpPost("iflikepost")]
        public IActionResult IfLikePet([FromBody] PetModel petModel)
        {
            Console.WriteLine("收到获取点赞信息请求,帖子ID：" + petModel.pet_id + "点赞人ID：" + petModel.user_id);
            int status = LikePetManager.IfLike(petModel.user_id, petModel.pet_id);
            return Ok(status);
        }
        [HttpPost("deletecomment")]
        public IActionResult DeleteComment([FromBody] PetModel petModel)
        {
            Console.WriteLine("收到获取点赞信息请求,帖子ID：" + petModel.pet_id + "点赞人ID：" + petModel.user_id);
            CommentPostManager.UndoACommentPost(petModel.user_id, petModel.pet_id, petModel.comment_time);
            return Ok();
        }
        [HttpPost("deletepost")]
        public IActionResult DeletePet([FromBody] PetModel petModel)
        {
            bool status = ForumPostManager.DeleteForumProfile(petModel.pet_id, petModel.user_id);
            Console.WriteLine("收到删除帖子请求：" + petModel.pet_id + "；用户id：" + petModel.user_id);
            if (status)
                return Ok();
            else
                return BadRequest();
        }
    }
}
