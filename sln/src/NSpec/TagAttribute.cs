using System;

namespace NSpec
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
    public class TagAttribute : Attribute
    {
        public TagAttribute(string tags)
        {
            Tags = tags;
        }

        public string Tags { get; set; }
    }
}