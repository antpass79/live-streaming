using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace StreamingEndpoint.Utils
{
    class PushStreamResult : IActionResult
    {
        Func<Stream, CancellationToken, Task> _pushAction;
        string _contentType;
        string _encoding = "ANSI";

        public PushStreamResult(Func<Stream, CancellationToken, Task> pushAction, string contentType, string encoding = null)
        {
            _pushAction = pushAction;
            _contentType = contentType;
            _encoding = encoding;
        }

        public Task ExecuteResultAsync(ActionContext context)
        {
            var response = context.HttpContext.Response;
            response.ContentType = string.IsNullOrWhiteSpace(_encoding) ? $"{_contentType}" : $"{_contentType}; charset={_encoding}";
            response.StatusCode = 200;

            return _pushAction(response.Body, context.HttpContext.RequestAborted);
        }
    }
}
