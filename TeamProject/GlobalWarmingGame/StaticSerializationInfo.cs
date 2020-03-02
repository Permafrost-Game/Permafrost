using System;
using System.Collections.Generic;
using System.Reflection;

namespace GlobalWarmingGame
{
    class StaticSerializationInfo : ISerializationInfo
    {
        List<FieldInfo> fieldsToSerialize;
        List<PropertyInfo> propertiesToSerialize;

        [PFSerializable]
        public Type Type { get; }

        [PFSerializable]
        public IEnumerable<FieldInfo> FieldsToSerialize { get => fieldsToSerialize; }

        [PFSerializable]
        public IEnumerable<PropertyInfo> PropertiesToSerialize { get => propertiesToSerialize; }

        public StaticSerializationInfo()
        {
            fieldsToSerialize = new List<FieldInfo>();
            propertiesToSerialize = new List<PropertyInfo>();
        }

        public StaticSerializationInfo(Type type) : this()
        {
            this.Type = type;
        }

        public StaticSerializationInfo(Type type, params string[] names) :
            this(type)
        {
            foreach (string name in names)
            {
                FieldInfo field = type.GetField(name);

                if (field != null)
                    fieldsToSerialize.Add(field);
                else
                {
                    PropertyInfo property = type.GetProperty(name);

                    if (property != null)
                        propertiesToSerialize.Add(property);
                }
            }
        }

        public void AddField(FieldInfo field)
        {
            fieldsToSerialize.Add(field);
        }

        public void AddProperty(PropertyInfo property)
        {
            propertiesToSerialize.Add(property);
        }

        public void Print()
        {
            Console.WriteLine("Type:\n-----");
            Console.WriteLine(Type);
            Console.WriteLine("\nProperties:\n-----");
            foreach (FieldInfo field in fieldsToSerialize)
            {
                Console.WriteLine(field);
            }
            Console.WriteLine("\nFields\n-----");
            foreach (PropertyInfo property in propertiesToSerialize)
            {
                Console.WriteLine(property);
            }

            Console.WriteLine();
        }
    }
}