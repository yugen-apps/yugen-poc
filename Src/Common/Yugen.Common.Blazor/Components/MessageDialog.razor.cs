using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Yugen.Common.Blazor.Components;

public partial class MessageDialog
{
    [CascadingParameter]
    private IMudDialogInstance MudDialog { get; set; }

    [Parameter]
    public string ContentText { get; set; }

    [Parameter]
    public string ButtonText { get; set; }

    [Parameter]
    public Color Color { get; set; }

    private void Submit() => MudDialog.Close(DialogResult.Ok(true));

    private void Cancel() => MudDialog.Cancel();
}
