namespace Lab5.Models
{
    public class BorrowedBookRecord
    {
        public int UserId { get; set; }
        public string UserName { get; set; } = "";
        public List<Book> Books { get; set; } = new();
    }
}