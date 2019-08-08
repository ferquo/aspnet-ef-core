﻿using System.ComponentModel.DataAnnotations;

namespace EntityFrameworkPlayground.Domain.Entitities
{
    public class Book
    {
        public int BookId { get; set; }
        public string Title { get; set; }


        public Author Author { get; set; }
        public int AuthorId { get; set; }
    }
}
