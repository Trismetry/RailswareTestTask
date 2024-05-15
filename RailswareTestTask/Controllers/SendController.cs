using Microsoft.AspNetCore.Mvc;
using RailswareTestTask.Interfaces;
using System.Threading.Tasks;

namespace RailswareTestTask.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MailController : ControllerBase
    {
        private readonly IMailService _mailService;
        public MailController(IMailService _MailService)
        {
            _mailService = _MailService;
        }

        [HttpPost]
        [Route("SendMail")]
        public async Task<bool> SendMail([FromForm] MailData mailData)
        {
            return await _mailService.SendMailAsync(mailData);
        }
    }
}
