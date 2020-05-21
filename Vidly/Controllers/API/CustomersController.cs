using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Vidly.DataAccessLayer;
using Vidly.Models;

namespace Vidly.Controllers.API
{
    public class CustomersController : ApiController
    {
        //GET /api/customers
        public IEnumerable<Customer> GetCustomers()
        {
            VidlyDAL vidlyDAL = new VidlyDAL();

            return vidlyDAL.Customers.ToList();
        }

        //GET /api/customers/1
        public Customer GetCustomer(int id)
        {
            VidlyDAL vidlyDAL = new VidlyDAL();

            var customer = vidlyDAL.Customers.SingleOrDefault(c => c.ID == id);

            if (customer == null)
                throw new HttpResponseException(HttpStatusCode.NotFound);

            return customer;
        }

        //POST /api/customers
        [HttpPost]
        public Customer CreateCustomer(Customer customer)
        {
            if (!ModelState.IsValid)
                throw new HttpResponseException(HttpStatusCode.BadRequest);

            VidlyDAL vidlyDAL = new VidlyDAL();

            vidlyDAL.Customers.Add(customer);

            vidlyDAL.SaveChanges();

            return customer;
        }

        //PUT /api/customers/1
        [HttpPut]
        public void UpdateCustomer(int id, Customer customer)
        {
            if (!ModelState.IsValid)
                throw new HttpResponseException(HttpStatusCode.BadRequest);

            VidlyDAL vidlyDAL = new VidlyDAL();

            var customerInDb = vidlyDAL.Customers.SingleOrDefault(c => c.ID == id);

            if (customerInDb == null)
                throw new HttpResponseException(HttpStatusCode.NotFound);

            customerInDb.Name = customer.Name;
            customerInDb.MembershipTypeID = customer.MembershipTypeID;
            customerInDb.IsSubscribedToNewsLetter = customer.IsSubscribedToNewsLetter;
            customerInDb.BirthDate = customer.BirthDate;

            vidlyDAL.SaveChanges();
        }

        // DELETE /api/customers/1
        [HttpDelete]
        public void DeleteCustomer (int id)
        {
            VidlyDAL vidlyDAL = new VidlyDAL();

            var customerInDb = vidlyDAL.Customers.SingleOrDefault(c => c.ID == id);

            if (customerInDb == null)
                throw new HttpResponseException(HttpStatusCode.NotFound);

            vidlyDAL.Customers.Remove(customerInDb);

            vidlyDAL.SaveChanges();
        }
    }
}
