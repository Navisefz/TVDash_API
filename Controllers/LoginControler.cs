using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.DirectoryServices.AccountManagement;
using System.IdentityModel.Tokens;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Claims;
using TV_DASH_API.Models;
using System.IdentityModel.Tokens.Jwt;
using SymmetricSecurityKey = Microsoft.IdentityModel.Tokens.SymmetricSecurityKey;
//test
namespace TV_DASH_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        public LoginController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        //[AllowAnonymous]
        //[HttpPost]
        //public IActionResult Login([FromBody] UserLogin userLogin) {
        //    var user = Authenticate(userLogin);

        //    if (user != null) {
        //        var token = Generate(user);
        //        return Ok(token);
        //    }

        //    return NotFound("User not found");
        //}

        [AllowAnonymous]
        [HttpPost]
        public IActionResult Login(UserLogin userLogin)
        {
            PrincipalContext pc = new PrincipalContext(ContextType.Domain, "apac", "a399_s_0001", "d@iml3r1992");
            bool isValid = pc.ValidateCredentials(userLogin.Username, userLogin.Password);

            if (isValid == true)
            {
                string query = @"
                select * from 
                dbo.TVDash_CommonUser
                where Username = '" + userLogin.Username + @"'
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
                return Ok(table);
            }
            else
            {
                return NotFound("User not found");
            }
        }

        [AllowAnonymous]
        [HttpPost("AccessCheckpoint")]
        [Authorize(Roles = "TVADMIN,TVMANAGER")]
        public JsonResult AccessCheckPoint(UserLogin userLogin)
        {
            string query = @"
                select * from 
                dbo.TVDash_CommonUser
                where Username = '" + userLogin.Username + @"' AND (Roles = 'TVADMIN' OR Roles = 'TVMANAGER') ";

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

        [HttpPost("GenerateTokenByLogin")]
        public IActionResult GenerateTokenByLogin(UserModel userModel)
        {

            if (userModel != null)
            {
                var token = Generate(userModel);
                return Ok(token);
            }

            return NotFound("Invalid Action, Generation of Token Error");
        }

        [HttpPost("GenerateToken")]
        [Authorize(Roles = "TVADMIN,TVMANAGER")]
        public IActionResult UserEndpointGenerateToken(UserModel userModel)
        {

            if (userModel != null)
            {
                var token = Generate(userModel);
                return Ok(token);
            }

            return NotFound("Invalid Action, Generation of Token Error");
        }

       /* [HttpPost("GenerateTokenByDevs")]
        [Authorize(Roles = "SeatR-01,SeatR-02,SeatR-03")]
        public IActionResult GenerateTokenByDevs(GenerateTokenByDevsModel generateTokenByDevsModel)
        {

            if (generateTokenByDevsModel != null)
            {
                var token = GenerateByDevs(generateTokenByDevsModel);

                string query = @"
                    insert into dbo.CommonGeneratedTokenLogs 
                    (Access_By,UserGenerated,Datestamp)
                    values 
                    (
                    '" + generateTokenByDevsModel.Access_By + @"'
                    ,'" + generateTokenByDevsModel.UserGenerated + @"'
                    ,'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + @"'
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

                return Ok(token);
            }

            return NotFound("Invalid Action, Generation of Token Error");
        }*/

        [HttpGet]
      
        public IActionResult Public()
        {
            var currentUser = GetCurrentUser();

            return Ok(currentUser);
        }

        private string Generate(UserModel user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Username),
                new Claim(JwtRegisteredClaimNames.UniqueName, user.Role),
                new Claim(ClaimTypes.NameIdentifier, user.Username),
                new Claim(ClaimTypes.Role, user.Role),

            };

            var token = new JwtSecurityToken(_configuration["Jwt:Issuer"],
              _configuration["Jwt:Audience"],
              claims,
              expires: DateTime.Now.AddMinutes(60),
              signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }


     /*   private string GenerateByDevs(GenerateTokenByDevsModel generateTokenByDevsModel)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, generateTokenByDevsModel.UserGenerated),
                new Claim(JwtRegisteredClaimNames.UniqueName, generateTokenByDevsModel.Role),
                new Claim(ClaimTypes.NameIdentifier, generateTokenByDevsModel.UserGenerated),
                new Claim(ClaimTypes.Role, generateTokenByDevsModel.Role),

            };

            var token = new JwtSecurityToken(_configuration["Jwt:Issuer"],
              _configuration["Jwt:Audience"],
              claims,
              expires: DateTime.Now.AddMinutes(60),
              signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
     */
        private UserModel Authenticate(UserLogin userLogin)
        {
            var currentUser = UserContants.Users.FirstOrDefault(o => o.Username.ToLower() == userLogin.Username.ToLower() && o.Password == userLogin.Password);

            if (currentUser != null)
            {
                return currentUser;
            }

            return null;
        }

        private UserModel GetCurrentUser()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;

            if (identity != null)
            {
                var userClaims = identity.Claims;

                return new UserModel
                {
                    Username = userClaims.FirstOrDefault(o => o.Type == ClaimTypes.NameIdentifier)?.Value,
                    Role = userClaims.FirstOrDefault(o => o.Type == ClaimTypes.Role)?.Value,
                };
            }
            return null;
        }
    }
}
