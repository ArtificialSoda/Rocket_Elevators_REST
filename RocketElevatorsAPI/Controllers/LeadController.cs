using System.Collections.Generic;
using System.Data;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using RocketElevatorsAPI.Models;

namespace RocketElevatorsAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LeadController : ControllerBase
    {
        // Context
        private readonly RocketElevatorsContext _context;

        public LeadController(RocketElevatorsContext context)
        {
            _context = context;

        }

        // Get full list of leads                                    
        // https://localhost:3000/api/lead/all
        // GET: api/lead/all           
        [HttpGet("all")]
        public IEnumerable<Lead> GetLeads()
        {
            IQueryable<Lead> leads =
            from lead in _context.Leads
            select lead;
            return leads.ToList();

        }

        // Retrieve list of Leads created in the last 30 days who have not yet become customers                                                       https://localhost:5001/api/lead/notcustomers
        // https://localhost:3000/api/lead/noncustomers
        // GET: api/lead         
        [HttpGet("noncustomers")]
        public IEnumerable<Lead> GetNonCustomers()
        {
            IQueryable<Lead> nonCustomers =
            from lead in _context.Leads
            where lead.CreatedAt >= System.DateTime.Now.AddDays(-30)
            select lead;
            return nonCustomers.ToList();
        }
    }
}