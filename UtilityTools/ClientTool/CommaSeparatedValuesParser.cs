//
//  CommaSeparatedValuesParser.cs
//
//  Copyright (c) Wiregrass Code Technology 2015-16
// 
using System;
using System.Collections;
using System.Text;

namespace ClientTool
{
    //
    //  Comma-separated values parser class.
    //
    public class CommaSeparatedValuesParser
    {
        //
        //  Separator field.
        //
        private const char separator = DefaultSeparator;

        //
        //  Values list field.
        //
        private readonly ArrayList values = new ArrayList();

        //
        //  Default separator field.
        //
        public const char DefaultSeparator = ',';

        //
        //  Class constructor for a comma-separated values parser.
        //
        public CommaSeparatedValuesParser()
        {
            values.Clear();
        }

        //
        //  Parse (line into values list).
        //
        public IEnumerator Parse(string line)
        {
            var buffer = new StringBuilder();
            var i = 0;

            if (line == null)
            {
                throw new ArgumentNullException("line");
            }

            values.Clear();

            if (line.Length == 0)
            {
                values.Add(line);
                return values.GetEnumerator();
            }

            do
            {
                buffer.Length = 0;
                if ((i < line.Length) && (line[i] == '"'))
                {
                    i = GetIndexNextQuotedValue(line, buffer, ++i);
                }
                else
                {
                    i = GetIndexNextUnquotedValue(line, buffer, i);
                }

                values.Add(buffer.ToString());
                i++;
            }
            while (i < line.Length);

            return values.GetEnumerator();
        }

        //
        //  Separator property.
        //
        public char Separator { get; set; }

        //
        //  Get index for next quoted value.
        //
        protected static int GetIndexNextQuotedValue(string text, StringBuilder buffer, int index)
        {
            int i;

            if (text == null)
            {
                throw new ArgumentNullException("text");
            }
            if (buffer == null)
            {
                throw new ArgumentNullException("buffer");
            }

            var len = text.Length;

            for (i = index; i < len; i++)
            {
                if ((text[i] == '"') && (i + 1 < len))
                {
                    if (text[i + 1] == '"')
                    {
                        i++;
                    }
                    else if (text[i + 1] == separator)
                    {
                        i++;
                        break;
                    }
                }
                else if ((text[i] == '"') && (i + 1 == len))
                {
                    break;
                }

                buffer.Append(text[i]);
            }

            return i;
        }

        //
        //  Get index for next unquoted value.
        //
        protected static int GetIndexNextUnquotedValue(string text, StringBuilder buffer, int index)
        {
            if (text == null)
            {
                throw new ArgumentNullException("text");
            }
            if (buffer == null)
            {
                throw new ArgumentNullException("buffer");
            }

            var i = text.IndexOf(separator, index);
            if (i == -1)
            {
                buffer.Append(text.Substring(index));
                return text.Length;
            }

            buffer.Append(text.Substring(index, (i - index)));
            return i;
        }
    }
}