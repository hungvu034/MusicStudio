using System.IO.Compression;
using Microsoft.EntityFrameworkCore ; 
using BTL.Models;

var builder = WebApplication.CreateBuilder(args);
string Connect = builder.Configuration.GetSection("ConnectionString").Value; 
// Add services to the container.
builder.Services.AddSingleton(typeof(List<Audio>));
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<EntityModel>(
    (builder) => builder.UseSqlServer(Connect)
);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}");

using (var scope = app.Services.CreateScope()){
    var context = scope.ServiceProvider.GetRequiredService<EntityModel>();
    context.Database.Migrate();
}
app.Run();
