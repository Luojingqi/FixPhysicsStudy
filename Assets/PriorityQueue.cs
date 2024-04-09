using System;
using System.Collections;
using System.Collections.Generic;

namespace DataStructure
{
    /// <summary>
    /// 优先队列 最小堆
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PriorityQueue<T> : IEnumerable<T>
    {
        private List<T> dataList = new List<T>();
        private List<float> costList = new List<float>();
        public int Count { get { return dataList.Count; } }


        public void Enqueue(T item, float cost)
        {
            int nowIndex = dataList.Count;
            dataList.Add(item);
            costList.Add(cost);
            while (nowIndex > 0)
            {
                int parentIndex = (nowIndex - 1) / 2;
                if (costList[parentIndex] > costList[nowIndex])
                {
                    Swap(parentIndex, nowIndex);
                    nowIndex = parentIndex;
                }
                else break;
            }
        }

        public T Dequeue()
        {
            if (Count <= 0) throw new InvalidOperationException("队列为空");

            int last = Count - 1;
            T ret = dataList[0];
            Swap(0, last);
            dataList.RemoveAt(last);
            costList.RemoveAt(last);

            int nowIndex = 0;
            while (nowIndex < last)
            {
                int L = nowIndex * 2 + 1;
                if (L >= last) break;
                int R = L + 1;

                //判断左右谁更小，如果右不存在直接使用左
                int next = R >= last || costList[L] < costList[R] ? L : R;
                if (costList[nowIndex] > costList[next])
                {
                    Swap(next, nowIndex);
                    nowIndex = next;
                }
                else break;
            }
            return ret;
        }

        public T Peek()
        {
            if (Count <= 0) throw new InvalidOperationException("队列为空");
            return dataList[0];
        }

        public bool Contains(T data)
        {
            return dataList.Contains(data);
        }

        public void Clear()
        {
            dataList.Clear();
            costList.Clear();
        }

        IEnumerator<T> GetEnumerator()
        {
            return dataList.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            return GetEnumerator();
        }

        private void Swap(int a, int b)
        {
            T temp = dataList[a];
            dataList[a] = dataList[b];
            dataList[b] = temp;
            float cost = costList[a];
            costList[a] = costList[b];
            costList[b] = cost;
        }
    }
}
