using Android.Gms.Extensions;
using Android.Graphics;
using Android.Runtime;
using Java.Util;
using Xamarin.Google.MLKit.Vision.Barcode.Common;
using Xamarin.Google.MLKit.Vision.BarCode;
using Xamarin.Google.MLKit.Vision.Common;

namespace LibreriaKennyXD
{
    // All the code in this file is only included on Android.
    public class Methods
    {
       

        internal static BarcodeEnums ConvertBarcodeResultTypes(int barcodeValueType)
        {
            switch (barcodeValueType)
            {
                case Barcode.TypeCalendarEvent:
                    return BarcodeEnums.CalendarEvent;
                case Barcode.TypeContactInfo:
                    return BarcodeEnums.ContactInfo;
                case Barcode.TypeDriverLicense:
                    return BarcodeEnums.DriversLicense;
                case Barcode.TypeEmail:
                    return BarcodeEnums.Email;
                case Barcode.TypeGeo:
                    return BarcodeEnums.GeographicCoordinates;
                case Barcode.TypeIsbn:
                    return BarcodeEnums.Isbn;
                case Barcode.TypePhone:
                    return BarcodeEnums.Phone;
                case Barcode.TypeProduct:
                    return BarcodeEnums.Product;
                case Barcode.TypeSms:
                    return BarcodeEnums.Sms;
                case Barcode.TypeText:
                    return BarcodeEnums.Text;
                case Barcode.TypeUrl:
                    return BarcodeEnums.Url;
                case Barcode.TypeWifi:
                    return BarcodeEnums.WiFi;
                default: return BarcodeEnums.Unknown;
            }
        }

        internal static int ConvertBarcodeFormats(BarcodeFormats barcodeFormats)
        {
            var formats = Barcode.FormatAllFormats;

            if (barcodeFormats.HasFlag(BarcodeFormats.Code128))
                formats |= Barcode.FormatCode128;
                formats |= Barcode.FormatPdf417;
            if (barcodeFormats.HasFlag(BarcodeFormats.QRCode))
                formats |= Barcode.FormatQrCode;
            return formats;
        }
        #region Public Methods

        public static void SetSupportBarcodeFormat(BarcodeFormats barcodeFormats)
        {
            int supportFormats = Methods.ConvertBarcodeFormats(barcodeFormats);
            Configuration.BarcodeFormats = supportFormats;
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
            using Bitmap bitmap = await BitmapFactory.DecodeByteArrayAsync(imageArray, 0, imageArray.Length);
            if (bitmap == null)
                return null;
            using var image = InputImage.FromBitmap(bitmap, 0);
            var scanner = BarcodeScanning.GetClient(new BarcodeScannerOptions.Builder().SetBarcodeFormats(Configuration.BarcodeFormats)
                .Build());
            return ProcessBarcodeResult(await scanner.Process(image));
        }

        public static List<BarcodeResult> ProcessBarcodeResult(Java.Lang.Object result)
        {
            if (result == null)
                return null;
            var javaList = result.JavaCast<ArrayList>();
            if (javaList.IsEmpty)
                return null;
            List<BarcodeResult> resultList = new List<BarcodeResult>();
            foreach (var barcode in javaList.ToArray())
            {
                var mapped = barcode.JavaCast<Barcode>();

                List<Microsoft.Maui.Graphics.Point> cornerPoints = new List<Microsoft.Maui.Graphics.Point>();

                foreach (var cornerPoint in mapped.GetCornerPoints())
                    cornerPoints.Add(new Microsoft.Maui.Graphics.Point(cornerPoint.X, cornerPoint.Y));

                resultList.Add(new BarcodeResult()
                {
                    BarcodeType = ConvertBarcodeResultTypes(mapped.ValueType),
                    BarcodeFormat = (BarcodeFormats)mapped.Format,
                    DisplayValue = mapped.DisplayValue,
                    RawValue = mapped.RawValue,
                    CornerPoints = cornerPoints.ToArray(),
                    RawData = mapped.GetRawBytes()
                });
            }

            return resultList;
        }
        #endregion
    }
}