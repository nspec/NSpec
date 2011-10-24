using System;

namespace NSpec
{
    [AttributeUsageAttribute( AttributeTargets.Class|AttributeTargets.Method, AllowMultiple = true )]
    public class TagAttribute : Attribute
    {
        public string Tags { get; set; } 

        public TagAttribute( string tags )
        {
            Tags = tags;
        }
    }
}