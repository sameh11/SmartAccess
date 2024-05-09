namespace SmartAccess.Locking.API.Model.Response
{
    public class LockAccessError : AccessResponse
    {
        public LockAccessError()
        {
        }
        public LockAccessError(int responseCode, string responseMessage)
        {
            ResponseCode = responseCode;
            ResponseMessage += "Lock Error";
        }
    }
}
