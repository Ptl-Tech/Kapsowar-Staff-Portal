var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
builder.Services.AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.PropertyNamingPolicy = null; // Use the original property names
                    options.JsonSerializerOptions.WriteIndented = true; // Format the JSON output for readability
                });
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
//builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();
//<custom>
//to enable use of sessions
builder.Services.AddDistributedMemoryCache();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromSeconds(3600);//60 mins
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

//</custom>
var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();
//<custom>
app.MapControllerRoute(
    name: "default",
    pattern: "api/{controller=Home}/{action=Index}/{id?}");
app.UseDefaultFiles();
app.UseStaticFiles();
//</custom>
// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
//<custom>
app.UseSession();
//</custom>
app.MapFallbackToFile("/index.html");

app.Run();
