using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Interfaces;

public class SwaggerFileOperationFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        var fileParams = context.MethodInfo.GetParameters()
            .Where(p => p.ParameterType == typeof(IFormFile) || p.ParameterType == typeof(IFormFile[]))
            .ToArray();

        if (fileParams.Any())
        {
            operation.RequestBody = new OpenApiRequestBody
            {
                Content = new Dictionary<string, OpenApiMediaType>
                {
                    ["multipart/form-data"] = new OpenApiMediaType
                    {
                        Schema = new OpenApiSchema
                        {
                            Type = "object",
                            Properties = new Dictionary<string, OpenApiSchema>()
                        }
                    }
                }
            };

            var formSchema = operation.RequestBody.Content["multipart/form-data"].Schema;

            // Add file parameter(s)
            foreach (var fileParam in fileParams)
            {
                formSchema.Properties[fileParam.Name!] = new OpenApiSchema
                {
                    Type = "string",
                    Format = "binary"
                };
            }

            // Add other form parameters
            var formParams = context.MethodInfo.GetParameters()
                .Where(p => p.GetCustomAttribute<Microsoft.AspNetCore.Mvc.FromFormAttribute>() != null && 
                           p.ParameterType != typeof(IFormFile) && 
                           p.ParameterType != typeof(IFormFile[]))
                .ToArray();

            foreach (var formParam in formParams)
            {
                var paramType = formParam.ParameterType;
                var schema = new OpenApiSchema();

                if (paramType == typeof(string))
                {
                    schema.Type = "string";
                }
                else if (paramType == typeof(int) || paramType == typeof(int?))
                {
                    schema.Type = "integer";
                }
                else if (paramType.IsEnum)
                {
                    schema.Type = "string";
                    schema.Enum = Enum.GetNames(paramType).Select(name => new OpenApiString(name) as IOpenApiAny).ToList();
                }
                else
                {
                    schema.Type = "string";
                }

                formSchema.Properties[formParam.Name!] = schema;
            }
        }
    }
}
