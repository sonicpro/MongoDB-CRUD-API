using BookStoreApi.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace BookStoreApi.Services
{
    /// <summary>
    /// CRUD service for a Book entity
    /// </summary>
    public class BooksService
    {
        private readonly IMongoCollection<Book> books;

        /// <summary>
        /// Initializes a new instance of the <see cref="BooksService"/> class.
        /// </summary>
        /// <param name="settings">DB settings oprions</param>
        public BooksService(IOptions<BookStoreDbSettings> settings)
        {
            BookStoreDbSettings dbSettings = settings.Value;
            var mongoClient = new MongoClient(dbSettings.ConnectionString);
            var mongoDatabase = mongoClient.GetDatabase(dbSettings.DatabaseName);
            this.books = mongoDatabase.GetCollection<Book>(dbSettings.BooksCollectionName);
        }

        /// <summary>
        /// Parameterless GET.
        /// </summary>
        /// <returns>the list of books.</returns>
        public async Task<List<Book>> GetAsync() => await this.books.Find(_ => true).ToListAsync();

        /// <summary>
        /// A book by Id.
        /// </summary>
        /// <param name="id">The id of the Book.</param>
        /// <returns>Book instance or null.</returns>
        public async Task<Book?> GetAsync(string id) =>
            await this.books.Find(b => b.Id == id).FirstOrDefaultAsync();

        /// <summary>
        /// Creates a book.
        /// </summary>
        /// <param name="newBook">Book model.</param>
        /// <returns>The result of asynchronous operation.</returns>
        public async Task CreateAsync(Book newBook) => await this.books.InsertOneAsync(newBook);

        /// <summary>
        /// Updates a book.
        /// </summary>
        /// <param name="id">The Book Id.</param>
        /// <param name="updatedBook">The book model.</param>
        /// <returns>The resule of asynchronous operation.</returns>
        public async Task UpdateAsync(string id, Book updatedBook) =>
            await this.books.ReplaceOneAsync(b => b.Id == id, updatedBook);

        /// <summary>
        /// Deletes a book.
        /// </summary>
        /// <param name="id">The Book Id.</param>
        /// <returns>The result of asynchronous operation.</returns>
        public async Task RemoveAsync(string id) => await this.books.DeleteOneAsync(b => b.Id == id);
    }
}
