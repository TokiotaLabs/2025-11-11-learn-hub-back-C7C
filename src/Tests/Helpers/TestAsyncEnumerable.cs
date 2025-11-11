using System.Linq.Expressions;

namespace LearnHub.Back.Tests.Helpers;

public class TestAsyncEnumerable<T> : IAsyncEnumerable<T>, IQueryable<T>
{
    private readonly IEnumerable<T> _enumerable;

    public TestAsyncEnumerable(IEnumerable<T> enumerable)
    {
        _enumerable = enumerable;
    }

    public IAsyncEnumerator<T> GetAsyncEnumerator(CancellationToken cancellationToken = default)
    {
        return new TestAsyncEnumerator<T>(_enumerable.GetEnumerator());
    }

    public IEnumerator<T> GetEnumerator()
    {
        return _enumerable.GetEnumerator();
    }

    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
    {
        return _enumerable.GetEnumerator();
    }

    public Type ElementType => typeof(T);

    public Expression Expression => _enumerable.AsQueryable().Expression;

    public IQueryProvider Provider => new TestAsyncQueryProvider<T>(_enumerable.AsQueryable().Provider);
}