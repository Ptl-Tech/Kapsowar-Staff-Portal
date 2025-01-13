// <summary>
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

/// Operation filter to add the requirement of the custom header
/// </summary>
public class ApiHeadersFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        if (operation.Parameters == null)
            operation.Parameters = new List<OpenApiParameter>();

        //operation.Parameters.Add(new OpenApiParameter()
        //{
        //    Name = "staffNo",
        //    In = ParameterLocation.Header,
        //    Required = false
        //});
        operation.Parameters.Add(new OpenApiParameter()
        {
            Name = "sessionToken",
            In = ParameterLocation.Header,
            Required = false
        });
        //operation.Parameters.Add(new OpenApiParameter()
        //{
        //    Name = "branchCode",
        //    In = ParameterLocation.Header,
        //    Required = false
        //});
    }
}