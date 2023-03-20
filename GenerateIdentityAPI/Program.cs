using GenerateIdentityServices;
using Infra;
using SendEventBus;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//builder.Services.AddMediatR(Assembly.GetExecutingAssembly());
//builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());


builder.Services.AddEndpointsApiExplorer()
    .AddSwagger("Generate Send API")
    .AddCustomMvc(Assembly.GetExecutingAssembly())
    .AddMassTransitGenerateService()
    .AddJWT()
    .AddEventPublishDI()
    .AddDependencieInjection()
    .AddConnactionDb();


var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();
app.UseSwagger();
app.UseSwaggerUI();
app.UseCustomMvc();

app.Run();
