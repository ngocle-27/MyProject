using BE.Models;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BE.Controllers
{
    [Route("api/useraccs")]
    [ApiController]
    public class UserAccsController : ControllerBase
    {
        // GET: api/<UserAccsController>
        [HttpGet]
        public IActionResult Get()
        {
            var connectionString = "Data Source=DESKTOP-3UP7C4H; Initial Catalog =QLBH_KLTN; Integrated Security= true";
            IDbConnection db = new SqlConnection(connectionString);
            var users = db.Query<UserAcc>("select UserId, UserName,UserFullName ,IsActive from dbo.USER_ACC", commandType: CommandType.Text);
            return Ok(users);
        }

        // GET api/<UserAccsController>/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var connectionString = "Data Source=DESKTOP-3UP7C4H; Initial Catalog =QLBH_KLTN; Integrated Security= true";
            IDbConnection db = new SqlConnection(connectionString);
            var users = db.Query<UserAcc>(" select * from dbo.USER_ACC  where UserId = '" + id + @"'", new { UserId = id }, commandType: CommandType.Text);
            return Ok(users);
        }

        // POST api/<UserAccsController>
        [HttpPost]
        public IActionResult Post(UserAcc user)
        {
            var connectionString = "Data Source=DESKTOP-3UP7C4H; Initial Catalog =QLBH_KLTN; Integrated Security= true";
            IDbConnection db = new SqlConnection(connectionString);
            var rowAffect = db.Execute("insert into dbo.USER_ACC values ('" + user.UserName + @"', N'" + user.UserFullName + @"', '" + user.UserPassword + @"', '" + user.IsActive + @"')", user, commandType: CommandType.Text);
            if (rowAffect > 0)
                return Created("Post success", user);
            else
                return NoContent();
        }



        // DELETE api/<UserAccsController>/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var connectionString = "Data Source=DESKTOP-3UP7C4H; Initial Catalog =QLBH_KLTN; Integrated Security= true";
            IDbConnection db = new SqlConnection(connectionString);
            var rowAffect = db.Execute("delete dbo.USER_ACC  where UserId = '" + id + @"'", id, commandType: CommandType.Text);
            if (rowAffect > 0)
                return Created("Delete success", id);
            else
                return NoContent();

        }
        [HttpPut("admin-changepwd/{id}")]
        public IActionResult Put(int id, [FromBody] UserAcc user)
        {
            var connectionString = "Data Source=DESKTOP-3UP7C4H; Initial Catalog =QLBH_KLTN; Integrated Security= true";
            IDbConnection db = new SqlConnection(connectionString);
            var users = db.Query<UserAcc>(" select * from dbo.USER_ACC  where UserId = '" + id + @"'", new { UserId = id }, commandType: CommandType.Text);
            var usersOld = users.First();
            usersOld.UserPassword = user.UserPassword;
            string query = "update dbo.USER_ACC set UserPassword = '" + usersOld.UserPassword + @"' where UserId  = '" + id + @"'";
            var rowAffect = db.Execute(query, user, commandType: CommandType.Text);
            if (rowAffect > 0)
                return Created("Put success", usersOld);
            else
                return NoContent();
        }

        
        [HttpPut("admin-lock/{id}")]
        public IActionResult PutLock(int id, [FromBody] UserAcc user)
        {
            var connectionString = "Data Source=DESKTOP-3UP7C4H; Initial Catalog =QLBH_KLTN; Integrated Security= true";
            IDbConnection db = new SqlConnection(connectionString);
            var users = db.Query<UserAcc>(" select * from dbo.USER_ACC  where UserId = '" + id + @"'", new { UserId = id }, commandType: CommandType.Text);
            var usersOld = users.First();
            usersOld.IsActive = user.IsActive;
            string query = "update dbo.USER_ACC set IsActive = 0 where UserId  = '" + id + @"'";
            var rowAffect = db.Execute(query, user, commandType: CommandType.Text);
            if (rowAffect > 0)
                return Created("Put success", usersOld);
            else
                return NoContent();
        }
        [HttpPut("admin-unlock/{id}")]
        public IActionResult PutUnlock(int id, [FromBody] UserAcc user)
        {
            var connectionString = "Data Source=DESKTOP-3UP7C4H; Initial Catalog =QLBH_KLTN; Integrated Security= true";
            IDbConnection db = new SqlConnection(connectionString);
            var users = db.Query<UserAcc>(" select * from dbo.USER_ACC  where UserId = '" + id + @"'", new { UserId = id }, commandType: CommandType.Text);
            var usersOld = users.First();
            usersOld.IsActive = user.IsActive;
            string query = "update dbo.USER_ACC set IsActive = 1 where UserId  = '" + id + @"'";
            var rowAffect = db.Execute(query, user, commandType: CommandType.Text);
            if (rowAffect > 0)
                return Created("Put success", usersOld);
            else
                return NoContent();
        }
        [HttpGet("filter")]
        public IActionResult GetUserFilter(UserAcc username)
        {
            var connectionString = "Data Source=DESKTOP-3UP7C4H; Initial Catalog =QLBH_KLTN; Integrated Security= true";
            IDbConnection db = new SqlConnection(connectionString);
            var users = db.Query<UserAcc>("select  UserName,UserFullName ,IsActive from dbo.USER_ACC where UserName  LIKE '%' + '" + username.UserName + @"'+'%'", commandType: CommandType.Text);
            return Ok(users);
        }

        
    }
}
