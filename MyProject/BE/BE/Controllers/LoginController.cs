using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BE.Models;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using System.Data.SqlClient;
using System.Data;
using Dapper;


namespace BE.Controllers
{
    [Route("api/login")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private IConfiguration _config;
        public LoginController(IConfiguration config)
        {
            _config = config;
        }

        [HttpPost]
        public ActionResult<ResponseLogin> Login([FromBody] Login login)
        {
            // ham goi database kiem tra user, password dung chua
            var connectionString = "Data Source=DESKTOP-3UP7C4H; Initial Catalog =QLBH_KLTN; Integrated Security= true";
            IDbConnection db = new SqlConnection(connectionString);
            string query = "select UserId, UserName,UserPassword  from dbo.USER_ACC where UserName = '" + login.UserName + @"' and UserPassword = '" + login.UserPassword + @"'";
            var rowAffect = db.Query<Login>(query, login, commandType: CommandType.Text);
            if (rowAffect.Count() <= 0)
            {
                HttpContext.Response.StatusCode = 401;
                return Ok(new ResponseLogin { Status = false, Token = null, Expirate = TimeSpan.Zero, StatusCode  = HttpContext.Response.StatusCode });
            }
                
            else
            {
                login.UserId = rowAffect.First().UserId;
                string query2 = "SELECT dbo.USER_FUNCTION.UserId, dbo.FUNCTION_ACC.FunctionName FROM dbo.FUNCTION_ACC INNER JOIN dbo.USER_FUNCTION ON dbo.FUNCTION_ACC.FunctionId = dbo.USER_FUNCTION.FunctionId where UserId = '" + login.UserId + @"'";
                //string query2 = "select UserFunctionId from dbo.USER_FUNCTION where UserId = '" + login.UserId + @"'";
                var roles = db.Query(query2, login, commandType: CommandType.Text);
                //string query3 = "SELECT dbo.CUSTOMER.CustomerId, dbo.USER_CUSTOMER.UserId, dbo.CUSTOMER.CustomerName, dbo.USER_FUNCTION.FunctionId, dbo.FUNCTION_ACC.FunctionName FROM dbo.CUSTOMER INNER JOIN dbo.USER_CUSTOMER ON dbo.CUSTOMER.CustomerId = dbo.USER_CUSTOMER.CustomerId INNER JOIN dbo.USER_ACC ON dbo.USER_CUSTOMER.UserId = dbo.USER_ACC.UserId INNER JOIN dbo.USER_FUNCTION ON dbo.USER_ACC.UserId = dbo.USER_FUNCTION.UserId INNER JOIN dbo.FUNCTION_ACC ON dbo.USER_FUNCTION.FunctionId = dbo.FUNCTION_ACC.FunctionId where CUSTOMER.CustomerId '";
                if (roles.Any())
                {
                    login.Role = roles.First().FunctionName;
                }    

                string token = GenerateJSONToken(login);
                return Ok(new ResponseLogin { Status = true, Token = token, Expirate = TimeSpan.FromHours(2), 
                    StatusCode = HttpContext.Response.StatusCode, Data = rowAffect });
            }
        }
        private Login AuthenticateUser(Login login)
        {
            Login user = null;
            if (login.UserName == "admin" && login.UserPassword == "123456")
            {
                user = new Login { UserName = "Admin", UserPassword = "123456" };
            }
            return user;
        }
        private string GenerateJSONToken(Login userinfo)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.NameId, userinfo.UserId.ToString()),
                new Claim(JwtRegisteredClaimNames.Sub,userinfo.UserName),
                new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Role, userinfo.Role)
            };
            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Issuer"],
                claims,
                expires: DateTime.Now.AddMinutes(120),
                signingCredentials: credentials);
            var encodetoeken = new JwtSecurityTokenHandler().WriteToken(token);
            return encodetoeken;

        }
        [Authorize]
        [HttpPost("Post")]
        public string Post()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            IList<Claim> claim = identity.Claims.ToList();
            var userName = claim[0].Value;
            return "Welcome" + userName;
        }
        [Authorize]
        [HttpGet("GetValue")]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] { "Value1", "Value2", "Value3" };
        }

    }
}

