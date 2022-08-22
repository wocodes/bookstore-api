using System;
using BookStoreApi.Models;
using BookStoreApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace BookStoreApi.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class BooksController : ControllerBase
    {
        private readonly BooksService _booksService;

        public BooksController(BooksService _booksService) => this._booksService = _booksService;


        [HttpGet]
        public async Task<List<Book>> Get() => await _booksService.GetBooksAsync();


        [HttpGet("{id:length(24)}")]
        public async Task<ActionResult<Book>> Get(string id)
        {
            var book = await _booksService.GetBookAsync(id);

            if (book is null)
            {
                return NotFound();
            }

            return book;
        }



        [HttpPost]
        public async Task<IActionResult> Post(Book newBook)
        {
            await _booksService.CreateBookAsync(newBook);

            return CreatedAtAction(nameof(Get), new { id = newBook.Id }, newBook);
        }



        [HttpPut("{id:length(24)}")]
        public async Task<IActionResult> Put(string id, Book updatedBook)
        {
            if (updatedBook.Id is null || id != updatedBook.Id)
            {
                return BadRequest();
            }

            var book = await _booksService.GetBookAsync(id);

            if (book is null)
            {
                return NotFound();
            }

            await _booksService.UpdateBookAsync(id, updatedBook);

            return NoContent();
        }



        [HttpDelete("{id:length(24)}")]
        public async Task<IActionResult> Delete(string id)
        {

            var book = await _booksService.GetBookAsync(id);

            if (book is null)
            {
                return NotFound();
            }

            await _booksService.DeleteBookAsync(id);

            return NoContent();
        }
    }
}

