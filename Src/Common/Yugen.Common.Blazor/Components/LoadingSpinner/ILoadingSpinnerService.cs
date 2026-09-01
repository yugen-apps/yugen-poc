using System;

namespace Yugen.Common.Blazor.Components.LoadingSpinner;

public interface ILoadingSpinnerService
{
    void Wait();

    void Resume();

    event Action<bool> OnSpinnerStateChanged;
}
