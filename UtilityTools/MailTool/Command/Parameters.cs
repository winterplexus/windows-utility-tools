//
//  Parameters.cs
//
//  Copyright (c) Wiregrass Code Technology 2015-16
// 
using System.Collections.Generic;
using System.Net.Mail;

namespace MailTool
{
    //
    //  Parameters class.
    //
    public class Parameters
    {
        //
        //  Mail to.
        //
        public string MailFrom { get; set; }

        //
        //  Mail from.
        //
        public string MailTo { get; set; }

        //
        //  Mail subject.
        //
        public string MailSubject { get; set; }

        //
        //  Mail body.
        //
        public string MailBody { get; set; }

        //
        //  Mail attachments.
        //
        public List<string> MailAttachments { get; set; }

        //
        //  Mail priority level.
        //
        public MailPriority MailPriorityLevel { get; set; }

        //
        //  SMTP host name.
        //
        public string SmtpHostName { get; set; }

        //
        //  SMTP port number.
        //
        public int SmtpPortNumber { get; set; }

        //
        //  SMTP use SSL Boolean.
        //
        public bool SmtpUseSsl { get; set; }

        //
        //  SMTP timeout.
        //
        public int SmtpTimeout { get; set; }

        //
        //  SMTP user name.
        //
        public string SmtpUserName { get; set; }

        //
        //  SMTP password.
        //
        public string SmtpPassword { get; set; }
    }
}
