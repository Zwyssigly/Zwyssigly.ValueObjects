namespace Zwyssigly.ValueObjects
{
    public abstract class SimpleValueObject<T> : AbstractValueObject<T> 
        where T : SimpleValueObject<T>
    {
        protected abstract object GetEqualityFields();

        public sealed override int GetHashCode()
            => GetEqualityFields().GetHashCode();

        protected sealed override bool EqualsImpl(T other)
            => GetEqualityFields().Equals(other.GetEqualityFields());

    }
}
