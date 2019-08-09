using System.ComponentModel.DataAnnotations;

namespace EntityFrameworkPlayground.API.ViewModels
{
    public class BookForCreationDTO
    {
        [Required]
        [MinLength(5)]
        [MaxLength(100)]
        public string Title { get; set; }
    }
}
