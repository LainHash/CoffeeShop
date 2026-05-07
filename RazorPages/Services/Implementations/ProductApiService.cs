using RazorPages.DTOs.Products;
using RazorPages.DTOs.Products.Create;
using RazorPages.DTOs.Products.Update;
using RazorPages.Helpers.Constants;
using RazorPages.Services.Interfaces;
using System.Net.Http.Headers;

namespace RazorPages.Services.Implementations
{
    public class ProductApiService : IProductApiService
    {
        private readonly HttpClient _http;
        private readonly IHttpContextAccessor _ctx;

        public ProductApiService(IHttpClientFactory factory, IHttpContextAccessor ctx)
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

        public async Task<List<ProductDTO>> GetAllAsync()
        {
            using var request = CreateRequest(HttpMethod.Get, RouteName.Product);
            using var response = await _http.SendAsync(request);

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<List<ProductDTO>>() ?? [];
        }

        public async Task<ProductDTO?> GetByIdAsync(Guid id)
        {
            using var request = CreateRequest(HttpMethod.Get, $"{RouteName.Product}/{id}");
            using var response = await _http.SendAsync(request);

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<ProductDTO>();
        }

        public async Task<bool> CreateAsync(CreateProductDTO dto)
        {
            using var request = CreateRequest(HttpMethod.Post, RouteName.Product);
            request.Content = JsonContent.Create(dto);

            using var response = await _http.SendAsync(request);
            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new Exception(error);
            }
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateAsync(Guid id, UpdateProductDTO dto)
        {
            using var request = CreateRequest(HttpMethod.Patch, $"{RouteName.Product}/{id}");
            request.Content = JsonContent.Create(dto);

            using var response = await _http.SendAsync(request);
            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new Exception(error);
            }
            return response.IsSuccessStatusCode;
        }

        public async Task DeleteAsync(Guid id)
        {
            using var request = CreateRequest(
                HttpMethod.Delete,
                $"{RouteName.Product}/{id}");

            using var response = await _http.SendAsync(request);

            response.EnsureSuccessStatusCode();
        }
    }
}
