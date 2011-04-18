using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.Text.Editor;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Media;
using Microsoft.VisualStudio.Text.Formatting;
using Microsoft.VisualStudio.Text;
using System.Windows.Shapes;

namespace NSpecVSExtension
{
    class NSpecMargin : Canvas, IWpfTextViewMargin
    {
        private IWpfTextView textView;
        private bool isDisposed;

        public NSpecMargin(IWpfTextView textView)
        {
            textView.LayoutChanged += textView_LayoutChanged;
            this.textView = textView;
            this.isDisposed = false;
            this.Width = 200;
            this.ClipToBounds = true;
            this.Background = new SolidColorBrush(Colors.LightGreen);
        }

        void textView_LayoutChanged(object sender, TextViewLayoutChangedEventArgs e)
        {
            Children.Clear();
            
            foreach (ITextSnapshotLine line in e.NewSnapshot.Lines)
            {
                CreateVisuals(line, e);
            }
        }

        private void CreateVisuals(ITextSnapshotLine line, TextViewLayoutChangedEventArgs e)
        {
            var textViewLines = textView.TextViewLines;

            int start = line.Start;
            int end = line.End;

            SnapshotSpan span = new SnapshotSpan(e.NewSnapshot, Span.FromBounds(start, end));
            Geometry g = textViewLines.GetMarkerGeometry(span);

            if(g != null)
            {
                var top = g.Bounds.Top;
                var ellipse = new TextBlock { Width = 100.0, Height = 30.0, Text = span.GetText() };
                ellipse.SetValue(Canvas.TopProperty, top);
                Children.Add(ellipse);
            }
        }

        public FrameworkElement VisualElement
        {
            get
            {
                ThrowIfDisposed();
                return this;
            }
        }

        public bool Enabled
        {
            get
            {
                ThrowIfDisposed();
                return true;
            }
        }

        public ITextViewMargin GetTextViewMargin(string marginName)
        {
            return (marginName == "NSpecMargin") ? (IWpfTextViewMargin)this : null;
        }

        public double MarginSize
        {
            get { throw new NotImplementedException(); }
        }

        public void Dispose()
        {
            if (!isDisposed)
            {
                GC.SuppressFinalize(this);
                isDisposed = true;
            }
        }

        private void ThrowIfDisposed()
        {
            if (isDisposed)
                throw new ObjectDisposedException("NSpecMargin");
        }
    }
}
