using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace P7CreateRestApi.SwaggerConfig
{
    [AttributeUsage(AttributeTargets.Method)]
    public class SwaggerDocumentationAttribute(string tag, int crudType) : Attribute
    {
        public string Tag { get; } = tag;
        public int CrudType { get; } = crudType;
    }

    public class SwaggerDocumentationOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (context.MethodInfo.GetCustomAttributes(typeof(SwaggerDocumentationAttribute), false)?.FirstOrDefault() is
                not SwaggerDocumentationAttribute descriptionAttribute)
                return;

            var crudType = descriptionAttribute.CrudType;

            // Remove the default 200 response, which was added by default by Swashbuckle
            operation.Responses.Remove("200");
            operation.Responses.Add("401", new OpenApiResponse { Description = "Unauthorized. Authentication is required and has failed or has not yet been provided." });
            operation.Responses.Add("403", new OpenApiResponse { Description = "Forbidden. The request was valid, but the server is refusing action. The user might not have the necessary permissions for a resource." });
            operation.Responses.Add("500", new OpenApiResponse { Description = "Internal Server Error along with the complete Error message." });
            switch (crudType)
            {
                case (int)CrudType.GetAll:
                    operation.Summary = $"List all {descriptionAttribute.Tag}s";
                    operation.Description = $"Create a list of all {descriptionAttribute.Tag}s present in the database";
                    operation.Responses.Add("200", new OpenApiResponse { Description = "A list of items was successfully retrieved." });
                    break;
                case (int)CrudType.GetById:
                    operation.Summary = $"Get one {descriptionAttribute.Tag}";
                    operation.Description = $"Retrieve a specific {descriptionAttribute.Tag} in the database";
                    operation.Responses.Add("200", new OpenApiResponse { Description = "The item was successfully retrieved." });
                    operation.Responses.Add("404", new OpenApiResponse { Description = "The item was not found." });
                    break;
                case (int)CrudType.Create:
                    operation.Summary = $"Add one {descriptionAttribute.Tag}";
                    operation.Description = $"Create a new {descriptionAttribute.Tag} in the database";
                    operation.Responses.Add("201", new OpenApiResponse { Description = "The item was successfully created." });
                    operation.Responses.Add("400", new OpenApiResponse { Description = "Bad Request. The request was invalid or cannot be served." });
                    break;
                case (int)CrudType.Update:
                    operation.Summary = $"Update one {descriptionAttribute.Tag}";
                    operation.Description = $"Update an existing {descriptionAttribute.Tag} stored in database";
                    operation.Responses.Add("204", new OpenApiResponse { Description = "The item was successfully updated." });
                    operation.Responses.Add("400", new OpenApiResponse { Description = "Bad Request. The request was invalid or cannot be served." });
                    operation.Responses.Add("404", new OpenApiResponse { Description = "The item was not found." });
                    break;
                case (int)CrudType.Delete:
                    operation.Summary = $"Delete one {descriptionAttribute.Tag}";
                    operation.Description = $"Delete a specific {descriptionAttribute.Tag} in the database";
                    operation.Responses.Add("204", new OpenApiResponse { Description = "The item was successfully deleted." });
                    operation.Responses.Add("404", new OpenApiResponse { Description = "The item was not found." });
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


