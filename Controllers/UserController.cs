using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using TV_DASH_API.Models;

namespace TV_DASH_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _env;

        public UserController(IConfiguration configuration, IWebHostEnvironment env)
        {
            _configuration = configuration;
            _env = env;
        }
        //test
        //test
        [HttpGet("GetUsers")]
        [Authorize(Roles = "TVADMIN,TVMANAGER")]
        public JsonResult GetUsers()
        {
            string query = @"
                select
                    dbo.CommonEmployeesActive.Employee_ID,
                    dbo.CommonEmployeesActive.FirstName,
                    dbo.CommonEmployeesActive.LastName,
                    dbo.CommonEmployeesActive.MiddleName,
                    dbo.CommonEmployeesActive.FullName,
                    dbo.CommonEmployeesActive.Phone_number,
                    dbo.CommonEmployeesActive.BusinessP_number,
                    dbo.CommonEmployeesActive.Email_Address,
                    dbo.CommonEmployeesActive.Home_Address1,
                    dbo.CommonEmployeesActive.City,
                    dbo.CommonEmployeesActive.Country,
                    dbo.CommonEmployeesActive.Postal_ID,
                    dbo.CommonEmployeesActive.Employee_Status,
                    dbo.CommonEmployeesActive.Department,
                    dbo.CommonEmployeesActive.Direct_SuperiorID,
                    dbo.CommonEmployeesActive.Site_Location,
                    dbo.CommonEmployeesActive.isFulltime,
                    dbo.CommonEmployeesActive.NewAddress,
                    dbo.CommonEmployeesActive.NewPhoneNumber,
                    dbo.CommonEmployeesActive.UserID, 
                    dbo.CommonEmployeesActive.AreaID,
                    dbo.TVDash_CommonUser.Roles,
                    dbo.TVDash_CommonUser.User_id,
                    dbo.TVDash_Roles.rolename,
                    dbo.CommonPreference.buildingid,
                    dbo.CommonPreference.floorid,
                    dbo.CommonPreference.sr_preference_id,
                    dbo.CommonPreference.theme,
                    dbo.CommonPreference.fixedseat,
                    dbo.SRSeat.seat_number,
                    dbo.CommonPreference.user_telecommuting,
                    dbo.CommonPreference.zoneid
                from
	            dbo.CommonEmployeesActive left join dbo.TVDash_CommonUser
	            on 
	            dbo.CommonEmployeesActive.UserId = dbo.TVDash_CommonUser.Username
	            left join dbo.CommonPreference
	            on
	            dbo.CommonEmployeesActive.UserID = dbo.CommonPreference.username
                left join dbo.TVDash_Roles
                on 
                dbo.TVDash_CommonUser.Roles = dbo.TVDash_Roles.tvrolesid
                left join dbo.SRSeat
                on
                dbo.CommonPreference.fixedseat = dbo.SRSeat.seat_id
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

            return new JsonResult(table);
        }

        [HttpPost("GetUsersBySearchKeyword")]
        [Authorize(Roles = "TVADMIN")]
        public JsonResult GetUsersBySearchKeyword(SearchParameterModel searchParameterModel)
        {
            string query = @"
                select
                    dbo.CommonEmployeesActive.Employee_ID,
                    dbo.CommonEmployeesActive.FirstName,
                    dbo.CommonEmployeesActive.LastName,
                    dbo.CommonEmployeesActive.MiddleName,
                    dbo.CommonEmployeesActive.FullName,
                    dbo.CommonEmployeesActive.Phone_number,
                    dbo.CommonEmployeesActive.BusinessP_number,
                    dbo.CommonEmployeesActive.Email_Address,
                    dbo.CommonEmployeesActive.Home_Address1,
                    dbo.CommonEmployeesActive.City,
                    dbo.CommonEmployeesActive.Country,
                    dbo.CommonEmployeesActive.Postal_ID,
                    dbo.CommonEmployeesActive.Employee_Status,
                    dbo.CommonEmployeesActive.Department,
                    dbo.CommonEmployeesActive.Direct_SuperiorID,
                    dbo.CommonEmployeesActive.Site_Location,
                    dbo.CommonEmployeesActive.isFulltime,
                    dbo.CommonEmployeesActive.NewAddress,
                    dbo.CommonEmployeesActive.NewPhoneNumber,
                    dbo.CommonEmployeesActive.UserID, 
                    dbo.CommonEmployeesActive.AreaID,
                    dbo.TVDash_CommonUser.Roles,
                    dbo.TVDash_CommonUser.User_id,
                    dbo.TVDash_Roles.rolename,
                    dbo.CommonPreference.buildingid,
                    dbo.CommonPreference.floorid,
                    dbo.CommonPreference.sr_preference_id,
                    dbo.CommonPreference.theme,
                    dbo.CommonPreference.fixedseat,
                    dbo.SRSeat.seat_number,
                    dbo.CommonPreference.user_telecommuting,
                    dbo.CommonPreference.zoneid
                from
	            dbo.CommonEmployeesActive left join dbo.TVDash_CommonUser 
	            on 
	            dbo.CommonEmployeesActive.UserId = dbo.TVDash_CommonUser.Username
	            left join dbo.CommonPreference
	            on
	            dbo.CommonEmployeesActive.UserID = dbo.CommonPreference.username
                left join dbo.TVDash_Roles
                on 
                dbo.TVDash_CommonUser.Roles = dbo.TVDash_Roles.tvrolesid
                left join dbo.SRSeat
                on
                dbo.CommonPreference.fixedseat = dbo.SRSeat.seat_id
                    where 
                        dbo.CommonEmployeesActive.LastName LIKE '%" + searchParameterModel.Keyword + "%' OR dbo.TVDash_Roles.rolename LIKE '%" + searchParameterModel.Keyword + "%' OR dbo.CommonEmployeesActive.Employee_ID LIKE '%" + searchParameterModel.Keyword + "%' OR dbo.CommonEmployeesActive.FirstName LIKE '%" + searchParameterModel.Keyword + "%'";

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

        [HttpGet("GetUsersByManagerView")]
        [Authorize(Roles = "TVMANAGER")]
        public JsonResult GetUsersByManagerView(UserLogin userLogin)
        {
            string query = @"
                select
                    dbo.CommonEmployeesActive.Employee_ID,
                    dbo.CommonEmployeesActive.FirstName,
                    dbo.CommonEmployeesActive.LastName,
                    dbo.CommonEmployeesActive.MiddleName,
                    dbo.CommonEmployeesActive.FullName,
                    dbo.CommonEmployeesActive.Phone_number,
                    dbo.CommonEmployeesActive.BusinessP_number,
                    dbo.CommonEmployeesActive.Email_Address,
                    dbo.CommonEmployeesActive.Home_Address1,
                    dbo.CommonEmployeesActive.City,
                    dbo.CommonEmployeesActive.Country,
                    dbo.CommonEmployeesActive.Postal_ID,
                    dbo.CommonEmployeesActive.Employee_Status,
                    dbo.CommonEmployeesActive.Department,
                    dbo.CommonEmployeesActive.Direct_SuperiorID,
                    dbo.CommonEmployeesActive.Site_Location,
                    dbo.CommonEmployeesActive.isFulltime,
                    dbo.CommonEmployeesActive.NewAddress,
                    dbo.CommonEmployeesActive.NewPhoneNumber,
                    dbo.CommonEmployeesActive.UserID, 
                    dbo.CommonEmployeesActive.AreaID,
                    dbo.TVDash_CommonUser.Roles,
                    dbo.TVDash_CommonUser.User_id
                    dbo.CommonPreference.buildingid,
                    dbo.CommonPreference.floorid,
                    dbo.CommonPreference.sr_preference_id,
                    dbo.CommonPreference.theme,
                    dbo.CommonPreference.fixedseat,
                    dbo.SRSeat.seat_number,
                    dbo.CommonPreference.user_telecommuting,
                    dbo.CommonPreference.zoneid
                    from
	                dbo.CommonEmployeesActive left join dbo.TVDash_CommonUser 
	                on 
	                dbo.CommonEmployeesActive.UserId = dbo.TVDash_CommonUser.Username
	                left join dbo.CommonPreference
	                on
	                dbo.CommonEmployeesActive.UserID = dbo.CommonPreference.username
                    left join dbo.SRSeat
                    on
                    dbo.CommonPreference.fixedseat = dbo.SRSeat.seat_id
                    where dbo.CommonEmployeesActive.Direct_SuperiorID = '" + userLogin.Username + @"'
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

            return new JsonResult(table);
        }

        [HttpPost("GetUser")]
        [Authorize(Roles = "TVADMIN,TVMANAGER")]
        public JsonResult GetUser(UserLogin userLogin)
        {
            string query = @"
                select
                    dbo.CommonEmployeesActive.Employee_ID,
                    dbo.CommonEmployeesActive.FirstName,
                    dbo.CommonEmployeesActive.LastName,
                    dbo.CommonEmployeesActive.MiddleName,
                    dbo.CommonEmployeesActive.FullName,
                    dbo.CommonEmployeesActive.Phone_number,
                    dbo.CommonEmployeesActive.BusinessP_number,
                    dbo.CommonEmployeesActive.Email_Address,
                    dbo.CommonEmployeesActive.Home_Address1,
                    dbo.CommonEmployeesActive.City,
                    dbo.CommonEmployeesActive.Country,
                    dbo.CommonEmployeesActive.Postal_ID,
                    dbo.CommonEmployeesActive.Employee_Status,
                    dbo.CommonEmployeesActive.Department,
                    dbo.CommonEmployeesActive.Direct_SuperiorID,
                    dbo.CommonEmployeesActive.Site_Location,
                    dbo.CommonEmployeesActive.isFulltime,
                    dbo.CommonEmployeesActive.NewAddress,
                    dbo.CommonEmployeesActive.NewPhoneNumber,
                    dbo.CommonEmployeesActive.UserID, 
                    dbo.CommonEmployeesActive.AreaID,
                    dbo.TVDash_CommonUser.Roles,
                    dbo.TVDash_CommonUser.User_id,
                    dbo.CommonPreference.buildingid,
                    dbo.CommonPreference.floorid,
                    dbo.CommonPreference.sr_preference_id,
                    dbo.CommonPreference.theme,
                    dbo.CommonPreference.fixedseat,
                    dbo.SRSeat.seat_number,
                    dbo.CommonPreference.user_telecommuting,
                    dbo.CommonPreference.zoneid,
                    dbo.CommonEmployeePhoto.Path
                    from
	                dbo.CommonEmployeesActive left join dbo.TVDash_CommonUser 
	                on 
	                dbo.CommonEmployeesActive.UserId = dbo.TVDash_CommonUser.Username
	                left join dbo.CommonPreference
	                on
	                dbo.CommonEmployeesActive.UserID = dbo.CommonPreference.username
                    left join dbo.SRSeat
                    on
                    dbo.CommonPreference.fixedseat = dbo.SRSeat.seat_id
                    left join dbo.CommonEmployeePhoto
	                on
	                dbo.CommonEmployeesActive.Employee_ID = dbo.CommonEmployeePhoto.EmployeeID
                        where dbo.CommonEmployeesActive.UserID  = '" + userLogin.Username + @"'
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

            return new JsonResult(table);
        }


        [HttpPost("GetUserRoles")]
        [Authorize(Roles = "TVADMIN,TVMANAGER")]
        public JsonResult GetUserRoles(UserLogin userLogin)
        {
            string query = @"
                select * from 
                dbo.MBGSP_EmployeesAct
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

            return new JsonResult(table);
        }

        /*[HttpPost("SaveUserRoles")]
        [Authorize(Roles = "TVADMIN")]
        public JsonResult SaveUpdateUserRoles(UserRolesModel userRolesModel)
        {
            string query = "";

            if (userRolesModel.UserRolesID != 0)
            {
                query = "Update dbo.TVDash_CommonUser set Roles = '" + userRolesModel.Roles + @"' 
                        where User_id = " + userRolesModel.UserRolesID + @" 
                     ";
            }
            else
            {
                query = @"
                    insert into dbo.TVDash_CommonUser 
                    (Username,Roles)
                    values 
                    (
                    '" + userRolesModel.Username + @"'
                    ,'" + userRolesModel.Roles + @"'
                    )
                    ";
            }

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

            return new JsonResult("User Data Updated Successfully.");
        }

        [HttpPost("GetUserPreference")]
        [Authorize(Roles = "TVADMIN,TVMANAGER")]
        public JsonResult GetUserPreference(UserPreferenceModel userPreferenceModel)
        {
            string query = @"
                select sr_preference_id, theme, user_telecommuting, buildingid, floorid, zoneid from 
                dbo.CommonPreference
                where Username = '" + userPreferenceModel.Username + @"'
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

            return new JsonResult(table);
        }

        [HttpPost("SaveUserPreference")]
        [Authorize(Roles = "TVADMIN,TVMANAGER")]
        public JsonResult SaveUserPreference(UserPreferenceModel userPreferenceModel)
        {
            string query = @"
                insert into dbo.CommonPreference(theme, buildingid, floorid, zoneid, username)
                values 
                    (
                    '" + userPreferenceModel.Theme + @"'
                    ,'" + userPreferenceModel.BuildingId + @"'
                    ,'" + userPreferenceModel.FloorId + @"'
                    ,'" + userPreferenceModel.ZoneId + @"'
                    ,'" + userPreferenceModel.Username + @"'
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

            return new JsonResult("User Preference Added Successfully.");
        }

        [HttpPost("UpdateUserPreference")]
        [Authorize(Roles = "TVADMIN,TVMANAGER")]
        public JsonResult UpdateUserPreference(UserPreferenceModel userPreferenceModel)
        {
            string query = @"
                update dbo.CommonPreference set 
                    theme = '" + userPreferenceModel.Theme + @"'
                    ,buildingid = '" + userPreferenceModel.BuildingId + @"'
                    ,floorid = '" + userPreferenceModel.FloorId + @"'
                    ,zoneid = '" + userPreferenceModel.ZoneId + @"'
                    where sr_preference_id = " + userPreferenceModel.Sr_Preference_Id + @" 
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

            return new JsonResult("User Preference Updated Successfully.");
        }

        //[HttpPost("UpdateTelecommutingOfUser")]
        //[Authorize(Roles = "SeatR-01")]
        //public JsonResult UpdateTelecommutingOfUser(UserPreferenceModel userPreferenceModel)
        //{
        //    string query = @"
        //        update dbo.CommonPreference
        //        set user_telecommuting = '" +userPreferenceModel.UserTelecommuting+ @"'
        //        where username = '"+ userPreferenceModel.Username + @"'
        //        ";

        //    DataTable table = new DataTable();
        //    string sqlDataSource = _configuration.GetConnectionString("MBGSPMainDBCon");
        //    SqlDataReader myReader;

        //    using (SqlConnection myCon = new SqlConnection(sqlDataSource))
        //    {

        //        myCon.Open();
        //        using (SqlCommand myCommand = new SqlCommand(query, myCon))
        //        {
        //            myReader = myCommand.ExecuteReader();
        //            table.Load(myReader);

        //            myReader.Close();
        //            myCon.Close();
        //        }
        //    }

        //    return new JsonResult(table);
        //}

        [HttpPost("SaveTelecommutingOfUser")]
        [Authorize(Roles = "TVADMIN")]
        public JsonResult SaveTelecommutingOfUser(UserPreferenceModel userPreferenceModel)
        {
            string query = "";

            if (userPreferenceModel.Sr_Preference_Id != 0)
            {
                query = @"
                update dbo.CommonPreference
                set user_telecommuting = '" + userPreferenceModel.UserTelecommuting + @"',
                fixedseat = '" + userPreferenceModel.FixedSeat + @"'
                where username = '" + userPreferenceModel.Username + @"'
                ";
            }
            else
            {
                query = @"
                insert into dbo.CommonPreference(user_telecommuting, fixedseat, username)
                values 
                    (
                    '" + userPreferenceModel.UserTelecommuting + @"'
                    ,'" + userPreferenceModel.FixedSeat + @"'
                    ,'" + userPreferenceModel.Username + @"'
                    )
                ";
            }

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

            return new JsonResult("User Data Updated Successfully.");
        }

        [HttpGet("GetUserRolesOption")]
        [Authorize(Roles = "TVADMIN")]
        public JsonResult GetUserRolesOption()
        {
            string query = @"
                select * from 
                dbo.TVDash_Roles
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

            return new JsonResult(table);
        }

        [HttpPost("SaveUserInfo")]
        [Authorize]
        public JsonResult SaveUserInfo(UserInformationModel user)
        {
            string query = @"
                    insert into dbo.CommonEmployeesActive 
                    (Employee_ID,UserID,FirstName,MiddleName,LastName, Employee_Status, Site_Location, Site_LocationCode, Reg_Status)
                    values 
                    (
                    '" + user.EmployeeID + @"'
                    ,'" + user.UserID + @"'
                    ,'" + user.Firstname + @"'
                    ,'" + user.MiddleName + @"'
                    ,'" + user.LastName + @"'
                    ,'Active'
                    ,'" + user.LocationName + @"'
                    ,'" + user.Location + @"'
                    ,'External'
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

            return new JsonResult(table);
        }

        [HttpPost("SaveProfilePicture")]
        [Authorize]
        public JsonResult SaveProfilePicture(UserInformationModel user)
        {
            string query = @"
                    update dbo.CommonEmployeePhoto
                    set Path = '" + user.Image + @"'
                    where EmployeeID = '" + user.EmployeeID + @"'
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

            return new JsonResult("Successfully Updated Profile Photo.");
        }*/
    }
}
