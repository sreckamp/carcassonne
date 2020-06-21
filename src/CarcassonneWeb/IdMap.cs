using System.Collections;
using System.Collections.Generic;

namespace CarcassonneServer
{
    public class IdMap<T> : IEnumerable<T>
    {
        private readonly object m_lock = new object();
        private readonly Dictionary<T, string> m_toId = new Dictionary<T, string>();
        private readonly Dictionary<string, T> m_fromId = new Dictionary<string, T>();

        public T FromId(string id) => m_fromId[id];
        public string ToId(T @object) => m_toId[@object];

        public void Add(T @object)
        {
            lock (m_lock)
            {
                var id = IdManager.Next();

                m_fromId[id] = @object;
                m_toId[@object] = id;
            }
        }

        public bool Contains(T @object) => m_toId.ContainsKey(@object);

        public string this[T @object] => ToId(@object);
        public T this[string id] => FromId(id);

        public IEnumerator<T> GetEnumerator()
        {
            return m_fromId.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}