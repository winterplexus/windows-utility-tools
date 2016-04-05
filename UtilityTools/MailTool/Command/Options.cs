//
//  Options.cs
//
//  Copyright (c) Wiregrass Code Technology 2015-16
// 
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Mail;

namespace MailTool
{
    //
    //  Options class.
    //
    public static class Options
    {
        //
        //  Options flag fields.
        //
        private const char ToFlag = 't';
        private const char FromFlag = 'f';
        private const char SubjectFlag = 's';
        private const char BodyFlag = 'b';
        private const char AttachmentFlag = 'a';
        private const char PriorityLevelFlag = 'l';
        private const char SmtpHostNameFlag = 'h';
        private const char SmtpUserNameFlag = 'u';
        private const char SmtpPasswordFlag = 'p';
        private const char SmtpPortNumberFlag = 'n';
        private const char SmtpUseSsl = 'e';

        //
        //  Boolean flag literal fields.             
        //
        private const string TrueValue = "true";
        private const string FalseValue = "false";

        //
        //  Priority level flag literal fields.
        //
        private const string LowValue = "low";
        private const string NormalValue = "normal";
        private const string HighValue = "high";

        //
        //  Parse (arguments into parameters).
        //
        public static bool Parse(string[] arguments, Parameters parameters)
        {
            if (arguments == null || arguments.Length < 1)
            {
                DisplayUsage();
                return false;
            }
            if (parameters == null)
            {
                throw new ArgumentException("parameters argument is null");
            }

            bool invalidValue = true;

            for (var index = 0; index < arguments.Length; index++)
            {
                if (arguments[index][0] != '-')
                {
                    Console.WriteLine("error-> option or option value is missing (argument index {0}): {1}", index, arguments[index]);
                    return false;
                }
                if (arguments[index].Length <= 1)
                {
                    continue;
                }

                var option = arguments[index][1];

                index++;
                switch (option)
                {
                    case ToFlag:
                         invalidValue = ParseMailToValue(arguments, index, parameters);
                         break;
                    case FromFlag:
                         invalidValue = ParseMailFromValue(arguments, index, parameters);
                         break;
                    case SubjectFlag:
                         invalidValue = ParseMailSubjectValue(arguments, index, parameters);
                         break;
                    case BodyFlag:
                         invalidValue = ParseMailBodyValue(arguments, index, parameters);
                         break;
                    case AttachmentFlag:
                         invalidValue = ParseMailAttachmentValue(arguments, index, parameters);
                         break;
                    case PriorityLevelFlag:
                         invalidValue = ParseMailPriorityLevelValue(arguments, index, parameters);
                         break;
                    case SmtpHostNameFlag:
                         invalidValue = ParseSmtpHostNameValue(arguments, index, parameters);
                         break;
                    case SmtpPortNumberFlag:
                         invalidValue = ParseSmtpPortNumberValue(arguments, index, parameters);
                         break;
                    case SmtpUseSsl:
                         invalidValue = ParseSmtpUseSslValue(arguments, index, parameters);
                         break;
                    case SmtpUserNameFlag:
                         invalidValue = ParseSmtpUserNameValue(arguments, index, parameters);
                         break;
                    case SmtpPasswordFlag:
                         invalidValue = ParseSmtpPasswordValue(arguments, index, parameters);
                         break;
                    default:
                         Console.WriteLine("error-> unknown option: {0}", option);
                         return false;
                }

                if (!invalidValue)
                {
                    return false;
                }
            }
            return true;
        }

        //
        //  Parse mail to value.
        //
        private static bool ParseMailToValue(string[] arguments, int i, Parameters parameters)
        {
            if (arguments.Length <= i || String.IsNullOrEmpty(arguments[i]))
            {
                Console.WriteLine("error-> mail to value is missing");
                return false;
            }
            parameters.MailTo = arguments[i];
            return true;
        }

        //
        //  Parse mail from value.
        //
        private static bool ParseMailFromValue(string[] arguments, int i, Parameters parameters)
        {
            if (arguments.Length <= i || String.IsNullOrEmpty(arguments[i]))
            {
                Console.WriteLine("error-> mail from value is missing");
                return false;
            }
            parameters.MailFrom = arguments[i];
            return true;
        }

        //
        //  Parse mail subject value.
        //
        private static bool ParseMailSubjectValue(string[] arguments, int i, Parameters parameters)
        {
            if (arguments.Length <= i || String.IsNullOrEmpty(arguments[i]))
            {
                Console.WriteLine("error-> mail subject value is missing");
                return false;
            }
            parameters.MailSubject = arguments[i];
            return true;
        }

        //
        //  Parse mail body value.
        //
        private static bool ParseMailBodyValue(string[] arguments, int i, Parameters parameters)
        {
            if (arguments.Length <= i || String.IsNullOrEmpty(arguments[i]))
            {
                Console.WriteLine("error-> mail body value is missing");
                return false;
            }
            parameters.MailBody = arguments[i];
            return true;
        }

