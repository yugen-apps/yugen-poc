using System;
using System.Threading;
using System.Threading.Tasks;
using Yugen.Common.Blazor.Account.Features.Users.DTOs;

namespace Yugen.Common.Blazor.Account.Features.Users.Services;

public interface IUserService
{
    UserDto UserProfileState { get; }

    event EventHandler<UserDto>? UserChanged;

    Task EnsureInitializedAsync(Guid? userId, CancellationToken cancellationToken = default);
}