namespace Climbing.Web.Tests.Unit
{
    using System;
    using Climbing.Web.Utilities.Mapper;
    using Xunit;

    public class AutoMapperTests
    {
        [Theory]
        [AutoMoqData]
        public void ShouldMap(ClassFrom from)
        {
            // Arrange
            AutoMapper.Setup((ClassFrom cf) => cf.Field).To((ClassTo ct) => ct.FieldTo);
            AutoMapper.Setup((ClassFrom cf) => cf.Property).To((ClassTo ct) => ct.Property1);
            AutoMapper.Setup((ClassFrom cf) => $"{cf.Field}|{cf.Property}").To((ClassTo ct) => ct.Property2);

            // Act
            var res = AutoMapper.Map<ClassTo>(from);

            // Assert
            Assert.Equal(from.Field, res.FieldTo);
            Assert.Equal(from.Property, res.Property1);
            Assert.Equal($"{from.Field}|{from.Property}", res.Property2);
        }

        public sealed class ClassFrom
        {
#pragma warning disable SA1401 // Fields should be private
            internal string Field = Guid.NewGuid().ToString();
#pragma warning restore SA1401 // Fields should be private

            public string Property { get; set; }
        }

        public sealed class ClassTo
        {
#pragma warning disable SA1401 // Fields should be private
            public string FieldTo = "11";
#pragma warning restore SA1401 // Fields should be private

            public string Property1 { get; set; }

            public string Property2 { get; set; }
        }
    }
}