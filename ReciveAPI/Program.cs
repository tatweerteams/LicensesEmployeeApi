using GenerateIdentityServices;
using IdentityServices;
using Infra;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//builder.Services.AddMediatR(Assembly.GetExecutingAssembly());
//builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());


builder.Services.AddEndpointsApiExplorer()
    .AddSwagger("Tatweer Recive API")
    .AddCustomMvc(Assembly.GetExecutingAssembly())
    .AddMassTransitReciveService()
    .AddJWT()
    .AddConnactionDb()
    .AddDependencieInjection();

//.AddTrackingServiceDI()
//.AddTrackingServiceDb()



var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();
app.UseSwagger();
app.UseSwaggerUI();
app.UseCustomMvc();
//app.UseHangFire();
app.Run();
