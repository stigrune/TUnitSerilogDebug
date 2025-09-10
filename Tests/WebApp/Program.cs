using Serilog;

Log.Logger = new LoggerConfiguration().WriteTo.Console().CreateLogger();
Log.Information("Test that the static logger works, before the AddSerilog"); //This ends up in the standard test output as expected.

try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.Services.AddEndpointsApiExplorer();

    // Wire Serilog together with Microsoft ILogger. By commenting this out, the test output starts working as expected.
    builder.Services.AddSerilog((loggerConfiguration) => loggerConfiguration.WriteTo.Console());

    var app = builder.Build();

    app.UseHttpsRedirection();

    app.MapGet(
        "/ping",
        (Microsoft.Extensions.Logging.ILogger<Program> logger) =>
        {
            logger.LogInformation("Hello world from Microsoft.ILogger");
            return "Hello, World!";
        }
    );

    Console.WriteLine("Just before App Run");
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}

public partial class Program;
