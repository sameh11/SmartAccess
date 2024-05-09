namespace SmartAccess.Locking.API.Model.Response
{
    public class LockAccessDenied : AccessResponse
    {
        public LockAccessDenied() : base()
        {
            ResponseCode = 3;
            ResponseMessage = "Denied, User doesnt have required Access";
        }
    }
}
