using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.Functions.Worker.Middleware;
using System.Threading.Tasks;

namespace TenantManagement.Middleware
{
    public class TenantIdMiddleware : IFunctionsWorkerMiddleware
    {
        public async Task Invoke(FunctionContext context, FunctionExecutionDelegate next)
        {
            var httpContext = await context.GetHttpRequestDataAsync()
                ?? throw new InvalidOperationException(nameof(context));

            var requestBody = await new StreamReader(httpContext.Body).ReadToEndAsync();

            // Parse request body to extract TenantId
            //var requestData = Newtonsoft.Json.JsonConvert.DeserializeObject<YourRequestBodyModel>(requestBody);
            //var tenantId = requestData.TenantId;

            // Add TenantId to context or other storage mechanism
            //context.Items["TenantId"] = tenantId;

            await next(context);
        }
    }
}