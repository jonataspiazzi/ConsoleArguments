using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsoleArguments
{
    public class Argument
    {
        private ArgumentFoundEventHandler _onFound;

        public string Name { get; set; }
        public string[] Alias { get; set; }
        public string[] BindArgs { get; set; }
        public string Description { get; set; }
        public bool Required { get; set; }
        public bool CaseSensitive { get; set; }

        public Argument(string name)
        {
            Name = name;
            CaseSensitive = true;
        }

        public Argument HasAlias(params string[] alias)
        {
            Alias = alias.ToArray();
            return this;
        }

        public Argument HasBindArgs(params string[] bindArgs)
        {
            BindArgs = bindArgs.ToArray();
            return this;
        }

        public Argument HasDescription(string description)
        {
            Description = description;
            return this;
        }

        public Argument IsRequired()
        {
            Required = true;
            return this;
        }

        public Argument IgnoreCase()
        {
            CaseSensitive = false;
            return this;
        }

        public Argument OnArgumentFound(ArgumentFoundEventHandler onFound)
        {
            _onFound = onFound;
            return this;
        }

        public bool EqualsToArg(string argumentName)
        {
            if (string.Compare(Name, argumentName, !CaseSensitive) == 0) return true;
            if (Alias == null) return false;
            if (Alias.Any(a => string.Compare(a, argumentName, !CaseSensitive) == 0)) return true;

            return false;
        }

        public bool Check(string[] args, int offset)
        {
            if (offset >= args.Length) return false;

            if (!EqualsToArg(args[offset])) return false;

            if (BindArgs?.Length >= args.Length - offset) return false;

            try
            {
                var bindArgs = args.Skip(offset + 1).Take(BindArgs?.Length ?? 0).ToArray();
                var e = new ArgumentFoundEventArgs(this, bindArgs);

                _onFound?.Invoke(this, e);

                return e.IsValid;
            }
            catch
            {
                return false;
            }
        }

        public string ToString(ArgumentStringType type)
        {
            var sb = new StringBuilder();

            if (!Required) sb.Append("[");

            sb.Append(Name);

            if (type == ArgumentStringType.WithAlias)
            {
                Alias?.ToList().ForEach(f => sb.Append("," + f));
            }

            BindArgs?.ToList().ForEach(f => sb.Append(" " + f));

            if (!Required) sb.Append("]");

            return sb.ToString();
        }
    }

    public enum ArgumentStringType
    {
        Short,
        WithAlias
    }
}
