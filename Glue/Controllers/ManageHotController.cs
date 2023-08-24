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
using System.Collections.Generic;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860
//8.24头文件再问一下
namespace Glue.Controllers
{
    [Route("api")]
    [ApiController]
    public class ManageHotController : Controller
    {
        public class Pet
        {
            public string petId { get; set; }
            public string Name { get; set; }
            public int views { get; set; }
            public int likes { get; set; }
            public Pet()
            {
                petId = "";
                Name = "";
                views = 0;
                likes = 0;
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
        // 获取点赞量加阅读量最高的10个宠物信息，包括id、名字、阅读量、点赞量
        [HttpGet]
        [Route("top-pets")]
        public IHttpActionResult GetTopPets()
        {
            try
            {
                // 在这里编写获取点赞量和阅读量最高的10个宠物信息的逻辑
                List<Pet> topPets = GetTop10Pets();
                return Ok(topPets);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // 发布三个人气榜
        [HttpPost]
        [Route("publish-popularity-chart")]
        public IActionResult PublishPopularityChart([FromBody] List<int> selectedPetIds)
        {
            try
            {
                // 在这里编写发布人气榜的逻辑，使用selectedPetIds参数
                PublishPopularity(selectedPetIds);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // 获取用户发布的人气榜宠物信息
        [HttpGet]
        [Route("user-popularity-pets/{userId}")]
        public IActionResult GetUserPopularityPets(int userId)
        {
            try
            {
                // 在这里编写获取用户发布的人气榜宠物信息的逻辑，使用userId参数
                List<Pet> userPopularityPets = GetUserPopularityPets(userId);
                return Ok(userPopularityPets);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // 模拟获取点赞量和阅读量最高的10个宠物信息的方法
        private List<Pet> GetTop10Pets()
        {
            // 在这里编写获取点赞量和阅读量最高的10个宠物信息的逻辑
            // 返回一个包含宠物信息的List<Pet>对象
            return new List<Pet>();
        }

        // 模拟发布人气榜的方法
        private void PublishPopularity(List<int> selectedPetIds)
        {
            // 在这里编写发布人气榜的逻辑，使用selectedPetIds参数
        }

        // 模拟获取用户发布的人气榜宠物信息的方法
        private List<Pet> GetUserPopularityPets(int userId)
        {
            // 在这里编写获取用户发布的人气榜宠物信息的逻辑，使用userId参数
            // 返回一个包含宠物信息的List<Pet>对象
            return new List<Pet>();
        }

    }
}
}
