using System;
using ServiceStack.Common;
using ServiceStack.Text;

namespace MetacognitiveTutor.Api.Helpers
{
    public static class Guard // https://gist.githubusercontent.com/PradeepLoganathan/bc4ae7daf64d5db4c4912000148011e2/raw/b0e24bcd12f2dc5350292124e860ab7f4a3db8d3/Guard.cs, 07/04/2018
    {
        public static void AgainstNull<T>(T value)
            where T : class
        {
            if (value == null)
                throw new ArgumentNullException();
        }

        public static void AgainstNull<T>(T value, string paramName)
            where T : class
        {
            if (value == null)
                throw new ArgumentNullException(paramName);
        }

        public static void AgainstNull<T>(T value, string paramName, string message)
            where T : class
        {
            if (value == null)
                throw new ArgumentNullException(paramName, message);
        }

        public static void AgainstNull<T>(T? value)
            where T : struct
        {
            if (!value.HasValue)
                throw new ArgumentNullException();
        }

        public static void AgainstNull<T>(T? value, string paramName)
            where T : struct
        {
            if (!value.HasValue)
                throw new ArgumentNullException(paramName);
        }

        public static void AgainstNull<T>(T? value, string paramName, string message)
            where T : struct
        {
            if (!value.HasValue)
                throw new ArgumentNullException(paramName, message);
        }

        public static void AgainstNull(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentNullException();
        }

        public static void AgainstNull(string value, string paramName)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentNullException(paramName);
        }

        public static void AgainstNull(string value, string paramName, string message)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentNullException(paramName, message);
        }

        public static void AgainstEmpty(string value)
        {
            if (value.IsEmpty())
                throw new ArgumentException("string value must not be empty");
        }

        public static void AgainstEmpty(string value, string paramName)
        {
            if (value.IsEmpty())
                throw new ArgumentException("string value must not be empty", paramName);
        }

        public static void AgainstEmpty(string value, string paramName, string message)
        {
            if (value.IsEmpty())
                throw new ArgumentException(message, paramName);
        }

        public static void GreaterThan<T>(T lowerLimit, T value)
            where T : IComparable<T>
        {
            if (value.CompareTo(lowerLimit) <= 0)
                throw new ArgumentOutOfRangeException();
        }

        public static void GreaterThan<T>(T lowerLimit, T value, string paramName)
            where T : IComparable<T>
        {
            if (value.CompareTo(lowerLimit) <= 0)
                throw new ArgumentOutOfRangeException(paramName);
        }

        public static void GreaterThan<T>(T lowerLimit, T value, string paramName, string message)
            where T : IComparable<T>
        {
            if (value.CompareTo(lowerLimit) <= 0)
                throw new ArgumentOutOfRangeException(paramName, message);
        }


        public static void LessThan<T>(T upperLimit, T value)
            where T : IComparable<T>
        {
            if (value.CompareTo(upperLimit) >= 0)
                throw new ArgumentOutOfRangeException();
        }

        public static void LessThan<T>(T upperLimit, T value, string paramName)
            where T : IComparable<T>
        {
            if (value.CompareTo(upperLimit) >= 0)
                throw new ArgumentOutOfRangeException(paramName);
        }

        public static void LessThan<T>(T upperLimit, T value, string paramName, string message)
            where T : IComparable<T>
        {
            if (value.CompareTo(upperLimit) >= 0)
                throw new ArgumentOutOfRangeException(paramName, message);
        }

        public static void IsTrue<T>(Func<T, bool> condition, T target)
        {
            if (!condition(target))
                throw new ArgumentException("condition was not true");
        }

        public static void IsTrue<T>(Func<T, bool> condition, T target, string paramName)
        {
            if (!condition(target))
                throw new ArgumentException("condition was not true", paramName);
        }

        public static void IsTrue<T>(Func<T, bool> condition, T target, string paramName, string message)
        {
            if (!condition(target))
                throw new ArgumentException(message, paramName);
        }


        public static T IsTypeOf<T>(object obj)
        {
            AgainstNull(obj);

            if (obj is T)
                return (T)obj;

            throw new ArgumentException("{0} is not an instance of type {1}".FormatWith(obj.GetType().Name, typeof(T).Name));
        }

    }
}