using System.Threading.Tasks;
using Yugen.Common.Blazor.Account.Features.Auth.DTOs;

namespace Yugen.Common.Blazor.Account.Features.Auth.Services;

public interface IAuthService
{
    Task<CurrentUser> GetCurrentUserAsync();
}