using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using net6_template_devstandard_api.Services;

namespace net6_template_devstandard_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MailServersController : ControllerBase
    {
        private readonly IMailServerService mailServerService;

        public MailServersController(IMailServerService mailServerService)
        {
            this.mailServerService = mailServerService;
        }

        [HttpGet]
        [Route("sendmail")]
        public async Task<IActionResult> SendMail(string msg)
        {
            try
            {
                var res = await mailServerService.SendMail(msg);
                return Ok(new { isSuccess = true, data = res });
            }
            catch (Exception ex)
            {
                return new JsonResult(new
                {
                    isSuccess = false,
                    statusCode = 500,
                    message = ex.Message
                });
            }
        }
    }
}
