using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RocketElevatorsAPI.Models;
using RocketElevatorsAPI.Data;

namespace RocketElevatorsAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ElevatorController : ControllerBase
    {
        // Context
        private readonly RocketElevatorsContext _context;

        public ElevatorController(RocketElevatorsContext context)
        {
            _context = context;

        }

        // Get full list of elevators                                   
        // https://localhost:5000/api/elevator/all
        // GET: api/elevator/all           
        [HttpGet("all")]
        public IEnumerable<Elevator> GetElevators()
        {
            IQueryable<Elevator> elevators =
            from elevator in _context.Elevators
            select elevator;
            return elevators.ToList();

        }

        // Retriving Status of All the Elevators not active             
        // https://localhost:5000/api/elevator/inoperational
        // GET: api/elevator/inoperational           

        [HttpGet("inoperational")]
        public IEnumerable<Elevator> GetInoperationalElevators()
        {
            IQueryable<Elevator> elevators = 
            from elevator in _context.Elevators
            where elevator.Status.ToLower() != "active" // Gets elevators with either "Inactive" or "Intervention" status
            select elevator;
            return elevators.ToList();
        }

        // Get status of specific elevator
        // http://localhost:5000/api/elevator/{id}
        // GET: api/elevators/{id}
        [HttpGet("{id}")]
        public string GetStatus(ulong id)
        {
            var elevators = _context.Elevators.Where(elevator => elevator.Id == id).ToList();
            return elevators[0].Status;
        }

         // Change status of specific elevator
        // http://localhost:5000/api/elevator/{id}
        // PUT api/elevators/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> PutStatus(ulong id, [FromBody] Elevator elevator)
        {
            if (id != elevator.Id)
            {
                return BadRequest();
            }

            _context.Entry(elevator).State = EntityState.Modified;

            // Columns that we don't want to change
            _context.Entry(elevator).Property(p => p.Id).IsModified                    = false;
            _context.Entry(elevator).Property(p => p.Column_Id).IsModified             = false;
            _context.Entry(elevator).Property(p => p.Customer_Id).IsModified           = false;
            _context.Entry(elevator).Property(p => p.SerialNumber).IsModified          = false;
            _context.Entry(elevator).Property(p => p.Model).IsModified                 = false;
            _context.Entry(elevator).Property(p => p.BuildingType).IsModified          = false;
            _context.Entry(elevator).Property(p => p.CommissioningDate).IsModified     = false;
            _context.Entry(elevator).Property(p => p.LastInspectionDate).IsModified    = false;
            _context.Entry(elevator).Property(p => p.InspectionCertificate).IsModified = false;
            _context.Entry(elevator).Property(p => p.Information).IsModified           = false;
            _context.Entry(elevator).Property(p => p.Notes).IsModified                 = false;
            _context.Entry(elevator).Property(p => p.CreatedAt).IsModified             = false;
            _context.Entry(elevator).Property(p => p.UpdatedAt).IsModified             = false;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (id != elevator.Id)
                {
                    // Resource doesn't exist.
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
           
            var dbElevator = _context.Elevators.FirstOrDefault(elevator => elevator.Id == id);          
            return  Content("Status of Elevator with ID #" + elevator.Id + ": changed status to " + elevator.Status);  
        }
    }
}