using Infra;
using System.Reflection;
using TatweerLogginServices;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//builder.Services.AddMediatR(Assembly.GetExecutingAssembly());
//builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());


builder.Services.AddEndpointsApiExplorer()
    .AddSwagger("Loggin Web API")
    .AddCustomMvc(Assembly.GetExecutingAssembly())
    .AddMassTransitLogginAPI()
    .AddJWT()
    .AddConnactionDb()
    //.AddEventPublishDI()
    .AddDependencieInjection();



var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();
app.UseSwagger();
app.UseSwaggerUI();
app.UseCustomMvc();

app.Run();
