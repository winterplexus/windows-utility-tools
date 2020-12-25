//
//  Options.cs
//
//  Copyright (c) Wiregrass Code Technology 2015-2021
// 
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Net.Mail;

namespace MailTool
{
    public static class Options
    {
        private const char toFlag = 't';
        private const char fromFlag = 'f';
        private const char subjectFlag = 's';
        private const char bodyFlag = 'b';
        private const char attachmentFlag = 'a';
        private const char priorityLevelFlag = 'l';
        private const char smtpHostNameFlag = 'h';
        private const char smtpUserNameFlag = 'u';
        private const char smtpPasswordFlag = 'p';
        private const char smtpPortNumberFlag = 'n';
        private const char smtpUseSsl = 'e';

        private const string trueValue = "true";
        private const string falseValue = "false";

        private const string lowValue = "low";
        private const string normalValue = "normal";
        private const string highValue = "high";

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

                bool invalidValue;

                switch (option)
                {
                    case toFlag:
                         invalidValue = ParseMailToValue(arguments, index, parameters);
                         break;
                    case fromFlag:
                         invalidValue = ParseMailFromValue(arguments, index, parameters);
                         break;
                    case subjectFlag:
                         invalidValue = ParseMailSubjectValue(arguments, index, parameters);
                         break;
                    case bodyFlag:
                         invalidValue = ParseMailBodyValue(arguments, index, parameters);
                         break;
                    case attachmentFlag:
                         invalidValue = ParseMailAttachmentValue(arguments, index, parameters);
                         break;
                    case priorityLevelFlag:
                         invalidValue = ParseMailPriorityLevelValue(arguments, index, parameters);
                         break;
                    case smtpHostNameFlag:
                         invalidValue = ParseSmtpHostNameValue(arguments, index, parameters);
                         break;
                    case smtpPortNumberFlag:
                         invalidValue = ParseSmtpPortNumberValue(arguments, index, parameters);
                         break;
                    case smtpUseSsl:
                         invalidValue = ParseSmtpUseSslValue(arguments, index, parameters);
                         break;
                    case smtpUserNameFlag:
                         invalidValue = ParseSmtpUserNameValue(arguments, index, parameters);
                         break;
                    case smtpPasswordFlag:
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

        private static bool ParseMailToValue(IReadOnlyList<string> arguments, int i, Parameters parameters)
        {
            if (arguments.Count <= i || string.IsNullOrEmpty(arguments[i]))
            {
                Console.WriteLine("error-> mail to value is missing");
                return false;
            }
            parameters.MailTo = arguments[i];
            return true;
        }

        private static bool ParseMailFromValue(IReadOnlyList<string> arguments, int i, Parameters parameters)
        {
            if (arguments.Count <= i || string.IsNullOrEmpty(arguments[i]))
            {
                Console.WriteLine("error-> mail from value is missing");
                return false;
            }
            parameters.MailFrom = arguments[i];
            return true;
        }

        private static bool ParseMailSubjectValue(IReadOnlyList<string> arguments, int i, Parameters parameters)
        {
            if (arguments.Count <= i || string.IsNullOrEmpty(arguments[i]))
            {
                Console.WriteLine("error-> mail subject value is missing");
                return false;
            }
            parameters.MailSubject = arguments[i];
            return true;
        }

        private static bool ParseMailBodyValue(IReadOnlyList<string> arguments, int i, Parameters parameters)
        {
            if (arguments.Count <= i || string.IsNullOrEmpty(arguments[i]))
            {
                Console.WriteLine("error-> mail body value is missing");
                return false;
            }
            parameters.MailBody = arguments[i];
            return true;
        }

        private static bool ParseMailAttachmentValue(IReadOnlyList<string> arguments, int i, Parameters parameters)
        {
            if (arguments.Count <= i || string.IsNullOrEmpty(arguments[i]))
            {
                Console.WriteLine("error-> mail attachment value is missing");
                return false;
            }
            parameters.AddMailAttachment(arguments[i]);
            return true;
        }

        private static bool ParseMailPriorityLevelValue(IReadOnlyList<string> arguments, int i, Parameters parameters)
        {
            if (arguments.Count <= i || string.IsNullOrEmpty(arguments[i]))
            {
                Console.WriteLine("error-> mail priority value is missing");
                return false;
            }
            if (arguments[i].Equals(lowValue, StringComparison.CurrentCultureIgnoreCase))
            {
                parameters.MailPriorityLevel = MailPriority.Low;
                return true;
            }
            if (arguments[i].Equals(normalValue, StringComparison.CurrentCultureIgnoreCase))
            {
                parameters.MailPriorityLevel = MailPriority.Normal;
                return true;
            }
            if (arguments[i].Equals(highValue, StringComparison.CurrentCultureIgnoreCase))
            {
                parameters.MailPriorityLevel = MailPriority.High;
                return true;
            }
            Console.WriteLine("error-> invalid mail priority value");
            return false;
        }

        private static bool ParseSmtpHostNameValue(IReadOnlyList<string> arguments, int i, Parameters parameters)
        {
            if (arguments.Count <= i || string.IsNullOrEmpty(arguments[i]))
            {
                Console.WriteLine("error-> SMTP host name value is missing");
                return false;
            }
            parameters.SmtpHostName = arguments[i];
            return true;
        }

        private static bool ParseSmtpPortNumberValue(IReadOnlyList<string> arguments, int i, Parameters parameters)
        {
            if (arguments.Count <= i || string.IsNullOrEmpty(arguments[i]))
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

        private static bool ParseSmtpUseSslValue(IReadOnlyList<string> arguments, int i, Parameters parameters)
        {
            if (arguments.Count <= i || string.IsNullOrEmpty(arguments[i]))
            {
                Console.WriteLine("error-> SMTP use SSL Boolean value is missing");
                return false;
            }
            if (arguments[i].Equals(trueValue, StringComparison.CurrentCultureIgnoreCase))
            {
                parameters.SmtpUseSsl = true;
                return true;
            }
            if (arguments[i].Equals(falseValue, StringComparison.CurrentCultureIgnoreCase))
            {
                parameters.SmtpUseSsl = false;
                return true;
            }
            Console.WriteLine("error-> invalid SMTP use SSL Boolean value");
            return false;
        }

        private static bool ParseSmtpUserNameValue(IReadOnlyList<string> arguments, int i, Parameters parameters)
        {
            if (arguments.Count <= i || string.IsNullOrEmpty(arguments[i]))
            {
                Console.WriteLine("error-> SMTP user name value is missing");
                return false;
            }
            parameters.SmtpUserName = arguments[i];
            return true;
        }

        private static bool ParseSmtpPasswordValue(IReadOnlyList<string> arguments, int i, Parameters parameters)
        {
            if (arguments.Count <= i || string.IsNullOrEmpty(arguments[i]))
            {
                Console.WriteLine("error-> SMTP password value is missing");
                return false;
            }
            parameters.SmtpPassword = arguments[i];
            return true;
        }

        private static void DisplayUsage()
        {
            Console.WriteLine("usage: {0}.exe (options)" + Environment.NewLine, Process.GetCurrentProcess().ProcessName);
            Console.WriteLine("options: -{0} <mail to address>", toFlag);
            Console.WriteLine("\t -{0} <mail from address>", fromFlag);
            Console.WriteLine("\t -{0} <mail subject>", subjectFlag);
            Console.WriteLine("\t -{0} <mail body>", bodyFlag);
            Console.WriteLine("\t -{0} <mail attachment file name>", attachmentFlag);
            Console.WriteLine("\t -{0} [{1}|{2}|{3}] (default priority level: {4})", priorityLevelFlag, lowValue, normalValue, highValue, normalValue);
            Console.WriteLine("\t -{0} <SMTP host name>", smtpHostNameFlag);
            Console.WriteLine("\t -{0} <SMTP port number>", smtpPortNumberFlag);
            Console.WriteLine("\t -{0} <SMTP user name>", smtpUserNameFlag);
            Console.WriteLine("\t -{0} <SMTP password>", smtpPasswordFlag);
            Console.WriteLine("\t -{0} [{1}|{2}] (default use SSL: {3})" + Environment.NewLine, smtpUseSsl, trueValue, falseValue, falseValue);
        }
    }
}