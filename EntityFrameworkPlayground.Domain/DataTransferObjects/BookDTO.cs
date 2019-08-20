namespace EntityFrameworkPlayground.Domain.DataTransferObjects
{
    public class BookDTO : LinkedResourceBaseDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int AuthorId { get; set; }
    }
}
