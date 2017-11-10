using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Vidly.Models;
using System.Data.Entity;
using System.Web.Configuration;
using AutoMapper;
using Vidly.Dto;

namespace Vidly.Controllers.Api
{
    public class CustomersController : ApiController
    {
        private MyDbContext _context;

        public CustomersController()
        {
            _context=new MyDbContext();
        }


        //api/GETCustomers
        public IEnumerable<CustomerDto> GetCustomers()
        {

            return _context.Customer.ToList().Select(Mapper.Map<Customer, CustomerDto>);
        }



        //api/GetCustomers/Id
        public CustomerDto GetCustomer(int id)
        {
            var customer = _context.Customer.SingleOrDefault(c => c.Id == id);
            if(customer == null)
                throw new HttpResponseException(HttpStatusCode.NotFound);
            return Mapper.Map<Customer, CustomerDto>(customer);
        }



        //api/Post/Customer
        [HttpPost]
        public IHttpActionResult CreateCustomer(CustomerDto customerdto)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var customer = Mapper.Map<CustomerDto, Customer>(customerdto);
              _context.Customer.Add(customer);
            _context.SaveChanges();
            return Created(new Uri(Request.RequestUri + "/" + customer.Id), customerdto);
        }



        //api/PUT/CUSTOMER/id
        [HttpPut]
        public void  UpdateCustomer(int id, CustomerDto customerdto)
        {
            if (!ModelState.IsValid)
                throw new HttpResponseException(HttpStatusCode.BadRequest);

            var customerIndb = _context.Customer.SingleOrDefault(c => c.Id == id);
            if (customerIndb == null) 
                throw new HttpResponseException(HttpStatusCode.NotFound);

            Mapper.Map(customerdto,customerIndb);
          
            _context.SaveChanges();



        }



        //api/Delete
        [HttpDelete]
        public void Delete(int id)
        {
            if(!ModelState.IsValid)
                throw new HttpResponseException(HttpStatusCode.NotFound);
            var customerIndb = _context.Customer.SingleOrDefault(c => c.Id == id);
            if (customerIndb == null)
                throw new HttpResponseException(HttpStatusCode.NotFound);

            _context.Customer.Remove(customerIndb);
            _context.SaveChanges();
        }
    }

}
