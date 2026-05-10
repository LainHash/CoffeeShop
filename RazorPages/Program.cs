using Microsoft.AspNetCore.Authentication.Cookies;
using RazorPages.Services.Implementations;
using RazorPages.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddRazorPages();


builder.Services.AddHttpClient("API", client =>
{
    client.BaseAddress = new Uri("https://localhost:7181/api/");
    client.DefaultRequestHeaders.Add("Accept", "application/json");
});

builder.Services.AddScoped<IProductApiService, ProductApiService>();
builder.Services.AddScoped<ICategoryApiService, CategoryApiService>();
builder.Services.AddScoped<ICustomerApiService, CustomerApiService>();


builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(opt => {
        opt.LoginPath = "/Account/Login";
        opt.ExpireTimeSpan = TimeSpan.FromHours(8);
    });

builder.Services.AddSession();
builder.Services.AddHttpContextAccessor();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseSession();
app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();

app.Run();
