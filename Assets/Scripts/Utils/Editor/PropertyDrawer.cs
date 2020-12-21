namespace Antada.M3RPG.Utilities.Editor
{
	using System.Reflection;

	using UnityEditor;

	public static class UtilityPropertyDrawer
	{
		/// <summary>
		/// Get object by property
		/// </summary>
		/// <param name="prop"></param>
		/// <returns></returns>
		public static object GetTargetObjectOfProperty(SerializedProperty prop)
		{
			var path = prop.propertyPath.Replace(".Array.data[", "[");
			object obj = prop.serializedObject.targetObject;
			var elements = path.Split('.');
			foreach (var element in elements)
			{
				if (element.Contains("["))
				{
					var elementName = element.Substring(0, element.IndexOf("["));
					var index = System.Convert.ToInt32(element.Substring(element.IndexOf("[")).Replace("[", "")
						.Replace("]", ""));
					obj = GetValueImp(obj, elementName, index);
				}
				else
				{
					obj = GetValueImp(obj, element);
				}
			}

			return obj;
		}

		private static object GetValueImp(object source, string name)
		{
			if (source == null)
			{
				return null;
			}

			var type = source.GetType();

			while (type != null)
			{
				var f = type.GetField(name, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
				if (f != null)
				{
					return f.GetValue(source);
				}

				var p = type.GetProperty(name,
					BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
				if (p != null)
				{
					return p.GetValue(source, null);
				}

				type = type.BaseType;
			}

			return null;
		}

		private static object GetValueImp(object source, string name, int index)
		{
			var enumerable = GetValueImp(source, name) as System.Collections.IEnumerable;
			if (enumerable == null) return null;
			var enm = enumerable.GetEnumerator();

			for (var i = 0; i <= index; i++)
			{
				if (!enm.MoveNext()) return null;
			}

			return enm.Current;
		}
	}
}

