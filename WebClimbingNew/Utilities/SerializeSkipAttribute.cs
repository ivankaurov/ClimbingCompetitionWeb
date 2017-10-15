using System;

namespace Utilities
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class SerializeSkipAttribute : Attribute
    {
    }
}