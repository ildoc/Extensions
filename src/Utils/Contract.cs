using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Utils
{
    public static class Contract
    {
        private static void ArgNotNull(object arg, Type t, string varName = "", [CallerMemberName] string callerName = "", [CallerFilePath] string filePath = "", [CallerLineNumber] int callerLineNumber = 0)
        {
            var typeName = t?.Name ?? "";
            var fileName = string.IsNullOrEmpty(filePath) ? "" : Path.GetFileName(filePath);
            if (arg == null || (arg is string strArgs && string.IsNullOrEmpty(strArgs)))
            {
                throw new ArgumentNullException($"{typeName} {varName}", $"{callerName}({typeName} {varName}) - {fileName}:{callerLineNumber} can't be null");
            }
        }

        private static void Requires<T>(RequireContract<T> arg, string info = "", [CallerMemberName] string callerName = "", [CallerFilePath] string filePath = "", [CallerLineNumber] int callerLineNumber = 0)
        {
            var fileName = string.IsNullOrEmpty(filePath) ? "" : Path.GetFileName(filePath);
            foreach (var c in arg.Conditions)
            {
                if (!c.Condition(arg.Arg))
                {
                    if (c.ExceptionType != null)
                        throw (Exception)Activator.CreateInstance(c.ExceptionType, $"Contract: {info} in {callerName} - {fileName}:{callerLineNumber} {c.ErrorMessage}");
                    throw new ContractsException($"Contract: {info} in {callerName} - {fileName}:{callerLineNumber} {c.ErrorMessage}");
                }
            }
        }

        private static void ArgListNotNull(IEnumerable<object> args, IEnumerable<Type> types = null, [CallerMemberName] string callerName = "", [CallerFilePath] string filePath = "",
            [CallerLineNumber] int callerLineNumber = 0)
        {
            For.EachIndex(args, (o, i) => ArgNotNull(o, types?.ElementAt(i), $"args[{i}]", callerName, filePath, callerLineNumber));
        }

        public static void ArgsNotNull<T>(T arg, [CallerMemberName] string callerName = "", [CallerFilePath] string filePath = "", [CallerLineNumber] int callerLineNumber = 0) => ArgListNotNull(new object[] { arg }, new[] { typeof(T) }, callerName, filePath, callerLineNumber);
        public static void ArgsNotNull<T0, T1>(T0 arg0, T1 arg1, [CallerMemberName] string callerName = "", [CallerFilePath] string filePath = "", [CallerLineNumber] int callerLineNumber = 0) => ArgListNotNull(new object[] { arg0, arg1 }, new[] { typeof(T0), typeof(T1) }, callerName, filePath, callerLineNumber);

        public static void ArgsNotNull<T0, T1, T2>(T0 arg0, T1 arg1, T2 arg2, [CallerMemberName] string callerName = "", [CallerFilePath] string filePath = "", [CallerLineNumber] int callerLineNumber = 0) => ArgListNotNull(new object[] { arg0, arg1, arg2 }, new[] { typeof(T0), typeof(T1), typeof(T2) }, callerName, filePath, callerLineNumber);

        public static void ArgsNotNull<T0, T1, T2, T3>(T0 arg0, T1 arg1, T2 arg2, T3 arg3, [CallerMemberName] string callerName = "", [CallerFilePath] string filePath = "", [CallerLineNumber] int callerLineNumber = 0) => ArgListNotNull(new object[] { arg0, arg1, arg2, arg3 }, new[] { typeof(T0), typeof(T1), typeof(T2), typeof(T3) }, callerName, filePath, callerLineNumber);

        public static void ArgsNotNull<T0, T1, T2, T3, T4>(T0 arg0, T1 arg1, T2 arg2, T3 arg3, T4 arg4, [CallerMemberName] string callerName = "", [CallerFilePath] string filePath = "", [CallerLineNumber] int callerLineNumber = 0) => ArgListNotNull(new object[] { arg0, arg1, arg2, arg3, arg4 }, new[] { typeof(T0), typeof(T1), typeof(T2), typeof(T3), typeof(T4) }, callerName, filePath, callerLineNumber);

        public static void ArgsNotNull<T0, T1, T2, T3, T4, T5>(T0 arg0, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, [CallerMemberName] string callerName = "", [CallerFilePath] string filePath = "", [CallerLineNumber] int callerLineNumber = 0) => ArgListNotNull(new object[] { arg0, arg1, arg2, arg3, arg4, arg5 }, new[] { typeof(T0), typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5) }, callerName, filePath, callerLineNumber);

        public static void Requires<T>(RequireContract<T> arg, [CallerMemberName] string callerName = "", [CallerFilePath] string filePath = "", [CallerLineNumber] int callerLineNumber = 0)
        {
            Requires(arg, arg.Name ?? "params[0]", callerName, filePath, callerLineNumber);
        }

        public static void Requires<T0, T1>(RequireContract<T0> arg0, RequireContract<T1> arg1, [CallerMemberName] string callerName = "", [CallerFilePath] string filePath = "", [CallerLineNumber] int callerLineNumber = 0)
        {
            Requires(arg0, arg0.Name ?? "params[0]", callerName, filePath, callerLineNumber);
            Requires(arg1, arg1.Name ?? "params[1]", callerName, filePath, callerLineNumber);
        }

        public static void Requires<T0, T1, T2>(RequireContract<T0> arg0, RequireContract<T1> arg1, RequireContract<T2> arg2, [CallerMemberName] string callerName = "",
            [CallerFilePath] string filePath = "", [CallerLineNumber] int callerLineNumber = 0)
        {
            Requires(arg0, arg0.Name ?? "params[0]", callerName, filePath, callerLineNumber);
            Requires(arg1, arg1.Name ?? "params[1]", callerName, filePath, callerLineNumber);
            Requires(arg2, arg2.Name ?? "params[2]", callerName, filePath, callerLineNumber);
        }
    }

    public static class ContractExtensions
    {
        public static RequireContract<T> NotNull<T>(this T obj, string name = null)
        {
            var r = new RequireContract<T>(obj, name);
            r.Conditions.Add(new RequireCondition<T>(o => o != default, $"{name} is null", typeof(ArgumentNullException)));
            return r;
        }

        public static RequireContract<int> InRange(this int obj, Func<int> min, Func<int> max, string name = null)
        {
            return new RequireContract<int>(obj, name).InRange(min, max, name);
        }

        public static RequireContract<int> InRange(this RequireContract<int> r, Func<int> min, Func<int> max, string name = null)
        {
            r.Conditions.Add(new RequireCondition<int>(o => min() <= o && o < max(), $"{name} not in range {min()}<{name ?? "x"}<{max()}", typeof(ArgumentOutOfRangeException)));
            return r;
        }

        public static RequireContract<int> InRange(this int obj, int min, int max, string name = null)
        {
            return new RequireContract<int>(obj, name).InRange(min, max, name);
        }

        public static RequireContract<int> InRange(this RequireContract<int> r, int min, int max, string name = null)
        {
            r.Conditions.Add(new RequireCondition<int>(o => min <= o && o < max, $"{name} not in range {min}<{name ?? "x"}<{max}", typeof(ArgumentOutOfRangeException)));
            return r;
        }

        public static RequireContract<bool> IsTrue(this bool obj, string name = null)
        {
            return new RequireContract<bool>(obj, name).IsTrue(o => o, name);
        }

        public static RequireContract<T> IsTrue<T>(this T obj, Predicate<T> pred, string name = null)
        {
            return new RequireContract<T>(obj, name).IsTrue(pred, name);
        }

        public static RequireContract<T> IsTrue<T>(this RequireContract<T> r, Predicate<T> pred, string name = null)
        {
            r.Conditions.Add(new RequireCondition<T>(pred, $"{name} IsTrue condition failed"));
            return r;
        }
    }

    public class RequireContract<T>
    {
        public RequireContract(T arg, string name = "")
        {
            Arg = arg;
            Name = name;
        }

        public string Name { get; }
        public T Arg { get; }
        public List<RequireCondition<T>> Conditions { get; } = new List<RequireCondition<T>>();
    }

    public class RequireCondition<T>
    {
        public RequireCondition(Predicate<T> condition, string errorMessage = null, Type exceptionType = null)
        {
            Condition = condition;
            ErrorMessage = errorMessage;
            ExceptionType = exceptionType;
        }

        public Predicate<T> Condition { get; }
        public string ErrorMessage { get; }
        public Type ExceptionType { get; }
    }

    public class ContractsException : Exception
    {
        public ContractsException(string msg) : base(msg) { }
    }
}
