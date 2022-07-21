using System;

namespace Yavuz.PathFind
{
    public class Heap<T> where T : IHeapItem<T>
    {
        T[] items;
        int currentItemCount;

        public Heap(int maxHeapSize)
        {
            items = new T[maxHeapSize];

        }
        public void Add(T item)
        {
            item.HeapIndex = currentItemCount;
            items[currentItemCount] = item;
            SortUp(item);
            currentItemCount++;
        }
        void SortUp(T item)
        {
            int parentIndex = (item.HeapIndex - 1) / 2;

            while (true)
            {
                T parentItem = items[parentIndex];
                if (item.CompareTo(parentItem) > 0)
                {
                    Swap(item, parentItem);
                }
                else
                    break;
            }

        }
        void Swap(T a, T b)
        {
            items[a.HeapIndex] = b;
            items[b.HeapIndex] = a;
            int itemAndex = a.HeapIndex;
            a.HeapIndex = b.HeapIndex;
            b.HeapIndex = itemAndex;
        }
        public T RemoveFirst()
        {
            T firstItem = items[0];
            currentItemCount--;
            items[0] = items[currentItemCount];
            items[0].HeapIndex = 0;
            SortDowm(items[0]);
            return firstItem;
        }

        private void SortDowm(T t)
        {
            while (true)
            {
                int chidLeft = t.HeapIndex * 2 + 1;
                int chidRight = t.HeapIndex * 2 + 2;

                int swapIndex = 0;
                if (chidLeft < currentItemCount)
                {
                    swapIndex = chidLeft;

                    if (chidRight < currentItemCount)
                    {
                        if (items[chidLeft].CompareTo(items[chidRight]) < 0)
                        {
                            swapIndex = chidRight;
                        }
                    }
                    if (t.CompareTo(items[swapIndex]) < 0)
                    {
                        Swap(t, items[swapIndex]);
                    }
                    else
                        return;
                }
                else
                    return;

            }
        }
        public void UpdateItem(T item)
        {
            SortUp(item);
        }
        public bool Contains(T item)
        {
            return Equals(items[item.HeapIndex], item);
        }
        public int Count
        {
            get
            {
                return currentItemCount;
            }
        }
    }
    public interface IHeapItem<T> : IComparable<T>
    {
        int HeapIndex
        {
            get;
            set;
        }
    }

}