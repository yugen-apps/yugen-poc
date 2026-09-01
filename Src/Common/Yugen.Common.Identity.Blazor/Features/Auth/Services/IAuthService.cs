using System.Threading.Tasks;
using Yugen.Common.Identity.Blazor.Features.Auth.DTOs;

namespace Yugen.Common.Identity.Blazor.Features.Auth.Services;

public interface IAuthService
{
    Task<CurrentUser> GetCurrentUserAsync();
}