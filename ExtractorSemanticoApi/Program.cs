using ExtractorSemanticoApi.Application.Interfaces;
using ExtractorSemanticoApi.Dominio;
using ExtractorSemanticoApi.Persistencia.Repository;
using ExtractorSemanticoApi.Services;
using Microsoft.EntityFrameworkCore;
using NLog.Web;
using System;

var builder = WebApplication.CreateBuilder(args);

// Configuración de NLog
builder.Logging.ClearProviders();
builder.Host.UseNLog();

// Configuración de CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularApp", policy =>
    {
        policy.WithOrigins("http://localhost:4200")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

// Configuración de servicios
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configuración de MediatR
builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssemblies(typeof(Program).Assembly));

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ExtractorsemanticoContext>(options =>
    options.UseMySql(
        connectionString,
        new MySqlServerVersion(new Version(8, 0, 33)),
        mysqlOptions =>
        {
            mysqlOptions.EnableRetryOnFailure(
                maxRetryCount: 5,
                maxRetryDelay: TimeSpan.FromSeconds(30),
                errorNumbersToAdd: null);
        }));

// Registro de servicios y repositorios
builder.Services.AddScoped<ITextProcessingService, TextProcessingService>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IReviewRepository, ReviewRepository>();
builder.Services.AddScoped<IExtractedDataRepository, ExtractedDataRepository>();
builder.Services.AddScoped<ISentimentRepository, SentimentRepository>();
builder.Services.AddScoped<IRdfTripleRepository, RdfTripleRepository>();
builder.Services.AddScoped<ITextProcessingService, TextProcessingService>();

// Configuración de HttpClient para servicios externos (si es necesario)
builder.Services.AddHttpClient();

var app = builder.Build();

// Configuración del pipeline HTTP
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();

// Agregar CORS antes de UseRouting
app.UseCors("AllowAngularApp");

app.UseRouting();
app.UseAuthorization();

app.MapControllers();

// Aplicar migraciones automáticamente (opcional)
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ExtractorsemanticoContext>();
    dbContext.Database.Migrate();
}

app.Run();