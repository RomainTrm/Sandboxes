using System;

namespace CSharpSandbox
{
    public interface Union<TBase, T1, T2, T3>
        where TBase : class
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
            where TBase : class
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
                    var errorMessage = $"Union value should be of type \"{typeof(T1).Name}\", \"{typeof(T2).Name}\" or \"{typeof(T3).Name}\"";
                    throw new InvalidOperationException(errorMessage);
            }
        }
    }

    public class Example
    {
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


        public static string PresentPet(Pet pet)
            => pet.Match(
                cat => $"It's a cat with {cat.RemainingLifes} life(s) left.",
                dog => "It's a dog.",
                bird => $"It's a bird, {(bird.CanFly ? string.Empty : "un")}fortunately, he can{(bird.CanFly ? string.Empty : "'t")} fly.");
    }
}
