using RazorPages.DTOs.Products;
using System.Net.Http.Headers;

namespace RazorPages.Services
{
    public class ProductApiService
    {
        private readonly HttpClient _http;
        private readonly IHttpContextAccessor _ctx;

        public ProductApiService(IHttpClientFactory factory, IHttpContextAccessor ctx)
        {
            _http = factory.CreateClient("API");
            _ctx = ctx;
        }

        // Gắn JWT token vào header nếu đã đăng nhập
        private void AttachToken()
        {
            var token = _ctx.HttpContext?.Session.GetString("JwtToken");
            if (!string.IsNullOrEmpty(token))
                _http.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", token);
        }

        public async Task<List<ProductBaseDTO>> GetAllAsync()
        {
            var res = await _http.GetAsync("product");
            res.EnsureSuccessStatusCode();
            return await res.Content.ReadFromJsonAsync<List<ProductBaseDTO>>() ?? [];
        }

        //public async Task CreateAsync(ProductBaseDTO dto)
        //{
        //    AttachToken();
        //    var res = await _http.PostAsJsonAsync("products", dto);
        //    res.EnsureSuccessStatusCode();
        //}
    }
}
