using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Markup;
using System.Windows.Media;

namespace H.Framework.WPF.Control.Controls
{
    public class RichTextEdit : RichTextBox
    {
        public static readonly DependencyProperty TextProperty = DependencyProperty.Register("Text", typeof(string), typeof(RichTextEdit), new PropertyMetadata("", TextPropertyChangedCallback)); //属性默认值

        public string Text
        {
            get
            {
                return (string)GetValue(TextProperty);
            }
            set
            {
                SetValue(TextProperty, value);
            }
        }

        public static readonly DependencyProperty RTFTextProperty = DependencyProperty.Register("RTFText", typeof(string), typeof(RichTextEdit), new PropertyMetadata("", RTFTextPropertyChangedCallback)); //属性默认值

        public string RTFText
        {
            get
            {
                return (string)GetValue(RTFTextProperty);
            }
            set
            {
                SetValue(RTFTextProperty, value);
            }
        }

        public static readonly DependencyProperty TextMaxWidthProperty = DependencyProperty.Register("TextMaxWidth", typeof(double), typeof(RichTextEdit), new PropertyMetadata((double)245, null)); //属性默认值

        public double TextMaxWidth
        {
            get
            {
                return (double)GetValue(TextMaxWidthProperty);
            }
            set
            {
                SetValue(TextMaxWidthProperty, value);
            }
        }

        private static void RTFTextPropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue == null)
                return;
            var rtbx = d as RichTextEdit;
            LoadRTF(e.NewValue.ToString(), rtbx);
            rtbx.Width = CalcMessageWidth(rtbx, rtbx.TextMaxWidth);
        }

        private static void TextPropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue == null)
                return;
            var rtbx = d as RichTextEdit;
            rtbx.Document.Blocks.Clear();
            if (string.IsNullOrWhiteSpace(e.NewValue.ToString()))
                return;
            var doc = new FlowDocument();
            doc.Blocks.Add((Section)XamlReader.Parse(e.NewValue.ToString()));
            rtbx.Document = doc;
        }

        private static void LoadRTF(string rtf, RichTextBox richTextBox)
        {
            if (string.IsNullOrEmpty(rtf))
            {
                throw new ArgumentNullException();
            }
            TextRange textRange = new TextRange(richTextBox.Document.ContentStart, richTextBox.Document.ContentEnd);
            using (MemoryStream rtfMemoryStream = new MemoryStream())
            {
                using (StreamWriter rtfStreamWriter = new StreamWriter(rtfMemoryStream))
                {
                    rtfStreamWriter.Write(rtf);
                    rtfStreamWriter.Flush();
                    rtfMemoryStream.Seek(0, SeekOrigin.Begin);
                    //Load the MemoryStream into TextRange ranging from start to end of RichTextBox.
                    textRange.Load(rtfMemoryStream, DataFormats.Rtf);
                }
            }
        }

        public static double CalcMessageWidth(RichTextEdit t, double w)
        {
            var range = new TextRange(t.Document.ContentStart, t.Document.ContentEnd);
            var text = range.Text;
            var formatText = GetFormattedText(t.Document);
            int count = SubstringCount(string.IsNullOrWhiteSpace(t.Text) ? t.RTFText : t.Text, "pict") / 2;
            return Math.Min(formatText.WidthIncludingTrailingWhitespace + 10 + count * 25, w);
        }

        public static FormattedText GetFormattedText(FlowDocument doc)
        {
            var output = new FormattedText(
                GetText(doc),
                System.Globalization.CultureInfo.CurrentCulture,
                doc.FlowDirection,
                new Typeface(doc.FontFamily, doc.FontStyle, doc.FontWeight, doc.FontStretch),
                doc.FontSize,
                doc.Foreground);

            int offset = 0;

            foreach (TextElement textElement in GetRunsAndParagraphs(doc))
            {
                var run = textElement as Run;

                if (run != null)
                {
                    int count = run.Text.Length;

                    output.SetFontFamily(run.FontFamily, offset, count);
                    output.SetFontSize(run.FontSize, offset, count);
                    output.SetFontStretch(run.FontStretch, offset, count);
                    output.SetFontStyle(run.FontStyle, offset, count);
                    output.SetFontWeight(run.FontWeight, offset, count);
                    output.SetForegroundBrush(run.Foreground, offset, count);
                    output.SetTextDecorations(run.TextDecorations, offset, count);

                    offset += count;
                }
                else
                {
                    offset += Environment.NewLine.Length;
                }
            }
            return output;
        }

        private static string GetText(FlowDocument doc)
        {
            var sb = new StringBuilder();
            foreach (TextElement text in GetRunsAndParagraphs(doc))
            {
                var run = text as Run;
                sb.Append(run == null ? Environment.NewLine : run.Text);
            }
            return sb.ToString();
        }

        private static IEnumerable<TextElement> GetRunsAndParagraphs(FlowDocument doc)
        {
            for (var position = doc.ContentStart;
                position != null && position.CompareTo(doc.ContentEnd) <= 0;
                position = position.GetNextContextPosition(LogicalDirection.Forward))
            {
                if (position.GetPointerContext(LogicalDirection.Forward) == TextPointerContext.ElementEnd)
                {
                    if (position.Parent is Run run)
                    {
                        yield return run;
                    }
                    else
                    {
                        if (position.Parent is Paragraph para)
                        {
                            yield return para;
                        }
                        else
                        {
                            if (position.Parent is LineBreak lineBreak)
                            {
                                yield return lineBreak;
                            }
                        }
                    }
                }
            }
        }

        public static int SubstringCount(string str, string substring)
        {
            if (str.Contains(substring))
            {
                string strReplaced = str.Replace(substring, "");
                return (str.Length - strReplaced.Length) / substring.Length;
            }
            return 0;
        }
    }
}