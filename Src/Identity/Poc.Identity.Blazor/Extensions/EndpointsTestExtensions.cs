using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Poc.Identity.Blazor.Extensions;

public static class EndpointsTestExtensions
{
    public static void MapTestEndpoints(this WebApplication app)
    {
        var mapGroup = app.MapGroup("/test");
        mapGroup.MapGet("/anon", GetAnon).AllowAnonymous();
        mapGroup.MapGet("/anon/{text}", GetAnon2).AllowAnonymous();
        mapGroup.MapGet("/auth", GetAuth).RequireAuthorization();
        //mapGroup.MapPost("/", CreateTodo);
    }

    private static IResult GetAnon([FromQuery] string? text)
    {
        return string.IsNullOrEmpty(text)
                ? TypedResults.NotFound()
                : TypedResults.Ok(text);
    }

    private static IResult GetAnon2([FromRoute] string? text)
    {
        return string.IsNullOrEmpty(text)
                ? TypedResults.NotFound()
                : TypedResults.Ok(text);
    }

    private static IResult GetAuth()
    {
        return TypedResults.Ok("hello");
    }

    //private static async Task<IResult> CreateTodo(Todo todo, TodoDb db)
    //{
    //    db.Todos.Add(todo);
    //    await db.SaveChangesAsync();

    //    return TypedResults.Created($"/todoitems/{todo.Id}", todo);
    //}
}