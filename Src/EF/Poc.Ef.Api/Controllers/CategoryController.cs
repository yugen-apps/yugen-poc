using Poc.Ef.Application.Contracts.Dtos;
using Poc.Ef.Application.Contracts.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;

namespace Poc.Ef.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoryController : ControllerBase
{
    private readonly ICategoryService _categoryService;

    public CategoryController(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    [HttpGet]
    //[HttpGet("[action]")]
    //[HttpGet("[action]/{id:int}")]
    public async Task<IActionResult> GetAllAsync()
    {
        var result = await _categoryService.GetAllAsync();

        return result.Succeeded ?
            Ok(result.Data) :
            NotFound();
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetAsync([FromRoute] int id)
    {
        var result = await _categoryService.GetAsync(id);

        return result.Succeeded ?
            Ok(result.Data) :
            NotFound();
    }

    [HttpPost]
    public async Task<IActionResult> AddAsync([FromBody] CreateCategoryInputDto input)
    {
        var result = await _categoryService.AddAsync(input, CancellationToken.None);

        return result.Succeeded ?
            Ok(result.Data) :
            NotFound();
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateAsync(int id, UpdateCategoryInputDto input)
    {
        //if (id != input.Id)
        //{
        //    return BadRequest();
        //}

        var result = await _categoryService.UpdateAsync(id, input, CancellationToken.None);

        return result.Succeeded ?
            Ok(result.Data) :
            NotFound();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAsync([FromRoute] int id)
    {
        var result = await _categoryService.DeleteAsync(id, CancellationToken.None);

        return result.Succeeded ?
            Ok(result.Data) :
            NotFound();
    }
}