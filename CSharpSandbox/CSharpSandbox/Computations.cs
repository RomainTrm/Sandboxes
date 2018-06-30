﻿using System;
using System.Collections.Generic;

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
        public static Maybe<int> Add(Maybe<int> maybeX, Maybe<int> maybeY)
        {
            return
                from x in maybeX
                from y in maybeY
                select x + y;
        }

        public static Maybe<int> AddFluent(Maybe<int> maybeX, Maybe<int> maybeY)
        {
            return maybeX.Bind(justX =>
                        maybeY.Bind(
                            justY => new Just<int>(justX + justY)));
        }
    }
}
