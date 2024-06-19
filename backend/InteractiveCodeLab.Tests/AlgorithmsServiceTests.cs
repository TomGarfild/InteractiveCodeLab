using FluentAssertions;
using InteractiveCodeLab.Application.Services;
using InteractiveCodeLab.Infrastructure.Models;
using InteractiveCodeLab.Infrastructure.Repositories;
using NSubstitute;

namespace InteractiveCodeLab.UnitTests
{
    public class AlgorithmsServiceTests
    {
        /* [Fact]
        public async Task ShouldGetAlgorithmById()
        {
            // Arrange
            var rep = Substitute.For<IRepository<AlgorithmData>>();
            var service = new AlgorithmsService(rep);
            var expected = new AlgorithmData("title", "description", 0, DateTime.Now) { Id = "id" };
            rep.GetById("id").Returns(expected);

            // Act
            var data = await service.Get("id");

            // Assert
            data.Id.Should().Be("id");
        }

        [Fact]
        public async Task ShouldGetNullAlgorithmByIdIfNotExists()
        {
            // Arrange
            var rep = Substitute.For<IRepository<AlgorithmData>>();
            var service = new AlgorithmsService(rep);

            // Act
            var data = await service.Get("id");

            // Assert
            data.Should().BeNull();
        }
        */
    }
}