using System.ComponentModel.DataAnnotations;
using System.Data.Common;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using RocketElevatorsAPI.Models;
using RocketElevatorsAPI.Data;

namespace RocketElevatorsAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        // Context
        private readonly RocketElevatorsContext _context;

        public CustomerController(RocketElevatorsContext context)
        {
            _context = context;

        }

        // Get full list of customers                                   
        // https://localhost:5000/api/customer/all
        // GET: api/customer/all  
              
        [HttpGet("all")]
        public IEnumerable<Customer> GetCustomers()
        {
            IQueryable<Customer> customers =
            from customer in _context.Customers
            select customer;
            System.Console.WriteLine(customers);
            return customers.ToList();

        }

        [HttpGet("{id}")]
        public ulong GetId(ulong id)
        {
            var customers = _context.Customers.Where(customer => customer.Id == id).ToList();
            return customers[0].Id;
        }

        [HttpGet("{id}/email")]
        public string GetEmail(ulong id)
        {

            var customers = _context.Customers.Where(customer => customer.Id == id).ToList();
            return customers[0].EmailCompanyContact;

        }

        [HttpGet("{customer_id}/batteries")]
        public IEnumerable<Battery> GetCustomerBatteries(ulong customer_id)
        {
            var batteries = _context.Batteries.Where(battery => battery.Customer_Id == customer_id).ToList();
            return batteries;
        }

        [HttpGet("{customer_id}/columns")]
        public IEnumerable<Column> GetCustomerColumns(ulong customer_id)
        {
            var columns = _context.Columns.Where(column => column.Customer_Id == customer_id).ToList();
            return columns;
        }

        [HttpGet("{customer_id}/elevators")]
        public IEnumerable<Elevator> GetCustomerElevators(ulong customer_id)
        {
            var elevators = _context.Elevators.Where(elevator => elevator.Customer_Id == customer_id).ToList();
            return elevators;
        }
    }
}