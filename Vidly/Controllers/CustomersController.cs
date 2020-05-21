using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vidly.DataAccessLayer;
using Vidly.Models;
using Vidly.ViewModels;

namespace Vidly.Controllers
{
    public class CustomersController : Controller
    {
        // GET: Customers
        public ActionResult Index()
        {
            var customers = GetCustomers();

            return View(customers);
        }

        public ActionResult New()
        {
            VidlyDAL vidlyDAL = new VidlyDAL();
            var membershipTypes = vidlyDAL.MembershipTypes.ToList();

            var viewModel = new CustomerFormViewModel
            {
                Customer = new Customer(),
                MembershipTypes = membershipTypes
            };

            return View("CustomerForm", viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Save(Customer customer)
        {
            VidlyDAL vidlyDAL = new VidlyDAL();
            if (!ModelState.IsValid)
            {
                var viewModel = new CustomerFormViewModel
                {
                    Customer = customer,
                    MembershipTypes = vidlyDAL.MembershipTypes.ToList()
                };
                return View ("CustomerForm",viewModel);
            }

            if (customer.ID == 0)
            {
                vidlyDAL.Customers.Add(customer);
            }
            else
            {
                var customerInDB = vidlyDAL.Customers.Single(c => c.ID == customer.ID);
                customerInDB.Name = customer.Name;
                customerInDB.BirthDate = customer.BirthDate;
                customerInDB.MembershipTypeID = customer.MembershipTypeID;
                customerInDB.IsSubscribedToNewsLetter = customer.IsSubscribedToNewsLetter;
            }
            vidlyDAL.SaveChanges();

            return RedirectToAction("Index","Customers");
        }

        public ActionResult Details(int id)
        {
            var customer = GetCustomers().SingleOrDefault(c => c.ID == id);

            if(customer == null)
            {
                return HttpNotFound();
            }

            return View(customer);
        }

        public ActionResult Edit(int id)
        {
            var customer = GetCustomers().SingleOrDefault(c => c.ID == id);

            if (customer == null)
            {
                return HttpNotFound();
            }

            VidlyDAL vidlyDAL = new VidlyDAL();
            var viewModel = new CustomerFormViewModel
            {
                Customer = customer,
                MembershipTypes = vidlyDAL.MembershipTypes.ToList()
            };

            return View("CustomerForm", viewModel);
        }

        private IEnumerable<Customer> GetCustomers()
        {
            VidlyDAL vidlyDAL = new VidlyDAL();
            return vidlyDAL.Customers.ToList<Customer>();
        }
    }
}