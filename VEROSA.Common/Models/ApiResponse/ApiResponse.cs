namespace VEROSA.Common.Models.ApiResponse
{
    public class ApiResponse<T>
    {
        public int Code { get; set; }
        public bool Success { get; set; }
        public string? Message { get; set; }
        public object? Data { get; set; }
    }
}
