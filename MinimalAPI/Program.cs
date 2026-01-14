using Microsoft.EntityFrameworkCore;
using MinimalAPI.Interface;
using MinimalAPI.Models;
using MinimalAPI.Repository;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<DatabaseContext>
    (options => options.UseSqlServer(builder.Configuration.GetConnectionString("BiappsDBCon")));

builder.Services.AddTransient<IStudentManager, StudentManagerRepository>();

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHealthChecks();

var app = builder.Build();

app.UseHealthChecks("/healthz");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

#region Machine API

app.MapGet("api/Get_AllStudent", (IStudentManager service, string? sSearchText
    , int PageNumber, int PageSize, string? SortBy, string? SortOrder) =>
{
    return service.Get_AllStudent (sSearchText
                       , PageNumber
                       , PageSize
                       , SortBy
                       , SortOrder);
})
.WithName("Get_AllStudent");

app.MapGet("/api/Get_SudentById", (IStudentManager service, int StudentId) =>
{
    return service.Get_SudentById(StudentId);
})
.WithName("Get_SudentById");

app.MapPost("/api/Save_Student", (Student obj, IStudentManager service) =>
{
    PostResult _result = new PostResult();
    _result.Id = service.Save_Student(obj);
    _result.Message = "Student has been saved successfully";

    return Results.Ok(_result);
});

app.MapDelete("/api/Remove_Student/{Id}", (int Id, IStudentManager service) =>
{
    service.Remove_Student(Id);
    return Results.Ok("Student has been deleted successfully");
});

app.MapGet("/terminate", () =>
{
    Task.Run(async () =>
    {
        await Task.Delay(2000);
        Environment.Exit(1);

    });
    return Results.Ok("Terminating");
})
    .WithName("Terminate")
    .WithOpenApi();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    var forecast = Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast")
.WithOpenApi();


#endregion


app.Run();

internal record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
