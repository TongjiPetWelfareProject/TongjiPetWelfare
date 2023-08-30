using Microsoft.AspNetCore.Mvc;
using PetFoster.BLL;
using System.Data;
using PetFoster.DAL;
using System.Text.Json;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Glue.Controllers
{
    [Route("api")]
    [ApiController]
    public class RoomController : ControllerBase
    {
        public class RoomRecord
        {
            public string? roomId { get; set; }
            public string? roomStatus { get; set; }
            public int storey { get; set; }
            public string? lastCleaningTime { get; set; }
        }

        public class Room
        {
            public string? roomId { get; set; }
        }
        
        // GET: api/<RoomController>
        [HttpGet("room")]
        public IActionResult Get()
        {
            string jsondata;
            try
            {
                DataTable dt = RoomManager.ShowRooms();
                List<RoomRecord> RecordList = new List<RoomRecord>();

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    RoomRecord RoomItem = new RoomRecord();
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        if (dt.Columns[j].ColumnName.ToLower() == "compartment")
                        {
                            RoomItem.roomId = dt.Rows[i][j].ToString();
                        }
                        else if (dt.Columns[j].ColumnName.ToLower() == "room_status")
                        {
                            RoomItem.roomStatus = dt.Rows[i][j].ToString();
                            if(RoomItem.roomStatus.ToUpper() == "Y")
                            {
                                RoomItem.roomStatus = "空闲";
                            }
                            else if(RoomItem.roomStatus.ToUpper() == "N")
                            {
                                RoomItem.roomStatus = "占用";
                            }
                            else
                            {
                                RoomItem.roomStatus = "Invalid";
                            }
                        }
                        else if (dt.Columns[j].ColumnName.ToLower() == "storey")
                        {
                            RoomItem.storey = Convert.ToInt32(dt.Rows[i][j]);
                        }
                        else if (dt.Columns[j].ColumnName.ToLower() == "cleaning_time")
                        {
                            RoomItem.lastCleaningTime = dt.Rows[i][j].ToString();
                            //RoomItem.lastCleaningTime = ConvertTools.DbDateStringConvertToNormal(RoomItem.lastCleaningTime);
                        }
                    }
                    RoomItem.roomId = RoomItem.storey.ToString() + "-" + RoomItem.roomId;
                    
                    RecordList.Add(RoomItem);
                }

                jsondata = JsonSerializer.Serialize(RecordList);
                // Console.WriteLine(jsondata);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(500, ex.Message);
            }
            return Ok(jsondata);
        }

        /*
        // GET api/<RoomController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }
        */

        public bool parseRoomId(string roomId, out (short,short) result)
        {
            result = (0, 0);
            string[] parts = roomId.Split('-');
            if(parts.Length == 2 && short.TryParse(parts[0],out short storey) && short.TryParse(parts[1],out short compartment))
            {
                result = (storey, compartment);
                return true;
            }
            else
            {
                return false;
            }
        }

        // POST api/<RoomController>
        [HttpPost("send-room")]
        public IActionResult Post([FromBody] Room RoomData)
        {
            if(RoomData.roomId == null)
            {
                return BadRequest("Empty RoomId.");
            }
            if(!parseRoomId(RoomData.roomId,out(short,short) parsed_result))
            {
                return BadRequest("不正确的房间号格式");
            }
            short storey = parsed_result.Item1;
            short compartment = parsed_result.Item2;
            try
            {
                RoomServer.UpdateRoom(storey, compartment,true);
                // 打扫
                return Ok();
            }
            catch(Exception ex)
            {
                return(StatusCode(500, ex.Message));
            }
        }

        /*
        // PUT api/<RoomController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<RoomController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
        */
    }
}
