

namespace API.Errors
{
    public class APIException
    {
       
        public APIException(int statusCode,string message,string details)
        {
            StatusCode = statusCode;
            Message=message;
            Details=details;
        }
        public int StatusCode{set; get;}
        public string Message{set; get;}
        public string Details{set; get;}
        
    }
}