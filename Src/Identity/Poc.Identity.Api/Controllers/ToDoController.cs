using Microsoft.AspNetCore.Mvc;
using Poc.Identity.Application.Features.ToDoItems.DTOs;
using Poc.Identity.Application.Features.ToDoItems.Queries;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Yugen.Common.Data.Cqrs;

namespace Poc.Identity.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ToDoController : ControllerBase
{
    private readonly IQueryHandler<GetAllToDoItemsQuery, List<ToDoItemDto>> _allHandler;
    private readonly IQueryHandler<GetTenantToDoItemsQuery, List<ToDoItemDto>> _tenantHandler;

    public ToDoController(
        IQueryHandler<GetAllToDoItemsQuery, List<ToDoItemDto>> allHandler,
        IQueryHandler<GetTenantToDoItemsQuery, List<ToDoItemDto>> tenantHandler)
    {
        _allHandler = allHandler;
        _tenantHandler = tenantHandler;
    }

    [HttpGet]
    //[HttpGet("[action]")]
    //[HttpGet("[action]/{id:int}")]
    public async Task<IActionResult> GetAllAsync()
    {
        var result = await _allHandler.Handle(new GetAllToDoItemsQuery(), CancellationToken.None);

        return result.IsSuccess ?
            Ok(result.Value) :
            NotFound();
    }

    [HttpGet("{tenantId:guid}")]
    public async Task<IActionResult> GetAsync([FromRoute] Guid? tenantId)
    {
        // TODO: not secure
        var result = await _tenantHandler.Handle(new GetTenantToDoItemsQuery(tenantId), CancellationToken.None);

        return result.IsSuccess ?
            Ok(result.Value) :
            NotFound();
    }

    //[HttpPost]
    //public async Task<IActionResult> AddAsync([FromBody] CreateCategoryInputDto input)
    //{
    //    Result<CategoryDto> result = await _categoryService.AddAsync(input, CancellationToken.None);

    //    return result.IsSuccess ?
    //        Ok(result.Value) :
    //        NotFound();
    //}

    //[HttpPut("{id}")]
    //public async Task<IActionResult> UpdateAsync(int id, UpdateCategoryInputDto input)
    //{
    //    //if (id != input.Id)
    //    //{
    //    //    return BadRequest();
    //    //}

    //    Result<CategoryDto> result = await _categoryService.UpdateAsync(id, input, CancellationToken.None);

    //    return result.IsSuccess ?
    //        Ok(result.Value) :
    //        NotFound();
    //}

    //[HttpDelete("{id}")]
    //public async Task<IActionResult> DeleteAsync([FromRoute] int id)
    //{
    //    Result<int> result = await _categoryService.DeleteAsync(id, CancellationToken.None);

    //    return result.IsSuccess ?
    //        Ok(result.Value) :
    //        NotFound();
    //}
}