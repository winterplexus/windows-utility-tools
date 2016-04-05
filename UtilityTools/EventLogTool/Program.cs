//
//  Program.cs
//
//  Copyright (c) Wiregrass Code Technology 2015-16
//
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Reflection;

[assembly: CLSCompliant(true)]

namespace EventLogTool
{
    //
    //  Program class.
    //
    public static class Program
    {
        //
        //  Main driver.
        //
        public static void Main(string[] arguments)
        {
            DisplayVersion();

            var parameters = new Parameters();
            if (Options.Parse(arguments, parameters))
            {
                switch (parameters.CommandAction)
                {
                    case Parameters.CommandActionType.Create:
                         CreateEventLog(parameters);
                         break;
                    case Parameters.CommandActionType.Delete:
                         DeleteEventLog(parameters);
                         break;
                }
            }
        }

        //
        //  Display version.
        //
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
            Console.Write("\r\n");
        }

        //
        //  Create event log.
        //
        private static void CreateEventLog(Parameters parameters)
        {
            try
            {
                if (!System.Diagnostics.EventLog.SourceExists(parameters.EventName))
                {
                    System.Diagnostics.EventLog.CreateEventSource(parameters.EventName, parameters.EventLogName);
                }

                System.Diagnostics.EventLog.WriteEntry(parameters.EventName, "information: log created", EventLogEntryType.Information);

                Console.WriteLine("status-> {0} event log created using event log name {1}", parameters.EventName, parameters.EventLogName);
            }
            catch (ArgumentException ae)
            {
                Console.WriteLine("argument exception-> {0}", ae.Message);
            }
        }

        //
        //  Delete event log.
        //
        private static void DeleteEventLog(Parameters parameters)
        {
            try
            {
                System.Diagnostics.EventLog.Delete(parameters.EventLogName);

                Console.WriteLine("status-> {0} event log deleted", parameters.EventName);
            }
            catch (ArgumentException ae)
            {
                Console.WriteLine("argument exception-> {0}\r\n{1}", ae.Message, ae.StackTrace);
            }
            catch (InvalidOperationException ioe)
            {
                Console.WriteLine("invalid operation exception-> {0}\r\n{1}", ioe.Message, ioe.StackTrace);
            }
            catch (Win32Exception we)
            {
                Console.WriteLine("WIN32 exception-> {0}\r\n{1}", we.Message, we.StackTrace);
            }
        }
    }
}
