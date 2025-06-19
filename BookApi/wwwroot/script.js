const API_BASE = "/books";

async function loadBooks() {
  const res = await fetch(API_BASE);
  const books = await res.json();
  const container = document.getElementById("book-list");
  container.innerHTML = "";

  books.forEach(book => {
    const div = document.createElement("div");
    div.className = "book";
    div.innerHTML = `
      <strong>${book.title}</strong> by ${book.author}<br />
      ⭐ Average Rating: ${book.averageRating?.toFixed(1) || "No ratings yet"}
      <form class="rating-form" data-book-id="${book.id}">
        <input type="number" min="1" max="5" placeholder="Rate 1–5" required />
        <button type="submit">Submit Rating</button>
      </form>
    `;
    container.appendChild(div);
  });

  attachRatingHandlers();
}

function attachRatingHandlers() {
  document.querySelectorAll(".rating-form").forEach(form => {
    form.addEventListener("submit", async (e) => {
      e.preventDefault();
      const bookId = form.getAttribute("data-book-id");
      const input = form.querySelector("input");
      const value = parseInt(input.value);

      if (value < 1 || value > 5) {
        alert("Rating must be between 1 and 5.");
        return;
      }

      await fetch(`${API_BASE}/${bookId}/ratings`, {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({ value })
      });

      input.value = "";
      loadBooks(); // refresh list to show new average
    });
  });
}

document.getElementById("book-form").addEventListener("submit", async e => {
  e.preventDefault();
  const title = document.getElementById("title").value;
  const author = document.getElementById("author").value;

  await fetch(API_BASE, {
    method: "POST",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify({ title, author })
  });

  document.getElementById("title").value = "";
  document.getElementById("author").value = "";
  loadBooks();
});

loadBooks();
