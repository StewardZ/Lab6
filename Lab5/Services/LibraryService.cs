using Lab5.Models;

namespace Lab5.Services
{
    public class LibraryService : ILibraryService
    {
        private readonly IWebHostEnvironment? env;

        private List<Book> books = new();
        private List<User> users = new();

        private Dictionary<int, List<Book>> borrowedBooks = new();

        public LibraryService(IWebHostEnvironment env)
        {
            this.env = env;
            LoadData(Path.Combine(env.ContentRootPath, "Data"));
        }
        public LibraryService(List<Book> testBooks, List<User> testUsers)
        {
            books = testBooks;
            users = testUsers;
        }

        public LibraryService(string basePath)
        {
            var fullBasePath = Path.GetFullPath(basePath);
            LoadData(Path.Combine(fullBasePath, "Data"));
        }

        private void LoadData(string dataPath)
        {
            var booksPath = Path.Combine(dataPath, "Books.csv");
            var usersPath = Path.Combine(dataPath, "Users.csv");

            books = File.ReadAllLines(booksPath)
                .Select(line => line.Split(','))
                .Select(fields => new Book
                {
                    Id = int.Parse(fields[0]),
                    Title = fields[1],
                    Author = fields[2],
                    ISBN = fields[3]
                }).ToList();

            users = File.ReadAllLines(usersPath)
                .Select(line => line.Split(','))
                .Select(fields => new User
                {
                    Id = int.Parse(fields[0]),
                    Name = fields[1],
                    Email = fields[2]
                }).ToList();
        }

        public List<Book> GetBooks() => books;
        public List<User> GetUsers() => users;

        public void AddBook(Book book)
        {
            book.Id = books.Any() ? books.Max(b => b.Id) + 1 : 1;
            books.Add(book);
        }

        public void EditBook(Book book)
        {
            var existing = books.First(x => x.Id == book.Id);
            existing.Title = book.Title;
            existing.Author = book.Author;
            existing.ISBN = book.ISBN;
        }

        public void DeleteBook(int id)
        {
            books.RemoveAll(x => x.Id == id);
        }

        public void AddUser(User user)
        {
            user.Id = users.Any() ? users.Max(u => u.Id) + 1 : 1;
            users.Add(user);
        }

        public void EditUser(User user)
        {
            var existing = users.First(x => x.Id == user.Id);
            existing.Name = user.Name;
            existing.Email = user.Email;
        }

        public void DeleteUser(int id)
        {
            users.RemoveAll(x => x.Id == id);
        }

        public bool BorrowBook(int bookId, int userId)
        {
            var book = books.FirstOrDefault(b => b.Id == bookId);
            if (book == null) return false;

            if (!borrowedBooks.ContainsKey(userId))
                borrowedBooks[userId] = new List<Book>();

            borrowedBooks[userId].Add(book);
            books.Remove(book);

            return true;
        }

        public bool ReturnBook(int userId, int bookId)
        {
            if (!borrowedBooks.ContainsKey(userId))
                return false;

            var book = borrowedBooks[userId].FirstOrDefault(b => b.Id == bookId);
            if (book == null) return false;

            borrowedBooks[userId].Remove(book);
            books.Add(book);

            return true;
        }

        public List<Book> GetBorrowedBooksByUser(int userId)
        {
            if (!borrowedBooks.ContainsKey(userId))
                return new List<Book>();

            return borrowedBooks[userId];
        }

        public List<BorrowedBookRecord> GetBorrowedBooks()
        {
            var result = new List<BorrowedBookRecord>();

            foreach (var entry in borrowedBooks)
            {
                var user = users.First(u => u.Id == entry.Key);

                result.Add(new BorrowedBookRecord
                {
                    UserId = user.Id,
                    UserName = user.Name,
                    Books = entry.Value
                });
            }

            return result;
        }
    }
}