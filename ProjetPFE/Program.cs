using Microsoft.AspNetCore.Http.Features;
using ProjetPFE.Context;
using ProjetPFE.Contracts;
using ProjetPFE.Contracts.services;
using ProjetPFE.EmailService;
using ProjetPFE.EmailService.EmailEntities;
using ProjetPFE.EmailService.Interfaces;
using ProjetPFE.Repository;
using ProjetPFE.Repository.service;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddSingleton<DapperContext>();
builder.Services.AddScoped<IDemandeRepository, DemandeRepository>();
builder.Services.AddScoped<IEmployeRepository, EmployeRepository>();
builder.Services.AddScoped<IOffreRepository, OffreRepository>();
builder.Services.AddScoped<IArchiveRepository, ArchiveRepository>();
builder.Services.AddScoped<IEmployeService, EmployeService>();
builder.Services.AddScoped<IStatutService, StatutService>();
builder.Services.AddScoped<IDiplomeRepository, DiplomeRepository>();
builder.Services.AddControllers();
builder.Services.AddAutoMapper(typeof(DapperProfile));

var emailConfig = builder.Configuration
        .GetSection("EmailConfiguration")
        .Get<EmailConfiguration>();
builder.Services.AddSingleton(emailConfig);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors();

builder.Services.Configure<FormOptions>(o => {
    o.ValueLengthLimit = int.MaxValue;
    o.MultipartBodyLengthLimit = int.MaxValue;
    o.MemoryBufferThreshold = int.MaxValue;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors(policy => policy.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());

app.UseAuthorization();

app.MapControllers();

app.Run();
