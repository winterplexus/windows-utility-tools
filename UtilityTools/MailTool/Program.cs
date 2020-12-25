//
//  Program.cs
//
//  Copyright (c) Wiregrass Code Technology 2015-2021
//             
using System;
using System.IO;
using System.Net.Mime;
using System.Net.Mail;
using System.Reflection;
using System.Net;

[assembly: CLSCompliant(true)]

namespace MailTool
{
    public static class Program
    {
        public static void Main(string[] arguments)
        {
            DisplayVersion();

            var parameters = new Parameters();
            if (Options.Parse(arguments, parameters))
            {
                SendMail(parameters);
            }
        }

        public static void DisplayVersion()
        {
            var assembly = Assembly.GetEntryAssembly();
            if (assembly == null)
            {
                Console.WriteLine("error-> unable to get version information from executable");
                return;
            }

            var assemblyName = assembly.GetName();

            var descriptionAttributes = assembly.GetCustomAttributes(typeof(AssemblyDescriptionAttribute), false);
            if ((descriptionAttributes.Length > 0))
            {
                Console.WriteLine("{0} v{1}", ((AssemblyDescriptionAttribute)descriptionAttributes[0]).Description, assemblyName.Version);
            }
#if _DISPLAY_COPYRIGHT
            var copyrightAttributes = assembly.GetCustomAttributes(typeof(AssemblyCopyrightAttribute), false);
            if ((copyrightAttributes.Length > 0))
            {
                Console.WriteLine("{0}", ((AssemblyCopyrightAttribute)copyrightAttributes[0]).Copyright);
            }
#endif
            Console.Write(Environment.NewLine);
        }

        private static void SendMail(Parameters parameters)
        {
            try
            {
                SendMailSmtp(parameters);

                Console.WriteLine("information-> mail sent");
            }
            catch (SmtpFailedRecipientException sfre)
            {
                Console.WriteLine("SMTP failed recipient exception-> {0}" + Environment.NewLine + "{1}", sfre.Message, sfre.StackTrace);
            }
            catch (SmtpException se)
            {
                Console.WriteLine("SMTP exception-> {0}" + Environment.NewLine + "{1}", se.Message, se.StackTrace);

                if (se.InnerException != null)
                {
                    Console.WriteLine("SMTP exception->inner exception-> {0}", se.InnerException);
                }
            }
            catch (IOException ioe)
            {
                Console.WriteLine("I/O exception-> {0}" + Environment.NewLine + "{1}", ioe.Message, ioe.StackTrace);

                if (ioe.InnerException != null)
                {
                    Console.WriteLine("I/O exception->inner exception-> {0}", ioe.InnerException);
                }
            }
        }

        private static void SendMailSmtp(Parameters parameters)
        {
            using (var message = new MailMessage())
            {
                SetMessage(message, parameters);
                SetMessageAttachments(message, parameters);

                using (var client = new SmtpClient(parameters.SmtpHostName))
                {
                    SetClient(client, parameters);
                    client.Send(message);
                }
            }
        }

        private static void SetMessage(MailMessage message, Parameters parameters)
        {
            message.From = new MailAddress(parameters.MailFrom);
            message.To.Add(parameters.MailTo);
            message.Subject = parameters.MailSubject;
            message.Body = parameters.MailBody;
            message.Priority = parameters.MailPriorityLevel;
        }

        private static void SetMessageAttachments(MailMessage message, Parameters parameters)
        {
            if (parameters.MailAttachments == null)
            {
                return;
            }

            foreach (var mailAttachment in parameters.MailAttachments)
            {
                {
                    if (File.Exists(mailAttachment))
                    {
                        Console.WriteLine("error-> mail attachment file {0} does not exist");
                        continue;
                    }

                    using (var fileStream = new FileStream(mailAttachment, FileMode.Open, FileAccess.Read))
                    {
                        message.Attachments.Add(new Attachment(fileStream, MediaTypeNames.Application.Octet));
                    }
                }
            }
        }

        private static void SetClient(SmtpClient client, Parameters parameters)
        {
            if (parameters.SmtpPortNumber > 0)
            {
                client.Port = parameters.SmtpPortNumber;
            }
            if (parameters.SmtpUseSsl)
            {
                client.EnableSsl = true;
            }
            if (!(string.IsNullOrEmpty(parameters.SmtpUserName) || string.IsNullOrEmpty(parameters.SmtpPassword)))
            {
                client.UseDefaultCredentials = false;
                client.Credentials = new NetworkCredential(parameters.SmtpUserName, parameters.SmtpPassword);
            }
        }
    }
}