namespace Climbing.Web.Utilities
{
    using System;

    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Class)]
    public class SerializeSkipAttribute : Attribute
    {
    }
}