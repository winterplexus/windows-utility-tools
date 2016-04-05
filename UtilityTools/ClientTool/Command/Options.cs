//
//  Options.cs
//
//  Copyright (c) Wiregrass Code Technology 2015-16
//
using System;
using System.Diagnostics;

namespace ClientTool
{
    //
    //  Options class.
    //
    public class Options
    {
        //
        //  Options flag fields.
        //
        private const char UrlFlag = 'u';
        private const char ContentTypeFlag = 'c';
        private const char EncodingTypeFlag = 'e';
        private const char InputDataFilePathFlag = 'i';
        private const char OutputDataFilePathFlag = 'o';
        private const char MethodFlag = 'm';
        private const char ProxyParametersFlag = 'p';
        private const char RequestParametersFlag = 'r';
        private const char VerboseMode = 'v';

        //
        //  Boolean flag literal fields.                
        //
        private const string TrueValue = "true";
        private const string FalseValue = "false";

        //
        //  Method flag literal fields.
        //
        private const string GetValue = "get";
        private const string PostValue = "post";
        private const string PutValue = "put";

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

                parameters.ContentType = "text/html";
                parameters.EncodingType = "utf-8";
                parameters.Method = GetValue;
                parameters.VerboseMode = false;

                var option = arguments[index][1];

