using System.Threading.Tasks;
using Yugen.Common.Blazor.Account.Models;

namespace Yugen.Common.Blazor.Account.Features.Auth;

public interface IAuthService
{
    Task<CurrentUser> GetCurrentUserAsync();
}