using Ocelot.DependencyInjection;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateSlimBuilder(args);
builder.Services.AddOcelot(builder.Configuration);


var app = builder.Build();



app.Run();



