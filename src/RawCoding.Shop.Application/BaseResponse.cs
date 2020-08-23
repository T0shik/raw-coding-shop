namespace RawCoding.Shop.Application
{
    public struct BaseResponse
    {
        public BaseResponse(string message, bool success = true)
        {
            Message = message;
            Success = success;
        }

        public string Message { get; set; }
        public bool Success { get; set; }
    }
}