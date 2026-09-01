using AwesomeAssertions;
using Bogus;
using Poc.Ef.Application.Contracts.Repositories;
using Poc.Ef.Application.Services;
using Poc.Ef.Domain.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System.Threading.Tasks;
using Poc.Common.Data;

namespace Poc.Ef.Infrastructure.UnitTests;

[TestClass]
public sealed class CategoryServiceTests
{
    [TestMethod]
    public async Task TestMethod1Async()
    {
        var category = new Faker<Category>()
            .RuleFor(u => u.Title, (f, u) => f.Name.FirstName())
            .Generate();

        var repository = Substitute.For<ICategoryRepository>();
        var unitOfWork = Substitute.For<IUnitOfWork>();

        repository.GetAsync(1).Returns(category);

        var categoryService = new CategoryService(unitOfWork, repository);
        var result = await categoryService.GetAsync(1);

        var actualCategoryDto = category.ToCategoryDto();

        result.Succeeded
            .Should().BeTrue();

        result.Data?.Title
            .Should().Be(actualCategoryDto.Title)
            .And.HaveLength(actualCategoryDto.Title.Length);
    }
}
