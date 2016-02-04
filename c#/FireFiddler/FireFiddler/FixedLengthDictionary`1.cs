using System;
using System.Collections.Generic;
using System.Linq;

namespace FireFiddler
{
    /// <summary>
    /// 固定长度的字典
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    public class FixedLengthDictionary<TKey, TValue> : Dictionary<TKey, TValue>
    {
        private Queue<TKey> mQueue;
        private int mLength;

        public FixedLengthDictionary(int length)
            : base(length)
        {
            mLength = length;
            mQueue = new Queue<TKey>(length);
        }

        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public new void Add(TKey key, TValue value)
        {
            if (mQueue.Count < mLength)
            {
                mQueue.Enqueue(key);
                base.Add(key, value);
            }
            else
            {
                TKey frist = mQueue.Dequeue(); //先移除
                base.Remove(frist);

                mQueue.Enqueue(key); //再添加
                base.Add(key, value);
            }
        }

        public new TValue this[TKey key]
        {
            get
            {
                return base[key];
            }
            set
            {
                this.Add(key, value);
            }
        }
    }
}
