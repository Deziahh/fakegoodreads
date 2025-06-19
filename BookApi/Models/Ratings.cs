namespace BookApi.Models;

public class Rating
{
    public int Id { get; set; }
    public int Value { get; set; } // e.g. 1–5
    public int BookId { get; set; }

    // Navigation back to the Book
    public Book? Book { get; set; }
}
