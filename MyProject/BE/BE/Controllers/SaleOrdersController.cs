using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using System.Data;
using System.Data.SqlClient;
using BE.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using BE.Extention;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BE.Controllers
{
    [Route("api/saleorders")]
    [ApiController]
    [Authorize]
    public class SaleOrderController : ControllerBase
    {
        public static List<SaleOrder> saleOrders= new List<SaleOrder>();
        private readonly IHttpContextAccessor _httpContextAccessor;
        public SaleOrderController(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        // GET: api/<SaleOrdersController>
        [HttpGet]
        public IActionResult Get()
        {
            var connectionString = "Data Source=DESKTOP-3UP7C4H; Initial Catalog =QLBH_KLTN; Integrated Security= true";
            IDbConnection db = new SqlConnection(connectionString);
            var role = _httpContextAccessor.Role();
            if(role == "Admin")
            {
                var saleorders = db.Query<SaleOrder>("SELECT dbo.AREA.AreaName, dbo.AREA.AreaId AS Expr1, dbo.CUSTOMER.CustomerId AS Expr2, dbo.CUSTOMER.CustomerName, dbo.ITEM.ItemId AS Expr3, dbo.ITEM.ItemName, dbo.USER_ACC.UserId AS Expr9, dbo.USER_ACC.UserName, dbo.SALE_ORDER.* FROM dbo.AREA INNER JOIN dbo.SALE_ORDER ON dbo.AREA.AreaId = dbo.SALE_ORDER.AreaId INNER JOIN dbo.CUSTOMER ON dbo.SALE_ORDER.CustomerId = dbo.CUSTOMER.CustomerId INNER JOIN dbo.ITEM ON dbo.SALE_ORDER.ItemId = dbo.ITEM.ItemId INNER JOIN dbo.USER_ACC ON dbo.SALE_ORDER.UserId = dbo.USER_ACC.UserId", commandType: CommandType.Text);
                // var saleorders = db.Query<SaleOrder>("dbo.View_List", commandType: CommandType.StoredProcedure);
                //var saleorders = db.Query<SaleOrder>("select * from dbo.SALE_ORDER", commandType: CommandType.Text);
                return Ok(saleorders);
            }
            else
            {
                var userId = _httpContextAccessor.UserId();
                var saleorders = db.Query<SaleOrder>("SELECT dbo.AREA.AreaName, dbo.AREA.AreaId AS Expr1, dbo.CUSTOMER.CustomerId AS Expr2, dbo.CUSTOMER.CustomerName, dbo.ITEM.ItemId AS Expr3, dbo.ITEM.ItemName, dbo.USER_ACC.UserId AS Expr9, dbo.USER_ACC.UserName, dbo.SALE_ORDER.* FROM dbo.AREA INNER JOIN dbo.SALE_ORDER ON dbo.AREA.AreaId = dbo.SALE_ORDER.AreaId INNER JOIN dbo.CUSTOMER ON dbo.SALE_ORDER.CustomerId = dbo.CUSTOMER.CustomerId INNER JOIN dbo.ITEM ON dbo.SALE_ORDER.ItemId = dbo.ITEM.ItemId INNER JOIN dbo.USER_ACC ON dbo.SALE_ORDER.UserId = dbo.USER_ACC.UserId Where dbo.SALE_ORDER.UserId = '" + userId + @"' ", commandType: CommandType.Text);
                // var saleorders = db.Query<SaleOrder>("dbo.View_List", commandType: CommandType.StoredProcedure);
                //var saleorders = db.Query<SaleOrder>("select * from dbo.SALE_ORDER", commandType: CommandType.Text);
                return Ok(saleorders);
            }    
            
        }

        // GET api/<SaleOrdersController>/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var connectionString = "Data Source=DESKTOP-3UP7C4H; Initial Catalog =QLBH_KLTN; Integrated Security= true";
            IDbConnection db = new SqlConnection(connectionString);
            var saleorders = db.Query<SaleOrder>(" select * from dbo.SALE_ORDER  where SaleOrderId = '" + id + @"'", new {SaleOrderId = id }, commandType: CommandType.Text);
              
            return Ok(saleorders);
        }
        [HttpGet("filter/statusorder")]
        public IActionResult GetFilter( [FromBody] SaleOrder saleorder)
        {
            var connectionString = "Data Source=DESKTOP-3UP7C4H; Initial Catalog =QLBH_KLTN; Integrated Security= true";
            IDbConnection db = new SqlConnection(connectionString);
            var saleorders = db.Query<SaleOrder>(" SELECT dbo.AREA.AreaName, dbo.AREA.AreaId AS Expr1, dbo.CUSTOMER.CustomerId AS Expr2, dbo.CUSTOMER.CustomerName, dbo.ITEM.ItemId AS Expr3, dbo.ITEM.ItemName, dbo.USER_ACC.UserId AS Expr9, dbo.USER_ACC.UserName, dbo.SALE_ORDER.* FROM dbo.AREA INNER JOIN dbo.SALE_ORDER ON dbo.AREA.AreaId = dbo.SALE_ORDER.AreaId INNER JOIN dbo.CUSTOMER ON dbo.SALE_ORDER.CustomerId = dbo.CUSTOMER.CustomerId INNER JOIN dbo.ITEM ON dbo.SALE_ORDER.ItemId = dbo.ITEM.ItemId INNER JOIN dbo.USER_ACC ON dbo.SALE_ORDER.UserId = dbo.USER_ACC.UserId Where dbo.SALE_ORDER.StatusOrder = N'" + saleorder.StatusOrder + @"'", commandType: CommandType.Text);
            return Ok(saleorders);
        }
        // POST api/<SaleOrdersController>
        [HttpPost]
         public IActionResult Post([FromBody]SaleOrder saleorder)
         {
            int userId = _httpContextAccessor.UserId();
             var connectionString = "Data Source=DESKTOP-3UP7C4H; Initial Catalog =QLBH_KLTN; Integrated Security= true";
             IDbConnection db = new SqlConnection(connectionString);
            var sequence = db.Query<Int64>("select next value for DeliveryOrder");
            string sequenceString = sequence.First().ToString();
            string deliveryCode = null;
            int index = 5 - sequenceString.Length;
            for(var i = 0; i < index; i++)
            {
                deliveryCode = "0" + deliveryCode;
            }
            deliveryCode = deliveryCode + sequenceString + "-" + DateTime.Now.Year.ToString().Substring(2,2);
            saleorder.DeliverCode = deliveryCode;
            string statusOrder = "Đã đặt hàng";
            saleorder.StatusOrder = statusOrder;
            DateTime orderDate = DateTime.Now;
            saleorder.OrderDate = orderDate;
            saleorder.Total = saleorder.UnitPrice * saleorder.OrderQuantity + saleorder.UnitPrice;
            var rowAffect = db.Execute("insert into dbo.SALE_ORDER(CustomerId,OrderType,TransportMethod,AreaId,ItemId,OrderQuantity,UnitPrice,PriceId,DescriptionOrd,DeliverCode,StatusOrder,OrderDate,UserId) values ('" + saleorder.CustomerId + @"',N'" + saleorder.OrderType + @"',N'" + saleorder.TransportMethod + @"','" + saleorder.AreaId + @"','" + saleorder.ItemId + @"','" + saleorder.OrderQuantity + @"','" + saleorder.UnitPrice + @"','" + saleorder.PriceId + @"',N'" + saleorder.DescriptionOrd + @"',N'" + deliveryCode + @"',N'" + statusOrder + @"','" + orderDate + @"','" + userId + @"')", saleorder, commandType: CommandType.Text);
            if (rowAffect > 0)
            {
                saleorder.UserId = userId;
                return Created("Post success", saleorder);
            }
             else
                 return NoContent(); 
         }



        // PUT api/<SaleOrdersController>/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] SaleOrder saleorder)
        {
            var connectionString = "Data Source=DESKTOP-3UP7C4H; Initial Catalog =QLBH_KLTN; Integrated Security= true";
            IDbConnection db = new SqlConnection(connectionString);
            var saleorders = db.Query<SaleOrder>(" select * from dbo.SALE_ORDER  where SaleOrderId = '" + id + @"'", new { SaleOrderId = id }, commandType: CommandType.Text);
            var saleorderOld = saleorders.First();
            saleorderOld.TransportMethod = saleorder.TransportMethod;
            saleorderOld.ItemId = saleorder.ItemId;
            saleorderOld.AreaId = saleorder.AreaId;
            saleorderOld.OrderType = saleorder.OrderType;
            saleorderOld.OrderQuantity = saleorder.OrderQuantity;
            saleorderOld.UnitPrice = saleorder.UnitPrice;
            saleorderOld.PriceId = saleorder.PriceId;
            saleorderOld.DescriptionOrd = saleorder.DescriptionOrd;
            saleorderOld.StatusOrder = saleorder.StatusOrder;
            string statusOrder = "Đã đặt hàng";
            saleorder.StatusOrder = statusOrder;

            string query = "update dbo.SALE_ORDER set OrderType = N'" + saleorderOld.OrderType + @"',
                            TransportMethod= N'" + saleorderOld.TransportMethod + @"',
                            AreaId = '" + saleorderOld.AreaId + @"',
                            ItemId = '" + saleorderOld.ItemId + @"',
                            OrderQuantity = '" + saleorderOld.OrderQuantity + @"', 
                            UnitPrice= '" + saleorderOld.UnitPrice + @"',
                            PriceId = '" + saleorderOld.PriceId + @"',
                            DescriptionOrd = N'" + saleorderOld.DescriptionOrd + @"'
                            where SaleOrderId  = '" + id + @"'";
            var rowAffect = db.Execute(query, saleorder, commandType: CommandType.Text);
            if (rowAffect > 0)
                return Created("Put success", saleorderOld);
            else
                return NoContent();
        }
        [HttpPut("cancel/{id}")]
        public IActionResult Cancel(int id, [FromBody] SaleOrder saleorder)
        {
            var connectionString = "Data Source=DESKTOP-3UP7C4H; Initial Catalog =QLBH_KLTN; Integrated Security= true";
            IDbConnection db = new SqlConnection(connectionString);
            var saleorders = db.Query<SaleOrder>(" select * from dbo.SALE_ORDER  where SaleOrderId = '" + id + @"'", new { SaleOrderId = id }, commandType: CommandType.Text);
            var saleorderOld = saleorders.First();
            string statusOrder = "Đã hủy";
            saleorder.StatusOrder = statusOrder;
            string query = "update dbo.SALE_ORDER set StatusOrder = N'" + statusOrder + @"'
                            where SaleOrderId  = '" + id + @"'";
            var rowAffect = db.Execute(query, saleorder, commandType: CommandType.Text);
            if (rowAffect > 0)
                return Created("Put success", saleorder);
            else
                return NoContent();
        }


        // DELETE api/<SaleOrdersController>/5
        /* [HttpDelete("{id}")]
         public IActionResult Delete(int id)
         {
             var connectionString = "Data Source=DESKTOP-3UP7C4H; Initial Catalog =QLBH_KLTN; Integrated Security= true";
             IDbConnection db = new SqlConnection(connectionString);
             var rowAffect = db.Execute("delete dbo.SALE_ORDER  where SaleOrderId = '" + id + @"'", id, commandType: CommandType.Text);
             if (rowAffect > 0)
                 return Created("Delete success", id);
             else
                 return NoContent();

         }*/
    }
}
