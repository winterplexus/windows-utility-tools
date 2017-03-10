//
//  Parameters.cs
//
//  Copyright (c) Wiregrass Code Technology 2015-17
// 
using System.Collections.ObjectModel;
using System.Net.Mail;

namespace MailTool
{
    public class Parameters
    {
        public string MailFrom { get; set; }

        public string MailTo { get; set; }

        public string MailSubject { get; set; }

        public string MailBody { get; set; }

        public Collection<string> MailAttachments { get; } = new Collection<string>();

        public void AddMailAttachment(string attachment) { MailAttachments.Add(attachment); }

        public MailPriority MailPriorityLevel { get; set; }

        public string SmtpHostName { get; set; }

        public int SmtpPortNumber { get; set; }

        public bool SmtpUseSsl { get; set; }

        public int SmtpTimeout { get; set; }

        public string SmtpUserName { get; set; }

        public string SmtpPassword { get; set; }
    }
}