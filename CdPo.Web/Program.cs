using System.Reflection;

using Castle.Windsor;

using CdPo.Model.Configuration;
using CdPo.Web.DataAccess;
using CdPo.Web.Providers;
using CdPo.Web.StartupHelpers;

using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

IWindsorContainer container = new WindsorContainer();
builder.Host.UseWindsorContainerServiceProvider(container);

builder.Services.AddControllers(o => 
        o.Conventions.Add(new GenericControllerRouteConvention()))
    .ConfigureApplicationPartManager(m => 
        m.FeatureProviders.Add(new GenericTypeControllerFeatureProvider()));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "ЦДПО", Version = "v1" });
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath, true);
    c.EnableAnnotations();
});
builder.Services.AddDbContext<DataContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("Default"));
});
builder.Services.AddControllersWithViews()
    .AddNewtonsoftJson(options =>
        options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
    );
builder.Services.Configure<FileManagerSection>(builder.Configuration.GetSection("FileManager"));
builder.Services.AddRazorPages();

ServicesRegistrationHelper.RegisterServices(container);
PrintFilesHelper.RegisterReportQueries(container);

var app = builder.Build();

app.UseStaticFiles();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.MapControllers();

app.MapControllerRoute(
  name: "api",
  pattern: "/api/{controller}/{id?}");

app.MapFallbackToFile("/index.html");

app.Run();