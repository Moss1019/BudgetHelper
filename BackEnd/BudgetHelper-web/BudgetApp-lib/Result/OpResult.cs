namespace BudgetApp_lib.Result;

public class OpResult<T>
{
    private bool _isSuccessful;
    public bool IsSuccessful
    {
        get { return _isSuccessful; }
    }

    private T? _value;
    public T Value
    {
        get { return _value!; }
    }

    private string? _message;
    public string Message
    {
        get { return _message!; }
    }

    private OpResult(bool isSuccessful, T? value, string? message)
    {
        _isSuccessful = isSuccessful;
        _value = value;
        _message = message;
    }

    public static OpResult<T> Success(T value)
    {
        return new OpResult<T>(true, value, null);
    }

    public static OpResult<T> Failed(string message)
    {
        return new OpResult<T>(false, default(T), message);
    }
}