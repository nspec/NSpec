using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using NSpec.Domain;

namespace NSpecSpecs
{
    public static class NSpecAssertionExtensions
    {
        public static List<string> should_contain_tag(this List<string> collection, string tag)
        {
            CollectionAssert.Contains(collection, tag.TrimStart(new[] { '@' }));

            return collection;
        }

        public static Tags should_tag_as_included(this Tags tags, string tag)
        {
            CollectionAssert.Contains(tags.IncludeTags, tag.TrimStart(new[] { '@' }));

            return tags;
        }

        public static Tags should_tag_as_excluded(this Tags tags, string tag)
        {
            CollectionAssert.Contains(tags.ExcludeTags, tag.TrimStart(new[] { '@' }));

            return tags;
        }
    }
}
