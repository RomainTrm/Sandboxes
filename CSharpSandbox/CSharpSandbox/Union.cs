using System;
using System.Collections.Generic;
// ReSharper disable FieldCanBeMadeReadOnly.Local
// ReSharper disable ConvertClosureToMethodGroup
// ReSharper disable ArrangeTypeMemberModifiers

// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable InconsistentNaming

namespace CSharpSandbox
{

    public interface Union<TBase, T1, T2, T3>
        where T1 : TBase
        where T2 : TBase
        where T3 : TBase
    { }

    public static class UnionExtensions
    {
        public static TResult Match<TBase, T1, T2, T3, TResult>(
            this Union<TBase, T1, T2, T3> union,
            Func<T1, TResult> caseT1,
            Func<T2, TResult> caseT2,
            Func<T3, TResult> caseT3)
            where T1 : TBase
            where T2 : TBase
            where T3 : TBase
        {
            switch (union)
            {
                case T1 t1: return caseT1(t1);
                case T2 t2: return caseT2(t2);
                case T3 t3: return caseT3(t3);
                default:
                    var errorMessage = $"Union value should be of type " +
                                       $"\"{typeof(T1).Name}\", \"{typeof(T2).Name}\" " +
                                       $"or \"{typeof(T3).Name}\"";
                    throw new InvalidOperationException(errorMessage);
            }
        }
    }

    public class Example
    {

        public class Eggs { }

        public abstract class Pet : Union<Pet, Cat, Dog, Bird> { }

        public sealed class Cat : Pet
        {
            public int RemainingLifes { get; }

            public Cat(int remainingLifes)
            {
                RemainingLifes = remainingLifes;
            }
        }

        public sealed class Dog : Pet { }

        public sealed class Bird : Pet
        {
            public bool CanFly { get; }

            public Bird(bool canFly)
            {
                CanFly = canFly;
            }
        }


        //public static Maybe<string> PresentPet(Maybe<Pet> maybePet)
        //    => maybePet.Match(
        //        none => new None<string>(),
        //        pet => new Just<string>(PresentPet(pet.Value)));

        //private static readonly Maybe<Pet> maybePet = new Just<Pet>(new Bird(true));

        Maybe<string> maybePresentation = maybePet.Select(pet => PresentPet(pet));

        static string PresentPet(Pet pet)
            => pet.Match(
                cat => $"It's a cat with {cat.RemainingLifes} life(s) left.",
                dog => "It's a dog.",
                bird => $"It's a bird, {(bird.CanFly ? string.Empty : "un")}fortunately, " +
                        $"he can{(bird.CanFly ? string.Empty : "'t")} fly.");


        Maybe<Eggs> maybeEggss = //...maybePet.Select(pet => CollectEggs(pet));

        static Maybe<Eggs> CollectEggs(Pet pet)
        {
            var t = pet;
            return new Just<Eggs>(new Eggs());
        }




        // IEnumerable<TResult> Select<TSource, TResult>(this IEnumerable<TSource>, Func<TSource, TResult>)
        public static IEnumerable<string> PresentPet(IEnumerable<Pet> pets)
        {
            return
                from pet in pets
                select PresentPet(pet);
        }

        // Maybe<TResult> Select<TValue, TResult>(this Maybe<TValue>, Func<TValue, TResult>)
        public static Maybe<string> PresentPet(Maybe<Pet> maybePet)
        {
            return
                from pet in maybePet
                select PresentPet(pet);
        }


        // IEnumerable<TResult> SelectMany<T, TSource, TResult>(this IEnumerable<T>, Func<T, IEnumerable<TSource>>, Func<T, TSource, TResult>)
        public static IEnumerable<int> Add(IEnumerable<IEnumerable<int>> collections)
        {
            return
                from collection in collections
                from item in collection
                select item;
        }

        // Maybe<TResult> SelectMany<T, TValue, TResult>(this Maybe<T>, Func<T, Maybe<TValue>>, Func<T, TValue, TResult>)
        public static Maybe<Eggs> Add(Maybe<Pet> maybePet)
        {
            return
                from pet in maybePet
                from eggs in CollectEggs(pet)
                select eggs;
        }

    }
}
