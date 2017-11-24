using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleArguments
{
    class RequiredArgumentsNotFoundException : Exception
    {
        public string[] Arguments { get; set; }

        public RequiredArgumentsNotFoundException(string message, string[] arguments) : base(message)
        {
            Arguments = arguments;
        }
    }
}
