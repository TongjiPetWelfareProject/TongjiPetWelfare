using Microsoft.AspNetCore.Mvc;
using PetFoster.BLL;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Glue.Controllers
{
    [Route("api")]
    [ApiController]
    public class PetAdoptController : ControllerBase
    {
        public class AdoptData
        {
            public string? user { get; set; }
            public string? pet { get; set; }
            public string? gender { get; set; }
            public string? pet_exp { get; set; }
            public string? long_term_care { get; set; }
            public string? w_to_treat { get; set; }
            public decimal d_care_h { get; set; }
            public string? P_caregiver { get; set; }
            public decimal f_popul { get; set; }
            public string? be_children { get; set; }
            public string? accept_vis { get; set; }
        }

        /*
        // GET: api/<PetAdoptController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<PetAdoptController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }
        */

        // POST api/<PetAdoptController>
        [HttpPost]
        public IActionResult Post([FromBody] AdoptData adopt_table)
        {
            if (adopt_table == null)
            {
                return BadRequest("Invalid data.");
            }

            bool gender = (adopt_table.gender == "M") ? true : false;
            bool pet_exp = (adopt_table.pet_exp == "Y") ? true : false;
            bool long_term_care = (adopt_table.long_term_care == "Y") ? true : false;
            bool w_to_treat = (adopt_table.w_to_treat == "Y") ? true : false;
            bool be_children = (adopt_table.be_children == "Y") ? true : false;
            bool accept_vis = (adopt_table.accept_vis == "Y") ? true : false;

            try
            {
                AdoptApplyManager.ApplyAdopt(adopt_table.user, adopt_table.pet,
                    gender, pet_exp, long_term_care,
                    w_to_treat, adopt_table.d_care_h, adopt_table.P_caregiver,
                    adopt_table.f_popul, be_children, accept_vis);
                return Ok();
            }
            catch (Exception ex)
            {
                return NotFound("不存在的用户或宠物");
            }
        }

        /*
        // PUT api/<PetAdoptController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<PetAdoptController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
        */
    }
}
