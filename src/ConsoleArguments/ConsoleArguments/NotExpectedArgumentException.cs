using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleArguments
{
    public class NotExpectedArgumentException : Exception
    {
        public string Argument { get; }

        public NotExpectedArgumentException(string message, string argumet) : base(message)
        {
            Argument = argumet;
        }
    }
}
