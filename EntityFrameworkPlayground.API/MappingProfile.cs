using AutoMapper;
using EntityFrameworkPlayground.API.ViewModels;
using EntityFrameworkPlayground.Domain.Entitities;

namespace EntityFrameworkPlayground.API
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            #region Books
            CreateMap<Book, BookDTO>().ForMember(
                dest => dest.Id,
                opt => opt.MapFrom(src => src.BookId)); ;

            CreateMap<BookForCreationDTO, Book>();
            CreateMap<BookForUpdateDTO, Book>();
            CreateMap<Book, BookForUpdateDTO>();
            #endregion

            #region Authors
            CreateMap<Author, AuthorDTO>()
                .ForMember(
                dest => dest.Id,
                opt => opt.MapFrom(src => src.AuthorId))
                .ForMember(
                dest => dest.Books,
                opt => opt.MapFrom(src => src.Books));

            CreateMap<AuthorForCreationDTO, Author>();
            #endregion

        }
    }
}
