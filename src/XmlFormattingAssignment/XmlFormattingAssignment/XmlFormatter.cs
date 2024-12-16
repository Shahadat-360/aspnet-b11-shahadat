using System.Collections;
using System.Reflection;
using System.Text;

namespace XmlFormattingAssignment
{
    public static class XmlFormatter
    {
        //Reducing Repetition
        private static void Tagger(StringBuilder stringBuilder, Object obj, string Tag)
        {
            stringBuilder.Append($"<{Tag}>");
            if (obj != null)
                stringBuilder.Append($"{obj.ToString()}");
            stringBuilder.Append($"</{Tag}>");
        }

        //Solver(Logic,Control,Program Flow)
        private static void XmlCreator(StringBuilder stringBuilder, Object obj, string Tag)
        {
            //For Null value
            if (obj is null)
            {
                Tagger(stringBuilder, obj, Tag);
                return;
            }
            var type = obj.GetType();
            //For Properties
            if (type.IsPrimitive || type == typeof(decimal) || type == typeof(string) || type == typeof(DateTime))
            {
                Tagger(stringBuilder, obj, Tag);
            }
            //For Collection
            else if (obj is IEnumerable && type != typeof(string))
            {
                stringBuilder.Append($"<{Tag}>");
                foreach (var item in (IEnumerable)obj)
                {
                    XmlCreator(stringBuilder, item, item.GetType().Name);
                }
                stringBuilder.Append($"</{Tag}>");
            }
            //For Class 
            else
            {
                stringBuilder.Append($"<{Tag}>");
                foreach (var property in type.GetProperties())
                {
                    XmlCreator(stringBuilder, property.GetValue(obj), property.Name);
                }
                stringBuilder.Append($"</{Tag}>");
            }
        }
        public static string Convert(object obj)
        {
            StringBuilder stringBuillder = new StringBuilder();
            XmlCreator(stringBuillder, obj, obj.GetType().Name);
            return stringBuillder.ToString();
        }
    }
}