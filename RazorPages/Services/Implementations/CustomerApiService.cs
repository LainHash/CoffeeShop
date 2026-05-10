using RazorPages.DTOs.Customers;
using RazorPages.Helpers.Constants;
using RazorPages.Services.Interfaces;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;

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
                    if (response.Headers.TryGetValues("Set-Cookie", out var cookies))
                    {
                        foreach (var cookie in cookies)
                        {
                            if (cookie.StartsWith("jwt="))
                            {
                                var jwtValue = cookie.Split(';')[0]["jwt=".Length..];
                                _ctx.HttpContext?.Session.SetString("JwtToken", jwtValue);

                                try
                                {
                                    var parts = jwtValue.Split('.');
                                    if (parts.Length == 3)
                                    {
                                        var payload = parts[1];
                                        payload = payload.Replace('-', '+').Replace('_', '/');
                                        payload += (payload.Length % 4) switch { 2 => "==", 3 => "=", _ => "" };
                                        var json64 = Encoding.UTF8.GetString(Convert.FromBase64String(payload));
                                        var node = JsonNode.Parse(json64);
                                        var username = node?["unique_name"]?.GetValue<string>()
                                                    ?? node?["name"]?.GetValue<string>();
                                        if (!string.IsNullOrEmpty(username))
                                            _ctx.HttpContext?.Session.SetString("Username", username);
                                    }
                                }
                                catch {  }
                            }
                        }
                    }

                    var result = JsonSerializer.Deserialize<ApiMessageResponse>(json, _jsonOpts);
                    return (true, result?.Message ?? "Đăng nhập thành công");
                }
                
                if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    var err = JsonSerializer.Deserialize<ApiMessageResponse>(json, _jsonOpts);
                    return (false, err?.Message ?? "Sai tên đăng nhập hoặc mật khẩu");
                }

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

                if (response.StatusCode == HttpStatusCode.Conflict)
                {
                    var err = JsonSerializer.Deserialize<ApiMessageResponse>(json, _jsonOpts);
                    return (false, err?.Message ?? "Thông tin đã được sử dụng");
                }

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
