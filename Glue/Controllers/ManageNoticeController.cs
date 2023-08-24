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
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Glue.Controllers
{
    [Route("api")]
    [ApiController]
    public class ManageNoticeController : Controller
    {
        public class NoticeModel
        {
            public string employeeId { get; set; }
            public string title { get; set; }
            public string content { get; set; }
            public DateTime time { get; set; }
            public NoticeModel()
            {
                employeeId = "";
                title = "";
                content = "";
                time = "";
            }
        }


        // GET: api/<ManageAdoptController>
        /*[HttpGet("manage-adopt")]
        public IActionResult Get()
        {
            int censorstate = 0; //默认未审核
            try
            {
                DataTable dt = AdoptApplyManager.ShowCensorAdopt();
                List<AdoptionRecord> RecordList = new List<AdoptionRecord>();

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    AdoptionRecord RecordItem = new AdoptionRecord();
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        if (dt.Columns[j].ColumnName.ToLower() == "apply_date")
                        {
                            RecordItem.date = dt.Rows[i][j].ToString();
                        }
                        else if (dt.Columns[j].ColumnName.ToLower() == "pet_id")
                        {
                            RecordItem.petId = PetServer.GetName(dt.Rows[i][j].ToString());
                        }
                        else if (dt.Columns[j].ColumnName.ToLower() == "adopter_id")
                        {
                            RecordItem.userId = UserServer.GetName(dt.Rows[i][j].ToString());
                        }
                        else if (dt.Columns[j].ColumnName.ToLower() == "reason")
                        {
                            RecordItem.reason = dt.Rows[i][j].ToString();
                        }
                    }
                    RecordItem.censor_status = JsonHelper.GetErrorMessage("censor_state", censorstate);
                    RecordList.Add(RecordItem);
                }

                string jsondata = JsonSerializer.Serialize(RecordList);
                Console.WriteLine(jsondata);

                return Ok(jsondata);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }*/

        //added by rqx 8.24
        // 获取全部公告
        /*[HttpGet]
        [Route("notice")]
        public IActionResult getNotice()
        {
            try
            {
                // 在这里编写获取全部公告的逻辑
                // 返回公告数据
                return Ok(noticeData);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }*/

        // 获取捐赠记录
        /*[HttpGet]
        [Route("donation")]
        public IActionResult getDonation()
        {
            try
            {
                // 在这里编写获取捐赠记录的逻辑
                // 返回捐赠记录数据
                return Ok(donationData);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }*/

        // 发送新公告
        [HttpPost]
        [Route("send-notice")]
        public IActionResult sendNewNotice([FromBody] NoticeModel notice)
        {
            try
            {
                // 在这里编写发送新公告的逻辑
                // 返回发送结果
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // 获取公告内容
        /*[HttpGet]
        [Route("get-notice-content/{noticeId}")]
        public IActionResult getNoticeContent(int noticeId)
        {
            try
            {
                // 在这里编写获取公告内容的逻辑
                // 返回公告内容数据
                return Ok(noticeContent);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }*/

        // 发送编辑过的公告
        [HttpPost]
        [Route("send-edited-notice")]
        public IActionResult sendEditedNotice([FromBody] NoticeModel notice)
        {
            try
            {
                // 在这里编写发送编辑过的公告的逻辑
                // 返回发送结果
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // 删除公告
        [HttpDelete]
        [Route("delete-notice/{noticeId}")]
        public IActionResult deleteNotice(int noticeId)
        {
            try
            {
                // 在这里编写删除公告的逻辑
                // 返回删除结果
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
}
