using Foundation;
using MLKit.BarcodeScanning;
using MLKit.Core;
using UIKit;

namespace LibreriaKenny
{
    // All the code in this file is only included on iOS.
    public class Methods
    {
        
        #region Public Methods

        public static void SetSupportBarcodeFormat(BarcodeFormats barcodeFormats)
        {
            BarcodeFormat supportFormats = Methods.ConvertBarcodeFormats(barcodeFormats);
            Configuration.BarcodeDetectorSupportFormat = supportFormats;
        }

        public static async Task<bool> AskForRequiredPermission()
        {
            try
            {
                var status = await Permissions.CheckStatusAsync<Permissions.Camera>();
                if (status != PermissionStatus.Granted)
                {
                    await Permissions.RequestAsync<Permissions.Camera>();
                }
                status = await Permissions.CheckStatusAsync<Permissions.Camera>();
                if (status == PermissionStatus.Granted)
                    return true;
            }
            catch (Exception ex)
            {
                //Something went wrong
            }
            return false;


        }

        public static async Task<List<BarcodeResult>> ScanFromImage(byte[] imageArray)
        {
            UIImage image = new UIImage(NSData.FromArray(imageArray));
            var visionImage = new MLImage(image);
            //VisionImageMetadata metadata = new VisionImageMetadata();
            //VisionApi vision = VisionApi.Create();
            //VisionBarcodeDetector barcodeDetector = vision.GetBarcodeDetector(Configuration.BarcodeDetectorSupportFormat);
            //VisionBarcode[] barcodes = await barcodeDetector.DetectAsync(visionImage);
            var options = new BarcodeScannerOptions(Configuration.BarcodeDetectorSupportFormat);
            var barcodeScanner = MLKit.BarcodeScanning.BarcodeScanner.BarcodeScannerWithOptions(options);

            var tcs = new TaskCompletionSource<List<BarcodeResult>>();

            barcodeScanner.ProcessImage(visionImage, (barcodes, error) =>
            {
                if (error != null)
                {
                    Console.WriteLine($"Error occurred : {error}");
                    tcs.TrySetResult(null);
                    return;
                }
                if (barcodes == null || barcodes.Length == 0)
                {
                    tcs.TrySetResult(new List<BarcodeResult>());
                    return;
                }

                var s = image.Size;
                List<BarcodeResult> resultList = new List<BarcodeResult>();
                foreach (var barcode in barcodes)
                    resultList.Add(ProcessBarcodeResult(barcode));

                tcs.TrySetResult(resultList);
                return;
            });
            return await tcs.Task;
        }

        public static BarcodeResult ProcessBarcodeResult(Barcode barcode)
        {
            List<Microsoft.Maui.Graphics.Point> cornerPoints = new List<Microsoft.Maui.Graphics.Point>();

            foreach (var cornerPoint in barcode.CornerPoints)
                cornerPoints.Add(new Microsoft.Maui.Graphics.Point(cornerPoint.CGPointValue.X, cornerPoint.CGPointValue.Y));
            
            return new BarcodeResult
            {
                BarcodeType = ConvertBarcodeResultTypes(barcode.ValueType),
                BarcodeFormat = (BarcodeFormats)barcode.Format,
                DisplayValue = barcode.DisplayValue,
                RawValue = barcode.RawValue,
                CornerPoints = cornerPoints.ToArray(),
                RawData = barcode.RawData.ToArray()
            };
        }
        #endregion
    }
}