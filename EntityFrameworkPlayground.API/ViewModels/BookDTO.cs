namespace EntityFrameworkPlayground.API.ViewModels
{
    public class BookDTO : LinkedResourceBaseDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int AuthorId { get; set; }
    }
}
