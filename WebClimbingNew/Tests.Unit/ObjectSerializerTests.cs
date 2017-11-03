using System;
using System.Linq.Expressions;
using Climbing.Web.Utilities;
using Xunit;

namespace Climbing.Web.Tests.Unit
{
    public class ObjectSerializerTests
    {
        [Theory]
        [AutoMoqInlineData(nameof(TestClassToSerialize.publicField), MemberType.Field)]
        [AutoMoqInlineData(nameof(TestClassToSerialize.internalField), MemberType.Field)]
        [AutoMoqInlineData("ProtectedProperty", MemberType.Property)]
        [AutoMoqInlineData("PrivateProperty", MemberType.Property)]
        public void ShouldSerializeStringProperties(string propertyName, MemberType memberType, TestClassToSerialize sut)
        {
            // Arrange
            var expectedValue = (string)MethodHelper.GetPropertyOrFieldValue(sut, propertyName);

            // Act
            var dict = ObjectSerializer.ExtractProperties(sut);
            var value = dict[propertyName];

            // Assert
            Assert.Equal(expectedValue, (string)value.Value);
            Assert.Equal(typeof(string), value.Type);
            Assert.Equal(memberType, value.MemberType);
        }

        [Theory]
        [AutoMoqInlineData(nameof(TestClassToSerialize.PublicProperty), MemberType.Property)]
        [AutoMoqInlineData(nameof(TestClassToSerialize.InternalProperty), MemberType.Property)]
        [AutoMoqInlineData("protectedField", MemberType.Field)]
        [AutoMoqInlineData("privateField", MemberType.Field)]
        public void ShouldSerializeIntProperties(string propertyName, MemberType memberType, TestClassToSerialize sut)
        {
            // Arrange
            var expectedValue = (int)MethodHelper.GetPropertyOrFieldValue(sut, propertyName);

            // Act
            var dict = ObjectSerializer.ExtractProperties(sut);
            var value = dict[propertyName];

            // Assert
            Assert.Equal(expectedValue, (int)value.Value);
            Assert.Equal(typeof(int), value.Type);
            Assert.Equal(memberType, value.MemberType);
        }

        [Theory]
        [AutoMoqData]
        public void ShouldNotFindField(TestClassToSerialize sut, string propertyOrFieldName)
        {
            // Act
            var dict = ObjectSerializer.ExtractProperties(sut);

            // Assert
            Assert.False(dict.ContainsKey(propertyOrFieldName));
        }

        [Theory]
        [AutoMoqData]
        public void ShouldGetAllMembers(TestClassToSerialize sut)
        {
            // Act
            var dict = ObjectSerializer.ExtractProperties(sut);

            // Assert
            Assert.Equal(8, dict.Count);
        }

        [Theory]
        [AutoMoqInlineData(nameof(TestClassToSerialize.skipField))]
        [AutoMoqInlineData(nameof(TestClassToSerialize.SkipProperty))]
        public void ShouldNotSerializeSkippedMembers(string memberName, TestClassToSerialize sut)
        {
            // Act
            var dict = ObjectSerializer.ExtractProperties(sut);

            // Assert
            Assert.False(dict.ContainsKey(memberName));
        }

        public class TestClassToSerialize
        {
            [SerializeSkip]
            public int skipField;

            public string publicField;
            internal string internalField = $"internalField{Guid.NewGuid()}";
            protected int protectedField = Guid.NewGuid().ToByteArray()[0];
            private int privateField = Guid.NewGuid().ToByteArray()[1];
            
            [SerializeSkip]
            public string SkipProperty { get; set; }

            public int PublicProperty { get; set; }
            internal int InternalProperty { get; set; } = Guid.NewGuid().ToByteArray()[2];
            protected string ProtectedProperty { get; set; } = $"ProtectedProperty{Guid.NewGuid()}";
            private string PrivateProperty { get; set; } = $"PrivateProperty{Guid.NewGuid()}";
        }
    }
}
