using Microsoft.VisualStudio.TestTools.UnitTesting;
using Lab5.Services;
using Lab5.Models;
using System.Linq;

namespace Lab5.Tests
{
    [TestClass]
    public class Test1
    {
        private static LibraryService CreateService()
        {
            var books = new List<Book>
            {
                new Book { Id = 1, Title = "Book 1", Author = "Author 1", ISBN = "111" },
                new Book { Id = 2, Title = "Book 2", Author = "Author 2", ISBN = "222" }
            };

            var users = new List<User>
            {
                new User { Id = 1, Name = "User 1", Email = "user1@test.com" },
                new User { Id = 2, Name = "User 2", Email = "user2@test.com" }
            };

            return new LibraryService(books, users);
        }

        [TestMethod]
        public void AddBook_Should_Add_Book()
        {
            var service = CreateService();
            var count = service.GetBooks().Count;

            service.AddBook(new Book
            {
                Title = "Test Book",
                Author = "Author",
                ISBN = "123"
            });

            Assert.AreEqual(count + 1, service.GetBooks().Count);
        }

        [TestMethod]
        public void AddUser_Should_Add_User()
        {
            var service = CreateService();
            var count = service.GetUsers().Count;

            service.AddUser(new User
            {
                Name = "Test User",
                Email = "test@test.com"
            });

            Assert.AreEqual(count + 1, service.GetUsers().Count);
        }

        [TestMethod]
        public void DeleteBook_Should_Remove_Book()
        {
            var service = CreateService();
            var book = service.GetBooks().First();

            service.DeleteBook(book.Id);

            Assert.IsFalse(service.GetBooks().Any(b => b.Id == book.Id));
        }

        [TestMethod]
        public void DeleteUser_Should_Remove_User()
        {
            var service = CreateService();
            var user = service.GetUsers().First();

            service.DeleteUser(user.Id);

            Assert.IsFalse(service.GetUsers().Any(u => u.Id == user.Id));
        }

        [TestMethod]
        public void EditBook_Should_Update_Book()
        {
            var service = CreateService();
            var book = service.GetBooks().First();

            book.Title = "Updated Title";
            service.EditBook(book);

            Assert.AreEqual("Updated Title", service.GetBooks().First(b => b.Id == book.Id).Title);
        }

        [TestMethod]
        public void EditUser_Should_Update_User()
        {
            var service = CreateService();
            var user = service.GetUsers().First();

            user.Name = "Updated User";
            service.EditUser(user);

            Assert.AreEqual("Updated User", service.GetUsers().First(u => u.Id == user.Id).Name);
        }

        [TestMethod]
        public void BorrowBook_Should_Return_True()
        {
            var service = CreateService();
            var book = service.GetBooks().First();
            var user = service.GetUsers().First();

            var result = service.BorrowBook(book.Id, user.Id);

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void ReturnBook_Should_Return_True()
        {
            var service = CreateService();
            var book = service.GetBooks().First();
            var user = service.GetUsers().First();

            service.BorrowBook(book.Id, user.Id);
            var result = service.ReturnBook(user.Id, book.Id);

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void BorrowBook_Should_Return_False_For_Invalid_Book()
        {
            var service = CreateService();
            var user = service.GetUsers().First();

            var result = service.BorrowBook(-1, user.Id);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void ReturnBook_Should_Return_False_When_Not_Borrowed()
        {
            var service = CreateService();
            var user = service.GetUsers().First();
            var book = service.GetBooks().First();

            var result = service.ReturnBook(user.Id, book.Id);

            Assert.IsFalse(false);
        }
    }
}