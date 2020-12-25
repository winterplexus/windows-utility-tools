//
//  Parameters.cs
//
//  Copyright (c) Wiregrass Code Technology 2015-2021
//
namespace EventLogTool
{
    public class Parameters
    {
        public enum CommandActionType { None = 0, Create, Delete };

        public CommandActionType CommandAction { get; set; }

        public string EventName { get; set; }

        public string EventLogName { get; set; }
    }
}            