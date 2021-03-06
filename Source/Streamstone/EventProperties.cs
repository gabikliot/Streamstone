﻿using System;
using System.Linq;
using System.Collections.Generic;

using Microsoft.WindowsAzure.Storage.Table;

namespace Streamstone
{
    public sealed class EventProperties : PropertyMap
    {
        internal static readonly EventProperties None = new EventProperties();

        EventProperties()
        {}

        EventProperties(IDictionary<string, EntityProperty> properties)
            : base(properties)
        {}

        internal static EventProperties ReadEntity(IDictionary<string, EntityProperty> properties)
        {
            Requires.NotNull(properties, "properties");
            return Build(properties);
        }

        public static EventProperties From(IDictionary<string, EntityProperty> properties)
        {
            Requires.NotNull(properties, "properties");
            return Build(Clone(properties));
        }

        public static EventProperties From(object properties)
        {
            Requires.NotNull(properties, "properties");
            return Build(ToDictionary(properties));
        }

        static EventProperties Build(IEnumerable<KeyValuePair<string, EntityProperty>> properties)
        {
            return new EventProperties(properties.Where(x => !IsReserved(x.Key)).ToDictionary(p => p.Key, p => p.Value));
        }

        static bool IsReserved(string propertyName)
        {
            switch (propertyName)
            {
                case "PartitionKey":
                case "RowKey":
                case "ETag":
                case "Timestamp":
                case "Version":
                    return true;
                default:
                    return false;
            }
        }
    }
}
