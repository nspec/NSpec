using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace NSpec.Domain.Formatters
{
    [Serializable]
    public class TiddlyWikiFormatter : IFormatter
    {
        public void Write(ContextCollection contexts)
        {
            contexts.Do(c => this.tiddlers.Add(c.Name, this.BuildTiddlerFrom(c)));

            StringBuilder menuItemsOutput = new StringBuilder();
            StringBuilder tiddlersOutput = new StringBuilder();
            foreach (var context in this.tiddlers.Keys)
            {
                menuItemsOutput.AppendFormat("[[{0}]]", context);
                menuItemsOutput.AppendLine();

                tiddlersOutput.Append(this.tiddlers[context]);
            }
            int examplesCount = contexts.Examples().Count();
            int failuresCount = contexts.Failures().Count();
            int pendingsCount = contexts.Pendings().Count();

            this.WriteTiddlyWiki(menuItemsOutput.ToString(), tiddlersOutput.ToString(),
                                 examplesCount, failuresCount, pendingsCount);
        }

        void WriteTiddlyWiki(
            string menuItems, string tiddlerItems,
            int examplesCount, int failuresCount, int pendingCount)
        {
            StreamReader templateReader = new StreamReader(templateFile);
            StreamWriter outputWriter = new StreamWriter(outputFile);

            while (!templateReader.EndOfStream)
            {
                string data = templateReader.ReadLine();
                if (!String.IsNullOrEmpty(data))
                {
                    data = data.Replace("$MAIN_MENU_CONTEXT_NAMES_GO_HERE$", menuItems);
                    data = data.Replace("<div id=\"storeArea\">", "<div id=\"storeArea\">" + tiddlerItems);
                    data = data.Replace("$TOTAL_SPECS$", examplesCount.ToString());
                    data = data.Replace("$TOTAL_FAILED_SPECS$", failuresCount.ToString());
                    data = data.Replace("$TOTAL_PENDING_SPECS$", pendingCount.ToString());
                }
                outputWriter.WriteLine(data);
            }

            templateReader.Close();
            outputWriter.Close();
        }

        string BuildTiddlerFrom(Context context)
        {
            StringBuilder result = new StringBuilder();

            result.AppendFormat("<div title=\"{0}\" modifier=\"NSpecRunner\" created=\"{1}\" tags=\"NSpec\" changecount=\"1\">",
                                context.Name, DateTime.Now.ToString("yyyyMMddHHmm"));
            result.AppendLine();
            result.Append("<pre>");

            result.Append(this.BuildTiddlerBody(context));

            result.AppendLine("</pre></div>");
            return result.ToString();
        }

        string BuildTiddlerBody(Context context, int level = 0)
        {
            StringBuilder result = new StringBuilder();

            if (level > 0)
            {
                result.AppendFormat("{0}{1}", "*".Times(level), context.Name);
                result.AppendLine();
            }

            context.Examples.Do(e => result.Append(this.BuildSpec(e, level + 1)));

            context.Contexts.Do(c => result.Append(this.BuildTiddlerBody(c, level + 1)));

            return result.ToString();
        }

        string BuildSpec(Example e, int level)
        {
            string output = "";

            if (e.Exception != null)
            {
                output = String.Format("{0}&lt;&lt;markSpecAsFailed '{1}'&gt;&gt; &lt;&lt;showException 'error_{2}' '{3}''&gt;&gt;",
                                       "*".Times(level), e.Spec, Guid.NewGuid(), HttpUtility.HtmlEncode(e.Exception.ToString()));
            }
            else if (e.Pending)
            {
                output = String.Format("{0}&lt;&lt;markSpecAsPending '{1}'&gt;&gt;", "*".Times(level), e.Spec);
            }
            else
            {
                output = String.Format("{0}{1}", "*".Times(level), e.Spec);
            }

            return output + Environment.NewLine;
        }

        TiddlyWikiFormatter() {}

        public TiddlyWikiFormatter(string templateFile, string outputFile)
        {
            this.templateFile = templateFile;
            this.outputFile = outputFile;
        }

        SortedList<string, string> tiddlers = new SortedList<string, string>();
        string templateFile;
        string outputFile;
    }
}