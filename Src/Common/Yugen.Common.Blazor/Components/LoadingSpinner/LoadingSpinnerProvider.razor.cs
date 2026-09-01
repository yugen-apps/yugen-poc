using Microsoft.AspNetCore.Components;
using MudBlazor;
using System;

namespace Yugen.Common.Blazor.Components.LoadingSpinner;

public partial class LoadingSpinnerProvider : MudComponentBase, IDisposable
{
    [Inject]
    private ILoadingSpinnerService LoadingSpinnerService { get; set; }

    protected bool IsBusy { get; set; }

    protected override void OnInitialized()
    {
        base.OnInitialized();
        LoadingSpinnerService.OnSpinnerStateChanged += OnSpinnerStateChanged;
    }

    private void OnSpinnerStateChanged(bool isBusy)
    {
        IsBusy = isBusy;
        InvokeAsync(StateHasChanged);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            LoadingSpinnerService.OnSpinnerStateChanged -= OnSpinnerStateChanged;
        }
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}
