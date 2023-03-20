using IdentityServices;
using Infra;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();


//builder.Services.AddMediatR(Assembly.GetExecutingAssembly());
//builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());

builder.Services.AddEndpointsApiExplorer()
    .AddSwagger("Tatweer Identity API")
    .AddCustomMvc(Assembly.GetExecutingAssembly())
    .AddJWT()
    .AddConnactionDb()
    .AddSectionFromAppSetting()
    .AddDependencieInjection();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();
app.UseSwagger();
app.UseSwaggerUI();
app.UseCustomMvc();

app.Run();
