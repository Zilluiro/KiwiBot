using System;
using System.Collections.Generic;
using System.Linq;

namespace KiwiBot.Attributes
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    class CommandAttribute: Attribute
    {
        public List<string> Commands { get; set; }

        public CommandAttribute(params string[] commands)
        {
            Commands = commands.ToList();
        }

        public bool HasCommand(string command)
        {
            return Commands is object && Commands.Contains(command);
        }
    }
}
