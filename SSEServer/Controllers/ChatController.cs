using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace SSESample.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ChatController : ControllerBase
    {
        [HttpGet("ServerTime")]
        public async Task ServerTime()
        {
            //get current action response
            Response.Headers.Add("ContentType", "text/event-stream");
            Response.Headers.Add("Cache-Control", "no-cache");
            Response.Headers.Add("Connection", "keep-alive");

            //an infinite loop
            while (true)
            {
                var bytes = Encoding.ASCII.GetBytes($"Current Server Time {DateTime.Now}");
                await Response.Body.WriteAsync(bytes, 0, bytes.Length);
                await HttpContext.Response.Body.FlushAsync();
                await Task.Delay(5 * 1000);
            }
        }
    }
}
