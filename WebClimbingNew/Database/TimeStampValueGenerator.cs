namespace Climbing.Web.Database
{
    using System;
    using Microsoft.EntityFrameworkCore.ChangeTracking;
    using Microsoft.EntityFrameworkCore.ValueGeneration;

    internal sealed class TimeStampValueGenerator : ValueGenerator
    {
        public override bool GeneratesTemporaryValues => false;

        protected override object NextValue(EntityEntry entry) => DateTimeOffset.UtcNow;
    }
}