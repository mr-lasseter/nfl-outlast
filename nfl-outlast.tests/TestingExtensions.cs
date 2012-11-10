using System;
using System.Collections;
using System.Reflection;
using NUnit.Framework;
using System.Linq;

namespace nfl_outlast.tests
{
    public delegate void MethodThatThrows();

    public static class TestingExtensions
    {
        public static void ShouldBeFalse(this bool condition)
        {
            Assert.IsFalse(condition);
        }

        public static void ShouldBeTrue(this bool condition)
        {
            Assert.IsTrue(condition);
        }

        public static object ShouldEqual<T>(this T actual, T expected)
        {
            Assert.AreEqual(expected, actual);
            return expected;
        }

        public static object ShouldNotEqual<T>(this T actual, T expected)
        {
            Assert.AreNotEqual(expected, actual);
            return expected;
        }

        public static void ShouldBeNull(this object anObject)
        {
            Assert.IsNull(anObject);
        }

        public static void ShouldNotBeNull(this object anObject)
        {
            Assert.IsNotNull(anObject);
        }

        public static object ShouldBeTheSameAs(this object actual, object expected)
        {
            Assert.AreSame(expected, actual);
            return expected;
        }

        public static object ShouldNotBeTheSameAs(this object actual, object expected)
        {
            Assert.AreNotSame(expected, actual);
            return expected;
        }

        public static void ShouldBeOfType<T>(this object actual)
        {
            ShouldBeOfType(actual, typeof(T));
        }

        public static void ShouldBeOfType(this object actual, Type expected)
        {
            Assert.IsInstanceOf(expected, actual);
        }

        public static void ShouldBe<T>(this object actual)
        {
            ShouldBe(actual, typeof(T));
        }

        public static void ShouldBe(this object actual, Type expected)
        {
            Assert.IsInstanceOf(expected, actual);
        }

        public static void ShouldNotBeOfType<T>(this object actual)
        {
            ShouldNotBeOfType(actual, typeof(T));
        }

        public static void ShouldNotBeOfType(this object actual, Type expected)
        {
            Assert.IsNotInstanceOf(expected, actual);
        }

        public static void ShouldContain(this IList actual, object expected)
        {
            Assert.IsTrue(actual.Contains(expected));
        }

        public static void ShouldNotContain(this IList collection, object expected)
        {
            CollectionAssert.DoesNotContain(collection, expected);
        }

        public static IComparable ShouldBeGreaterThan(this IComparable arg1, IComparable arg2)
        {
            Assert.Greater(arg1, arg2);
            return arg2;
        }

        public static IComparable ShouldBeLessThan(this IComparable arg1, IComparable arg2)
        {
            Assert.Less(arg1, arg2);
            return arg2;
        }

        public static void ShouldBeEmpty(this ICollection collection)
        {
            Assert.IsEmpty(collection);
        }

        public static void ShouldBeEmpty(this string aString)
        {
            Assert.IsEmpty(aString);
        }

        public static void ShouldNotBeEmpty(this ICollection collection)
        {
            Assert.IsNotEmpty(collection);
        }

        public static void ShouldNotBeEmpty(this string aString)
        {
            Assert.IsNotEmpty(aString);
        }

        public static void ShouldContain(this string actual, string expected)
        {
            StringAssert.Contains(expected, actual);
        }

        public static void ShouldNotContain(this string actual, string expected)
        {
            try
            {
                StringAssert.Contains(expected, actual);
            }
            catch (AssertionException)
            {
                return;
            }

            throw new AssertionException(String.Format("\"{0}\" should not contain \"{1}\".", actual, expected));
        }

        public static string ShouldBeEqualIgnoringCase(this string actual, string expected)
        {
            StringAssert.AreEqualIgnoringCase(expected, actual);
            return expected;
        }

        public static void ShouldStartWith(this string actual, string expected)
        {
            StringAssert.StartsWith(expected, actual);
        }

        public static void ShouldEndWith(this string actual, string expected)
        {
            StringAssert.EndsWith(expected, actual);
        }

        public static void ShouldBeSurroundedWith(this string actual, string expectedStartDelimiter, string expectedEndDelimiter)
        {
            StringAssert.StartsWith(expectedStartDelimiter, actual);
            StringAssert.EndsWith(expectedEndDelimiter, actual);
        }

        public static void ShouldBeSurroundedWith(this string actual, string expectedDelimiter)
        {
            StringAssert.StartsWith(expectedDelimiter, actual);
            StringAssert.EndsWith(expectedDelimiter, actual);
        }

        public static void ShouldContainErrorMessage(this Exception exception, string expected)
        {
            StringAssert.Contains(expected, exception.Message);
        }

        public static Exception ShouldBeThrownBy(this Type exceptionType, MethodThatThrows method)
        {
            var exception = method.GetException();

            Assert.IsNotNull(exception);
            Assert.AreEqual(exceptionType, exception.GetType());

            return exception;
        }

        public static Exception GetException(this MethodThatThrows method)
        {
            Exception exception = null;

            try
            {
                method();
            }
            catch (Exception e)
            {
                exception = e;
            }

            return exception;
        }

        public static void ShouldNotBeDefault<T>(this T actual)
        {
            Assert.AreNotEqual(actual, default(T));
        }

        public static void PropertyValuesShouldEqual<T>(this T expected, T actual) where T : class
        {
            var properties = typeof(T).GetProperties().Where(x => x.CanRead);
            foreach (var property in properties)
            {
                var expectedValue = property.GetValue(expected, null);
                var actualValue = property.GetValue(actual, null);

                if (actualValue == null && expectedValue == null) return;

                if (actualValue is IList)
                {
                    AssertListsAreEqual(property, (IList)actualValue, (IList)expectedValue);
                }
                else if (actualValue.GetType().IsClass)
                {
                    expectedValue.PropertyValuesShouldEqual(actualValue);
                }
                else if (actualValue.GetType() == typeof(DateTime))
                {
                    var actualDate = (DateTime)actualValue;
                    var expectedDate = (DateTime)expectedValue;
                    actualDate.ToString().ShouldEqual(expectedDate.ToString());
                }
                else if (!Equals(expectedValue, actualValue))
                {
                    Assert.Fail("Property {0}.{1} does not match. Expected: {2} but was: {3}", property.DeclaringType.Name, property.Name, expectedValue, actualValue);
                }
            }
        }

        private static void AssertListsAreEqual(PropertyInfo property, IList actualList, IList expectedList)
        {
            if (actualList.Count != expectedList.Count)
            {
                Assert.Fail("Property {0}.{1} does not match. Expected IList containing {2} elements but was IList containing {3} elements", property.PropertyType.Name, property.Name, expectedList.Count, actualList.Count);
            }

            for (var i = 0; i < actualList.Count; i++)
            {
                if (actualList[i].GetType().IsClass)
                {
                    actualList[i].PropertyValuesShouldEqual(expectedList[i]);
                }
                else if (!Equals(actualList[i], expectedList[i]))
                {
                    Assert.Fail("Property {0}.{1} does not match. Expected IList with element {1} equals to {2} but was IList with element {1} equals to {3}", property.PropertyType.Name, property.Name, expectedList[i], actualList[i]);
                }
            }
        }
    }
}
