using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Description;
using WebApi2.Models;

namespace WebApi2.Controllers
{
    //[MyException]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [RoutePrefix("clients")]
    public class ClientsController : ApiController
    {
        private FabricsEntities db = new FabricsEntities();

        public ClientsController()
        {
            db.Configuration.LazyLoadingEnabled = false;
        }

        [Route("")]
        public IHttpActionResult GetClient()
        {
            return Ok(db.Client.Take(10));
        }

        [Route("{id:int}", Name = nameof(GetClientById))]
        [ResponseType(typeof(Client))]
        public HttpResponseMessage GetClientById(int id)
        {
            Client client = db.Client.Find(id);
            if (client == null)
            {
                throw new ArgumentException("Not Found");
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            return Request.CreateResponse(client);
        }

        [Route("{id:int}/orders")]
        [ResponseType(typeof(Client))]
        public HttpResponseMessage GetClientOrders(int id)
        {
            var orders = db.Order.Where(p => p.ClientId == id);
            return new HttpResponseMessage()
            {
                ReasonPhrase = "HELLO",
                StatusCode = HttpStatusCode.OK,
                Content = new ObjectContent<IQueryable<Order>>(orders,
                    GlobalConfiguration.Configuration.Formatters.JsonFormatter)
            };
        }

        [Route("{id:int}/orders/{date:datetime}")]
        [ResponseType(typeof(Client))]
        public IHttpActionResult GetClientOrdersByDate1(int id, DateTime date)
        {
            var next_day = date.AddDays(1);
            var orders = db.Order.Where(p => p.ClientId == id && p.OrderDate >= date && p.OrderDate <= next_day);
            return Ok(orders.ToList());
        }

        [Route("{id:int}/orders/{*date:datetime}")]
        [ResponseType(typeof(Client))]
        public IHttpActionResult GetClientOrdersByDate2(int id, DateTime date)
        {
            var next_day = date.AddDays(1);
            var orders = db.Order.Where(p => p.ClientId == id && p.OrderDate >= date && p.OrderDate <= next_day);
            return Ok(orders.ToList());
        }

        [Route("{id:int}")]
        [ResponseType(typeof(void))]
        public IHttpActionResult PutClient(int id, Client client)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != client.ClientId)
            {
                return BadRequest();
            }

            db.Entry(client).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ClientExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        [Route("")]
        [ResponseType(typeof(Client))]
        public IHttpActionResult PostClient(Client client)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Client.Add(client);
            db.SaveChanges();

            return CreatedAtRoute(nameof(GetClientById), new { id = client.ClientId }, client);
        }

        [Route("{id:int}")]
        [ResponseType(typeof(Client))]
        public IHttpActionResult DeleteClient(int id)
        {
            Client client = db.Client.Find(id);
            if (client == null)
            {
                return NotFound();
            }

            db.Client.Remove(client);
            db.SaveChanges();

            return Ok(client);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ClientExists(int id)
        {
            return db.Client.Count(e => e.ClientId == id) > 0;
        }
    }
}