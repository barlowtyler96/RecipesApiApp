using FakeItEasy;
using FluentAssertions;
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
    private readonly RecipesController _recipesController;

    public RecipesControllerTests()
    {
        //Dependencies
        _data = A.Fake<IRecipeData>();
        _logger = A.Fake<ILogger<RecipesController>>();

        //SUT
        _recipesController = new RecipesController(_data, _logger);
    }

    [Fact]
    public void GetAllRecipeDtos_ReturnsSuccess()
    {
        //Arrange - Go get your variables, classes, function, whatever the function needs
        int currentPageNumber = 1;
        int pageSize = 8;
        var recipes = A.Fake<PaginationResponse<List<RecipeDto>>>();
        A.CallTo(() => _data.GetAllRecipeDtos(currentPageNumber, pageSize)).Returns(recipes);

        //Act Execute this function
        var result = _recipesController.GetPagedRecipeDtos(currentPageNumber, pageSize);

        //Assert - Whatever is returned is what is expected
        result.Should().BeOfType<Task<ActionResult<PaginationResponse<List<RecipeDto>>>>>();
    }
}
