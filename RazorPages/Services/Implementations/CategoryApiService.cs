using RazorPages.DTOs.Categories;
using RazorPages.DTOs.Products;
using RazorPages.Helpers.Constants;
using RazorPages.Services.Interfaces;
using System.Net.Http.Headers;

namespace RazorPages.Services.Implementations
{
    public class CategoryApiService : ICategoryApiService
    {
        private readonly HttpClient _http;
        private readonly IHttpContextAccessor _ctx;

        public CategoryApiService(IHttpClientFactory factory, IHttpContextAccessor ctx)
        {
            _http = factory.CreateClient("API");
            _ctx = ctx;
        }

        private HttpRequestMessage CreateRequest(HttpMethod method, string url)
        {
            var request = new HttpRequestMessage(method, url);

            var token = _ctx.HttpContext?.Session.GetString("JwtToken");
            if (!string.IsNullOrWhiteSpace(token))
            {
                request.Headers.Authorization =
                    new AuthenticationHeaderValue("Bearer", token);
            }

            return request;
        }
        public async Task<IEnumerable<CategoryDTO>> GetAllAsync()
        {
            using var request = CreateRequest(HttpMethod.Get, RouteName.Category);
            using var response = await _http.SendAsync(request);

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<List<CategoryDTO>>() ?? [];
        }

        public async Task<CategoryDTO?> GetByIdAsync(int id)
        {
            using var request = CreateRequest(HttpMethod.Get, $"{RouteName.Category}/{id}");
            using var response = await _http.SendAsync(request);

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<CategoryDTO>();
        }
    }
}
