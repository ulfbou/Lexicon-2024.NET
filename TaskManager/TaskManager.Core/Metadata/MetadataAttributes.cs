using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManager.Core.Metadata
{
    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = true)]
    sealed class MetadataAttribute : Attribute
    {
        public string Description { get; }
        public MetadataAttribute(string description) => Description = description;
    }

    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = true)]
    sealed class TagAttribute : Attribute
    {
        public string Tag { get; }
        public TagAttribute(string tag) => Tag = tag;
    }
}
