using AutoFixture;
using AutoMapper;
using EntityFrameworkPlayground.DataAccess.Repositories.Interfaces;
using EntityFrameworkPlayground.Domain.DataTransferObjects;
using EntityFrameworkPlayground.Domain.Entitities;
using EntityFrameworkPlayground.Service.Books;
using Moq;
using NUnit.Framework;
using System.Threading.Tasks;

namespace EntityFrameworkPlayground.Service.Tests.Books
{
    public class CreateBookStrategyFixtures
    {
        private IMapper _mapper;
        public IMapper Mapper
        {
            get
            {
                if (_mapper == null)
                {
                    var config = new MapperConfiguration(opts =>
                    {
                        // Add your mapper profile configs or mappings here
                        opts.CreateMap<Book, BookDTO>().ForMember(
                            dest => dest.Id,
                            opt => opt.MapFrom(src => src.BookId));
                        opts.CreateMap<BookForCreationDTO, Book>();
                    });

                    _mapper = config.CreateMapper();
                }

                return _mapper;
            }
        }

        public Fixture Fixture
        {
            get
            {
                var _fixture = new Fixture();
                _fixture.Behaviors.Remove(new ThrowingRecursionBehavior());
                _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
                return _fixture;
            }
        }


        [Test]
        public async Task ShouldCreateNewBook()
        {
            //arrange
            var authorId = Fixture.Create<int>();
            var bookForCreation = Fixture.Create<BookForCreationDTO>();

            var mockAuthorsRepository = new Mock<IAuthorRepository>();

            var mockBooksRepository = new Mock<IBooksRepository>();
            mockBooksRepository
                .Setup(repo => repo.AddBookToAuthor(authorId, It.IsAny<Book>()))
                .Callback<int, Book>((authId, newBook) => {
                    newBook.AuthorId = authorId;
                    newBook.Author = Fixture.Create<Author>();
                    newBook.BookId = Fixture.Create<int>();
                });

            var mockCreateLinksStrategy = new Mock<ICreateBookLinksStrategy>();
            mockCreateLinksStrategy
                .Setup(x => x.CreateLinksForBookResource(It.IsAny<BookDTO>()))
                .Returns<BookDTO>(x => x);

            var sut = new CreateBookStrategy(
                Mapper, mockBooksRepository.Object,
                mockAuthorsRepository.Object,
                mockCreateLinksStrategy.Object);

            //act
            var result = await sut.CreateBook(authorId, bookForCreation);

            //assert
            Assert.AreNotEqual(0, result.Id);
            Assert.AreEqual(result.Title, bookForCreation.Title);
        }

        [Test]
        public async Task AddBookToAuthorShouldBeCalled()
        {
            //arrange
            var authorId = Fixture.Create<int>();
            var bookForCreation = Fixture.Create<BookForCreationDTO>();

            var mockAuthorsRepository = new Mock<IAuthorRepository>();
            var bookEntity = Mapper.Map<Book>(bookForCreation);

            var mockBooksRepository = new Mock<IBooksRepository>();
            mockBooksRepository
                .Setup(repo => repo.AddBookToAuthor(authorId, It.IsAny<Book>()));

            var mockCreateLinksStrategy = new Mock<ICreateBookLinksStrategy>();
            mockCreateLinksStrategy
                .Setup(x => x.CreateLinksForBookResource(It.IsAny<BookDTO>()))
                .Returns<BookDTO>(x => x);

            var sut = new CreateBookStrategy(
                Mapper, mockBooksRepository.Object,
                mockAuthorsRepository.Object,
                mockCreateLinksStrategy.Object);

            //act
            var result = await sut.CreateBook(authorId, bookForCreation);

            //assert
            mockBooksRepository.Verify(x => x.AddBookToAuthor(authorId, It.IsAny<Book>()));
            mockCreateLinksStrategy.Verify(x => x.CreateLinksForBookResource(It.IsAny<BookDTO>()));
        }
    }
}
