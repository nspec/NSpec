using System.Collections.Generic;
using System.Linq;

namespace NSpec.Domain
{
    public class Tags
    {
        public static string Focus = "focus";

        /// <summary>Parses a string containing tags into a collection of normalized tags</summary>
        public static List<string> ParseTags(string tags)
        {
            var tagsCollection = new List<string>();

            // store one or more tags delimited by either commas or spaces
            if (string.IsNullOrEmpty(tags)) return tagsCollection;

            foreach (var tag in tags.Split(new[] { ',', ' ' }))
            {
                if (!string.IsNullOrEmpty(tag)) tagsCollection.Add(tag);
            }

            return tagsCollection;
        }

        /// <summary>Parses a string containing tag filters into tag filter collections</summary>
        public static void ParseTagFilters(string tags, ref List<string> includeTags, ref List<string> excludeTags)
        {
            // store one or more tags delimited by either commas or spaces
            if (string.IsNullOrEmpty(tags)) return;

            foreach (var tag in tags.Split(new[] { ',', ' ' }))
            {
                // determine whether tag is an include or exclude filter
                List<string> targetTagCollection = tag.StartsWith("~") ? excludeTags : includeTags;

                var trimmedTag = tag.TrimStart('~');

                // store tags without any leading @ in the tag (e.g., '@mytag' is stored as 'mytag')
                if (!string.IsNullOrEmpty(tag)) targetTagCollection.Add(trimmedTag);
            }
        }

        /// <summary>Parses a string containing tag filters into the internal tag filter collections</summary>
        public Tags Parse(string tags)
        {
            ParseTagFilters(tags, ref IncludeTags, ref ExcludeTags);

            return this;
        }

        public bool ShouldSkip(List<string> tags)
        {
            if (!IncludesAny(tags)) return true;

            if (ExcludesAny(tags)) return true;

            return false;
        }

        public bool IncludesAny(List<string> tags)
        {
            return !IncludeTags.Any() || IncludeTags.Intersect(tags).Any();
        }

        public bool ExcludesAny(List<string> tags)
        {
            return ExcludeTags.Any() && ExcludeTags.Intersect(tags).Any();
        }

        public bool HasTagFilters()
        {
            return IncludeTags.Any() || ExcludeTags.Any();
        }

        public List<string> IncludeTags = new List<string>();
        public List<string> ExcludeTags = new List<string>();
    }
}