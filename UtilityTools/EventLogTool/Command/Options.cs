//
//  Options.cs
//
//  Copyright (c) Wiregrass Code Technology 2015-16
//
using System;
using System.Diagnostics;

namespace EventLogTool
{
    //
    //  Options class.
    //
    public static class Options
    {
        //
        //  Options flag fields.
        //
        private const char CommandActionFlag = 'c';
        private const char EventNameFlag = 'n';
        private const char EventLogNameFlag = 'l';

        //
        //  Command action flag value fields.               
        //
        private const string CreateAction = "create";
        private const string DeleteAction = "delete";

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

            parameters.CommandAction = Parameters.CommandActionType.None;

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
                    case CommandActionFlag:
                         invalidValue = ParseCommandActionValue(arguments, index, parameters);
                         break;
                    case EventNameFlag:
                         invalidValue = ParseEventNameValue(arguments, index, parameters);
                         break;
                    case EventLogNameFlag:
                         invalidValue = ParseEventLogNameValue(arguments, index, parameters);
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

            if (parameters.CommandAction == Parameters.CommandActionType.None)
            {
                Console.WriteLine("error-> command action is missing");
                return false;
            }
            if (String.IsNullOrEmpty(parameters.EventName))
            {
                Console.WriteLine("error-> event name is missing");
                return false;
            }
            if (String.IsNullOrEmpty(parameters.EventLogName))
            {
                Console.WriteLine("error-> event log name is missing");
                return false;
            }

            return true;
        }

        //
        //  Parse command action value.
        //
        private static bool ParseCommandActionValue(string[] arguments, int i, Parameters parameters)
        {
            if (arguments.Length <= i || String.IsNullOrEmpty(arguments[i]))
            {
                Console.WriteLine("error-> command action value is missing");
                return false;
            }
            if (arguments[i].Equals(CreateAction, StringComparison.CurrentCultureIgnoreCase))
            {
                parameters.CommandAction = Parameters.CommandActionType.Create;
                return true;
            }
            if (arguments[i].Equals(DeleteAction, StringComparison.CurrentCultureIgnoreCase))
            {
                parameters.CommandAction = Parameters.CommandActionType.Delete;
                return true;
            }
            Console.WriteLine("error-> invalid command action value");
            return false;
        }

        //
        //  Parse event name value.
        //
        private static bool ParseEventNameValue(string[] arguments, int i, Parameters parameters)
        {
            if (arguments.Length <= i || String.IsNullOrEmpty(arguments[i]))
            {
                Console.WriteLine("error-> event name value is missing");
                return false;
            }
            parameters.EventName = arguments[i];
            return true;
        }

        //
        //  Parse event log name value.
        //
        private static bool ParseEventLogNameValue(string[] arguments, int i, Parameters parameters)
        {
            if (arguments.Length <= i || String.IsNullOrEmpty(arguments[i]))
            {
                Console.WriteLine("error-> event log name value is missing");
                return false;
            }
            parameters.EventLogName = arguments[i];
            return true;
        }

        //
        //  Display usage.
        //
        private static void DisplayUsage()
        {
            Console.Write("usage: {0}.exe (options)\r\n", Process.GetCurrentProcess().ProcessName);
            Console.Write("options: -{0} [{1}|{2}] ", CommandActionFlag, CreateAction, DeleteAction);
            Console.Write("\t -{0} <event name> ", EventNameFlag);
            Console.Write("\t -{0} <event log name>\r\n", EventLogNameFlag);
        }
    }
}