                index++;
                switch (option)
                {
                    case UrlFlag:
                         invalidValue = ParseUrlValue(arguments, index, parameters);
                         break;
                    case ContentTypeFlag:
                         invalidValue = ParseContentTypeValue(arguments, index, parameters);
                         break;
                    case EncodingTypeFlag:
                         invalidValue = ParseEncodingTypeValue(arguments, index, parameters);
                         break;
                    case InputDataFilePathFlag:
                         invalidValue = ParseInputDataFilePathValue(arguments, index, parameters);
                         break;
                    case OutputDataFilePathFlag:
                         invalidValue = ParseOutputDataFilePathValue(arguments, index, parameters);
                         break;
                    case MethodFlag:
                         invalidValue = ParseMethodValue(arguments, index, parameters);
                         break;
                    case ProxyParametersFlag:
                         invalidValue = ParseProxyParametersValue(arguments, index, parameters);
                         break;
                    case RequestParametersFlag:
                         invalidValue = ParseRequestParametersValue(arguments, index, parameters);
                         break;
                    case VerboseMode:
                         invalidValue = ParseVerboseModeValue(arguments, index, parameters);
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
        //  Parse HTTP URL value.
        //
        private static bool ParseUrlValue(string[] arguments, int i, Parameters parameters)
        {
            if (arguments.Length <= i || String.IsNullOrEmpty(arguments[i]))
            {
                Console.WriteLine("error-> HTTP URL value is missing");
                return false;
            }
            parameters.Url = arguments[i];
            return true;
        }

        //
        //  Parse HTTP content type value.
        //
        private static bool ParseContentTypeValue(string[] arguments, int i, Parameters parameters)
        {
            if (arguments.Length <= i || String.IsNullOrEmpty(arguments[i]))
            {
                Console.WriteLine("error-> HTTP content type value is missing");
                return false;
            }
            parameters.ContentType = arguments[i];
            return true;
        }

        //
        //  Parse HTTP encoding type value.
        //
        private static bool ParseEncodingTypeValue(string[] arguments, int i, Parameters parameters)
        {
            if (arguments.Length <= i || String.IsNullOrEmpty(arguments[i]))
            {
                Console.WriteLine("error-> HTTP encoding type value is missing");
                return false;
            }
            parameters.EncodingType = arguments[i];
            return true;
        }

        //
        //  Parse HTTP input data file path value.
        //
        private static bool ParseInputDataFilePathValue(string[] arguments, int i, Parameters parameters)
        {
            if (arguments.Length <= i || String.IsNullOrEmpty(arguments[i]))
            {
                Console.WriteLine("error-> HTTP input data file path value is missing");
                return false;
            }
            parameters.InputDataFilePath = arguments[i];
            return true;
        }

        //
        //  Parse HTTP output data file path value.
        //
        private static bool ParseOutputDataFilePathValue(string[] arguments, int i, Parameters parameters)
        {
            if (arguments.Length <= i || String.IsNullOrEmpty(arguments[i]))
            {
                Console.WriteLine("error-> HTTP output data file path value is missing");
                return false;
            }
            parameters.OutputDataFilePath = arguments[i];
            return true;
        }

        //
        //  Parse HTTP method value.
        //
        private static bool ParseMethodValue(string[] arguments, int i, Parameters parameters)
        {
            if (arguments.Length <= i || String.IsNullOrEmpty(arguments[i]))
            {
                Console.WriteLine("error-> HTTP method value is missing");
                return false;
            }
            if (arguments[i].Equals(GetValue, StringComparison.CurrentCultureIgnoreCase))
            {
                parameters.Method = GetValue;
                return true;
            }
            if (arguments[i].Equals(PostValue, StringComparison.CurrentCultureIgnoreCase))
            {
                parameters.Method = PostValue;
                return true;
            }
            if (arguments[i].Equals(PutValue, StringComparison.CurrentCultureIgnoreCase))
            {
                parameters.Method = PutValue;
                return true;
            }
            Console.WriteLine("error-> invalid HTTP method value");
            return false;
        }

        //
        //  Parse HTTP proxy parameters value.
        //
        private static bool ParseProxyParametersValue(string[] arguments, int i, Parameters parameters)
        {
            if (arguments.Length <= i || String.IsNullOrEmpty(arguments[i]))
            {
                Console.WriteLine("error-> HTTP proxy parameters value is missing");
                return false;
            }
            parameters.ProxyParameters = arguments[i];
            return true;
        }

        //
        //  Parse HTTP request parameters value.
        //
        private static bool ParseRequestParametersValue(string[] arguments, int i, Parameters parameters)
        {
            if (arguments.Length <= i || String.IsNullOrEmpty(arguments[i]))
            {
                Console.WriteLine("error-> HTTP request parameters value is missing");
                return false;
            }
            parameters.RequestParameters = arguments[i];
            return true;
        }

        //
        //  Parse verbose mode Boolean value.
        //
        private static bool ParseVerboseModeValue(string[] arguments, int i, Parameters parameters)
        {
            if (arguments.Length <= i || String.IsNullOrEmpty(arguments[i]))
            {
                Console.WriteLine("error-> verbose mode Boolean value is missing");
                return false;
            }
            if (arguments[i].Equals(TrueValue, StringComparison.CurrentCultureIgnoreCase))
            {
                parameters.VerboseMode = true;
                return true;
            }
            if (arguments[i].Equals(FalseValue, StringComparison.CurrentCultureIgnoreCase))
            {
                parameters.VerboseMode = false;
                return true;
            }
            Console.WriteLine("error-> invalid verbose mode Boolean value");
            return false;
        }

        //
        //  Display usage.
        //
        private static void DisplayUsage()
        {
            Console.WriteLine("usage: {0}.exe (options)\r\n", Process.GetCurrentProcess().ProcessName);
            Console.WriteLine("options: -{0} <HTTP URL>", UrlFlag);
            Console.WriteLine("\t -{0} <HTTP content type (default: text/html>", ContentTypeFlag);
            Console.WriteLine("\t -{0} <HTTP encoding type (default: utf-8)>", EncodingTypeFlag);
            Console.WriteLine("\t -{0} <HTTP input data file path>", InputDataFilePathFlag);
            Console.WriteLine("\t -{0} <HTTP output data file path>", OutputDataFilePathFlag);
            Console.WriteLine("\t -{0} [{1}|{2}|{3}] (default HTTP method: {4})", MethodFlag, GetValue, PostValue, PutValue, GetValue);
            Console.WriteLine("\t -{0} \"proxy HTTP parameters\"", ProxyParametersFlag);
            Console.WriteLine("\t -{0} \"request HTTP parameters\"", RequestParametersFlag);
            Console.WriteLine("\t -{0} [{1}|{2}] (default verbose mode: {3})", VerboseMode, TrueValue, FalseValue, FalseValue);
        }
    }
}
