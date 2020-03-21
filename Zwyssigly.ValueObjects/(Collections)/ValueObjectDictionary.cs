using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Zwyssigly.ValueObjects
{
    public class ValueObjectDictionary<TKey, TValue> : AbstractValueObject<ValueObjectDictionary<TKey, TValue>>, IReadOnlyDictionary<TKey, TValue>
    {
        private readonly Dictionary<TKey, TValue> _dictionary;

        public static ValueObjectDictionary<TKey, TValue> Empty = new ValueObjectDictionary<TKey, TValue>();

        public int Count => _dictionary.Count;

        public IEnumerable<TKey> Keys => _dictionary.Keys;

        public IEnumerable<TValue> Values => _dictionary.Values;

        public TValue this[TKey key] => _dictionary[key];

        public ValueObjectDictionary(IEnumerable<KeyValuePair<TKey, TValue>> items)
        {
            _dictionary = items.ToDictionary(i => i.Key, i => i.Value);
        }

        public ValueObjectDictionary(params KeyValuePair<TKey, TValue>[] items)
            : this((IEnumerable<KeyValuePair<TKey, TValue>>) items)
        { }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator() => _dictionary.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => _dictionary.GetEnumerator();

        public bool ContainsKey(TKey key) => _dictionary.ContainsKey(key);

        public override int GetHashCode()
        {
            unchecked
            {
                return _dictionary.Aggregate(0, (hashcode, item) => hashcode ^ item.GetHashCode());
            }
        }

        public override string ToString() => GetType().Name;

        protected override bool EqualsImpl(ValueObjectDictionary<TKey, TValue> other)
        {
            if (_dictionary.Count != other._dictionary.Count)
                return false;

            return _dictionary.All(kv => other._dictionary.TryGetValue(kv.Key, out var value) && Equals(kv.Value, value));
        }

        public bool TryGetValue(TKey key, out TValue value) => _dictionary.TryGetValue(key, out value);
    }

    public static class ValueObjectDictionaryExtensions
    {
        public static ValueObjectDictionary<TKey, T> ToValueObjectDictionary<TKey, T>(this IEnumerable<T> self, Func<T, TKey> selector)
        {
            return new ValueObjectDictionary<TKey, T>(self.Select(s => new KeyValuePair<TKey, T>(selector(s), s)));
        }

        public static ValueObjectDictionary<TKey, TValue> ToValueObjectDictionary<TKey, TValue, T>(
            this IEnumerable<T> self, Func<T, TKey> keySelector, Func<T, TValue> valueSelector)
        {
            return new ValueObjectDictionary<TKey, TValue>(self.Select(s => new KeyValuePair<TKey, TValue>(keySelector(s), valueSelector(s))));
        }
    }
}
