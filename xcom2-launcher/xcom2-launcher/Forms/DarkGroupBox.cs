using System.Drawing;
using System.Windows.Forms;

namespace XCOM2Launcher.Forms
{
    /// <summary>
    /// A GroupBox that can fully take over its own border and caption painting.
    ///
    /// The default GroupBox draws its border using the OS's themed GroupBoxRenderer,
    /// which pulls the outline color straight from the active Windows visual style.
    /// It ignores BackColor/ForeColor for the outline, and - it turns out - ignores
    /// anything painted over it from an external Paint event handler too. Overriding
    /// OnPaint directly (and skipping the base call) is the only reliable way to
    /// replace it.
    /// </summary>
    public class DarkGroupBox : GroupBox
    {
        /// <summary>When false, this behaves like a completely normal GroupBox.</summary>
        public bool UseDarkTheme { get; set; }

        protected override void OnPaint(PaintEventArgs e)
        {
            if (!UseDarkTheme)
            {
                base.OnPaint(e);
                return;
            }

            var g = e.Graphics;
            g.Clear(BackColor);

            var textSize = g.MeasureString(Text, Font);
            int top = (int)(textSize.Height / 2);
            var rect = Rectangle.FromLTRB(0, top, Width - 1, Height - 1);

            using (var pen = new Pen(ThemeManager.Border))
            {
                g.DrawLine(pen, rect.Left, rect.Top, rect.Left + 6, rect.Top);
                g.DrawLine(pen, rect.Left + 8 + (int)textSize.Width, rect.Top, rect.Right, rect.Top);
                g.DrawLine(pen, rect.Left, rect.Top, rect.Left, rect.Bottom);
                g.DrawLine(pen, rect.Left, rect.Bottom, rect.Right, rect.Bottom);
                g.DrawLine(pen, rect.Right, rect.Top, rect.Right, rect.Bottom);
            }

            using (var textBrush = new SolidBrush(ForeColor))
                g.DrawString(Text, Font, textBrush, rect.Left + 6, 0);
        }
    }
}
