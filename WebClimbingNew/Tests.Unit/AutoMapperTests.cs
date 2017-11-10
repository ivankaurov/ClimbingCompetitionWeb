using System;
using Xunit;
using Climbing.Web.Utilities.Mapper;

namespace Climbing.Web.Tests.Unit
{
    public class AutoMapperTests
    {
        [Theory]
        [AutoMoqData]
        public void ShouldMap(ClassFrom from)
        {
            //Arrange
            AutoMapper.Setup((ClassFrom cf)=> cf.field).To((ClassTo ct) => ct.fieldTo);
            AutoMapper.Setup((ClassFrom cf) => cf.Property).To((ClassTo ct) => ct.Property1);
            AutoMapper.Setup((ClassFrom cf) => $"{cf.field}|{cf.Property}").To((ClassTo ct) => ct.Property2);

            // Act
            var res = AutoMapper.Map<ClassTo>(from);

            // Assert
            Assert.Equal(from.field, res.fieldTo);
            Assert.Equal(from.Property, res.Property1);
            Assert.Equal($"{from.field}|{from.Property}", res.Property2);
        }
        public sealed class ClassFrom
        {
            internal string field = Guid.NewGuid().ToString();

            public string Property{ get; set; }
        }

        public sealed class ClassTo
        {
            public string fieldTo = "11";

            public string Property1{get;set;}
            public string Property2 {get;set;}
        }
    }
}