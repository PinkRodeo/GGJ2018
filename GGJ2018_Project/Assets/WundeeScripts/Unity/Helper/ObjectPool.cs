using System.Collections.Generic;

namespace WundeeUnity
{
    public class ObjectPool<T> where T : new()
	{
		private Queue<T> _inactiveObjects;

		public List<T> objects;

		public ObjectPool(int initialSize = 0)
		{
			_inactiveObjects = new Queue<T>(initialSize);
			objects = new List<T>(initialSize);
		}

		public T GetObject()
		{
			if (_inactiveObjects.Count > 0)
				return _inactiveObjects.Dequeue();
			else
			{
				T item = new T();
				objects.Add(item);
				return item;
			}
		}

		public void PutObject(T item)
		{
			_inactiveObjects.Enqueue(item);
		}
	}


}
