using System.Collections.Generic;
using System.Linq;

namespace NSpec.Domain
{
    public class Tags
    {
        public List< string > IncludeTags = new List<string>();
        public List< string > ExcludeTags = new List<string>();

        /// <summary>Parses a string containing tags into a collection of normalized tags</summary>
        public static List<string> ParseTags( string tags )
        {
            var tagsCollection = new List<string>();

            // store one or more tags delimited by either commas or spaces
            if( !string.IsNullOrEmpty( tags ) )
            {
                foreach( var tag in tags.Split( new[] { ',', ' ' } ) )
                {
                    // store tags without any leading ampersat in the tag (e.g., '@mytag' is stored as 'mytag')
                    var rawTag = tag.TrimStart( new[] { '@' } );
                    if( !string.IsNullOrEmpty( rawTag ) )
                        tagsCollection.Add( rawTag );
                }
            }

            return tagsCollection;
        }

        /// <summary>Parses a string containing tag filters into tag filter collections</summary>
        public static void ParseTagFilters( string tags, ref List<string> includeTags, ref List<string> excludeTags )
        {
            if( includeTags == null )
                includeTags = new List<string>();
            if( excludeTags == null )
                excludeTags = new List<string>();

            // store one or more tags delimited by either commas or spaces
            if( !string.IsNullOrEmpty( tags ) )
            {
                foreach( var tag in tags.Split( new[] { ',', ' ' } ) )
                {
                    // determine whether tag is an include or exclude filter
                    List< string > targetTagCollection = tag.StartsWith( "~" ) ? excludeTags : includeTags;

                    // store tags without any leading ampersat in the tag (e.g., '@mytag' is stored as 'mytag')
                    var rawTag = tag.TrimStart( new[] { '~', '@' } );
                    if( !string.IsNullOrEmpty( rawTag ) )
                        targetTagCollection.Add( rawTag );
                }
            }
        }

        /// <summary>Parses a string containing tag filters into the internal tag filter collections</summary>
        public Tags ParseTagFilters( string tags )
        {
            ParseTagFilters( tags, ref IncludeTags, ref ExcludeTags );

            return this;
        }

        public bool Includes( string tag )
        {
            return !IncludeTags.Any() || IncludeTags.Contains( tag.TrimStart( new[] { '@' } ) );
        }

        public bool IncludesAny( List<string> tags )
        {
            return !IncludeTags.Any() || IncludeTags.Intersect( tags ).Any();
        }

        public bool Excludes( string tag )
        {
            return ExcludeTags.Any() && ExcludeTags.Contains( tag.TrimStart( new[] { '@' } ) );
        }

        public bool ExcludesAny( List<string> tags )
        {
            return ExcludeTags.Any() && ExcludeTags.Intersect( tags ).Any();
        }

        public bool HasTagFilters()
        {
            return IncludeTags.Any() || ExcludeTags.Any();
        }
    }
}