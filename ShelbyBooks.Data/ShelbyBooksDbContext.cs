using Microsoft.EntityFrameworkCore;
using ShelbyBooks.Data.Models;

namespace ShelbyBooks.Data;

public sealed class ShelbyBooksDbContext : DbContext
{
    public DbSet<User> Users { get; set; } = null!;
    public DbSet<Book> Books { get; set; } = null!;
    public DbSet<Order> Orders { get; set; } = null!;
    public DbSet<Cart> Carts { get; set; } = null!;
    public DbSet<OrderBook> OrderBooks { get; set; } = null!;
    

    public ShelbyBooksDbContext() 
    {
        bool databaseCreated = Database.EnsureCreated();
        if (databaseCreated)
        {
            #region Fill_Books
            Book book1 = new Book
            {
                Isbn = "978-5-699-92529-2", Title = "Мастер и Маргарита", Author = "М.А. Булгаков", Year = 1966,
                Image = FileConverter.GetBinaryFile("C:\\Projects\\Work\\Code\\internship_perepechaev\\src\\Backend\\Sms.BookStore\\Sms.BookStore.Data\\src\\M&M.jpg"), 
                Cost = 560, Quantity = 3 
            };
            Book book2 = new Book
            {
                Isbn = "978-5-389-12911-5", Title = "Двадцать тысяч лье под водой", Author = "Ж. Верн", Year = 1869,
                Image = FileConverter.GetBinaryFile("C:\\Projects\\Work\\Code\\internship_perepechaev\\src\\Backend\\Sms.BookStore\\Sms.BookStore.Data\\src\\20000.jpg"),
                Cost = 420, Quantity = 5
            };
            Book book3 = new Book
            {
                Isbn = "978-5-496-03210-0", Title = "Паттерны проектирования", Author = "Э. Фримен", Year = 2018,
                Image = FileConverter.GetBinaryFile("C:\\Projects\\Work\\Code\\internship_perepechaev\\src\\Backend\\Sms.BookStore\\Sms.BookStore.Data\\src\\DP.jpg"),
                Cost = 1650, Quantity = 0
            };

            Books.Add(book1);
            Books.Add(book2);
            Books.Add(book3);
            #endregion

            #region Fill_Users
            User user1 = new User { Login = "Nikolai4", Name = "Kolya", Email = "kolya@book.ru", Wallet = 0 };
            User user2 = new User { Login = "O_o", Name = "Mariya", Email = "mariya.2000@book.ru", Wallet = 0 };
            User user3 = new User { Login = "alice", Name = "Alice", Email = "alice.bot@book.ru", Wallet = 0 };

            Users.Add(user1);
            Users.Add(user2);
            Users.Add(user3);
            #endregion

            SaveChanges();
            
            #region Fill_Orders
            Order order1 = new Order("Выполнен", book3.Cost, user1.Id, user1);
            Order order2 = new Order("Выполнен", book2.Cost, user2.Id, user2);
            
            Orders.Add(order1);
            Orders.Add(order2);
            #endregion

            #region FillCarts
            Cart cart1 = new Cart(user3.Id, book3.Id, 1, user3, book3);

            Carts.Add(cart1);
            #endregion

            #region Fill_OrderBooks
            OrderBook orderBook1 = new OrderBook { OrderId = order1.Id, BookId = book3.Id, Quantity = 1, Order = order1, Book = book3};
            OrderBook orderBook2 = new OrderBook { OrderId = order2.Id, BookId = book2.Id, Quantity = 1, Order = order2, Book = book2};

            OrderBooks.Add(orderBook1);
            OrderBooks.Add(orderBook2);
            #endregion
            
            SaveChanges();
        }
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=ShelbyBooks;Username=postgres;Password=123");
    }
    
}