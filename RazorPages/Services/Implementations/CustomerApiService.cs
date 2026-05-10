using RazorPages.DTOs.Customers;
using RazorPages.Helpers.Constants;
using RazorPages.Services.Interfaces;
using System.Net;
using System.Net.Http.Headers;
using System.Text.Json;

namespace RazorPages.Services.Implementations
{
    public class CustomerApiService : ICustomerApiService
    {
        private readonly HttpClient _http;
        private readonly IHttpContextAccessor _ctx;
        private readonly ILogger<CustomerApiService> _logger;

        private static readonly JsonSerializerOptions _jsonOpts = new()
        {
            PropertyNameCaseInsensitive = true
        };

        public CustomerApiService(
            IHttpClientFactory factory,
            IHttpContextAccessor ctx,
            ILogger<CustomerApiService> logger)
        {
            _http = factory.CreateClient("API");
            _ctx = ctx;
            _logger = logger;
        }

        public async Task<(bool Success, string Message)> LoginAsync(LoginRequestDTO dto)
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Post, $"{RouteName.Customer}/login");
                request.Content = JsonContent.Create(dto);

                using var response = await _http.SendAsync(request);
                var json = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    // Lưu cookie jwt nhận được từ API vào session của Razor Pages
                    if (response.Headers.TryGetValues("Set-Cookie", out var cookies))
                    {
                        foreach (var cookie in cookies)
                        {
                            if (cookie.StartsWith("jwt="))
                            {
                                var jwtValue = cookie.Split(';')[0]["jwt=".Length..];
                                _ctx.HttpContext?.Session.SetString("JwtToken", jwtValue);
                            }
                        }
                    }

                    var result = JsonSerializer.Deserialize<ApiMessageResponse>(json, _jsonOpts);
                    return (true, result?.Message ?? "Đăng nhập thành công");
                }

                // 401 Unauthorized
                if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    var err = JsonSerializer.Deserialize<ApiMessageResponse>(json, _jsonOpts);
                    return (false, err?.Message ?? "Sai tên đăng nhập hoặc mật khẩu");
                }

                // 400 Bad Request — validation errors
                if (response.StatusCode == HttpStatusCode.BadRequest)
                {
                    var err = JsonSerializer.Deserialize<ApiErrorResponse>(json, _jsonOpts);
                    var errorText = err?.Errors.Count > 0
                        ? string.Join("; ", err.Errors)
                        : (err?.Message ?? "Dữ liệu không hợp lệ");
                    return (false, errorText);
                }

                return (false, $"Lỗi không xác định ({(int)response.StatusCode})");
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Không thể kết nối tới API khi login");
                return (false, "Không thể kết nối đến máy chủ. Vui lòng thử lại sau.");
            }
        }

        public async Task<(bool Success, string Message)> RegisterAsync(RegisterRequestDTO dto)
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Post, $"{RouteName.Customer}/register");
                request.Content = JsonContent.Create(dto);

                using var response = await _http.SendAsync(request);
                var json = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    var result = JsonSerializer.Deserialize<ApiMessageResponse>(json, _jsonOpts);
                    return (true, result?.Message ?? "Đăng ký thành công! Vui lòng kiểm tra email.");
                }

                // 409 Conflict — email/username đã tồn tại
                if (response.StatusCode == HttpStatusCode.Conflict)
                {
                    var err = JsonSerializer.Deserialize<ApiMessageResponse>(json, _jsonOpts);
                    return (false, err?.Message ?? "Thông tin đã được sử dụng");
                }

                // 400 Bad Request — validation errors
                if (response.StatusCode == HttpStatusCode.BadRequest)
                {
                    var err = JsonSerializer.Deserialize<ApiErrorResponse>(json, _jsonOpts);
                    var errorText = err?.Errors.Count > 0
                        ? string.Join("; ", err.Errors)
                        : (err?.Message ?? "Dữ liệu không hợp lệ");
                    return (false, errorText);
                }

                return (false, $"Lỗi không xác định ({(int)response.StatusCode})");
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Không thể kết nối tới API khi register");
                return (false, "Không thể kết nối đến máy chủ. Vui lòng thử lại sau.");
            }
        }
    }
}
