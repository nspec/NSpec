using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Xml;

namespace NSpec.Domain.Formatters
{
    public class HtmlFormatter : IFormatter
    {
        public void Write( ContextCollection contexts )
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine( "<html>" );
            sb.AppendLine( "<head>" );
            sb.AppendLine( "<style type=\"text/css\">" );
            sb.AppendLine( "  span {padding: 1em}" );
            sb.AppendLine( "  .results {background-color: #FFFFCC; border-color: #000000; border-style: dashed; border-width: thin; font-weight:bold; white-space: pre-wrap; text-align: center;}" );
            sb.AppendLine( "  .context-parent-name {font-weight:bold;}" );
            sb.AppendLine( "  .spec-passed {font-weight:bold; color:green;}" );
            sb.AppendLine( "  .spec-failed {font-weight:bold; color:#FF0000;}" );
            sb.AppendLine( "  .spec-pending {font-weight:bold; color:#FF6600;}" );
            sb.AppendLine( "  .spec-exception {background-color: #FFD2CF; border-color: #FF828D; border-style: dashed; border-width: thin; padding: 1em; white-space: pre-wrap;}" );
            sb.AppendLine( "  .runDate {color:grey; font-size:x-small;}" );
            sb.AppendLine( "</style>" );
            sb.AppendLine( "</head>" );
            sb.AppendLine( "<body>" );

            sb.AppendLine();
            sb.Append( "<div class=\"results\">" );
            sb.AppendFormat( "Specs: <span>{0}</span>", contexts.Examples().Count() ); 
            sb.AppendFormat( "Failed: <span class=\"spec-failed\">{0}</span>", contexts.Failures().Count() ); 
            sb.AppendFormat( "Pending: <span class=\"spec-pending\">{0}</span>", contexts.Pendings().Count() ); 
            sb.AppendFormat( "<div class=\"runDate\">Run Date: {0}</div>", DateTime.Now ); 
            sb.Append( "</div>" );
            sb.AppendLine();
            sb.AppendLine( "<br />" );

            contexts.Do( c => this.BuildParentContext( sb, c ) );

            sb.AppendLine( "</html>" );
            sb.AppendLine( "</body>" );

            Console.WriteLine(sb.ToString());
        }
        void BuildParentContext( StringBuilder sb, Context context )
        {
            sb.AppendLine( "<div class=\"parent-context\">" );
            sb.AppendFormat( "  <span class=\"context-parent-name\">{0}</span>", context.Name );
            sb.AppendLine();
            context.Contexts.Do( c => this.BuildChildContext( sb, c ) );
            sb.AppendLine( "</div>" );
        }

        void BuildChildContext( StringBuilder sb, Context context )
        {
            sb.AppendLine( "<ul>" );
            sb.AppendFormat( "<li>{0}", context.Name );
            sb.AppendLine();
            if( context.Examples.Count > 0 )
            {
                sb.AppendLine( "<ul>" );
                context.Examples.Do( e => this.BuildSpec( sb, e ) );
                sb.AppendLine( "</ul>" );
            }
            context.Contexts.Do( c => this.BuildChildContext( sb, c ) );
            sb.AppendLine();
            sb.AppendLine( "</li>" );
            sb.AppendLine( "</ul>" );
        }

        void BuildSpec( StringBuilder sb, Example example )
        {
            sb.AppendFormat( "<li>{0}", example.Spec );
            if( example.Exception != null )
            {
                sb.AppendLine( "<span class=\"spec-failed\">&lArr; Failed</span>" );
                sb.Append( "<div class=\"spec-exception\"><code>" );
                sb.Append( HttpUtility.HtmlEncode( example.Exception.ToString() ) );
                sb.AppendLine( "</code></div>" );
            }
            else if( example.Pending )
            {
                sb.Append( "<span class=\"spec-pending\">&lArr; Pending</span>" );
            }
            else
            {
                sb.Append( "<span class=\"spec-passed\">&lArr; Passed</span>" );
            }
            sb.AppendLine( "</li>" );
        }
    }
}