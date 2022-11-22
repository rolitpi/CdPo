using System.Reflection;

using CdPo.Model.Interfaces;
using CdPo.Model.Interfaces.Files;
using CdPo.Web.DataAccess;
using CdPo.Web.Providers;
using CdPo.Web.Services.Files;
using CdPo.Web.Services.Storage;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers(o => 
        o.Conventions.Add(new GenericControllerRouteConvention())).
    ConfigureApplicationPartManager(m => 
        m.FeatureProviders.Add(new GenericTypeControllerFeatureProvider()));
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "ЦДПО", Version = "v1" });
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath, true);
});
builder.Services.AddDbContext<DataContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("Default"));
});

builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(EfGenericRepository<>)); 
builder.Services.TryAddTransient<IFileManager, FileManager>();
builder.Services.TryAddTransient<IFileProvider, LocalFileProvider>();
builder.Services.TryAddTransient<IFileMetadataRepository, EfFileMetadataRepository>();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.MapControllers();

app.Run();