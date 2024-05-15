using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RailswareTestTask
{
    public class MailData
    {
        public string SenderNameAndEmail { get; set; }
        public string RecipientNameAndEmail { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public string HTML { get; set; }
        public IFormFileCollection Attachments { get; set; }
    }
}
