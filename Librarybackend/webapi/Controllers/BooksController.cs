using AdminWebAPI.Data;
using AdminWebAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using webapi.Models;

namespace AdminWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly AdminWebAPI.Data.DbContext _context;

        public BooksController(AdminWebAPI.Data.DbContext context)
        {
            _context = context;
        }

        // GET: api/Books
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BookViewModel>>> GetBooks()
        {
            var books = await _context.Books
                                      .Include(b => b.Borrower)
                                      .Include(b => b.Category)
                                      .ToListAsync();

            var bookViewModels = books.Select(book => new BookViewModel
            {
                BookId = book.BookId,
                Title = book.Title,
                Author = book.Author,
                Status = book.Status,
                BorrowerId = book.BorrowerId,
                CategoryId = book.CategoryId,
                // Removed CoverImageBase64 from here
            }).ToList();

            return Ok(bookViewModels);
        }

        // GET: api/Books/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<BookViewModel>> GetBook(int id)
        {
            var book = await _context.Books
                                     .Include(b => b.Borrower)
                                     .Include(b => b.Category)
                                     .FirstOrDefaultAsync(b => b.BookId == id);

            if (book == null)
            {
                return NotFound();
            }

            var bookViewModel = new BookViewModel
            {
                BookId = book.BookId,
                Title = book.Title,
                Author = book.Author,
                Status = book.Status,
                BorrowerId = book.BorrowerId,
                CategoryId = book.CategoryId,
                // Removed CoverImageBase64 from here
            };

            return Ok(bookViewModel);
        }


        // PUT: api/Books/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBook(int id, [FromForm] BookViewModel bookViewModel)
        {
            if (id != bookViewModel.BookId) 
            {
                return BadRequest("Book ID mismatch.");
            }

            var book = await _context.Books.FindAsync(id);
            if (book == null)
            {
                return NotFound($"Book with ID {id} not found.");
            }

            // Update the book as before

            _context.Entry(book).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Books.Any(e => e.BookId == id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            // Instead of returning NoContent, return the updated book
            return Ok(new BookViewModel
            {
                BookId = book.BookId,
                Title = book.Title,
                Author = book.Author,
                Status = book.Status,
                BorrowerId = book.BorrowerId,
                CategoryId = book.CategoryId
            });
        }

        // DELETE: api/Books/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBook(int id)
        {
            var book = await _context.Books.FindAsync(id);
            if (book == null)
            {
                return NotFound($"Book with ID {id} not found.");
            }

            _context.Books.Remove(book);
            await _context.SaveChangesAsync();

            // Return a message or status after deletion
            return Ok(new { message = "Book deleted successfully" });
        }


        // GET: api/Books/search
        [HttpGet("search")]
        public IActionResult SearchBooks(string query)
        {
            if (string.IsNullOrEmpty(query))
            {
                return BadRequest("Search query cannot be empty.");
            }

            var books = _context.Books
                                .Where(b => b.Title.Contains(query) 
                                        || b.Author.Contains(query) 
                                        || (b.Category != null && b.Category.CategoryName.Contains(query)))
                                .Include(b => b.Category)  // Ensure Category is included in the result
                                .Include(b => b.Borrower)  // Ensure Borrower is included in the result
                                .ToList();

            return Ok(books);
        }
    }
}