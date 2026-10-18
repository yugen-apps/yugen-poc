using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Http;
using Poc.Shared.Lifetime;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Poc.Blazor.Components.Pages;

public partial class Lifetime
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IOperationTransient _transientOperation;
    private readonly IOperationScoped _scopedOperation;
    private readonly IOperationSingleton _singletonOperation;
    private readonly OperationService _operationService;
    private List<string> results = [];

    public Lifetime(
        IOperationTransient transientOperation,
        IOperationScoped scopedOperation,
        IOperationSingleton singletonOperation,
        OperationService operationService,
        IHttpContextAccessor httpContextAccessor)
    {
        _transientOperation = transientOperation;
        _scopedOperation = scopedOperation;
        _singletonOperation = singletonOperation;
        _operationService = operationService;
        _httpContextAccessor = httpContextAccessor;
    }

    protected override void OnInitialized()
    {
        UpdateResult(_transientOperation, nameof(_transientOperation));
        UpdateResult(_scopedOperation, nameof(_scopedOperation));
        UpdateResult(_singletonOperation, nameof(_singletonOperation));

        UpdateHttpContextResult();

        _operationService.Print();

        base.OnInitialized();
    }

    private void OnClick()
    {
        UpdateResult(_transientOperation, nameof(_transientOperation), "Test1");
        UpdateResult(_scopedOperation, nameof(_scopedOperation), "Test2");
        UpdateResult(_singletonOperation, nameof(_singletonOperation), "Test3");

        if (_httpContextAccessor.HttpContext != null)
        {
            _httpContextAccessor.HttpContext?.Items["Key"] = "Test4";
        }
        UpdateHttpContextResult();

        _operationService.Print();
    }

    private Task UpdateHttpContextResult()
    {
        string? value = string.Empty;
        if (_httpContextAccessor.HttpContext?.Items.TryGetValue("Key", out var tmp) == true)
        {
            value = tmp as string;
        }
        results.Add($"HttpContext: {value}");
        return Task.CompletedTask;
    }

    private Task UpdateResult(IOperation operation, string? name, string? value = null)
    {
        if (!string.IsNullOrWhiteSpace(value))
        {
            operation.TestString = value;
        }
        results.Add($"{name}: {operation.OperationId} - {operation.TestString}");
        return Task.CompletedTask;
    }
}
