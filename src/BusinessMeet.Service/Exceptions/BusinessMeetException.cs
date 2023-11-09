namespace BusinessMeet.Service.Exceptions;

public class BusinessMeetException : Exception
{
    public int StatusCode { get; set; }

    public BusinessMeetException(int code, string message) : base(message)
    {
        StatusCode = code;
    }
}
