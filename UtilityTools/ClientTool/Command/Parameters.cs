//
//  Parameters.cs
//
//  Copyright (c) Wiregrass Code Technology 2015-16
//                                            
namespace ClientTool
{
    //
    //  Parameters class.
    //
    public class Parameters
    {
        //
        //  URL.
        //
        public string Url { get; set; }

        //
        //  Content type.
        //
        public string ContentType { get; set; }

        //
        //  Encoding type.
        //
        public string EncodingType { get; set; }

        //
        //  Input data file path.
        //
        public string InputDataFilePath { get; set; }

        //
        //  Output data file path.
        //
        public string OutputDataFilePath { get; set; }

        //
        //  Method.
        //
        public string Method { get; set; }

        //
        //  Proxy parameters.
        //
        public string ProxyParameters { get; set; }

        //
        //  Request parameters.
        //
        public string RequestParameters { get; set; }

        //
        //  Verbose mode.
        //
        public bool VerboseMode { get; set; }

    }
}
