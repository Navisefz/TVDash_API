/*using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TV_DASH_API.Models;

namespace TV_DASH_API.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class FloorController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _env;

        public FloorController(IConfiguration configuration, IWebHostEnvironment env)
        {
            _configuration = configuration;
            _env = env;
        }

        [HttpGet]
        [Authorize(Roles = "SeatR-01,SeatR-02,SeatR-03")]
        public JsonResult Get()
        {
            string query = @"
                select 
                TvDash_Floor.floor_id, 
                 TvDash_Floor.floorid, 
                 TvDash_Floor.floor_name, 
                 TvDash_Floor.is_Active, 
                 TvDash_Floor.building_id, 
                SRBuilding.building_name
                from 
                dbo. TvDash_Floor inner join dbo.SRBuilding 
                    on  TvDash_Floor.building_id = SRBuilding.building_id 
                        where  TvDash_Floor.is_Active = 1";

            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("MBGSPMainDBCon");
            SqlDataReader myReader;

            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {

                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);

                    myReader.Close();
                    myCon.Close();
                }
            }

            return new JsonResult(table);
        }

        [HttpPost]
        [Authorize(Roles = "SeatR-01")]
        public JsonResult Post(Floor floor)
        {
            floor.IsActive = 1; //IsActive 1 = true otherwise Deleted

            string query = @"
                    insert into dbo. TvDash_Floor 
                    (floor_name,is_Active, building_id)
                    values 
                    (
                    '" + floor.FloorName + @"'
                    ,'" + floor.IsActive + @"'
                    ,'" + floor.BuildingId + @"'
                    )
                    ";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("MBGSPMainDBCon");
            SqlDataReader myReader;

            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {

                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);

                    myReader.Close();
                    myCon.Close();

                }
            }

            return new JsonResult("Floor Added Successfully");
        }

        [HttpPost("UpdateFloor")]
        [Authorize(Roles = "SeatR-01")]
        public JsonResult Put(Floor floor)
        {
            string query = "";

            query = @"
                    update dbo. TvDash_Floor set 
                    floor_name = '" + floor.FloorName + @"'
                    ,building_id = '" + floor.BuildingId + @"'
                    where floor_id = " + floor.FloorId + @" 
                    ";

            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("MBGSPMainDBCon");
            SqlDataReader myReader;

            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {

                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);

                    myReader.Close();
                    myCon.Close();

                }
            }

            return new JsonResult("Floor Updated Successfully");
        }

        [HttpPost("RemoveFloor")]
        [Authorize(Roles = "SeatR-01")]
        public JsonResult RemoveFloor(Floor floor)
        {
            string query = "";

            query = @"
                    update dbo. TvDash_Floor set 
                    is_Active = '" + floor.IsActive + @"'
                    where floor_id = " + floor.FloorId + @" 
                    ";


            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("MBGSPMainDBCon");
            SqlDataReader myReader;

            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {

                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);

                    myReader.Close();
                    myCon.Close();

                }
            }

            return new JsonResult("Floor Updated Successfully");
        }

        [Route("SaveFile")]
        [HttpPost]
        public JsonResult SaveFile()
        {
            try
            {
                var httpRequest = Request.Form;
                var postedFile = httpRequest.Files[0];
                string filename = postedFile.FileName;
                var physicalPath = _env.ContentRootPath + "/Images/" + filename;

                using (var stream = new FileStream(physicalPath, FileMode.Create))
                {
                    postedFile.CopyTo(stream);
                }

                return new JsonResult(filename);
            }
            catch (Exception)
            {
                return new JsonResult("anonymous.png");
            }
        }

        [HttpPost("Search")]
        [Authorize(Roles = "SeatR-01")]
        public IActionResult Search([FromBody] Floor floor)
        {
            string query = @"
                select 
                 TvDash_Floor.floor_id, 
                 TvDash_Floor.floorid,
                 TvDash_Floor.floor_name, 
                 TvDash_Floor.is_Active, 
                 TvDash_Floor.building_id, 
                SRBuilding.building_name
                from dbo. TvDash_Floor inner join dbo.SRBuilding 
                    on  TvDash_Floor.building_id = SRBuilding.building_id 
                        where  TvDash_Floor.is_Active = 1 and  TvDash_Floor.floor_name LIKE '%" + floor.FloorName + "%'";

            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("MBGSPMainDBCon");
            SqlDataReader myReader;

            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {

                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);

                    myReader.Close();
                    myCon.Close();
                }
            }

            return new JsonResult(table);
        }
    }
}*/
