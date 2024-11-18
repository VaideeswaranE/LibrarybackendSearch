using AdminWebAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace AdminWebAPI.Data
{
    public class DbContext : Microsoft.EntityFrameworkCore.DbContext
    {
        public DbContext(DbContextOptions<DbContext> options) : base(options) { }

        public DbSet<Admin> Admins { get; set; }
        public DbSet<Borrower> Borrowers { get; set; }
        public DbSet<Book> Books { get; set; }  // Added for Books table
        public DbSet<BorrowHistory> BorrowHistory { get; set; } // DbSet for BorrowHistory
        public DbSet<Category> Categories { get; set; }
 protected override void OnModelCreating(ModelBuilder modelBuilder)
        {    

            base.OnModelCreating(modelBuilder);

             modelBuilder.Entity<Book>()
                .HasOne(b => b.Category)
                .WithMany()  // Assuming a Category can have multiple Books, adjust accordingly
                .HasForeignKey(b => b.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);  // Adjust delete behavior as needed

            modelBuilder.Entity<Book>()
                .HasOne(b => b.Borrower)
                .WithMany()
                .HasForeignKey(b => b.BorrowerId)
                .OnDelete(DeleteBehavior.Restrict);  // Prevent cascading deletes

            // Define HistoryId as the primary key for BorrowHistory
            modelBuilder.Entity<BorrowHistory>()
                .HasKey(bh => bh.HistoryId);

            modelBuilder.Entity<BorrowHistory>()
                .ToTable("BorrowHistory");  // Map to "BorrowHistory" table

            // Configuring foreign key relationships
            modelBuilder.Entity<BorrowHistory>()
                .HasOne(bh => bh.Borrower)
                .WithMany()
                .HasForeignKey(bh => bh.BorrowerId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<BorrowHistory>()
                .HasOne(bh => bh.Book)
                .WithMany()
                .HasForeignKey(bh => bh.BookId)
                .OnDelete(DeleteBehavior.Restrict);
        }

    }
}
