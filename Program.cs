using ApiVersioning.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.AddApi();

var app = builder.Build();

app.UseMinimalEndpoints();
app.UseNormalEndpoints();
app.UseHttpsRedirection();
app.ConfigureSwagger();

app.Run();
