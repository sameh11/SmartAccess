namespace SmartAccess.Locking.API.Model.Response
{
    public class AccessResponse
    {
        public AccessResponse()
        {
        }
        public AccessResponse(int responseCode = 0, string responseMessage = "Unknown Response")
        {
            ResponseCode = responseCode;
            ResponseMessage += responseMessage;
        }
        public int ResponseCode { get; set; }
        public string ResponseMessage { get; set; }

    }
}
