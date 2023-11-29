using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RecipeLibrary.DataAccess;
using RecipeLibrary.Models;
using RecipesApi.Controllers.v1;

namespace RecipeApiTests.RecipesApiTests;

public class RecipesControllerTests
{
    private readonly IRecipeData _data;
    private readonly ILogger<RecipesController> _logger;

    public RecipesControllerTests()
    {
        //Dependencies
        _data = A.Fake<IRecipeData>();
        _logger = A.Fake<ILogger<RecipesController>>();
    }

    [Theory]
    [InlineData(1)]
    public async Task GetById_ReturnsOk(int id)
    {
        //Arrange
        var recipe = A.Fake<RecipeModel>();
        A.CallTo(() => _data.GetById(id)).Returns(Task.FromResult(recipe));
        var controller = new RecipesController(_data, _logger);

        //Act
        var actionResult = await controller.GetById(id);

        //Assert
        actionResult.Result.Should().BeOfType<OkObjectResult>();
        var result = actionResult.Result as OkObjectResult;
        result!.Value.Should().BeOfType<RecipeModel>();
    }
    [Theory]
    [InlineData()]
    public async Task GetById_ReturnsNotFound(int id)
    {
        //Arrange
        var recipe = A.Fake<RecipeModel>();
        A.CallTo(() => _data.GetById(id)).Returns(Task.FromResult(recipe));
        var controller = new RecipesController(_data, _logger);

        //Act
        var actionResult = await controller.GetById(id);

        //Assert
        actionResult.Result.Should().BeOfType<NotFoundObjectResult>();
    }

    [Theory]
    [InlineData(1, 8)]
    public async Task GetPagedRecipeDtos_ReturnsOk(int currentPageNumber, int pageSize)
    {
        // Arrange 
        // Create fake data
        var fakeRecipeDtos = A.CollectionOfFake<RecipeDto>(pageSize).ToList();
        var fakePagedRecipes = new PaginationResponse<List<RecipeDto>>(
            pageSize * 2, 
            fakeRecipeDtos, 
            currentPageNumber, 
            pageSize);

        A.CallTo(() => _data.GetAllRecipeDtos(currentPageNumber, pageSize)).Returns(Task.FromResult(fakePagedRecipes));
        var controller = new RecipesController(_data, _logger);

        // Act 
        var actionResult = await controller.GetPagedRecipeDtos(currentPageNumber, pageSize);

        // Assert 
        actionResult.Result.Should().BeOfType<OkObjectResult>();
        var result = actionResult.Result as OkObjectResult;
        result!.Value.Should().BeOfType<PaginationResponse<List<RecipeDto>>>();
        var returnedRecipes = result.Value as PaginationResponse<List<RecipeDto>>;
        returnedRecipes!.Data.Should().HaveCount(pageSize);
        returnedRecipes!.TotalPages.Should().Be((int)Math.Ceiling(
            (double)returnedRecipes.TotalCount / (double)pageSize));
    }

    [Theory]
    [InlineData(0, 8)]
    [InlineData(1, 0)]
    public async Task GetPagedRecipeDtos_ReturnsBadRequest(int currentPageNumber, int pageSize)
    {
        // Arrange 
        // Create fake data
        var fakeRecipeDtos = A.CollectionOfFake<RecipeDto>(pageSize).ToList();
        var fakePagedRecipes = new PaginationResponse<List<RecipeDto>>(
            pageSize * 2,
            fakeRecipeDtos,
            currentPageNumber,
            pageSize);

        A.CallTo(() => _data.GetAllRecipeDtos(currentPageNumber, pageSize)).Returns(Task.FromResult(fakePagedRecipes));
        var controller = new RecipesController(_data, _logger);

        // Act 
        var actionResult = await controller.GetPagedRecipeDtos(currentPageNumber, pageSize);

        // Assert 
        actionResult.Result.Should().BeOfType<BadRequestObjectResult>();
    }

    [Theory]
    [InlineData(1, 8)]
    public async Task GetPagedFullRecipes_ReturnsOk(int currentPageNumber, int pageSize)
    {
        // Arrange 
        // Create fake data
        var fakeRecipeDtos = A.CollectionOfFake<RecipeModel>(pageSize).ToList();
        var fakePagedRecipes = new PaginationResponse<List<RecipeModel>>(
            pageSize * 2,
            fakeRecipeDtos,
            currentPageNumber,
            pageSize);

        A.CallTo(() => _data.GetAllRecipeModels(currentPageNumber, pageSize)).Returns(Task.FromResult(fakePagedRecipes));
        var controller = new RecipesController(_data, _logger);

        // Act 
        var actionResult = await controller.GetPagedFullRecipes(currentPageNumber, pageSize);

        // Assert 
        actionResult.Result.Should().BeOfType<OkObjectResult>();
        var result = actionResult.Result as OkObjectResult;
        result!.Value.Should().BeOfType<PaginationResponse<List<RecipeModel>>>();
        var returnedRecipes = result.Value as PaginationResponse<List<RecipeModel>>;
        returnedRecipes!.Data.Should().HaveCount(pageSize);
        returnedRecipes!.TotalPages.Should().Be((int)Math.Ceiling(
            (double)returnedRecipes.TotalCount / (double)pageSize));
    }

    [Theory]
    [InlineData(0, 8)]
    [InlineData(1, 0)]
    public async Task GetPagedFullRecipes_ReturnsBadRequest(int currentPageNumber, int pageSize)
    {
        // Arrange 
        // Create fake data
        var fakeRecipeModels = A.CollectionOfFake<RecipeModel>(pageSize).ToList();
        var fakePagedRecipes = new PaginationResponse<List<RecipeModel>>(
            pageSize * 2,
            fakeRecipeModels,
            currentPageNumber,
            pageSize);

        A.CallTo(() => _data.GetAllRecipeModels(currentPageNumber, pageSize)).Returns(Task.FromResult(fakePagedRecipes));
        var controller = new RecipesController(_data, _logger);

        // Act 
        var actionResult = await controller.GetPagedFullRecipes(currentPageNumber, pageSize);

        // Assert 
        actionResult.Result.Should().BeOfType<BadRequestObjectResult>();
    }
}
