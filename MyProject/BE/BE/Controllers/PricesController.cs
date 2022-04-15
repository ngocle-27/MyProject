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
    [Route("api/prices")]
    [ApiController]
    public class PricesController : ControllerBase
    {
        // GET: api/<PricesController>
        [HttpGet]
        [HttpGet]
        public IActionResult Get()
        {
            var connectionString = "Data Source=DESKTOP-3UP7C4H; Initial Catalog =QLBH_KLTN; Integrated Security= true";
            IDbConnection db = new SqlConnection(connectionString);
            var prices = db.Query<Price>("select PriceId, PriceName,UnitPrice from dbo.PRICE", commandType: CommandType.Text);
            return Ok(prices);
        }
    

        // GET api/<PricesController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<PricesController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<PricesController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<PricesController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
