using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace GlobalWarmingGame
{
    static class Serializer
    {
        const string SerializationInfoPath = @"Content/ISerializationInfo.json";

        static readonly IDictionary<Type, ISerializationInfo> lookupTable;

        static Serializer()
        {
            lookupTable = new Dictionary<Type, ISerializationInfo>();

            try
            {
                IEnumerable<object> serializationInfoList = Deserialize(SerializationInfoPath)[typeof(ISerializationInfo)];

                foreach (ISerializationInfo serializationInfo in serializationInfoList)
                    AddType(serializationInfo);
            }
            catch (FileNotFoundException)
            {
                GenerateSerializationInfo(new StaticSerializationInfo(typeof(Vector2),
                    nameof(Vector2.X),
                    nameof(Vector2.Y)));
            }
        }

        static public void AddType(ISerializationInfo serializationInfo)
        {
            lookupTable[serializationInfo.Type] = serializationInfo;
        }

        static bool IsSimpleType(Type type)
        {
            return type.IsPrimitive || type == typeof(Decimal)
                || type == typeof(String) || type == typeof(DateTime)
                || type == typeof(DateTimeOffset) || type == typeof(TimeSpan)
                || type == typeof(Guid);
        }

        static void HandleNewType(Type type, object obj)
        {
            StaticSerializationInfo serializationInfo = new StaticSerializationInfo(type);

            foreach (FieldInfo field in type.GetFields().Where(f => Attribute.IsDefined(f, typeof(PFSerializable))))
            {
                Type fieldType = field.FieldType;

                if (!lookupTable.ContainsKey(fieldType) && !IsSimpleType(fieldType))
                    HandleNewType(fieldType, field.GetValue(obj));

                serializationInfo.AddField(field);
            }

            foreach (PropertyInfo property in type.GetProperties().Where(p => Attribute.IsDefined(p, typeof(PFSerializable))))
            {
                Type propertyType = property.PropertyType;

                if (!lookupTable.ContainsKey(propertyType) && !IsSimpleType(propertyType))
                    HandleNewType(propertyType, property.GetValue(obj));

                serializationInfo.AddProperty(property);
            }

            lookupTable[type] = serializationInfo;
        }

        static JToken GetSerializedData(Type type, object obj)
        {
            if (type != typeof(string) && typeof(IEnumerable).IsAssignableFrom(type)) //type.GetInterface(nameof(IEnumerable)) != null)
            {
                JArray collectionData = new JArray();

                foreach (object element in ((IEnumerable)obj).Cast<object>())
                    collectionData.Add(GetSerializedData(element.GetType(), element));

                return collectionData;
            }

            ISerializationInfo serializationInfo = null;

            if (lookupTable.ContainsKey(type)) serializationInfo = lookupTable[type];
            else if (lookupTable.ContainsKey(type.BaseType)) serializationInfo = lookupTable[type.BaseType];

            if (serializationInfo != null)
            {
                JObject objectData = new JObject();

                foreach (FieldInfo field in serializationInfo.FieldsToSerialize)
                    objectData.Add(field.Name, GetSerializedData(field.FieldType, field.GetValue(obj)));

                foreach (PropertyInfo property in serializationInfo.PropertiesToSerialize)
                    objectData.Add(property.Name, GetSerializedData(property.PropertyType, property.GetValue(obj)));

                return objectData;
            }

            if (type.IsSubclassOf(typeof(FieldInfo)))
                return GetSerializedData(typeof(FieldInfo), (FieldInfo)obj);

            if (type.IsSubclassOf(typeof(PropertyInfo)))
                return GetSerializedData(typeof(PropertyInfo), (PropertyInfo)obj);

            return new JValue(obj.ToString());
        }

        public static void Serialize(string path, IEnumerable<object> objects)
        {
            JObject objectsData = new JObject();
            IDictionary<string, JArray> typeArrays = new Dictionary<string, JArray>();

            foreach (object obj in objects)
            {
                Type objType = obj.GetType();

                if (!lookupTable.ContainsKey(objType))
                    HandleNewType(objType, obj);

                if (!typeArrays.ContainsKey(objType.FullName))
                {
                    typeArrays.Add(objType.FullName, new JArray());
                    objectsData.Add(objType.FullName, typeArrays[objType.FullName]);
                }

                typeArrays[objType.FullName].Add(GetSerializedData(objType, obj));
            }

            Console.WriteLine(objectsData);

            WriteSerializationInfo();

            using (JsonTextWriter writer = new JsonTextWriter(File.CreateText(path)))
                objectsData.WriteTo(writer);
        }

        public static object GetDeserailizedObject(Type type, JToken data)
        {
            if (type == typeof(ISerializationInfo))
            {
                Type objType = Type.GetType(data["Type"]["FullName"].ToString());

                if (objType == null)
                    objType = Type.GetType(string.Format("{0}, Monogame.Framework", data["Type"]["FullName"].ToString()));

                StaticSerializationInfo obj = new StaticSerializationInfo(objType); ;

                foreach (var fieldData in data["FieldsToSerialize"])
                    obj.AddField(obj.Type.GetField(fieldData["Name"].ToString()));

                foreach (var propertyData in data["PropertiesToSerialize"])
                    obj.AddProperty(obj.Type.GetProperty(propertyData["Name"].ToString()));

                return obj;
            }
            else
            {
                if (IsSimpleType(type))
                    return Convert.ChangeType(data.ToString(), type);

                object obj = Activator.CreateInstance(type);

                foreach (var memberData in (JObject)data)
                {
                    FieldInfo field = type.GetField(memberData.Key);

                    if (field != null)
                        field.SetValue(obj, GetDeserailizedObject(field.FieldType, memberData.Value));
                    else
                    {
                        PropertyInfo property = type.GetProperty(memberData.Key);
                        // Console.WriteLine(property.PropertyType);
                        property.SetValue(obj, GetDeserailizedObject(property.PropertyType, memberData.Value));
                    }
                }

                if (typeof(IReconstructable).IsAssignableFrom(type))
                    obj = ((IReconstructable)obj).Reconstruct();
                //((IReconstructable)obj).Reconstruct();

                return obj;
            }
        }

        public static IDictionary<Type, IEnumerable<object>> Deserialize(string path)
        {
            IDictionary<Type, IEnumerable<object>> objects = new Dictionary<Type, IEnumerable<object>>();

            using (JsonTextReader reader = new JsonTextReader(File.OpenText(path)))
            {
                JObject data = (JObject)JToken.ReadFrom(reader);

                foreach (var typeData in data)
                {
                    Type objType = Type.GetType(typeData.Key);
                    IList<object> objTypeList = new List<object>();

                    objects.Add(objType, objTypeList);

                    foreach (var objData in typeData.Value)
                        objTypeList.Add(GetDeserailizedObject(objType, objData));
                }
            }

            return objects;
        }

        public static void WriteSerializationInfo()
        {
            JObject serializationInfoData = new JObject();
            JArray serializationInfoArray = new JArray();

            serializationInfoData.Add(typeof(ISerializationInfo).FullName, serializationInfoArray);

            foreach (ISerializationInfo serializationInfo in lookupTable.Values)
                serializationInfoArray.Add(GetSerializedData(typeof(ISerializationInfo), serializationInfo));

            // Console.WriteLine(serializationInfoData);

            using (JsonTextWriter writer = new JsonTextWriter(File.CreateText(SerializationInfoPath)))
                serializationInfoData.WriteTo(writer);
        }

        public static void GenerateSerializationInfo(params ISerializationInfo[] serializationInfoList)
        {
            AddType(new StaticSerializationInfo(typeof(Type), nameof(Type.FullName)));

            AddType(new StaticSerializationInfo(typeof(FieldInfo), nameof(FieldInfo.FieldType), nameof(FieldInfo.Name)));

            AddType(new StaticSerializationInfo(typeof(PropertyInfo), nameof(PropertyInfo.PropertyType), nameof(PropertyInfo.Name)));

            AddType(new StaticSerializationInfo(typeof(ISerializationInfo), nameof(ISerializationInfo.Type),
                nameof(ISerializationInfo.FieldsToSerialize),
                nameof(ISerializationInfo.PropertiesToSerialize)));

            foreach (ISerializationInfo serializationInfo in serializationInfoList)
                AddType(serializationInfo);

            WriteSerializationInfo();
        }
    }
}