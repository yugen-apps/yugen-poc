using System;
using System.Threading.Tasks;
using Yugen.Common.Blazor.Account.Features.Users.DTOs;

namespace Poc.Identity.Blazor.Services.User;

public interface IUserService
{
    UserDto UserProfileState { get; }

    event EventHandler? UserChanged;

    Task InitializedAsync();
}