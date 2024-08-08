using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RunBuddies.App.Automapper;
using RunBuddies.DataModel;

namespace RunBuddies.App
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            //Add Service to use the Database Context

            builder.Services.AddDbContext<AppDBContext>(opts =>
            {
                opts.UseSqlServer(builder.Configuration.GetConnectionString("Yash"))
                    .EnableSensitiveDataLogging()
                    .UseQueryTrackingBehavior(QueryTrackingBehavior.TrackAll);

            });

            ////Add Service to use the AutoMapper
            builder.Services.AddScoped<AppDBContext>();


            builder.Services.AddAutoMapper(typeof(AutoMapperConfig));

            builder.Services.AddControllersWithViews(options =>
            {
                options.ModelBindingMessageProvider.SetValueMustNotBeNullAccessor(
                    _ => "The field is required.");
            });
            ////Add Service for the Repository

            //Add Service for the Microsoft Identity

            builder.Services.AddIdentity<User, IdentityRole>(options =>
            {   //Configure Authentication Requirements
                options.Password.RequiredLength = 10;
                options.Password.RequireDigit = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;

                options.SignIn.RequireConfirmedEmail = false;
            }).AddEntityFrameworkStores<AppDBContext>();

        //Configuring Application Cookie
            builder.Services.ConfigureApplicationCookie(options =>
            {
            options.LoginPath = "/Profile/SignIn";
            options.LogoutPath = "/Profile/SignOut";
            options.ExpireTimeSpan = TimeSpan.FromHours(1);
                options.SlidingExpiration = true;
            });

            // Add services to the container.
            builder.Services.AddControllersWithViews();

        // Add services for sessions
        builder.Services.AddSession();

            var app = builder.Build();

            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var context = services.GetRequiredService<AppDBContext>();
                var userManager = services.GetRequiredService<UserManager<User>>();
                await context.Database.EnsureCreatedAsync();
                //await context.SeedDataAsync();
            }

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
        app.UseSession();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();
        app.UseAuthentication();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}