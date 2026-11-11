namespace StudentsServer;

// The body of every error response: {"ErrorMessage": "..."}
public class Error
{
    public string ErrorMessage { get; set; }

    public Error(string errorMessage)
    {
        ErrorMessage = errorMessage;
    }
}
