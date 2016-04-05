//
//  Parameters.cs
//
//  Copyright (c) Wiregrass Code Technology 2015-16
//
namespace EventLogTool
{
    //
    //  Parameters class.
    //
    public class Parameters
    {
        //
        //  Command action type.
        //
        public enum CommandActionType { None = 0, Create, Delete };

        //
        //  Command action.
        //
        public CommandActionType CommandAction { get; set; }

        //
        //  Event name.
        //
        public string EventName { get; set; }

        //
        //  Event log name.
        //
        public string EventLogName { get; set; }
    }
}
