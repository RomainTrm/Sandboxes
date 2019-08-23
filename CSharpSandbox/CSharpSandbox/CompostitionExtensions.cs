using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
// ReSharper disable MemberCanBePrivate.Global

namespace CSharpSandbox
{
    public static class TaskExtensions
    {
        public static async Task<TResult> Select<TValue, TResult>(
            this Task<TValue> task,
            Func<TValue, TResult> selector)
        {
            var value = await task;
            return selector(value);
        }

        public static async Task<TResult> SelectMany<T, TValue, TResult>(
            this Task<T> task,
            Func<T, Task<TValue>> valueSelector,
            Func<T, TValue, TResult> resultSelector)
        {
            var t = await task;
            var value = await valueSelector(t);
            return resultSelector(t, value);
        }
    }

    public static class LazyExtentions
    {
        public static Lazy<TResult> Select<TValue, TResult>(
            this Lazy<TValue> lazy,
            Func<TValue, TResult> selector)
        {
            return new Lazy<TResult>(() => selector(lazy.Value));
        }

        public static Lazy<TResult> SelectMany<T, TValue, TResult>(
            this Lazy<T> lazy,
            Func<T, Lazy<TValue>> valueSelector,
            Func<T, TValue, TResult> resultSelector)
        {
            return new Lazy<TResult>(() =>
            {
                var l = lazy.Value;
                var value = valueSelector(l).Value;
                return resultSelector(l, value);
            });
        }
    }


    public static class MaybeExtensions
    {
        public static Maybe<TResult> Select<TValue, TResult>(
            this Maybe<TValue> maybe,
            Func<TValue, TResult> selector)
        {
            return maybe.Match(
                none => new None<TResult>(),
                just => new Just<TResult>(selector(just.Value)));
        }

        public static Maybe<TResult> SelectMany<T, TValue, TResult>(
            this Maybe<T> maybe,
            Func<T, Maybe<TValue>> valueSelector,
            Func<T, TValue, TResult> resultSelector)
        {
            return maybe.Match(
                none => new None<TResult>(),
                just => valueSelector(just.Value).Match(
                    none => new None<TResult>(),
                    justValue =>
                    {
                        var result = resultSelector(just.Value, justValue.Value);
                        return new Just<TResult>(result);
                    }));
        }
    }

    public static class EnumerableSandbox
    {
        private static readonly IEnumerable<int> EnumerableInt = Enumerable.Empty<int>();

        public IEnumerable<string> EnumerableString => EnumerableInt.Select(i => i.ToString());
        public IEnumerable<string> EnumerableString2 => EnumerableInt.SelectMany(i => new[]{i}, (i, collection) => i.ToString());


        public static IEnumerable<TResult> Select<TSource, TResult>(
            this IEnumerable<TSource> source,
            Func<TSource, TResult> selector)



        {

            return Enumerable.Select(source, selector);
        }


        public static IEnumerable<TResult> SelectMany<T, TSource, TResult>(
            this IEnumerable<T> enumerable,
            Func<T, IEnumerable<TSource>> sourceSelector,
            Func<T, TSource, TResult> resultSelector)
        {
            return Enumerable.SelectMany(enumerable, sourceSelector, resultSelector);
        }

    }

    public static class ComputationsSandbox
    {


        // IEnumerable<TResult> SelectMany<T, TSource, TResult>(this IEnumerable<T>, Func<T, IEnumerable<TSource>>, Func<T, TSource, TResult>)
        public static IEnumerable<int> Add(IEnumerable<int> enumerableX, IEnumerable<int> enumerableY)
        {
            return
                from x in enumerableX
                from y in enumerableY
                select x + y;
        }
        
        // Maybe<TResult> SelectMany<T, TValue, TResult>(this Maybe<T>, Func<T, Maybe<TValue>>, Func<T, TValue, TResult>)
        public static Maybe<int> Add(Maybe<int> maybeX, Maybe<int> maybeY)
        {
            return
                from x in maybeX
                from y in maybeY
                select x + y;
        }

    }
}
