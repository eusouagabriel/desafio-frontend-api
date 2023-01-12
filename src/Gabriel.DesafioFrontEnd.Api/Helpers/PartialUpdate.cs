using System.ComponentModel;
using System.Dynamic;
using System.Reflection;

namespace Gabriel.DesafioFrontEnd.Api.Helpers
{

    /// <summary>
    /// https://learn.microsoft.com/en-us/dotnet/api/system.dynamic.dynamicobject?view=net-7.0
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PartialUpdate<T> : DynamicObject
    {

        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            var name = binder.Name;

            if (binder.IgnoreCase)
                name = name.ToLower();

            if (value is null)
            {
                if (updatedableProperties.ContainsKey(name))
                {
                    var prop = updatedableProperties[name];
                    var typeConverter = TypeDescriptor.GetConverter(prop.PropertyType);
                    value = typeConverter.ConvertFromString(value.ToString());
                }
            }
            values.Add(name, value);

            return true;
        }

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            string name = binder.Name;
            if (binder.IgnoreCase)
                name = name.ToLower();

            return values.TryGetValue(name, out result);
        }

        public virtual void Patch(T baseItem)
        {
            foreach (string key in values.Keys)
            {
                var result = values[key];
                var prop = updatedableProperties[key];
                prop.SetValue(baseItem, result);
            }
        }

        private static Dictionary<string, PropertyInfo> updatedableProperties = null;
        private Dictionary<string, object> values = new Dictionary<string, object>();

        public PartialUpdate()
        {
            if (updatedableProperties is null)
            {
                updatedableProperties = new Dictionary<string, PropertyInfo>();

                var props = typeof(T).GetProperties();
                foreach (var prop in props)
                {
                    if (prop.CanWrite)
                    {
                        var attr = prop.GetCustomAttribute(typeof(NotUpdateableAttribute));
                        if (attr is null)
                            updatedableProperties.Add(prop.Name, prop);
                    }
                }
            }
        }
    }

    public class NotUpdateableAttribute : Attribute
    {
    }
}

