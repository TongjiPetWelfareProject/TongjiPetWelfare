using Microsoft.AspNetCore.Mvc;
using PetFoster.BLL;
using PetFoster.DAL;
using System.Data;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Glue.Controllers
{
    [Route("api")]
    [ApiController]
    public class PetInteractionController : ControllerBase
    {
        public class InteractionData
        {
            public string? user { get; set; }
            public string? pet { get; set; }
        }
        public class CommentData : InteractionData
        {
            public string text { get; set; }
            public string time { get; set; }
            public CommentData()
            {
                text = "";
                time = "";
            }
        }
        
        // GET: api/<PetInteractionController>
        [HttpGet("comment-list")]
        public IActionResult Get()
        {
            try
            {
                DataTable dt = CommentPetManager.ShowCommentPet();
                List<CommentData> CommentsList = new List<CommentData>();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    CommentData CommentItem = new CommentData();
                    string? date = null;
                    string? time = null;
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        if (dt.Columns[j].ColumnName.ToLower() == "user_id")
                        {
                            CommentItem.user = dt.Rows[i][j].ToString();
                        }
                        else if (dt.Columns[j].ColumnName.ToLower() == "pet_id")
                        {
                            CommentItem.pet = dt.Rows[i][j].ToString();
                        }
                        else if (dt.Columns[j].ColumnName.ToLower() == "comment_contents")
                        {
                            CommentItem.text = dt.Rows[i][j].ToString();
                        }
                        else if (dt.Columns[j].ColumnName.ToLower() == "comment_date")
                        {
                            date = dt.Rows[i][j].ToString();
                        }
                        else if(dt.Columns[j].ColumnName.ToLower() == "comment_time")
                        {
                            time = dt.Rows[i][j].ToString();
                        }
                    }
                    if(!string.IsNullOrEmpty(date) && !string.IsNullOrEmpty(time))
                    {
                        CommentItem.time = date + time;
                    }
                    CommentsList.Add(CommentItem);
                }
                return Ok(CommentsList);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        /*
        // GET api/<PetInteractionController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }
        */

        // POST api/<PetInteractionController>
        [HttpPost("pet-submit-like")]
        public IActionResult PostLike([FromBody] InteractionData like_data)
        {
            if(like_data == null)
            {
                return BadRequest("Invalid Data");
            }
            if(like_data.user == null)
            {
                return BadRequest("Invalid User_Id");
            }
            if(like_data.pet == null)
            {
                return BadRequest("Invalid Pet_Id");
            }
            try
            {
                if(!LikePetServer.GetLikePetEntry(like_data.user, like_data.pet))
                    LikePetManager.GiveALike(like_data.user, like_data.pet, true);
                else
                    LikePetManager.GiveALike(like_data.user, like_data.pet, false);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        // POST api/<PetInteractionController>
        [HttpPost("pet-cancel-like")]
        public IActionResult PostUndoLike([FromBody] InteractionData like_data)
        {
            if (like_data == null)
            {
                return BadRequest("Invalid Data");
            }
            if (like_data.user == null)
            {
                return BadRequest("Invalid User_Id");
            }
            if (like_data.pet == null)
            {
                return BadRequest("Invalid Pet_Id");
            }
            try
            {
                LikePetManager.GiveALike(like_data.user, like_data.pet, false);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        // POST api/<PetInteractionController>
        [HttpPost("pet-submit-favorite")]
        public IActionResult PostFavorite([FromBody] InteractionData favorite_data)
        {
            if (favorite_data == null)
            {
                return BadRequest("Invalid Data");
            }
            if (favorite_data.user == null)
            {
                return BadRequest("Invalid User_Id");
            }
            if (favorite_data.pet == null)
            {
                return BadRequest("Invalid Pet_Id");
            }
            try
            {
                if(!CollectPetInfoServer.GetCollectPetInfoEntry(favorite_data.user, favorite_data.pet))
                    CollectPetInfoServer.InsertCollectPetInfo(favorite_data.user, favorite_data.pet);
                else
                    CollectPetInfoServer.DeleteCollectPetInfo(favorite_data.user, favorite_data.pet);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        // POST api/<PetInteractionController>
        [HttpPost("pet-cancel-favorite")]
        public IActionResult PostUndoFavorite([FromBody] InteractionData favorite_data)
        {
            if (favorite_data == null)
            {
                return BadRequest("Invalid Data");
            }
            if (favorite_data.user == null)
            {
                return BadRequest("Invalid User_Id");
            }
            if (favorite_data.pet == null)
            {
                return BadRequest("Invalid Pet_Id");
            }
            try
            {
                CollectPetInfoManager.GiveACollect(favorite_data.user, favorite_data.pet, false);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        // POST api/<PetInteractionController>
        [HttpPost("pet-submit-comment")]
        public IActionResult PostComment([FromBody] CommentData comment_data)
        {
            if (comment_data == null)
            {
                return BadRequest("Invalid Data");
            }
            if (comment_data.user == null)
            {
                return BadRequest("Invalid User_Id");
            }
            if (comment_data.pet == null)
            {
                return BadRequest("Invalid Pet_Id");
            }
            try
            {
                CommentPetManager.GiveAComment(comment_data.user, comment_data.pet, comment_data.text);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        // POST api/<PetInteractionController>
        [HttpPost("pet-delete-comment")]
        public IActionResult PostUndoComment([FromBody] CommentData comment_data)
        {
            if (comment_data == null)
            {
                return BadRequest("Invalid Data");
            }
            if (comment_data.user == null)
            {
                return BadRequest("Invalid User_Id");
            }
            if (comment_data.pet == null)
            {
                return BadRequest("Invalid Pet_Id");
            }
            DateTime? time = ConvertTools.StringConvertToDate(comment_data.time);
            if(time == null)
            {
                return BadRequest("Failed to parse the date.");
            }
            try
            {
                CommentPetManager.UndoAComment(comment_data.user, comment_data.pet, (DateTime)time);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        /*
        // PUT api/<PetInteractionController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<PetInteractionController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
        */
    }
}