        //
        //  Parse mail attachment value.
        //
        private static bool ParseMailAttachmentValue(string[] arguments, int i, Parameters parameters)
        {
            if (arguments.Length <= i || String.IsNullOrEmpty(arguments[i]))
            {
                Console.WriteLine("error-> mail attachment value is missing");
                return false;
            }
            if (parameters.MailAttachments == null)
            {
                parameters.MailAttachments = new List<string>();
            }
            parameters.MailAttachments.Add(arguments[i]);
            return true;
        }

        //
        //  Parse mail priority level value.
        //
        private static bool ParseMailPriorityLevelValue(string[] arguments, int i, Parameters parameters)
        {
            if (arguments.Length <= i || String.IsNullOrEmpty(arguments[i]))
            {
                Console.WriteLine("error-> mail priority value is missing");
                return false;
            }
            if (arguments[i].Equals(LowValue, StringComparison.CurrentCultureIgnoreCase))
            {
                parameters.MailPriorityLevel = MailPriority.Low;
                return true;
            }
            if (arguments[i].Equals(NormalValue, StringComparison.CurrentCultureIgnoreCase))
            {
                parameters.MailPriorityLevel = MailPriority.Normal;
                return true;
            }
            if (arguments[i].Equals(HighValue, StringComparison.CurrentCultureIgnoreCase))
            {
                parameters.MailPriorityLevel = MailPriority.High;
                return true;
            }
            Console.WriteLine("error-> invalid mail priority value");
            return false;
        }

        //
        //  Parse SMTP host name value.
        //
        private static bool ParseSmtpHostNameValue(string[] arguments, int i, Parameters parameters)
        {
            if (arguments.Length <= i || String.IsNullOrEmpty(arguments[i]))
            {
                Console.WriteLine("error-> SMTP host name value is missing");
                return false;
            }
            parameters.SmtpHostName = arguments[i];
            return true;
        }

        //
        //  Parse SMTP port number value.
        //
        private static bool ParseSmtpPortNumberValue(string[] arguments, int i, Parameters parameters)
        {
            if (arguments.Length <= i || String.IsNullOrEmpty(arguments[i]))
            {
                Console.WriteLine("error-> SMTP port number value is missing");
                return false;
            }

            int value;
            if (int.TryParse(arguments[i], out value) == false)
            {
                Console.WriteLine("error-> SMTP port number value is not a number");
                return false;
            }
            parameters.SmtpPortNumber = value;
            return false;
        }

        //
        //  Parse SMTP use SSL Boolean value.
        //
        private static bool ParseSmtpUseSslValue(string[] arguments, int i, Parameters parameters)
        {
            if (arguments.Length <= i || String.IsNullOrEmpty(arguments[i]))
            {
                Console.WriteLine("error-> SMTP use SSL Boolean value is missing");
                return false;
            }
            if (arguments[i].Equals(TrueValue, StringComparison.CurrentCultureIgnoreCase))
            {
                parameters.SmtpUseSsl = true;
                return true;
            }
            if (arguments[i].Equals(FalseValue, StringComparison.CurrentCultureIgnoreCase))
            {
                parameters.SmtpUseSsl = false;
                return true;
            }
            Console.WriteLine("error-> invalid SMTP use SSL Boolean value");
            return false;
        }

        //
        //  Parse SMTP user name value.
        //
        private static bool ParseSmtpUserNameValue(string[] arguments, int i, Parameters parameters)
        {
            if (arguments.Length <= i || String.IsNullOrEmpty(arguments[i]))
            {
                Console.WriteLine("error-> SMTP user name value is missing");
                return false;
            }
            parameters.SmtpUserName = arguments[i];
            return true;
        }

        //
        //  Parse SMTP password value.
        //
        private static bool ParseSmtpPasswordValue(string[] arguments, int i, Parameters parameters)
        {
            if (arguments.Length <= i || String.IsNullOrEmpty(arguments[i]))
            {
                Console.WriteLine("error-> SMTP password value is missing");
                return false;
            }
            parameters.SmtpPassword = arguments[i];
            return true;
        }

        //
        //  Display usage.
        //
        private static void DisplayUsage()
        {
            Console.WriteLine("usage: {0}.exe (options)\r\n", Process.GetCurrentProcess().ProcessName);
            Console.WriteLine("options: -{0} <mail to address>", ToFlag);
            Console.WriteLine("\t -{0} <mail from address>", FromFlag);
            Console.WriteLine("\t -{0} <mail subject>", SubjectFlag);
            Console.WriteLine("\t -{0} <mail body>", BodyFlag);
            Console.WriteLine("\t -{0} <mail attachment file name>", AttachmentFlag);
            Console.WriteLine("\t -{0} [{1}|{2}|{3}] (default priority level: {4})", PriorityLevelFlag, LowValue, NormalValue, HighValue, NormalValue);
            Console.WriteLine("\t -{0} <SMTP host name>", SmtpHostNameFlag);
            Console.WriteLine("\t -{0} <SMTP port number>", SmtpPortNumberFlag);
            Console.WriteLine("\t -{0} <SMTP user name>", SmtpUserNameFlag);
            Console.WriteLine("\t -{0} <SMTP password>", SmtpPasswordFlag);
            Console.WriteLine("\t -{0} [{1}|{2}] (default use SSL: {3})", SmtpUseSsl, TrueValue, FalseValue, FalseValue);
        }
    }
}
