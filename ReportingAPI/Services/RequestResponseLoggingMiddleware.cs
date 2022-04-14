using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportingAPI.Services
{
        public class RequestResponseLoggingMiddleware
        {
            private readonly RequestDelegate next;
            private readonly ILogger logger;

            public RequestResponseLoggingMiddleware(RequestDelegate next, ILoggerFactory loggerFactory)
            {
                this.next = next;
                logger = loggerFactory.CreateLogger<RequestResponseLoggingMiddleware>();
            }

            public async Task Invoke(HttpContext context)
            {
                //context.Request.EnableRewind();

                var buffer = new byte[Convert.ToInt32(context.Request.ContentLength)];
            //await context.Request.Body.ReadAsync(buffer, 0, buffer.Length);
            //var requestBody = Encoding.UTF8.GetString(buffer);
            //context.Request.Body.Seek(0, SeekOrigin.Begin);

            //logger.LogInformation(requestBody);

            var originalBodyStream = context.Response.Body;

            using (var responseBody = new MemoryStream())
            {
                context.Response.Body = responseBody;

                await next(context);

                context.Response.Body.Seek(0, SeekOrigin.Begin);
                var response = await new StreamReader(context.Response.Body).ReadToEndAsync();
                context.Response.Body.Seek(0, SeekOrigin.Begin);

                logger.LogInformation(response);
                await responseBody.CopyToAsync(originalBodyStream);
            }
            }
        }
    }
