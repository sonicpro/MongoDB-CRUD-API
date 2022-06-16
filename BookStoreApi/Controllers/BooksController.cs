using BookStoreApi.Models;
using BookStoreApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace BookStoreApi.Controllers
{
    /// <summary>
    /// The controller for Books resource.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class BooksController : ControllerBase
    {
        /// <summary>
        /// A constant for the book route name.
        /// </summary>
        private const string CreatedAtRouteName = "GetBook";

        // BooksService instance.
        private readonly BooksService booksService;

        /// <summary>
        /// Initializes a new instance of the <see cref="BooksController"/> class.
        /// </summary>
        /// <param name="booksService">
        /// Injected by concrete type.
        /// </param>
        public BooksController(BooksService booksService)
        {
            this.booksService = booksService;
        }

        /// <summary>
        /// A resource for getting the list of Book entities.
        /// </summary>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        [HttpGet]
        public async Task<List<Book>> Get() => await this.booksService.GetAsync();

        /// <summary>
        /// The resource for a single book.
        /// </summary>
        /// <param name="id">The book id.</param>
        /// <returns>The book or HTTP 404.</returns>
        [HttpGet("{id:length(24)}", Name = CreatedAtRouteName)]
        public async Task<ActionResult<Book>> Get(string id)
        {
            var book = await this.booksService.GetAsync(id);
            if (book == null)
            {
                return this.NotFound();
            }

            return book;
        }

        /// <summary>
        /// A resource for inserting the new book.
        /// </summary>
        /// <param name="newBook">The book model.</param>
        /// <returns>HTTP 201 "Created" with the "Location" header corresponding to Get(id) resource.</returns>
        [HttpPost]
        public async Task<IActionResult> Post(Book newBook)
        {
            await this.booksService.CreateAsync(newBook);
            return this.CreatedAtRoute(CreatedAtRouteName, newBook);
        }

        [HttpPut("{id:length(24)}")]
    }
}
