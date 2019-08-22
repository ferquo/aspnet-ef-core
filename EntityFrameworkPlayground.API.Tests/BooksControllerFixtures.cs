using AutoFixture;
using EntityFrameworkPlayground.API.Controllers;
using EntityFrameworkPlayground.Domain.DataTransferObjects;
using EntityFrameworkPlayground.Service.Books;
using EntityFrameworkPlayground.Service.Core;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System.Threading.Tasks;

namespace EntityFrameworkPlayground.API.Tests
{
    public class BooksControllerFixtures
    {
        #region Fixture Setup
        private Mock<IGetBooksStrategy> mockGetBooksStrategy;
        private Mock<IGetBookStrategy> mockGetBookStrategy;
        private Mock<ICreateBookStrategy> mockCreateBookStrategy;
        private Mock<IUpdateBookStrategy> mockUpdateBookStrategy;
        private Mock<IDeleteBookStrategy> mockDeleteBookStrategy;
        private Mock<IValidationStrategy> mockBookValidationStrategy;
        private readonly Fixture fixture = new Fixture();

        [SetUp]
        public void Setup()
        {
            mockGetBooksStrategy = new Mock<IGetBooksStrategy>();
            mockGetBookStrategy = new Mock<IGetBookStrategy>();
            mockCreateBookStrategy = new Mock<ICreateBookStrategy>();
            mockUpdateBookStrategy = new Mock<IUpdateBookStrategy>();
            mockDeleteBookStrategy = new Mock<IDeleteBookStrategy>();
            mockBookValidationStrategy = new Mock<IValidationStrategy>();
        }

        [TearDown]
        public void TearDown()
        {
            mockGetBooksStrategy = null;
            mockGetBookStrategy = null;
            mockCreateBookStrategy = null;
            mockUpdateBookStrategy = null;
            mockDeleteBookStrategy = null;
            mockBookValidationStrategy = null;
        }
        #endregion

        [Test]
        public async Task ShouldGetBookById()
        {
            //arrange
            var authorId = fixture.Create<int>();
            var bookId = fixture.Create<int>();

            var bookToReturn = new BookDTO() {
                Id = bookId,
                AuthorId = authorId,
                Title = fixture.Create<string>()
            };

            mockGetBookStrategy
                .Setup(x => x.GetBookById(authorId, bookId))
                .Returns(Task.FromResult(bookToReturn));
            var sut = new BooksController(
                mockGetBooksStrategy.Object,
                mockGetBookStrategy.Object,
                mockCreateBookStrategy.Object,
                mockUpdateBookStrategy.Object,
                mockDeleteBookStrategy.Object,
                mockBookValidationStrategy.Object);

            //act
            var result = await sut.Get(authorId, bookId);
            var okResult = result as OkObjectResult;

            //assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            Assert.IsInstanceOf<BookDTO>(okResult.Value);
        }

        [Test]
        public async Task ShouldReturnNotFoundIfBookNotExists()
        {
            //arrange
            var authorId = fixture.Create<int>();
            var bookId = fixture.Create<int>();

            var bookToReturn = new BookDTO()
            {
                Id = bookId,
                AuthorId = authorId,
                Title = fixture.Create<string>()
            };

            mockGetBookStrategy
                .Setup(x => x.GetBookById(authorId, bookId))
                .Returns(Task.FromResult<BookDTO>(null));
            var sut = new BooksController(
                mockGetBooksStrategy.Object,
                mockGetBookStrategy.Object,
                mockCreateBookStrategy.Object,
                mockUpdateBookStrategy.Object,
                mockDeleteBookStrategy.Object,
                mockBookValidationStrategy.Object);

            //act
            var result = await sut.Get(authorId, bookId);

            //assert
            Assert.IsInstanceOf<NotFoundResult>(result);
        }
    }
}
