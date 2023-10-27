namespace ShelbyBooks.Logic.Exceptions;

public class NotFoundException : Exception
{
    public int Code { get; }
    public override string Message { get; }

    public NotFoundException(string message)
    {
        Code = 500;
        Message = message;
    }
}