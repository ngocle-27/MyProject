using BE.Models;
using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace BE.Controllers
{
    [Route("api/changepwd")]
    [ApiController]
    public class ChangePwdController : ControllerBase
    {
        [HttpPut("{id}")]
        public IActionResult Post(int id,  ChangePassword chp)
        {
            var connectionString = "Data Source=DESKTOP-3UP7C4H; Initial Catalog =QLBH_KLTN; Integrated Security= true";
            IDbConnection db = new SqlConnection(connectionString);
            var rowAffect = db.Execute("update dbo.USER_ACC set UserPassword = '" + chp.CurrentPassword + @"' where UserId = '" + id + @"'", chp, commandType: CommandType.Text);
            if (rowAffect > 0)
                return Created("Put success", chp);
            else
                return NoContent();
        }
    }
}
