using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using API.Errors;


namespace API.MiddleWare
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate next;
        public ILogger<ExceptionMiddleware> logger;
        public IHostEnvironment env;
        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger, IHostEnvironment env)
        {
            this.env = env;
            this.logger = logger;
            this.next = next;

        }

        public async Task InvokeAsync(HttpContext context){

            try{
                await this.next(context);
            }catch(Exception e){
                this.logger.LogError(e, e.Message);
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int) HttpStatusCode.InternalServerError;


                var response = this.env.IsDevelopment()
                    ? new ApiExceptions(context.Response.StatusCode, e.Message, e.StackTrace?.ToString())
                    : new ApiExceptions(context.Response.StatusCode, "Internal Server Error");

                var options = new JsonSerializerOptions{PropertyNamingPolicy = JsonNamingPolicy.CamelCase};

                var json = JsonSerializer.Serialize(response, options);

                await context.Response.WriteAsync(json); 
            }
        }


    }
}