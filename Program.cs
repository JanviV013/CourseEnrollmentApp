var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Register HttpClient service for external API calls
builder.Services.AddHttpClient();

// Enable Session service
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(20);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// Add Memory Cache service (for caching course list in memory)
builder.Services.AddMemoryCache();

// Add Response Caching service (for caching HTTP responses)
builder.Services.AddResponseCaching();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

// Serve static files (CSS, JS, images)
app.UseStaticFiles();

// Routing middleware
app.UseRouting();

// Enable Session middleware (must be placed after UseRouting and before UseAuthorization)
app.UseSession();

// Enable Response Caching middleware
app.UseResponseCaching();

app.UseAuthorization();

// Map default controller route directly to CourseController -> Index Action
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Course}/{action=Index}/{id?}");

app.Run();