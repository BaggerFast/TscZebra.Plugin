namespace TscZebra.Plugin.Common;

internal abstract class BaseValidator<T>
{
    public abstract bool Validate(T item);
}