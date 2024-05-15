﻿using Microsoft.AspNetCore.Mvc;
using RailswareTestTask.Interfaces;

namespace RailswareTestTask.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MailController : ControllerBase
    {
        private readonly IMailService _mailService;
        //injecting the IMailService into the constructor
        public MailController(IMailService _MailService)
        {
            _mailService = _MailService;
        }

        [HttpPost]
        [Route("SendMail")]
        public bool SendMail([FromForm] MailData mailData)
        {
            return _mailService.SendMail(mailData);
        }
    }
}