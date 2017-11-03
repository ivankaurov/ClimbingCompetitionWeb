using System;

namespace Climbing.Web.Utilities
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class SerializeSkipAttribute : Attribute
    {
    }
}