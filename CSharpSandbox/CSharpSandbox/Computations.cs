using System;
using System.Collections.Generic;
using NFluent;
using NUnit.Framework;

namespace CSharpSandbox
{
    public abstract class Maybe<T>
    {
        public abstract Maybe<TResult> Map<TResult>(
            Func<None<T>, Maybe<TResult>> none,
            Func<Just<T>, Maybe<TResult>> just);

        public abstract Maybe<TResult> Bind<TResult>(
            Func<T, Maybe<TResult>> binding);

        public static bool operator ==(Maybe<T> left, Maybe<T> rigth) => Equals(left, rigth);

        public static bool operator !=(Maybe<T> left, Maybe<T> rigth) => !(left == rigth);
    }

    public sealed class Just<T> : Maybe<T>, IEquatable<Just<T>>
    {
        public T Value { get; }

        public Just(T value)
        {
            Value = value;
        }

        public override Maybe<TResult> Map<TResult>(
            Func<None<T>, Maybe<TResult>> none,
            Func<Just<T>, Maybe<TResult>> just)
            => just(this);

        public override Maybe<TResult> Bind<TResult>(
            Func<T, Maybe<TResult>> binding)
            => binding(Value);

        public override string ToString() => $"Just {Value}";

        #region IEquatable pattern

        public bool Equals(Just<T> other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return EqualityComparer<T>.Default.Equals(Value, other.Value);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj is Just<T> && Equals((Just<T>)obj);
        }

        public override int GetHashCode()
        {
            return EqualityComparer<T>.Default.GetHashCode(Value);
        }

        #endregion
    }

    public sealed class None<T> : Maybe<T>, IEquatable<None<T>>
    {
        public override Maybe<TResult> Map<TResult>(
            Func<None<T>, Maybe<TResult>> none,
            Func<Just<T>, Maybe<TResult>> just)
            => none(this);

        public override Maybe<TResult> Bind<TResult>(
            Func<T, Maybe<TResult>> binding)
            => new None<TResult>();

        public override string ToString() => "None";

        #region IEquatable pattern

        public bool Equals(None<T> other) => true;

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj is None<T> && Equals((None<T>)obj);
        }

        public override int GetHashCode() => 0; 

        #endregion
    }

    public static class MaybeExtensions
    {
        public static Maybe<TResult> Select<T, TResult>(
            this Maybe<T> maybe,
            Func<T, TResult> selector)
        {
            return maybe.Bind(just => new Just<TResult>(selector(just)));
        }

        public static Maybe<TResult> SelectMany<T, TValue, TResult>(
            this Maybe<T> source,
            Func<T, Maybe<TValue>> valueSelector,
            Func<T, TValue, TResult> resultSelector)
        {
            return source.Bind(sourceValue =>
                valueSelector(sourceValue).Bind(just =>
                    new Just<TResult>(resultSelector(sourceValue, just))));
        }
    }

    public class Computation
    {
        public static Maybe<int> AddLinq(Maybe<int> maybeX, Maybe<int> maybeY)
        {
            return
                from x in maybeX
                from y in maybeY
                select x + y;
        }

        public static Maybe<int> AddFluent(Maybe<int> maybeX, Maybe<int> maybeY)
        {
            return maybeX.Bind(justX =>
                        maybeY.Bind(justY => 
                            new Just<int>(justX + justY)));
        }

        public static Maybe<int> AddExtensions(Maybe<int> maybeX, Maybe<int> maybeY)
        {
            return maybeX.SelectMany(x => maybeY, (x, y) => x + y);
        }

        public static Maybe<string> ToStringLinq(Maybe<int> maybe)
        {
            return
                from value in maybe
                select value.ToString();
        }

        public static Maybe<string> ToStringFluent(Maybe<int> maybe)
        {
            return maybe.Bind(value => new Just<string>(value.ToString()));
        }

        public static Maybe<string> ToStringExtensions(Maybe<int> maybe)
        {
            return maybe.Select(value => value.ToString());
        }
    }

    [TestFixture]
    public class ComputationsTests
    {
        private static readonly (Maybe<int> maybeX, Maybe<int> maybeY, Maybe<int> result)[] AdditionTestCases = {
            (new Just<int>(5), new Just<int>(4), new Just<int>(9)),
            (new Just<int>(5), new None<int>(), new None<int>()),
            (new None<int>(), new Just<int>(5), new None<int>()),
            (new None<int>(), new None<int>(), new None<int>())
        };

        [Test]
        [TestCaseSource(nameof(AdditionTestCases))]
        public void Addition((Maybe<int> maybeX, Maybe<int> maybeY, Maybe<int> result) testCase)
        {
            Check
                .That(Computation.AddLinq(testCase.maybeX, testCase.maybeY))
                .IsEqualTo(Computation.AddFluent(testCase.maybeX, testCase.maybeY))
                .And.IsEqualTo(Computation.AddExtensions(testCase.maybeX, testCase.maybeY))
                .And.IsEqualTo(testCase.result);
        }


        private static readonly (Maybe<int> input, Maybe<string> output)[] ToStringTestCases = {
            (new Just<int>(5), new Just<string>("5")),
            (new None<int>(),new None<string>())
        };

        [Test]
        [TestCaseSource(nameof(ToStringTestCases))]
        public void ToString((Maybe<int> input, Maybe<string> output) testCase)
        {
            Check
                .That(Computation.ToStringLinq(testCase.input))
                .IsEqualTo(Computation.ToStringFluent(testCase.input))
                .And.IsEqualTo(Computation.ToStringExtensions(testCase.input))
                .And.IsEqualTo(testCase.output);
        }
    }
}
