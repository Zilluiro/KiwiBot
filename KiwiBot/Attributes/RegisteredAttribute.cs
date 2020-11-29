using System;

namespace KiwiBot.Attributes
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    class RegisteredAttribute: Attribute
    {
    }
}
