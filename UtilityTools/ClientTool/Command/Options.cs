//
//  Options.cs
//
//  Copyright (c) Wiregrass Code Technology 2015-17
//
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace ClientTool
{
    public static class Options
    {
        private const char urlFlag = 'u';
        private const char contentTypeFlag = 'c';
        private const char encodingTypeFlag = 'e';
        private const char inputDataFilePathFlag = 'i';
        private const char outputDataFilePathFlag = 'o';
        private const char methodFlag = 'm';
        private const char proxyParametersFlag = 'p';
        private const char requestParametersFlag = 'r';
        private const char verboseMode = 'v';

        private const string trueValue = "true";
        private const string falseValue = "false";

        private const string getValue = "get";
        private const string postValue = "post";
        private const string putValue = "put";

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

                parameters.ContentType = "text/html";
                parameters.EncodingType = "utf-8";
                parameters.Method = getValue;
                parameters.VerboseMode = false;

                var option = arguments[index][1];

                index++;

                bool invalidValue;

                switch (option)
                {
                    case urlFlag:
                         invalidValue = ParseUrlValue(arguments, index, parameters);
                         break;
                    case contentTypeFlag:
                         invalidValue = ParseContentTypeValue(arguments, index, parameters);
                         break;
                    case encodingTypeFlag:
                         invalidValue = ParseEncodingTypeValue(arguments, index, parameters);
                         break;
                    case inputDataFilePathFlag:
                         invalidValue = ParseInputDataFilePathValue(arguments, index, parameters);
                         break;
                    case outputDataFilePathFlag:
                         invalidValue = ParseOutputDataFilePathValue(arguments, index, parameters);
                         break;
                    case methodFlag:
                         invalidValue = ParseMethodValue(arguments, index, parameters);
                         break;
                    case proxyParametersFlag:
                         invalidValue = ParseProxyParametersValue(arguments, index, parameters);
                         break;
                    case requestParametersFlag:
                         invalidValue = ParseRequestParametersValue(arguments, index, parameters);
                         break;
                    case verboseMode:
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

        private static bool ParseUrlValue(IReadOnlyList<string> arguments, int i, Parameters parameters)
        {
            if (arguments.Count <= i || string.IsNullOrEmpty(arguments[i]))
            {
                Console.WriteLine("error-> HTTP URL value is missing");
                return false;
            }
            try
            {
                parameters.Url = new Uri(arguments[i]);
                return true;
            }
            catch (UriFormatException)
            {
                Console.WriteLine("error-> HTTP URL is not properly formatted");
            }
            return false;
        }

        private static bool ParseContentTypeValue(IReadOnlyList<string> arguments, int i, Parameters parameters)
        {
            if (arguments.Count <= i || string.IsNullOrEmpty(arguments[i]))
            {
                Console.WriteLine("error-> HTTP content type value is missing");
                return false;
            }
            parameters.ContentType = arguments[i];
            return true;
        }

        private static bool ParseEncodingTypeValue(IReadOnlyList<string> arguments, int i, Parameters parameters)
        {
            if (arguments.Count <= i || string.IsNullOrEmpty(arguments[i]))
            {
                Console.WriteLine("error-> HTTP encoding type value is missing");
                return false;
            }
            parameters.EncodingType = arguments[i];
            return true;
        }

        private static bool ParseInputDataFilePathValue(IReadOnlyList<string> arguments, int i, Parameters parameters)
        {
            if (arguments.Count <= i || string.IsNullOrEmpty(arguments[i]))
            {
                Console.WriteLine("error-> HTTP input data file path value is missing");
                return false;
            }
            parameters.InputDataFilePath = arguments[i];
            return true;
        }

        private static bool ParseOutputDataFilePathValue(IReadOnlyList<string> arguments, int i, Parameters parameters)
        {
            if (arguments.Count <= i || string.IsNullOrEmpty(arguments[i]))
            {
                Console.WriteLine("error-> HTTP output data file path value is missing");
                return false;
            }
            parameters.OutputDataFilePath = arguments[i];
            return true;
        }

        private static bool ParseMethodValue(IReadOnlyList<string> arguments, int i, Parameters parameters)
        {
            if (arguments.Count <= i || string.IsNullOrEmpty(arguments[i]))
            {
                Console.WriteLine("error-> HTTP method value is missing");
                return false;
            }
            if (arguments[i].Equals(getValue, StringComparison.CurrentCultureIgnoreCase))
            {
                parameters.Method = getValue;
                return true;
            }
            if (arguments[i].Equals(postValue, StringComparison.CurrentCultureIgnoreCase))
            {
                parameters.Method = postValue;
                return true;
            }
            if (arguments[i].Equals(putValue, StringComparison.CurrentCultureIgnoreCase))
            {
                parameters.Method = putValue;
                return true;
            }
            Console.WriteLine("error-> invalid HTTP method value");
            return false;
        }

        private static bool ParseProxyParametersValue(IReadOnlyList<string> arguments, int i, Parameters parameters)
        {
            if (arguments.Count <= i || string.IsNullOrEmpty(arguments[i]))
            {
                Console.WriteLine("error-> HTTP proxy parameters value is missing");
                return false;
            }
            parameters.ProxyParameters = arguments[i];
            return true;
        }

        private static bool ParseRequestParametersValue(IReadOnlyList<string> arguments, int i, Parameters parameters)
        {
            if (arguments.Count <= i || string.IsNullOrEmpty(arguments[i]))
            {
                Console.WriteLine("error-> HTTP request parameters value is missing");
                return false;
            }
            parameters.RequestParameters = arguments[i];
            return true;
        }

        private static bool ParseVerboseModeValue(IReadOnlyList<string> arguments, int i, Parameters parameters)
        {
            if (arguments.Count <= i || string.IsNullOrEmpty(arguments[i]))
            {
                Console.WriteLine("error-> verbose mode Boolean value is missing");
                return false;
            }
            if (arguments[i].Equals(trueValue, StringComparison.CurrentCultureIgnoreCase))
            {
                parameters.VerboseMode = true;
                return true;
            }
            if (arguments[i].Equals(falseValue, StringComparison.CurrentCultureIgnoreCase))
            {
                parameters.VerboseMode = false;
                return true;
            }
            Console.WriteLine("error-> invalid verbose mode Boolean value");
            return false;
        }

        private static void DisplayUsage()
        {
            Console.WriteLine("usage: {0}.exe (options)\r\n", Process.GetCurrentProcess().ProcessName);
            Console.WriteLine("options: -{0} <HTTP URL>", urlFlag);
            Console.WriteLine("\t -{0} <HTTP content type (default: text/html>", contentTypeFlag);
            Console.WriteLine("\t -{0} <HTTP encoding type (default: utf-8)>", encodingTypeFlag);
            Console.WriteLine("\t -{0} <HTTP input data file path>", inputDataFilePathFlag);
            Console.WriteLine("\t -{0} <HTTP output data file path>", outputDataFilePathFlag);
            Console.WriteLine("\t -{0} [{1}|{2}|{3}] (default HTTP method: {4})", methodFlag, getValue, postValue, putValue, getValue);
            Console.WriteLine("\t -{0} \"proxy HTTP parameters\"", proxyParametersFlag);
            Console.WriteLine("\t -{0} \"request HTTP parameters\"", requestParametersFlag);
            Console.WriteLine("\t -{0} [{1}|{2}] (default verbose mode: {3})", verboseMode, trueValue, falseValue, falseValue);
        }
    }
}                              