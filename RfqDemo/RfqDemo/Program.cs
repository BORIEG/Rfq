using Microsoft.OpenApi.Models;
using RfqDemo.MarketData;
using RfqDemo.Services;

namespace RfqDemo
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);
			builder.Logging.ClearProviders();
			builder.Logging.AddConsole();
			builder.Services.AddSwaggerGen(swagger =>
			{
				swagger.SwaggerDoc("rfq", new OpenApiInfo
				{
					Version = "v1",
					Title = "RFQ Swagger",
				});
			});

			builder.Services.AddSingleton<IPricingService, PricingService>();
			builder.Services.AddSingleton<RfqCache>();
			builder.Services.AddSingleton<RfqObserver>();

			builder.Services.AddMvcCore()
				.AddApiExplorer();

			var app = builder.Build();

			app.UseSwagger();
			app.UseSwaggerUI(swagger =>
			{
				swagger.SwaggerEndpoint("/swagger/rfq/swagger.json", "RFQ Swagger");
				swagger.RoutePrefix = string.Empty;
			});

			app.MapControllers();
			app.Run();
		}
	}
}
