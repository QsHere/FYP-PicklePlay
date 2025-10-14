using FYP_QS_CODE.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

// register in-memory repo (swap with EF/MySQL later)
builder.Services.AddSingleton<IScheduleRepository, InMemoryScheduleRepository>();

var app = builder.Build();

// seed demo data
using (var scope = app.Services.CreateScope())
{
    var repo = scope.ServiceProvider.GetRequiredService<IScheduleRepository>();
    repo.Seed();
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Schedule}/{action=Index}/{id?}");

app.Run();
