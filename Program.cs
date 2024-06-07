using TestAssignmentGrcp.Data;
using TestAssignmentGrcp.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContextFactory<TestAssignmentContext>();
// Add services to the container.
builder.Services.AddGrpc();
builder.Services.AddGrpcReflection();


var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapGrpcService<GreeterService>();
app.MapGrpcService<FiboService>();
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

IWebHostEnvironment env = app.Environment;

if (env.IsDevelopment())
{
    app.MapGrpcReflectionService();
}

app.Run();
