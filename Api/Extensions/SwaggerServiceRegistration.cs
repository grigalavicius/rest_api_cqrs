using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace RestApiTask.Extensions
{
    internal static class SwaggerServiceRegistration
    {
        internal static void AddSwaggerService(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "VismaRestApiTask", 
                    Version = "v1"
                });
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath, true);
                
                c.SchemaFilter<EnumSchemaFilter>();
            });
        }
    }

    internal class EnumSchemaFilter : ISchemaFilter
    {
        public void Apply(OpenApiSchema schema, SchemaFilterContext context)
        {
            if (context.Type.IsEnum)
            {
                schema.Enum.Clear();
                Enum.GetNames(context.Type)
                    .ToList()
                    .ForEach(name => schema.Enum.Add(new OpenApiString($"{Convert.ToInt64(Enum.Parse(context.Type, name))} - {name}")));
            }
        }
    }
}