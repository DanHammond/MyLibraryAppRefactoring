using MyLibraryApp.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyLibraryApp.Repository
{
    public interface IBookRepository
    {
        List<Book> GetAll();
        Book Get(int bookId);
    }
}
