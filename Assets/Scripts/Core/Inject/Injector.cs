using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace Injection
{
	public class Injector
	{
		private readonly Dictionary<Type, FieldInfo[]> _fieldsMap;
		private readonly Context _context;

		public Injector(Context context)
		{
			_context = context;
			_fieldsMap = new Dictionary<Type, FieldInfo[]>(100);
		}

		public void Inject(object value)
		{
			if (value == null)
				return;

			var type = value.GetType();

			TryToMapFields(type);

			var fields = _fieldsMap[type];
			foreach (var fieldInfo in fields)
			{
				fieldInfo.SetValue(value, _context.Get(fieldInfo.FieldType));
			}
		}

		public T Get<T>() where T : class
		{
			return _context.Get<T>();
		}

		private void TryToMapFields(Type type)
		{
			if (_fieldsMap.ContainsKey(type))
			{
				return;
			}
			var fields = type.GetFields(BindingFlags.NonPublic | BindingFlags.Instance)
				.Where(field => field.GetCustomAttributes(typeof(Inject), true).Any())
				.ToArray();

			_fieldsMap[type] = fields;
		}
	}
}