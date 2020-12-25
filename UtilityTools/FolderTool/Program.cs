//
//  Program.cs
//
//  Copyright (c) Wiregrass Code Technology 2015-2021
// 
using System;
using System.Reflection;

[assembly: CLSCompliant(true)]

namespace FolderTool
{
    public class Program
    {
        public static void Main(string[] args)
        {
            DisplayVersion();

            var parameters = new Parameters();
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
    }
}