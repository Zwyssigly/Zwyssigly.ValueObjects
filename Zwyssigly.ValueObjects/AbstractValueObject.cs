using System;

namespace Zwyssigly.ValueObjects
{
    public abstract class AbstractValueObject<T> : IEquatable<T>
        where T : AbstractValueObject<T>
    {
        public bool Equals(T other)
        {
            if (other is null)
                return false;

            if (ReferenceEquals(other, this))
                return true;

            return EqualsImpl(other);
        }

        public sealed override bool Equals(object obj)
        {
            return Equals(obj as T);
        }

        protected abstract bool EqualsImpl(T other);

        public abstract override int GetHashCode();

        public abstract override string ToString();

        public static bool operator ==(AbstractValueObject<T> a, AbstractValueObject<T> b)
        {
            if (a is null)
                return b is null;

            return a.Equals(b);
        }

        public static bool operator !=(AbstractValueObject<T> a, AbstractValueObject<T> b)
        {
            if (a is null)
                return b is object;

            return !a.Equals(b);
        }
    }
}
