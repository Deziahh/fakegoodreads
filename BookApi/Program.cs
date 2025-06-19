using BookApi.Data;
using BookApi.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// DB connection
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();

app.MapGet("/books", async (AppDbContext db) =>
{
    var books = await db.Books
        .Include(b => b.Ratings)
        .Select(b => new
        {
            b.Id,
            b.Title,
            b.Author,
            AverageRating = b.Ratings.Any() ? b.Ratings.Average(r => r.Value) : 0
        })
        .ToListAsync();

    return Results.Ok(books);
});

// Add a new book
app.MapPost("/books", async (Book book, AppDbContext db) =>
{
    db.Books.Add(book);
    await db.SaveChangesAsync();
    return Results.Created($"/books/{book.Id}", book);
});

// Submit a rating for a book
app.MapPost("/books/{bookId}/ratings", async (int bookId, Rating rating, AppDbContext db) =>
{
    var book = await db.Books.FindAsync(bookId);
    if (book == null) return Results.NotFound("Book not found");

    rating.BookId = bookId;
    db.Ratings.Add(rating);
    await db.SaveChangesAsync();
    return Results.Ok(rating);
});

app.MapGet("/books/{bookId}/ratings", async (int bookId, AppDbContext db) =>
{
    var ratings = await db.Ratings
        .Where(r => r.BookId == bookId)
        .ToListAsync();

    return Results.Ok(ratings);
});

// Auto-migrate
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
}

app.Run();
