using Microsoft.EntityFrameworkCore;
using BookApi.Models;

namespace BookApi.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Book> Books { get; set; }
    public DbSet<Rating> Ratings { get; set; }
}
