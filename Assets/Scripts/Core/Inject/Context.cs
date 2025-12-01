using System;
using System.Collections.Generic;

namespace Injection
{
	public class Context : IDisposable
	{
		private readonly Dictionary<Type, object> contextMap;

		public Context()
		{
			contextMap = new Dictionary<Type, object>(100);
			contextMap[typeof(Context)] = this;
		}

		public void Install(params object[] objects)
		{
			foreach (object obj in objects)
			{
				contextMap[obj.GetType()] = obj;
			}
		}

		public void InstallByType(object obj, Type type)
		{
			contextMap[type] = obj;
		}

		public void Uninstall(Type type)
		{
			contextMap.Remove(type);
		}

		public void ApplyInstall()
		{
			Injector injector = Get<Injector>();
			foreach (object obj in contextMap.Values)
			{
				injector.Inject(obj);
			}
		}

		public T Get<T>() where T : class
		{
			if (!contextMap.ContainsKey(typeof(T)))
			{
				throw new KeyNotFoundException("Not found " + typeof(T));
			}

			return (T)contextMap[typeof(T)];
		}

		public object Get(Type type)
		{
			if (!contextMap.ContainsKey(type))
			{
				throw new KeyNotFoundException("Not found " + type);
			}

			return contextMap[type];
		}

		public void Dispose()
		{
			foreach (var obj in contextMap)
			{
				if (this == obj.Value)
					continue;

				if (obj.Value is IDisposable disposable)
				{
					disposable.Dispose();
				}
			}

			contextMap.Clear();
		}
	}
}