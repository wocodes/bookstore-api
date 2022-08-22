using System;
using BookStoreApi.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace BookStoreApi.Services
{
    public class BooksService
    {
        // define a readonly Collection Object as _booksCollection baed on MongoDB Collection Interface
        // I'd come back to this sense/nonsense description I wrote above later in the future :-D
        private readonly IMongoCollection<Book> _booksCollection;


        // initialize constructor
        public BooksService(IOptions<BookStoreDatabaseSettings> bookStoreDatabaseSettings)
        {
            // get the connection string from the BookStoreDatabaseSettings Model
            // and use value to create a new MongoClient Object which is set mongoClient variable
            var mongoClient = new MongoClient(bookStoreDatabaseSettings.Value.ConnectionString);

            // get the database name string from the BookStoreDatabaseSettings Model
            // and use value to get database instance from newly created MongoClient above
            var mongoDatabase = mongoClient.GetDatabase(bookStoreDatabaseSettings.Value.DatabaseName);
            
            // get the collection name string from the BookStoreDatabaseSettings Model
            // and use value to get collection instance from mongoDatabase above
            _booksCollection = mongoDatabase.GetCollection<Book>(bookStoreDatabaseSettings.Value.BooksCollectionName);
        }


        public async Task<List<Book>> GetBooksAsync()
        {
            return await _booksCollection.Find(_ => true).ToListAsync();
        }

        public async Task<Book?> GetBookAsync(string id)
        {
            return await _booksCollection.Find(x => x.Id == id).FirstOrDefaultAsync();
        }

        public async Task CreateBookAsync(Book newBook)
        {
            await _booksCollection.InsertOneAsync(newBook);
        }

        public async Task UpdateBookAsync(string id, Book updatedBook)
        {
            await _booksCollection.ReplaceOneAsync(x => x.Id == id, updatedBook);
        }

        public async Task DeleteBookAsync(string id)
        {
            await _booksCollection.DeleteOneAsync(x => x.Id == id);
        }
    }
}

