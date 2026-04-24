using System;

namespace Mors.AppPlatform.Adapters.Dispatching;

public sealed class CommandKey : IEquatable<CommandKey>
{
    private readonly Type _commandType;

    private CommandKey(Type commandType)
    {
        _commandType = commandType;
    }

    public static CommandKey From(Type commandType)
    {
        return new CommandKey(commandType);
    }

    public static CommandKey From(object command)
    {
        var commandType = command.GetType();
        return new CommandKey(commandType);
    }

    public static CommandKey From<TCommand>()
    {
        var commandType = typeof(TCommand);
        return new CommandKey(commandType);
    }

    public bool Equals(CommandKey? other)
    {
        return other != null && other._commandType == _commandType;
    }

    public override bool Equals(object? obj)
    {
        return obj is CommandKey other && Equals(other);
    }

    public override int GetHashCode()
    {
        return _commandType.GetHashCode();
    }

    public override string ToString()
    {
        return _commandType.ToString();
    }
}