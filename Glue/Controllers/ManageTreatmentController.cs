using Microsoft.AspNetCore.Mvc;
using PetFoster.BLL;
using PetFoster.DAL;
using System.Data;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Glue.Controllers
{
    [Route("api")]
    [ApiController]
    public class ManageTreatmentController : ControllerBase
    {
        // GET: api/<ManageTreatmentController>
        [HttpGet("treatlist")]
        public IActionResult GetTreatList()
        {
            try
            {
                DataTable dt = AppointmentManager.ShowApplies();

                string jsondata = ConvertTools.DataTableToJson(dt);
                //Console.WriteLine(jsondata);
                return Content(jsondata, "application/json");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(500, ex.Message);
            }
        }
        /*
        // GET api/<ManageTreatmentController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }
        */
        public class MedicalRecord
        {
            public string? pet { get; set; }
            public string? vet { get; set; }
            public string? reserveTime { get; set; } // 原来的预约时间
        }
        public class PostPoneRecord: MedicalRecord
        {
            public string? newReserveTime { get; set; } //选择延期到这个时间
        }
        // POST api/<ManageTreatmentController>
        // 完成医疗
        [HttpPost("approve-medical-application")]
        public IActionResult PostApprove([FromBody] MedicalRecord record)
        {
            if(record == null)
            {
                return BadRequest("Empty Data.");
            }
            if(record.pet == null || !int.TryParse(record.pet, out int pid))
            {
                return BadRequest("Invalid Pet Id.");
            }
            if(record.vet == null || !int.TryParse(record.vet, out int vid))
            {
                return BadRequest("Invalid Vet ID.");
            }
            // 转换时间为DateTime格式
            DateTime? time = ConvertTools.StringConvertToDate(record.reserveTime);
            if(time == null)
            {
                //时间为空或格式错误，直接返回
                return BadRequest("Invalid DateTime Format.");
            }
            // 调试
            if(time.Value.DayOfWeek== DayOfWeek.Saturday||
                time.Value.DayOfWeek==DayOfWeek.Sunday||
                time.Value.Day-DateTime.Now.Day<0||
                time.Value.Hour>=8||time.Value.Hour<=17) {
                return BadRequest("我们只在周一到周五8点到17点营业");
            }
            AppointmentManager.DoneTreatment(pid,vid,time.Value);
            try
            {
                // logic:完成并结束医疗
                return Ok();
            }
            catch(Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // POST api/<ManageTreatmentController>
        // 延期
        [HttpPost("postpone-medical-application")]
        public IActionResult PostPostpone([FromBody] PostPoneRecord record)
        {
            if (record == null)
            {
                return BadRequest("Empty Data.");
            }
            if (record.pet == null || !int.TryParse(record.pet, out int pid))
            {
                return BadRequest("Invalid Pet Id.");
            }
            if (record.vet == null || !int.TryParse(record.vet, out int vid))
            {
                return BadRequest("Invalid Vet ID.");
            }
            // 转换时间为DateTime格式
            DateTime? origin_time = ConvertTools.StringConvertToDate(record.reserveTime);
            if (origin_time == null)
            {
                //时间为空或格式错误，直接返回
                return BadRequest("reserveTime: Invalid DateTime Format.");
            }
            DateTime? postpone_time = ConvertTools.StringConvertToDate(record.newReserveTime);
            if (postpone_time == null)
            {
                //时间为空或格式错误，直接返回
                return BadRequest("postponeTime: Invalid DateTime Format.");
            }
            // 调试
            if (origin_time.Value.Day - postpone_time.Value.Day > 0 ||
                origin_time.Value.Day - postpone_time.Value.Day < -7 ||
                postpone_time.Value.DayOfWeek == DayOfWeek.Saturday ||
                postpone_time.Value.DayOfWeek == DayOfWeek.Sunday)
                return BadRequest("请在一周内的工作日延迟预约");
            AppointmentServer.UpdateAppointment(vid, pid,origin_time.Value,postpone_time.Value);
            try
            {
                // logic:延期
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        /*
        // PUT api/<ManageTreatmentController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<ManageTreatmentController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
        */
    }
}
