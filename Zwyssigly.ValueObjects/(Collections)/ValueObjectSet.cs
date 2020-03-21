using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Zwyssigly.ValueObjects
{
    public class ValueObjectSet<T> : AbstractValueObject<ValueObjectSet<T>>, IReadOnlyCollection<T>
    {
        private readonly HashSet<T> _set;

        public static ValueObjectSet<T> Empty = new ValueObjectSet<T>();

        public int Count => _set.Count;

        public ValueObjectSet(IEnumerable<T> items)
        {
            _set = new HashSet<T>(items);
        }

        public ValueObjectSet(params T[] items) 
            : this((IEnumerable<T>) items) 
        { }

        public IEnumerator<T> GetEnumerator() => _set.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => _set.GetEnumerator();

        public bool Contains(T item) => _set.Contains(item);

        public override int GetHashCode()
        {
            unchecked
            {
                return _set.Aggregate(0, (hashcode, item) => hashcode ^ item.GetHashCode());
            }
        }

        public override string ToString() => GetType().Name;

        protected override bool EqualsImpl(ValueObjectSet<T> other)
        {
            return _set.Count == other._set.Count
                && _set.All(item => other._set.Contains(item));
        }
    }

    public static class ValueObjectSetExtensions
    {
        public static ValueObjectSet<T> ToValueObjectSet<T>(this IEnumerable<T> self)
        {
            return new ValueObjectSet<T>(self);
        } 
    }
}
