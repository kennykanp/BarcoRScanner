using LibreriaKennyXD;
using SkiaSharp;
using SkiaSharp.Views.Maui;

namespace ConFeMLKit8
{
    public partial class MainPage : ContentPage
    {
        readonly SKPaint paint = new SKPaint
        {
            Style = SKPaintStyle.Stroke,
            Color = SKColor.Parse("#CC52B54B"),
            StrokeWidth = 4
        };

        List<BarcodeResult> Barcodes { get; set; }

        public MainPage()
        {
            InitializeComponent();

        }

        private void Camera_OnDetected(object sender, OnDetectedEventArg e)
        {
            Barcodes = e.BarcodeResults;
            Canvas.InvalidateSurface();
            cameraView.IsScanning = true;

            //List<BarcodeResult> obj = e.BarcodeResults;

            //string result = string.Empty;
            //for (int i = 0; i < obj.Count; i++)
            //{
            //    result += $"Type : {obj[i].BarcodeType}, Value: {obj[i].DisplayValue}{Environment.NewLine}";
            //}

            //Dispatcher.Dispatch(async () =>
            //{
            //    await DisplayAlert("Barcode", result, "OK");

            //    cameraView.IsScanning = true;
            //});
        }

        private void SKCanvasView_PaintSurface(object sender, SKPaintSurfaceEventArgs args)
        {
            SKImageInfo info = args.Info;
            SKSurface surface = args.Surface;
            SKCanvas canvas = surface.Canvas;

            canvas.Clear();

            if (Barcodes != null)
            {
                foreach (var b in Barcodes)
                {
                    if (b.CornerPoints?.Length > 1)
                    {
                        var points = b.CornerPoints.Select(p => new SKPoint((float)p.X, (float)p.Y)).ToList();
                        points.Add(points[0]);
                        canvas.DrawPoints(SKPointMode.Polygon, points.ToArray(), paint);
                    }
                    break;
                }
            }
        }
    }

}
