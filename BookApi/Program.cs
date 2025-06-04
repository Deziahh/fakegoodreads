var builder = WebApplication.CreateBuilder(args);
builder.Services.AddCors();
var app = builder.Build();

var books = new List<Book>
{
    new Book { Id = 1, Title = "1984", Author = "George Orwell" },
    new Book { Id = 2, Title = "To Kill a Mockingbird", Author = "Harper Lee" },
    new Book { Id = 3, Title = "The Great Gatsby", Author = "F. Scott Fitzgerald" }
};

app.MapGet("/books", () => books);
app.MapGet("/books/{id:int}", (int id) =>
{
    var book = books.FirstOrDefault(b => b.Id == id);
    return book is not null ? Results.Ok(book) : Results.NotFound();
});

app.UseCors(policy =>
    policy.AllowAnyOrigin()
          .AllowAnyHeader()
          .AllowAnyMethod());

app.Run();

public class Book
{
    public int Id { get; set; }
    public string Title { get; set; } = "";
    public string Author { get; set; } = "";
}

