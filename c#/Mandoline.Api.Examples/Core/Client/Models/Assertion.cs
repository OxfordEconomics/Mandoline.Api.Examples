using System.Net;

namespace Core.Client.Models;

public class Assertion<T>
{
    private readonly bool _passed;
    private readonly string _description;
    private readonly T _result;
    private readonly HttpStatusCode _statusCode;

    public Assertion(bool passed, HttpStatusCode statusCode, string description, T result)
    {
        this._statusCode = statusCode;
        this._passed = passed;
        this._result = result;
        this._description = description;
    }

    public HttpStatusCode Reason
    {
        get
        {
            return this._statusCode;
        }
    }

    public string Description
    {
        get
        {
            return this._description;
        }
    }

    public T Result
    {
        get
        {
            return this._result;
        }
    }

    public bool Passed
    {
        get
        {
            return this._passed;
        }
    }

    public bool Failed
    {
        get
        {
            return !this._passed;
        }
    }

    public static Assertion<T> Pass(T result, HttpStatusCode statusCode = HttpStatusCode.OK)
    {
        return new Assertion<T>(true, statusCode, string.Empty, result);
    }

    public static Assertion<T> Fail(T result, HttpStatusCode statusCode, string reason)
    {
        return new Assertion<T>(false, statusCode, reason, result);
    }

    public static Assertion<T> Fail<T2>(Assertion<T2> result)
        where T2 : class
    {
        return Fail(default(T), result.Reason, result.Description);
    }
}
