using System;
using System.Collections;
using NUnit.Framework;
using NUnit.Framework.Constraints;

namespace AutomationUtils.Utils
{
    public class Verify
    {
        public static void IsNotEmpty(string aString, string message, params object[] args)
        {
            That(aString, Is.Not.Empty, message, args);
        }

        public static void DoesNotContain(string expected, string actual, string message, params object[] args)
        {
            That(actual, Does.Not.Contain(expected), message, args);
        }

        public static void IsFalse(bool condition, string message, params object[] args)
        {
            That(condition, Is.False, message, args);
        }

        public static void AreNotEqual(object expected, object actual, string message, params object[] args)
        {
            That(actual, Is.Not.EqualTo(expected), message, args);
        }

        public static void IsTrue(bool? condition, string message, params object[] args)
        {
            That(condition, Is.True, message, args);
        }

        public static void IsEmpty(IEnumerable collection, string message, params object[] args)
        {
            That(collection, new EmptyCollectionConstraint(), message, args);
        }

        public static void AreNotEqualIgnoringCase(string expected, string actual, string message, params object[] args)
        {
            That(actual, Is.Not.EqualTo(expected).IgnoreCase, message, args);
        }

        public static void AreEqualIgnoringCase(string expected, string actual, string message, params object[] args)
        {
            That(actual, Is.EqualTo(expected).IgnoreCase, message, args);
        }

        public static void AreEqual(object expected, object actual, string message, params object[] args)
        {
            That(actual, Is.EqualTo(expected), message, args);
        }

        public static void Contains(object expected, ICollection actual)
        {
            That(actual, new SomeItemsConstraint(new EqualConstraint(expected)), null, null);
        }

        public static void Contains(object expected, ICollection actual, string message, params object[] args)
        {
            That(actual, new SomeItemsConstraint(new EqualConstraint(expected)), message, args);
        }

        public static void Contains(string expected, string actual, string message, params object[] args)
        {
            That(actual, Does.Contain(expected), message, args);
        }

        public static void That(bool condition, string message, params object[] args)
        {
            That(condition, Is.True, message, args);
        }

        public static void That<TActual>(ActualValueDelegate<TActual> del, IResolveConstraint expr, string message, params object[] args)
        {
            var constraint = expr.Resolve();

            var result = constraint.ApplyTo(del);
            if (!result.IsSuccess)
            {
                Logger.Write($"Expected {result.Description}");
                throw new Exception(message);
            }
        }

        public static void That<TActual>(TActual actual, IResolveConstraint expression, string message, params object[] args)
        {
            var constraint = expression.Resolve();

            var result = constraint.ApplyTo(actual);
            if (!result.IsSuccess)
            {
                var exceptionMessage = $"{message}\r\nExpected: {result.Description}\r\nBut was: {MsgUtils.FormatValue(actual)}";
                Logger.Write($"Expected {result.Description}");
                Logger.Write($"But was: {MsgUtils.FormatValue(actual)}");
                Logger.Write(message);
                throw new Exception(exceptionMessage);
            }
        }

        public static void That<TActual>(
            TActual actual,
            IResolveConstraint expression,
            Func<string> getExceptionMessage)
        {
            var constraint = expression.Resolve();

            var result = constraint.ApplyTo(actual);
            if (!result.IsSuccess)
            {
                var exceptionMessage = $"{getExceptionMessage()}\r\nExpected {result.Description}\r\nBut was: {MsgUtils.FormatValue(actual)}";
                Logger.Write($"Expected {result.Description}");
                Logger.Write($"But was: {MsgUtils.FormatValue(actual)}");
                throw new Exception(exceptionMessage);
            }
        }

        public static void That<TActual>(TActual actual, IResolveConstraint expression)
        {
            That(actual, expression, null, null);
        }
    }
}
