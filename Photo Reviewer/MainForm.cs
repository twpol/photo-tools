using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Eto.Drawing;
using Eto.Forms;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace Photo_Reviewer
{
    public partial class MainForm : Form
    {
        readonly PhotoFolder Folder;

        Bitmap Image = new(1, 1, PixelFormat.Format32bppRgba);

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
            if (Folder.CurrentValid)
            {
                ImageLabel.Text = $"Showing photo {Folder.CurrentIndex + 1} of {Folder.Count}: {Folder.Current.Path}";
                await Task.Run(BackgroundLoadImage);
            }
            else
            {
                ImageLabel.Text = $"No photos: {Folder.Path}";
                Image = new(1, 1, PixelFormat.Format32bppRgba);
            }
            ImageDrawable.Invalidate();
        }

        void BackgroundLoadImage()
        {
            var decoder = new DecoderOptions()
            {
                MaxFrames = 1,
            };

            using var imageSharp = SixLabors.ImageSharp.Image.Load<Bgra32>(decoder, Folder.Current.Path);
            imageSharp.Mutate(x => x.AutoOrient());

            Image = new(imageSharp.Width, imageSharp.Height, PixelFormat.Format32bppRgba);
            using var bitmapData = Image.Lock();

            // NOTE: Holy hacks, Batman!
            // NOTE: Eto.Forms does not seem to provide either Span<T> or Memory<T> access to BitmapData
            var span = MemoryMarshal.CreateSpan(ref Unsafe.AddByteOffset(ref Unsafe.NullRef<byte>(), bitmapData.Data), bitmapData.ScanWidth * bitmapData.Image.Height);
            imageSharp.CopyPixelDataTo(span);
        }
    }
}
