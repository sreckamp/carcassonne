using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using GameBase.Model;

namespace Carcassonne.Model
{
    public class MeepleCollection : IObservableList<IMeeple>
    {
        private readonly Dictionary<MeepleType, Stack<IMeeple>> m_meeple = new Dictionary<MeepleType, Stack<IMeeple>>();
        private readonly List<IMeeple> m_allMeeple = new List<IMeeple>();

        public MeepleCollection()
        {
            CollectionChanged += (sender, args) => { };
        }

        public event NotifyCollectionChangedEventHandler CollectionChanged;

        public void Push(IMeeple m)
        {
            if (!m_meeple.ContainsKey(m.Type))
            {
                m_meeple[m.Type] = new Stack<IMeeple>();
            }
            if (!AvailableTypes.Contains(m.Type))
            {
                AvailableTypes.Add(m.Type);
            }
            m_meeple[m.Type].Push(m);
            UpdateAllMeepleList();
            var args = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, m, m_allMeeple.IndexOf(m));
            CollectionChanged?.Invoke(this, args);
        }

        public IMeeple Pop(MeepleType t)
        {
            if (!m_meeple.ContainsKey(t)) return NopMeeple.Instance;
            var m = m_meeple[t].Pop();
            if (m_meeple[t].Count == 0)
            {
                AvailableTypes.Remove(t);
            }
            var idx = m_allMeeple.IndexOf(m);
            UpdateAllMeepleList();
            var args = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, m, idx);
            CollectionChanged?.Invoke(this, args);
            return m;
        }

        public IMeeple Peek(MeepleType t)
        {
            return m_meeple.ContainsKey(t) ? m_meeple[t].Peek() : NopMeeple.Instance;
        }

        public void Clear()
        {
            m_allMeeple.Clear();
            m_meeple.Clear();
            var args = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset);
            CollectionChanged?.Invoke(this, args);
        }

        public List<MeepleType> AvailableTypes { get; } = new List<MeepleType>();

        private void UpdateAllMeepleList()
        {
            m_allMeeple.Clear();
            foreach (var meeple in m_meeple.Values)
            {
                m_allMeeple.AddRange(meeple);
            }
        }

        #region IEnumerable<IMeeple> Members

        public IEnumerator<IMeeple> GetEnumerator()
        {
            return m_allMeeple.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)m_allMeeple).GetEnumerator();
        }

        #endregion

        #region IList<IMeeple> Members

        public int IndexOf(IMeeple item)
        {
            return m_allMeeple.IndexOf(item);
        }

        public void Insert(int index, IMeeple item)
        {
            throw new NotImplementedException();
        }

        public void RemoveAt(int index)
        {
            throw new NotImplementedException();
        }

        public IMeeple this[int index]
        {
            get => m_allMeeple[index];
            set => throw new NotImplementedException();
        }

        #endregion

        #region ICollection<Meeple> Members

        public void Add(IMeeple item)
        {
            throw new NotImplementedException();
        }

        public bool Contains(IMeeple item)
        {
            return m_allMeeple.Contains(item);
        }

        public void CopyTo(IMeeple[] array, int arrayIndex)
        {
            m_allMeeple.CopyTo(array, arrayIndex);
        }

        public int Count => m_allMeeple.Count;

        public bool IsReadOnly => true;

        public bool Remove(IMeeple item)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
