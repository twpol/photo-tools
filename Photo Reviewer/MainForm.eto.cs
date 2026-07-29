using Eto.Drawing;
using Eto.Forms;

namespace Photo_Reviewer
{
    partial class MainForm : Form
    {
        const int DefaultSpacing = 10;

        readonly Label ImageLabel = new();
        readonly Drawable ImageDrawable = new();

        void InitializeComponent()
        {
            var font = SystemFonts.Default();

            Title = "JGR Photo Reviewer";
            MinimumSize = new Size((int)(font.LineHeight * 56), (int)(font.LineHeight * 42));
            Padding = DefaultSpacing;
            Shown += MainForm_Shown;

            ImageDrawable.CanFocus = true;
            ImageDrawable.KeyDown += ImageDrawable_KeyDown;
            ImageDrawable.Paint += ImageDrawable_Paint;

            Content = new TableLayout
            {
                Spacing = new Size(DefaultSpacing, DefaultSpacing),
                Rows =
                {
                    ImageLabel,
                    ImageDrawable,
                }
            };
        }
    }
}
