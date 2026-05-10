namespace RazorPages.DTOs.Customers
{
    /// <summary>Response trả về từ API (cả Login và Register đều trả về { message }).</summary>
    public class ApiMessageResponse
    {
        public string Message { get; set; } = string.Empty;
    }

    /// <summary>Response khi API trả về validation errors (400 BadRequest).</summary>
    public class ApiErrorResponse
    {
        public string Message { get; set; } = string.Empty;
        public List<string> Errors { get; set; } = [];
    }
}
