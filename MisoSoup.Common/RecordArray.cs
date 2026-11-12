using System.Collections;
using System.Runtime.CompilerServices;

namespace MisoSoup.Common;

[CollectionBuilder(typeof(RecordArrayBuilder), nameof(RecordArrayBuilder.Create))]
public class RecordArray<T>(params ReadOnlySpan<T> array) : IEnumerable<T>
{
    private readonly T[] _array = [.. array];

    public IEnumerator<T> GetEnumerator() => ((IEnumerable<T>)_array).GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public T this[int index] => _array[index];

    public override bool Equals(object? obj) =>
        obj is RecordArray<T> other && (ReferenceEquals(this, other) || other.SequenceEqual(_array));

    public override int GetHashCode()
    {
        var hash = new HashCode();
        foreach (var item in _array) 
            hash.Add(item);
        return hash.ToHashCode();
    }
}

public static class RecordArrayBuilder
{
    public static RecordArray<T> Create<T>(ReadOnlySpan<T> array) =>
        new(array);
}