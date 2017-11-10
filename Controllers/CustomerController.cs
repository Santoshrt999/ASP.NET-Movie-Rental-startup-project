using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vidly.Models;
using System.Data.Entity;
using Vidly.ViewModel;

namespace Vidly.Controllers
{
    public class CustomerController : Controller
    {
        private MyDbContext _context;

        public CustomerController()
        {
            _context=new MyDbContext();
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }

        public ActionResult New()
        {
            var membershiptype = _context.MembershipTypes.ToList();
            var viewmodel = new CustomerFormViewModel
            {
                MembershipTypes = membershiptype,

            };
            return View("CustomerForm",viewmodel);
        }

        [HttpPost]
        public ActionResult Save(Customer customer)

        {

            if (!ModelState.IsValid)
            {
                var viewmodel = new CustomerFormViewModel
                {
                    Customer = customer,
                    MembershipTypes = _context.MembershipTypes.ToList()

                };
                return View("CustomerForm", viewmodel);
            }
        
            if (customer.Id == 0)

                _context.Customer.Add(customer);
            else
            {
                var customerInDb = _context.Customer.SingleOrDefault(c => c.Id == customer.Id);
                TryUpdateModel(customerInDb);
            }
            _context.SaveChanges();

            return RedirectToAction("Index", "Customer");
        }

        public ViewResult Index()
        {
            var customers = _context.Customer.Include(c=>c.MembershipType).ToList();

            return View(customers);
        }

        public ActionResult Details(int id)
        {
            // var customer = _context.Customer.SingleOrDefault(c => c.Id == id); //getting from datatabse
            var customer = _context.Customer.Include(c => c.MembershipType).SingleOrDefault(c => c.Id == id);
            if (customer == null)
                return HttpNotFound();

            return View(customer);
        }

        public ActionResult Edit(int id)
        {

            var customer = _context.Customer.SingleOrDefault(c => c.Id == id);
            if (customer == null)
                return HttpNotFound();

            var viewmodel = new CustomerFormViewModel
            {
                Customer = customer,
                MembershipTypes = _context.MembershipTypes.ToList()
            };
            return View("CustomerForm", viewmodel);
        }
       
    }
}