namespace AdventCode2022.DataStructures;

public interface IMinHeap<T> where T : IComparable<T>
{
    int Length { get; }

    ICollection<T> Print();

    IMinHeap<T> Insert(ICollection<T> list);

    IMinHeap<T> Insert(T value);

    T? GetItem(T value);

    bool Contains(T value);

    IMinHeap<T> Update(T oldValue, T newValue);

    T? Delete();
}