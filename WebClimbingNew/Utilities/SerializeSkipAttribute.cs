using System;

namespace Climbing.Web.Utilities
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Class)]
    public class SerializeSkipAttribute : Attribute
    {
    }
}