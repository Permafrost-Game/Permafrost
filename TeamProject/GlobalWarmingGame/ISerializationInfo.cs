using System;
using System.Collections.Generic;
using System.Reflection;

namespace GlobalWarmingGame
{
    public interface ISerializationInfo
    {
        Type Type { get; }

        IEnumerable<FieldInfo> FieldsToSerialize { get; }

        IEnumerable<PropertyInfo> PropertiesToSerialize { get; }
    }
}