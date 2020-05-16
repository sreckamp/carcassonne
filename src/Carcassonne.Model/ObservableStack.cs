using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace Carcassonne.Model
{
    public interface IObservableStack<T> : IEnumerable<T>, ICollection, INotifyCollectionChanged { }
    public class ObservableStack<T> : Stack<T>, IObservableStack<T>
    {
        public ObservableStack()
        {
            CollectionChanged += (sender, args) => { };
        }

        public event NotifyCollectionChangedEventHandler CollectionChanged;

        public new void Push(T t)
        {
            base.Push(t);
            var args = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, t, 0);
            CollectionChanged?.Invoke(this, args);
        }

        public new T Pop()
        {
            var item = base.Pop();
            var args = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, item, 0);
            CollectionChanged?.Invoke(this, args);
            return item;
        }

        public new void Clear()
        {
            base.Clear();
            var args = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset);
            CollectionChanged?.Invoke(this, args);
        }
    }
}
