using FluentValidation.Results;

namespace PizzaStore.Lib.Services;

public class OperationResult<T> where T : new()
{
    private readonly string[] _errorMessages = [];
    private OperationResult(List<ValidationFailure> errors)
    {
        _errorMessages = errors.Select(e => e.ErrorMessage).ToArray();
    }
    private OperationResult(T value)
    {
        IsSuccess = true;
        Value = value;
    }

    public bool IsSuccess { get; }
    public T? Value { get; }
    public string[] ErrorMessages => _errorMessages;

    internal static OperationResult<T> Fail(List<ValidationFailure> errors)
    {
        return new OperationResult<T>(errors);
    }

    internal static OperationResult<T> Success(T value)
    {
        return new OperationResult<T>(value);
    }
}