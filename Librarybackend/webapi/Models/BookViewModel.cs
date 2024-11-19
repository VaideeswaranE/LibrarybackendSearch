using System.ComponentModel.DataAnnotations;

namespace webapi.Models
{
    // BookViewModel (remove CoverImageBase64)
    public class BookViewModel
    {
        public int BookId { get; set; }

        [StringLength(255)]
        public required string Title { get; set; }

        [StringLength(255)]
        public required string Author { get; set; }

        [StringLength(50)]
        public required string Status { get; set; }
        public int? BorrowerId { get; set; }
        public int? CategoryId { get; set; }
        // Remove CoverImageBase64
        // public string CoverImageBase64 { get; set; }  // <-- remove this line
    }

}
