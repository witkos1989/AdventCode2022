namespace AdventCode2022.DataStructures;

public class MinHeap<T> : IMinHeap<T> where T : IComparable<T>
{
    public int Length { get; private set; }
    private IList<T> List;
    private readonly IComparer<T> comparer;

    public MinHeap(IComparer<T>? customComparer = null)
    {
        Length = 0;

        List = new List<T>();

        if (customComparer is null)
        {
            comparer = Comparer<T>.Default;
        }
        else
        {
            comparer = customComparer;
        }
    }

    public ICollection<T> Print()
    {
        return List;
    }

    public IMinHeap<T> Insert(ICollection<T> list)
    {
        foreach (var item in list)
        {
            Insert(item);
        }

        return this;
    }

    public IMinHeap<T> Insert(T value)
    {
        List.Add(value);

        HeapifyUp(Length);

        Length++;

        return this;
    }

    public T? GetItem(T value)
    {
        if (Contains(value))
        {
            int index = List.IndexOf(value);

            return List[index];
        }

        return default;
    }

    public bool Contains(T value) =>
        List.Contains(value);

    public IMinHeap<T> Update(T oldValue, T newValue)
    {
        if (List.Count == 0 || !List.Contains(oldValue))
        {
            return this;
        }

        int index = List.IndexOf(oldValue);

        List[index] = newValue;

        if (comparer.Compare(newValue, oldValue) <= 0)
        {
            HeapifyUp(index);
        }
        else
        {
            HeapifyDown(index);
        }

        return this;
    }

    public T? Delete()
    {
        T? current = List[0];

        Length--;

        if (Length == 0)
        {
            List = new List<T>();

            return current;
        }

        List[0] = List[Length];

        List.RemoveAt(Length);

        HeapifyDown(0);

        return current;
    }

    private void HeapifyDown(int index)
    {
        int leftChildIndex = LeftChild(index);
        int rightChildIndex = RightChild(index);

        if (index >= Length || leftChildIndex >= Length)
        {
            return;
        }

        T leftChildValue = List[leftChildIndex];
        T value = List[index];

        if (rightChildIndex >= Length)
        {
            if (comparer.Compare(value, leftChildValue) > 0)
            {
                (List[index], List[leftChildIndex]) = (leftChildValue, value);
            }

            return;
        }

        T rightChildValue = List[rightChildIndex];

        if (comparer.Compare(leftChildValue, rightChildValue) > 0 && comparer.Compare(value, rightChildValue) > 0)
        {
            (List[index], List[rightChildIndex]) = (rightChildValue, value);

            HeapifyDown(rightChildIndex);
        }
        else if (comparer.Compare(rightChildValue, leftChildValue) > 0 && comparer.Compare(value, leftChildValue) > 0)
        {
            (List[index], List[leftChildIndex]) = (leftChildValue, value);

            HeapifyDown(leftChildIndex);
        }
    }

    private void HeapifyUp(int index)
    {
        if (index == 0)
        {
            return;
        }

        int parentIndex = Parent(index);
        T parentValue = List[parentIndex];
        T value = List[index];

        if (comparer.Compare(parentValue, value) > 0)
        {
            (List[index], List[parentIndex]) = (parentValue, value);

            HeapifyUp(parentIndex);
        }
    }

    private static int Parent(int index) =>
        (int)Math.Floor((float)(index - 1) / 2);

    private static int LeftChild(int index) =>
        index * 2 + 1;

    private static int RightChild(int index) =>
        index * 2 + 2;
}