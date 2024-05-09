namespace SmartAccess.Locking.API.Model.Response
{
    public class LockAccessSuccess : AccessResponse
    {
        public LockAccessSuccess(int responseCode, string responseMessage = "the lock has been opened successfuly") 
            : base(responseCode, responseMessage)
        {
            ResponseCode = 1;
        }
    }
}
