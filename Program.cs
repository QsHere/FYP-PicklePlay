using FYP_QS_CODE.Data;
using Microsoft.EntityFrameworkCore; // Add this

var builder = WebApplication.CreateBuilder(args);

// --- Add DbContext ---
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));
// ---------------------

builder.Services.AddControllersWithViews();

// --- SWAP REPOSITORIES ---
// REMOVE: builder.Services.AddSingleton<IScheduleRepository, InMemoryScheduleRepository>();
// ADD:
builder.Services.AddScoped<IScheduleRepository, MySqlScheduleRepository>();
// -------------------------

var app = builder.Build();

// --- REMOVE SEEDING ---
// The old seed method is for in-memory data.
// using (var scope = app.Services.CreateScope())
// {
//     var repo = scope.ServiceProvider.GetRequiredService<IScheduleRepository>();
//     repo.Seed();
// }
// ------------------------

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