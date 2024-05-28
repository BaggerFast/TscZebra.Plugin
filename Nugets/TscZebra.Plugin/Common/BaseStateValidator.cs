namespace TscZebra.Plugin.Common;

public abstract class BaseValidator<T>
{
    public abstract bool Validate(T item);
}