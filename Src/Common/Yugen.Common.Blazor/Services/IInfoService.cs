using System.Collections.Generic;
using System.Threading.Tasks;
//using Yugen.Common.Blazor.Account.Services;

namespace Yugen.Common.Blazor.Services;

public interface IInfoService
{
    // bool CanConnect();
    Task<Dictionary<string, string>> GetAsync();
}

