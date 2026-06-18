using System;
using System.Threading.Tasks;
using Eto.Drawing;
using Eto.Forms;

namespace Photo_Reviewer
{
    public partial class MainForm : Form
    {
        readonly PhotoFolder Folder;

        Bitmap Image = new(1, 1, PixelFormat.Format32bppRgb);

        public MainForm(PhotoFolder folder)
        {
            InitializeComponent();
            Folder = folder;
        }

        async void MainForm_Shown(object? sender, EventArgs e)
        {
            await LoadFolder();
            await LoadAndShowImage();
        }

        async void ImageDrawable_KeyDown(object? sender, KeyEventArgs e)
        {
            if (e.IsKeyDown(Keys.Left, Keys.None) && Folder.CurrentIndex > 0)
            {
                Folder.CurrentIndex--;
                await LoadAndShowImage();
            }
            else if (e.IsKeyDown(Keys.Right, Keys.None) && Folder.CurrentIndex + 1 < Folder.Count)
            {
                Folder.CurrentIndex++;
                await LoadAndShowImage();
            }
        }

        void ImageDrawable_Paint(object? sender, PaintEventArgs e)
        {
            var input = new SizeF(Image.Width, Image.Height);
            var output = new SizeF(ImageDrawable.Size.Width, ImageDrawable.Size.Height);
            var scaleW = output.Width / input.Width;
            var scaleH = output.Height / input.Height;
            var scale = Math.Min(scaleW, scaleH);
            var size = new SizeF(input.Width * scale, input.Height * scale);
            var location = new PointF((output.Width - size.Width) / 2, (output.Height - size.Height) / 2);
            e.Graphics.DrawImage(Image, new RectangleF(location, size));
        }

        async Task LoadFolder()
        {
            ImageLabel.Text = $"Loading folder: {Folder.Path}";
            await Task.Run(Folder.Load);
        }

        async Task LoadAndShowImage()
        {
            ImageLabel.Text = $"Showing photo {Folder.CurrentIndex + 1} of {Folder.Count}: {Folder.Current.Path}";
            await Task.Run(BackgroundLoadImage);
            ImageDrawable.Invalidate();
        }

        void BackgroundLoadImage()
        {
            Image = new Bitmap(Folder.Current.Path);
        }
    }
}
