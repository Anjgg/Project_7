using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace P7CreateRestApi.SwaggerConfig
{
    [AttributeUsage(AttributeTargets.Method)]
    public class SwaggerDocumentationAttribute(string summary, string description, int crudType) : Attribute
    {
        public string Summary { get; } = summary;
        public string Description { get; } = description;
        public int CrudType { get; } = crudType;
    }

    public class SwaggerDocumentationOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (context.MethodInfo.GetCustomAttributes(typeof(SwaggerDocumentationAttribute), false)?.FirstOrDefault() is
                not SwaggerDocumentationAttribute descriptionAttribute)
                return;

            operation.Summary = descriptionAttribute.Summary;
            operation.Description = descriptionAttribute.Description;
            var crudType = descriptionAttribute.CrudType;

            // Remove the default 200 response, which was added by default by Swashbuckle
            operation.Responses.Remove("200");
            switch (crudType)
            {
                case (int)CrudType.GetAll:
                    operation.Responses.Add("200", new OpenApiResponse { Description = "A list of items was successfully retrieved." });
                    operation.Responses.Add("500", new OpenApiResponse { Description = "Internal Server Error along with the complete Error message." });
                    break;
                case (int)CrudType.GetById:
                    operation.Responses.Add("200", new OpenApiResponse { Description = "The item was successfully retrieved." });
                    operation.Responses.Add("404", new OpenApiResponse { Description = "The item was not found." });
                    operation.Responses.Add("500", new OpenApiResponse { Description = "Internal Server Error along with the complete Error message." });
                    break;
                case (int)CrudType.Create:
                    operation.Responses.Add("201", new OpenApiResponse { Description = "The item was successfully created." });
                    operation.Responses.Add("400", new OpenApiResponse { Description = "Bad Request. The request was invalid or cannot be served." });
                    operation.Responses.Add("500", new OpenApiResponse { Description = "Internal Server Error along with the complete Error message." });
                    break;
                case (int)CrudType.Update:
                    operation.Responses.Add("204", new OpenApiResponse { Description = "The item was successfully updated." });
                    operation.Responses.Add("400", new OpenApiResponse { Description = "Bad Request. The request was invalid or cannot be served." });
                    operation.Responses.Add("404", new OpenApiResponse { Description = "The item was not found." });
                    operation.Responses.Add("500", new OpenApiResponse { Description = "Internal Server Error along with the complete Error message." });
                    break;
                case (int)CrudType.Delete:
                    operation.Responses.Add("204", new OpenApiResponse { Description = "The item was successfully deleted." });
                    operation.Responses.Add("404", new OpenApiResponse { Description = "The item was not found." });
                    operation.Responses.Add("500", new OpenApiResponse { Description = "Internal Server Error along with the complete Error message." });
                    break;
            }
        }
    }

    public enum CrudType
    {
        GetAll = 0,
        GetById = 1,
        Create = 2,
        Update = 3,
        Delete = 4
    }
}


