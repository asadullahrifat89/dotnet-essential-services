using System.Reflection;

namespace Base.Application.Extensions
{
    public static class ClassExtensions
    {
        public static List<FieldInfo> GetConstants(Type type)
        {
            FieldInfo[] fieldInfos = type.GetFields(BindingFlags.Public |
                 BindingFlags.Static | BindingFlags.FlattenHierarchy);

            var fields = fieldInfos.Where(fi => fi.IsLiteral && !fi.IsInitOnly).ToList();

            return fields;
        }
    }
}
