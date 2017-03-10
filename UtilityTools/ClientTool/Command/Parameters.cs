//
//  Parameters.cs
//
//  Copyright (c) Wiregrass Code Technology 2015-17
//                        
using System;

namespace ClientTool
{
    public class Parameters
    {
        public Uri Url { get; set; }

        public string ContentType { get; set; }

        public string EncodingType { get; set; }

        public string InputDataFilePath { get; set; }

        public string OutputDataFilePath { get; set; }

        public string Method { get; set; }

        public string ProxyParameters { get; set; }

        public string RequestParameters { get; set; }

        public bool VerboseMode { get; set; }

    }
}                        