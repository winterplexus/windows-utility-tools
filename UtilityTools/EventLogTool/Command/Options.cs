//
//  Options.cs
//
//  Copyright (c) Wiregrass Code Technology 2015-2021
//
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace EventLogTool
{
    public static class Options
    {
        private const char commandActionFlag = 'c';
        private const char eventNameFlag = 'n';
        private const char eventLogNameFlag = 'l';

        private const string createAction = "create";
        private const string deleteAction = "delete";

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
                    case commandActionFlag:
                         invalidValue = ParseCommandActionValue(arguments, index, parameters);
                         break;
                    case eventNameFlag:
                         invalidValue = ParseEventNameValue(arguments, index, parameters);
                         break;
                    case eventLogNameFlag:
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
            if (string.IsNullOrEmpty(parameters.EventName))
            {
                Console.WriteLine("error-> event name is missing");
                return false;
            }
            if (string.IsNullOrEmpty(parameters.EventLogName))
            {
                Console.WriteLine("error-> event log name is missing");
                return false;
            }

            return true;
        }

        private static bool ParseCommandActionValue(string[] arguments, int i, Parameters parameters)
        {
            if (arguments.Length <= i || string.IsNullOrEmpty(arguments[i]))
            {
                Console.WriteLine("error-> command action value is missing");
                return false;
            }
            if (arguments[i].Equals(createAction, StringComparison.CurrentCultureIgnoreCase))
            {
                parameters.CommandAction = Parameters.CommandActionType.Create;
                return true;
            }
            if (arguments[i].Equals(deleteAction, StringComparison.CurrentCultureIgnoreCase))
            {
                parameters.CommandAction = Parameters.CommandActionType.Delete;
                return true;
            }
            Console.WriteLine("error-> invalid command action value");
            return false;
        }

        private static bool ParseEventNameValue(IReadOnlyList<string> arguments, int i, Parameters parameters)
        {
            if (arguments.Count <= i || string.IsNullOrEmpty(arguments[i]))
            {
                Console.WriteLine("error-> event name value is missing");
                return false;
            }
            parameters.EventName = arguments[i];
            return true;
        }

        private static bool ParseEventLogNameValue(IReadOnlyList<string> arguments, int i, Parameters parameters)
        {
            if (arguments.Count <= i || string.IsNullOrEmpty(arguments[i]))
            {
                Console.WriteLine("error-> event log name value is missing");
                return false;
            }
            parameters.EventLogName = arguments[i];
            return true;
        }

        private static void DisplayUsage()
        {
            Console.Write("usage: {0}.exe (options)" + Environment.NewLine, Process.GetCurrentProcess().ProcessName);
            Console.Write("options: -{0} [{1}|{2}] ", commandActionFlag, createAction, deleteAction);
            Console.Write("\t -{0} <event name> ", eventNameFlag);
            Console.Write("\t -{0} <event log name>" + Environment.NewLine, eventLogNameFlag);
        }
    }
}   