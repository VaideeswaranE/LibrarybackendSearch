using AdminWebAPI.Data;
using AdminWebAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdminWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BorrowersController : ControllerBase
    {
        private readonly AdminWebAPI.Data.DbContext _context;  // Fully qualify the DbContext

        public BorrowersController(AdminWebAPI.Data.DbContext context)  // Fully qualify the DbContext
        {
            _context = context;
        }

        // GET: api/Borrowers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AdminWebAPI.Models.Borrower>>> GetBorrowers()
        {
            return await _context.Borrowers.ToListAsync();
        }

        // GET: api/Borrowers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<AdminWebAPI.Models.Borrower>> GetBorrower(int id)
        {
            var borrower = await _context.Borrowers.FindAsync(id);

            if (borrower == null)
            {
                return NotFound();
            }

            return borrower;
        }

        // POST: api/Borrowers
        [HttpPost]
        public async Task<ActionResult<Borrower>> PostBorrower(Borrower borrower)
        {
            if (borrower == null)
            {
                return BadRequest("Borrower data is required.");
            }

            _context.Borrowers.Add(borrower);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetBorrower", new { id = borrower.BorrowerId }, borrower);
        }

        // PUT: api/Borrowers/5
[HttpPut("{id}")]
public async Task<IActionResult> PutBorrower(int id, Borrower borrower)
{
    if (id != borrower.BorrowerId)
    {
        return BadRequest("Borrower ID in the URL does not match the Borrower ID in the body.");
    }

    // Check if the borrower exists in the database
    var existingBorrower = await _context.Borrowers.FindAsync(id);
    if (existingBorrower == null)
    {
        return NotFound("The specified borrower does not exist.");
    }

    // Update the properties of the existing borrower
    existingBorrower.Name = borrower.Name;
    existingBorrower.PhoneNumber = borrower.PhoneNumber;
    existingBorrower.DueDate = borrower.DueDate;

    // Mark the borrower entity as modified
    _context.Entry(existingBorrower).State = EntityState.Modified;

    try
    {
        await _context.SaveChangesAsync();
    }
    catch (DbUpdateConcurrencyException)
    {
        if (!_context.Borrowers.Any(e => e.BorrowerId == id))
        {
            return NotFound("The borrower was not found during the update.");
        }
        else
        {
            throw;
        }
    }

    return NoContent();
}
        // DELETE: api/Borrowers/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBorrower(int id)
        {
            var borrower = await _context.Borrowers.FindAsync(id);
            if (borrower == null)
            {
                return NotFound();
            }

            _context.Borrowers.Remove(borrower);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
