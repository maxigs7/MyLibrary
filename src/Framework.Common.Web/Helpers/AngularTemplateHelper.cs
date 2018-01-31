using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Framework.Common.Web.Helpers
{
    public static class AngularTemplateHelper
    {
        private static readonly Dictionary<Type, string> TemplateMap
            = new Dictionary<Type, string>
            {
                {typeof (byte), "Byte"},
                {typeof (byte?), "Byte"},
                {typeof (sbyte), "Byte"},
                {typeof (sbyte?), "Byte"},
                {typeof (short), "Int16"},
                {typeof (short?), "Int16"},
                {typeof (ushort), "Int16"},
                {typeof (ushort?), "Int16"},
                {typeof (int), "Int32"},
                {typeof (int?), "Int32"},
                {typeof (uint), "Int32"},
                {typeof (uint?), "Int32"},
                {typeof (long), "Int64"},
                {typeof (long?), "Int64"},
                {typeof (ulong), "Int64"},
                {typeof (ulong?), "Int64"},
                {typeof (bool), "Boolean"},
                {typeof (bool?), "Boolean"},
                {typeof (decimal), "Decimal"},
                {typeof (decimal?), "Decimal"},
            };

        public static string GetTemplateForProperty(ModelMetadata propertyMetadata)
        {
            var templateName = propertyMetadata.TemplateHint ??
                               propertyMetadata.DataTypeName;

            if (templateName == null)
            {
                templateName = TemplateMap.ContainsKey(propertyMetadata.ModelType)
                    ? TemplateMap[propertyMetadata.ModelType]
                    : propertyMetadata.ModelType.Name;
            }

            return "Angular/" + templateName;
        }
    }
}