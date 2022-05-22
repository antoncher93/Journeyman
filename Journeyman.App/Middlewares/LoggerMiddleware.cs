using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Journeyman.App.Middlewares
{
    public class LoggerMiddleware
    {
        private readonly RequestDelegate _next;

        public LoggerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            await _next.Invoke(context);

            var request = context.Request;
            var body = request.Body;
            var value = string.Empty;
            using (var reader = new StreamReader(body))
            {
                try
                {
                    value = reader.ReadToEnd();
                }
                catch (Exception e)
                {

                }
            }

            if(context.Response.StatusCode == 400)
            {

            }


        }
    }
}
