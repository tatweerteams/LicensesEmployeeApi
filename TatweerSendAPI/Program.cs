using Infra;
using SendEventBus;
using System.Reflection;
using TatweerSendServices;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//builder.Services.AddMediatR(Assembly.GetExecutingAssembly());
//builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());


builder.Services.AddEndpointsApiExplorer()
    .AddSwagger("Tatweer Send API")
    .AddCustomMvc(Assembly.GetExecutingAssembly())
    .AddMassTransitSendAPI()
    .AddJWT()
    .AddConnactionDb()
    .AddEventPublishDI()
    .AddDependencieInjection();



var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();
app.UseSwagger();
app.UseSwaggerUI();
app.UseCustomMvc();

app.Run();
