using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ConsoleArguments
{
    public class ArgumentPool : IEnumerable<Argument>
    {
        private readonly List<Argument> _args = new List<Argument>();

        public Argument BuildArgument(string name)
        {
            if (_args.Any(a => a.EqualsToArg(name))) throw new Exception($"Name {name} already set to other argument.");

            var arg = new Argument(name);

            _args.Add(arg);

            return arg;
        }

        /// <summary>
        /// Process a argument list checking for all previous defined arguments.
        /// </summary>
        /// <param name="args"></param>
        /// <exception cref="NotExpectedArgumentException">Throws NotExpectedArgumentException if a specific argument isn't expected.</exception>
        /// <exception cref="RequiredArgumentsNotFoundException">Throws RequiredArgumentsNotFoundException if required arguments aren't found.</exception>
        public void ProcessArguments(string[] args)
        {
            var toFind = _args.ToList();

            for (var i = 0; i < args.Length; i++)
            {
                var found = false;

                for (var f = 0; f < toFind.Count; f++)
                {
                    if (toFind[f].Check(args, i))
                    {
                        i += toFind[f].BindArgs?.Length ?? 0;
                        toFind.Remove(toFind[f]);
                        found = true;
                        break;
                    }
                }

                if (!found)
                {
                    throw new NotExpectedArgumentException($"The argument '{args[i]}' was not expected.", args[i]);
                }
            }

            var notFound = toFind.Where(w => w.Required).ToList();

            if (notFound.Any())
            {
                var names = notFound.Aggregate("", (ac, i) => ac + ", " + i.Name).Substring(2);

                throw new RequiredArgumentsNotFoundException($"Arguments {names} were not found.", notFound.Select(s => s.Name).ToArray());
            }
        }

        public string GetHelperText(string commandName)
        {
            var sb = new StringBuilder();

            using (TextWriter tw = new StringWriter(sb))
            {
                tw.WriteLine(commandName + _args.Aggregate("", (ac, i) => ac + " " + i.ToString(ArgumentStringType.Short)));

                var helps = _args
                .Select(s => new
                {
                    Arg = s,
                    Cmd = s.ToString(ArgumentStringType.WithAlias)
                })
                .ToList();

                var padMax = +helps.Max(m => m.Cmd.Length);

                foreach (var help in helps)
                {
                    tw.WriteLine($"  {help.Cmd.PadRight(padMax, ' ')}  {help.Arg.Description}");
                }
            }

            return sb.ToString();
        }

        public IEnumerator<Argument> GetEnumerator()
        {
            return _args.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
