﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Specialized;
using System.Collections;
using GameBase.Model;

namespace Carcassonne.Model
{
    public class MeepleCollection : IObservableList<Meeple>
    {
        private Dictionary<MeepleType, Stack<Meeple>> m_meeple = new Dictionary<MeepleType, Stack<Meeple>>();
        private List<Meeple> m_allMeeple = new List<Meeple>();

        public event NotifyCollectionChangedEventHandler CollectionChanged;

        public MeepleCollection()
        {
            AvailableTypes = new List<MeepleType>();
        }

        public void Push(Meeple m)
        {
            if (!m_meeple.ContainsKey(m.Type))
            {
                m_meeple[m.Type] = new Stack<Meeple>();
            }
            if (!AvailableTypes.Contains(m.Type))
            {
                AvailableTypes.Add(m.Type);
            }
            m_meeple[m.Type].Push(m);
            updateAllMeepleList();
            var args = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, m, m_allMeeple.IndexOf(m));
            CollectionChanged?.Invoke(this, args);
        }

        public Meeple Pop(MeepleType t)
        {
            if (m_meeple.ContainsKey(t))
            {
                var m = m_meeple[t].Pop();
                if (m_meeple[t].Count == 0)
                {
                    AvailableTypes.Remove(t);
                }
                var idx = m_allMeeple.IndexOf(m);
                updateAllMeepleList();
                var args = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, m, idx);
                CollectionChanged?.Invoke(this, args);
            }
            return null;
        }

        public Meeple Peek(MeepleType t)
        {
            if (m_meeple.ContainsKey(t))
            {
                var m = m_meeple[t].Peek();
            }
            return null;
        }

        public void Clear()
        {
            m_allMeeple.Clear();
            m_meeple.Clear();
            var args = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset);
            CollectionChanged?.Invoke(this, args);
        }

        public List<MeepleType> AvailableTypes { get; private set; }

        private void updateAllMeepleList()
        {
            m_allMeeple.Clear();
            foreach (var meeple in m_meeple.Values)
            {
                m_allMeeple.AddRange(meeple);
            }
        }

        #region IEnumerable<Meeple> Members

        public IEnumerator<Meeple> GetEnumerator()
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

        #region IList<Meeple> Members

        public int IndexOf(Meeple item)
        {
            return m_allMeeple.IndexOf(item);
        }

        public void Insert(int index, Meeple item)
        {
            throw new NotImplementedException();
        }

        public void RemoveAt(int index)
        {
            throw new NotImplementedException();
        }

        public Meeple this[int index]
        {
            get
            {
                return m_allMeeple[index];
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        #endregion

        #region ICollection<Meeple> Members

        public void Add(Meeple item)
        {
            throw new NotImplementedException();
        }

        public bool Contains(Meeple item)
        {
            return m_allMeeple.Contains(item);
        }

        public void CopyTo(Meeple[] array, int arrayIndex)
        {
            m_allMeeple.CopyTo(array, arrayIndex);
        }

        public int Count
        {
            get { return m_allMeeple.Count; }
        }

        public bool IsReadOnly
        {
            get { return true; }
        }

        public bool Remove(Meeple item)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
