using Spectre.Console.Cli;
using System;

namespace Poc.Common.Spectre;

public sealed class TypeResolver : ITypeResolver
{
    private readonly IServiceProvider _provider;

    public TypeResolver(IServiceProvider provider)
    {
        _provider = provider;
    }

    public object? Resolve(Type? type) => type == null ? null : _provider.GetService(type);

    public object? Resolve(string name) => Resolve(CommandRegistry.GetCommandDefinition(name)?.Type);
}
