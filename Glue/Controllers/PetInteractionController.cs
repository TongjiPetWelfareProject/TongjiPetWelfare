using Microsoft.AspNetCore.Mvc;
using PetFoster.BLL;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Glue.Controllers
{
    [Route("api")]
    [ApiController]
    public class PetInteractionController : ControllerBase
    {
        /*
        // GET: api/<PetInteractionController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<PetInteractionController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }
        */
        public class InteractionData
        {
            public string? user { get; set; }
            public string? pet { get; set; }
        }
        public class CommentData:InteractionData
        {
            public string? text { get; set; }
        }
        // POST api/<PetInteractionController>
        [HttpPost("pet-like")]
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
                LikePetManager.GiveALike(like_data.user, like_data.pet, true);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        // POST api/<PetInteractionController>
        [HttpPost("pet-favorite")]
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
                CollectPetInfoManager.GiveACollect(favorite_data.user, favorite_data.pet, true);
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
