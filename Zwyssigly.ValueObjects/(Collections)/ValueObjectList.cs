using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zwyssigly.ValueObjects
{
    public class ValueObjectList<T> : AbstractValueObject<ValueObjectList<T>>, IReadOnlyList<T>
    {
        private IReadOnlyList<T> _inner;

        public static ValueObjectList<T> Empty = new ValueObjectList<T>();

        public ValueObjectList(IEnumerable<T> items)
        {
            _inner = new List<T>(items);
        }

        public ValueObjectList(params T[] items)
            : this((IEnumerable<T>)items)
        { }


        public T this[int index] => _inner[index];
        public int Count => _inner.Count;

        public IEnumerator<T> GetEnumerator() => _inner.GetEnumerator();

        public override int GetHashCode()
        {
            unchecked
            {
                var hash = 17;
                foreach (var item in _inner)
                    hash = hash * 23 + item.GetHashCode();
                return hash;
            }
        }

        public override string ToString() => GetType().Name;

        protected override bool EqualsImpl(ValueObjectList<T> other)
        {
            if (Count != other.Count)
                return false;

            return this.Zip(other, (a, b) => Equals(a, b)).All(a => a);
        }

        IEnumerator IEnumerable.GetEnumerator() => _inner.GetEnumerator();
    }

    public static class ValueObjectListExtensions
    {
        public static ValueObjectList<T> ToValueObjectList<T>(this IEnumerable<T> self)
        {
            return new ValueObjectList<T>(self);
        }
    }
}
