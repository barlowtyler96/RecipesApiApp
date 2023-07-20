﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;
using RecipeLibrary.DataAccess;
using RecipeLibrary.Models;
using RecipesApi.Constants;

namespace RecipesApi.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class RecipesController : ControllerBase
{
    private readonly IRecipeData _data;
    private readonly ILogger _logger;

    public RecipesController(IRecipeData data, ILogger<RecipesController> logger)
    {
        _data = data;
        _logger = logger;
    }

    // GET: api/Recipes
    [HttpGet]
    public async Task<ActionResult<List<RecipeModel>>> Get()
    {
        _logger.LogInformation("GET: api/Recipes");

        try
        {
            var output = await _data.GetAll();
            return Ok(output);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "The GET call to api/Recipes failed.");
            return BadRequest();
        }
    }

    // GET: api/Recipes/recent
    [HttpGet("recent")]
    public async Task<ActionResult<List<RecipeModel>>> GetByDate()
    {
        _logger.LogInformation("GET: api/Recipes/recent");

        try
        {
            var output = await _data.GetByDate();
            return Ok(output);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "The GET call to api/Recipes/recent failed.");
            return BadRequest();
        }
    }

    // GET: api/Recipes/keyword/{keyword}
    [HttpGet("keyword/{keyword}")]
    public async Task<ActionResult<List<RecipeModel>>> GetByKeyword(string keyword)
    {
        _logger.LogInformation("GET: api/Recipes/{keyword}", keyword);

        try
        {
            var output = await _data.GetByKeyword(keyword);
            return Ok(output);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "The GET call to api/Recipes/keyword/{keyword} failed.", keyword);
            return BadRequest();
        }
    }

    // GET api/Recipes/id/5
    [HttpGet("id/{recipeId}")]
    public async Task<ActionResult<RecipeModel>> Get(int recipeId)
    {
        _logger.LogInformation("GET: api/Recipes/id/{RecipeId}", recipeId);

        try
        {
            var output = await _data.GetById(recipeId);
            return Ok(output);
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex, 
                "The GET call to {ApiPath} failed. The Id was {RecipeId}", $"api/Recipes/Id", recipeId);
            return BadRequest();
        }
    }
}
