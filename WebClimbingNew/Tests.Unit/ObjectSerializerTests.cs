using System;
using System.Linq.Expressions;
using Utilities;
using Xunit;

namespace Tests.Unit
{
    public class ObjectSerializerTests
    {
        [Theory]
        [AutoMoqInlineData(nameof(TestClassToSerialize.publicField))]
        [AutoMoqInlineData(nameof(TestClassToSerialize.internalField))]
        [AutoMoqInlineData("ProtectedProperty")]
        [AutoMoqInlineData("PrivateProperty")]
        public void ShouldSerializeStringProperties(string propertyName, TestClassToSerialize sut)
        {
            // Arrange
            var expectedValue = (string)MethodHelper.GetPropertyOrFieldValue(sut, propertyName);

            // Act
            var dict = ObjectSerializer.ExtractProperties(sut);
            var value = dict[propertyName];

            // Assert
            Assert.Equal(expectedValue, (string)value.Value);
            Assert.Equal(typeof(string), value.Type);
        }

        [Theory]
        [AutoMoqInlineData(nameof(TestClassToSerialize.PublicProperty))]
        [AutoMoqInlineData(nameof(TestClassToSerialize.InternalProperty))]
        [AutoMoqInlineData("protectedField")]
        [AutoMoqInlineData("privateField")]
        public void ShouldSerializeIntProperties(string propertyName, TestClassToSerialize sut)
        {
            // Arrange
            var expectedValue = (int)MethodHelper.GetPropertyOrFieldValue(sut, propertyName);

            // Act
            var dict = ObjectSerializer.ExtractProperties(sut);
            var value = dict[propertyName];

            // Assert
            Assert.Equal(expectedValue, (int)value.Value);
            Assert.Equal(typeof(int), value.Type);
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
            Assert.Equal(12, dict.Count);
        }

        public class TestClassToSerialize
        {
            public string publicField;
            internal string internalField = $"internalField{Guid.NewGuid()}";
            protected int protectedField = Guid.NewGuid().ToByteArray()[0];
            private int privateField = Guid.NewGuid().ToByteArray()[1];

            public int PublicProperty { get; set; }
            internal int InternalProperty { get; set; } = Guid.NewGuid().ToByteArray()[2];
            protected string ProtectedProperty { get; set; } = $"ProtectedProperty{Guid.NewGuid()}";
            private string PrivateProperty { get; set; } = $"PrivateProperty{Guid.NewGuid()}";
        }
    }
}
