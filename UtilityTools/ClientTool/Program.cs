//
//  Program.cs
//
//  Copyright (c) Wiregrass Code Technology 2015-16
//
using System;
using System.IO;
using System.Net;
using System.Reflection;
using System.Text;

[assembly: CLSCompliant(true)]

namespace ClientTool
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
                SendRequest(parameters);
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
        //  Send request.
        //
        private static void SendRequest(Parameters parameters)
        {
            try
            {
                SendRequestHttp(parameters);
            }
            catch (WebException we)
            {
                Console.WriteLine("web exception-> {0}\r\n{1}", we.Message, we.StackTrace);
            }
            catch (IOException ioe)
            {
                Console.WriteLine("I/O exception-> {0}\r\n{1}", ioe.Message, ioe.StackTrace);
            }
        }

        //
        //  Send request using HTTP.
        //
        private static void SendRequestHttp(Parameters parameters)
        {
            var requestContents = GetInputDataFileContents(parameters);
            if (requestContents == null)
            {
                return;
            }

            if (parameters.VerboseMode)
            {
                Console.WriteLine("\r\nrequest:");
                Console.WriteLine("--------------------------------------------------------------------------------");
                Console.WriteLine(requestContents);
            }

            var httpRequest = (HttpWebRequest)WebRequest.Create(new Uri(parameters.Url));
            httpRequest.Method = parameters.Method;
            httpRequest.ContentType = String.Format("{0}; encoding='{1}'", parameters.ContentType, parameters.EncodingType);

            if (!StoreRequestParameters(httpRequest, parameters))
            {
                return;
            }

            var encoding = new ASCIIEncoding();
            if (requestContents != null)
            {
                var requestBytes = encoding.GetBytes(requestContents);
                var requestStream = httpRequest.GetRequestStream();
                requestStream.Write(requestBytes, 0, requestBytes.Length);
                requestStream.Close();
            }

            var httpResponse = (HttpWebResponse)httpRequest.GetResponse();

            var responseStream = httpResponse.GetResponseStream();
            var responseBytes = new Byte[httpResponse.ContentLength];
            if (responseStream != null)
            {
                responseStream.Read(responseBytes, 0, (int)httpResponse.ContentLength);
            }

            if (!String.IsNullOrEmpty(parameters.OutputDataFilePath))
            {
                StoreResponseData(parameters, encoding.GetString(responseBytes));
            }

            if (parameters.VerboseMode)
            {
                Console.WriteLine("\r\nresponse:");
                Console.WriteLine("--------------------------------------------------------------------------------");
                Console.WriteLine(encoding.GetString(responseBytes));

                Console.WriteLine("\r\nresponse HTTP status:");
                Console.WriteLine("--------------------------------------------------------------------------------");
                Console.WriteLine(httpResponse.StatusCode.ToString());

                Console.WriteLine("\r\nresponse HTTP headers:");
                Console.WriteLine("--------------------------------------------------------------------------------");
                foreach (var key in httpResponse.Headers.AllKeys)
                {
                    Console.WriteLine(key + ": " + httpResponse.Headers[key]);
                }
            }
        }

        //
        //  Get input data file contents.
        //
        private static String GetInputDataFileContents(Parameters parameters)
        {
            if (!File.Exists(parameters.InputDataFilePath))
            {
                return null;
            }

            var data = new StringBuilder(Int16.MaxValue);

            try
            {
                using (var reader = new StreamReader(parameters.InputDataFilePath))
                {
                    var line = reader.ReadToEnd();
                    data = data.Append(line);
                }
                return data.ToString();
            }
            catch (IOException ioe)
            {
                Console.WriteLine("exception-> unable to read input data file: " + ioe.Message);
            }
            return null;
        }

        //
        //  Store response data.
        //
        private static void StoreResponseData(Parameters parameters, string responseData)
        {
            try
            {
                CreateOutputDataFile(parameters, responseData);
            }
            catch (IOException ioe)
            {
                Console.WriteLine("exception-> unable to write output data file: " + ioe.Message);
            }
        }

        //
        //  Create output data file.
        //
        private static void CreateOutputDataFile(Parameters parameters, string responseData)
        {
            using (var fileStream = new FileStream(parameters.OutputDataFilePath, FileMode.Create))
            {
                using (var writer = new StreamWriter(fileStream))
                {
                    writer.WriteLine(responseData);
                    writer.Flush();
                }
            }
        }

        //
        //  Store request parameters.
        //
        private static bool StoreRequestParameters(HttpWebRequest request, Parameters parameters)
        {
            if (String.IsNullOrEmpty(parameters.RequestParameters))
            {
                return true;
            }

            var enumerator = new CommaSeparatedValuesParser().Parse(parameters.RequestParameters);

            try
            {
                while (enumerator.MoveNext())
                {
                    if (enumerator.Current != null)
                    {
                        var variable = (String)enumerator.Current;
                        string[] variableParts = variable.Split(':');

                        if (variableParts.Length < 2)
                        {
                            Console.WriteLine("exception-> invalid HTTP request parameter field (format: name:value)");
                            return false;
                        }

                        request.Headers.Add(variableParts[0], variableParts[1]);
                    }
                }
            }
            catch (InvalidOperationException)
            {
                Console.WriteLine("invalid operation exception-> invalid HTTP request parameters list");
                return false;
            }
            return true;
        }
    }
}
