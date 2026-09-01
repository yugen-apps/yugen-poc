using System.ComponentModel.DataAnnotations;

namespace Yugen.Common.Blazor.Navigation;

public enum PageStatus
{
    [Display(Name = "Coming Soon")]
    ComingSoon,

    [Display(Name = "WIP")]
    Wip,

    [Display(Name = "New")]
    New,

    [Display(Name = "Completed")]
    Completed
}