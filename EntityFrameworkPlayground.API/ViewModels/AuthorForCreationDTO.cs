using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EntityFrameworkPlayground.API.ViewModels
{
    public class AuthorForCreationDTO
    {
        [Required]
        [MinLength(5)]
        [MaxLength(100)]
        public string Name { get; set; }

        public ICollection<BookForCreationDTO> Books { get; set; }
    }
}
