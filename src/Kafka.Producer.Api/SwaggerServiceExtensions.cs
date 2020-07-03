using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kafka.Producer.Api
{
    public static class SwaggerServiceExtensions
    {
        public static void AddSwaggerDocumentation(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Apache Kafka with Asp.Net Core",
                    Description = "Learning Apache Kafa",
                    TermsOfService = new Uri("https://example.com/terms"),
                    Contact = new OpenApiContact
                    {
                        Name = "Roberto Camara",
                        Email = "robertocamara@outlook.com.br",
                        Url = new Uri("https://github.com/robertocamara")
                    }
                });
            });
        }

        public static void UseSwaggerDocumentation(this IApplicationBuilder app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("v1/swagger.json", "Apache Kafka with Asp.Net Core API v1");
                c.DocumentTitle = "KafkaAspNetCoreApi Documentation";
                c.DocExpansion(DocExpansion.None);
            });
        }
    }
}
