using Microsoft.AspNetCore.Builder;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

//allow cors
app.UseCors(config =>
{
    config.AllowAnyOrigin();
    config.AllowAnyMethod();
    config.AllowAnyHeader();
});

//SSE Configuration
app.Use(async (context, next) =>
{
    if (context.Request.Path.ToString().Equals("/Chat/ServerTime"))
    {
        var response = context.Response;
        response.Headers.Add("Content-Type", "text/event-stream");

        for (var i = 0; true; ++i)
        {
            // WriteAsync requires `using Microsoft.AspNetCore.Http`
            await response
                .WriteAsync($"data: Middleware {i} at {DateTime.Now}\r\r");

            await response.Body.FlushAsync();
            await Task.Delay(5 * 1000);
        }
    }

    await next.Invoke();
});

app.Run();
