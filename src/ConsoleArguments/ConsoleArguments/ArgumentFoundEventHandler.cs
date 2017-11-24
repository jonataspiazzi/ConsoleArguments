using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleArguments
{
    public delegate void ArgumentFoundEventHandler(object sender, ArgumentFoundEventArgs e);

    public class ArgumentFoundEventArgs
    {
        public Argument Argument { get; }

        public string[] BindArguments { get; }
        public bool IsValid { get; set; }

        public ArgumentFoundEventArgs(Argument argument, string[] bindArguments)
        {
            Argument = argument;
            BindArguments = bindArguments;
        }
    }
}
