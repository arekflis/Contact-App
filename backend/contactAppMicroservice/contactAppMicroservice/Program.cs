using contactAppMicroservice.AutoMapper;
using contactAppMicroservice.Database;
using contactAppMicroservice.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using contactAppMicroservice.Services.ContactServices;
using contactAppMicroservice.Services.CategoryServices;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddScoped<IContactService, ContactService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddDbContext<ContactDbContext>(options =>
    options.UseInMemoryDatabase("ContactAppDb"));

builder.Services.AddAutoMapper(typeof(ContactMappingProfile));

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidIssuer = builder.Configuration["JwtTokenSettings:Issuer"],
            ValidAudience = builder.Configuration["JwtTokenSettings:Audience"],
            ValidateLifetime = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["JwtTokenSettings:Token"])),
            ValidateIssuerSigningKey = true
        };
    });

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ContactDbContext>();
    var databaseInitializer = new DatabaseInitializer(dbContext);
    await databaseInitializer.initializeDatabase();
}


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
