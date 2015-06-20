using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Text;

namespace algorithm
{
    class Program
    {
        static void Main(string[] args)
        {
            var container1 = new Container("C1", new string[] { "obj1", "obj3", "obj4", "obj5" });
            var container2 = new Container("C2", new string[] { "obj1", "obj4", "obj3", "obj4", "obj6" });
            var container3 = new Container("C3", new string[] { "obj1", "obj2", "obj4", "obj6", "obj5", "obj8" });
            var container4 = new Container("C4", new string[] { "obj1", "obj2", "obj4", "obj6", "obj2", "obj8" });

            container1.Stuff();
            container2.Stuff();
            container3.Stuff();
            container4.Stuff();
            var ContainerList = new ContainerCollections(new Container[] { container1, container2, container3, container4 });

            Stopwatch watch = new Stopwatch();
            watch.Start();

            while (!ContainerList.IsEnd)
            {
                foreach (var item in ContainerList.FindKeyContainerPair())
                {
                    foreach (var item1 in item.Value.Pop(item.Key))
                    {
                        Console.WriteLine("{0}->{1}", item.Value.Name, item1.Value);
                    }
                }
            }
            watch.Stop();
            Console.WriteLine("总运行时间：{0}(ms)", watch.ElapsedMilliseconds);
            Console.Read();
        }
    }

    /// <summary>
    /// 装入容器的项
    /// </summary>
    public class Item
    {
        public Item(string val)
        {
            Value = val;
            IsExport = false;
        }

        /// <summary>
        /// 值
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// 是否导出
        /// </summary>
        public bool IsExport { get; set; }
    }

    /// <summary>
    /// 装入容器的项 的比较器
    /// </summary>
    public class ItemComparer : IComparer<Item>
    {
        public int Compare(Item x, Item y)
        {
            if (x != null && y != null)
            {
                return StringComparer.Ordinal.Compare(x.Value, y.Value);
            }
            if (x == null && y == null) return 0;
            if (x == null) return -1;
            return 1;
        }
    }
    /// <summary>
    /// 工具类
    /// </summary>
    public static class Util
    {
        /// <summary>
        /// 求枚举类型的数量
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        public static int Count<T>(IEnumerable<T> list)
        {
            int i = 0;
            IEnumerator<T> enumerator = list.GetEnumerator();
            while (enumerator.MoveNext())
            {
                i++;
            }
            return i;
        }
        /// <summary>
        /// 列表中是否有满足条件的数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public static bool Any<T>(IEnumerable<T> list, Predicate<T> predicate)
        {
            foreach (var item in list)
            {
                if (predicate(item))
                    return true;
            }
            return false;
        }
    }

    /// <summary>
    /// 容器
    /// </summary>
    public class Container
    {
        private List<Item> _data = new List<Item>(10);
        /// <summary>
        /// 是否填充完毕
        /// </summary>
        private bool IsStuff = false;
        /// <summary>
        /// 容器名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 比较器
        /// </summary>
        private IComparer<Item> comparer = null;
        
        public Container(string name, IEnumerable<string> data)
            : this(name, data, new ItemComparer())
        { }

        public Container(string name, IEnumerable<string> data, IComparer<Item> comp)
        {
            if (data != null)
                foreach (var item in data)
                {
                    _data.Add(new Item(item));
                }

            comparer = comp;
            Name = name;
        }

        public void Add(string item)
        {
            _data.Add(new Item(item));
        }

        /// <summary>
        /// 填充完毕
        /// </summary>
        public void Stuff()
        {
            _data.Sort(comparer);
            IsStuff = true;
        }

        public IEnumerable<Item> FindItems(Item value)
        {
            if (!IsStuff)
            {
                throw new Exception("没有填充完毕！");
            }
            if (value != null)
            {
                foreach (var item in _data)
                {
                    int diff = comparer.Compare(value, item);
                    if (diff < 0) 
                    {
                        yield break;
                    }
                    else if (diff == 0) 
                    {
                        yield return item;
                    }
                }
            }
        }

        /// <summary>
        /// 是否包含数据
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool Contains(Item item)
        {
            if (item == null) return false;
            return Util.Any(FindItems(item), i => true);
        }

        public IEnumerable<Item> Pop(Item value)
        {
            foreach (var item in FindItems(value))
            {
                if (!item.IsExport)
                {
                    item.IsExport = true;
                    yield return item;
                }
            }
        }

        /// <summary>
        /// 是否结束
        /// </summary>
        public bool IsEnd
        {
            get
            {
                return _data.FindIndex(i => !i.IsExport) == -1;
            }
        }

        /// <summary>
        /// 查找第一个元素
        /// </summary>
        public Item FristValue
        {
            get
            {
                return _data.Find(i => !i.IsExport);
            }
        }
    }

    /// <summary>
    /// 容器列表
    /// </summary>
    public class ContainerCollections
    {
        private List<Container> _data = new List<Container>(10);
        /// <summary>
        /// 当前的节点
        /// </summary>
        private int index = 0;

        public ContainerCollections(IEnumerable<Container> data)
        {
            if (data != null)
            {
                _data.AddRange(data);
            }
        }

        /// <summary>
        /// 添加容器
        /// </summary>
        /// <param name="container"></param>
        public void Add(Container container)
        {
            _data.Add(container);
        }

        /// <summary>
        /// 是否结束
        /// </summary>
        public bool IsEnd { get { return index >= _data.Count; } }

        /// <summary>
        /// 当前容器
        /// </summary>
        public Container CurrentCont
        {
            get
            {
                if (IsEnd) { return null; }
                return _data[index];
            }
        }

        /// <summary>
        /// 查找下一个节点
        /// </summary>
        /// <returns></returns>
        private int FindNext()
        {
            for (; !IsEnd; index++)
            {
                if (CurrentCont != null && !CurrentCont.IsEnd)
                {
                    break;
                }
            }
            return index;
        }

        /// <summary>
        /// 返回一组key与对应的容器
        /// </summary>
        /// <returns></returns>
        public IEnumerable<KeyContainerPair> FindKeyContainerPair()
        {
            int start = FindNext();
            if (IsEnd) yield break;
            Item key = CurrentCont.FristValue;
            for (int i = start; i < _data.Count; i++)
            {
                if (_data[i].Contains(key))
                {
                    yield return new KeyContainerPair(key, _data[i]);
                }
            }
        }
    }

    /// <summary>
    /// 项与容器的对
    /// </summary>
    public class KeyContainerPair
    {
        public Item Key { get; set; }
        public Container Value { get; set; }

        public KeyContainerPair(Item key, Container value)
        {
            Key = key;
            Value = value;
        }
    }
}
