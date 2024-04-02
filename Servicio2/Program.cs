using Serilog;
using Serilog.Events;
using Serilog.Formatting.Json;


namespace Servicio1
{
    public class Program
    {

        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();


            //CorrelationId 
            builder.Services.AddHttpContextAccessor();
            builder.Host.UseSerilog((context, lc) => lc
                .WriteTo.File(new JsonFormatter(), ".\\Logger\\Logs2.txt")
                //.MinimumLevel.Information()
                .MinimumLevel.Warning()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
                .Enrich.FromLogContext()
                .Enrich.WithCorrelationIdHeader("Custom-Correlation-ID")
              );

            builder.Services.AddHeaderPropagation(options => options.Headers.Add("Custom-Correlation-ID"));

            builder.Services.AddHttpClient("WeatherForecast/servicio2", c =>
            {
                c.BaseAddress = new Uri("https://localhost:5170/");
            }).AddHeaderPropagation();



            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseAuthorization();

            app.MapControllers();

            app.Run();

        }

    }
}
