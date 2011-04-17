using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.Text.Editor;
using System.ComponentModel.Composition;
using Microsoft.VisualStudio.Utilities;

namespace NSpecVSExtension.Providers
{
    [Export(typeof(IWpfTextViewMarginProvider))]
    [Name("NSpecMargin")]
    [Order(After = PredefinedMarginNames.LineNumber)]
    [MarginContainer(PredefinedMarginNames.Left)]
    [ContentType("code")]
    [TextViewRole(PredefinedTextViewRoles.Interactive)]
    class NSpecMarginFactory : IWpfTextViewMarginProvider
    {
        public IWpfTextViewMargin CreateMargin(IWpfTextViewHost wpfTextViewHost, IWpfTextViewMargin marginContainer)
        {
            return new NSpecMargin(wpfTextViewHost.TextView);
        }
    }
}
