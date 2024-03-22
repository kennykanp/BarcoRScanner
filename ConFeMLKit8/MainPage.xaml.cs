using BarcodeScanner.Mobile;

namespace ConFeMLKit8
{
    public partial class MainPage : ContentPage
    {

        public MainPage()
        {
            BarcodeScanner.Mobile.Methods.SetSupportBarcodeFormat(BarcodeScanner.Mobile.BarcodeFormats.QRCode);
            InitializeComponent();
            BarcodeScanner.Mobile.Methods.AskForRequiredPermission();

        }

        private void Camera_OnDetected(object sender, BarcodeScanner.Mobile.OnDetectedEventArg e)
        {
            List<BarcodeResult> obj = e.BarcodeResults;

            string result = string.Empty;
            for (int i = 0; i < obj.Count; i++)
            {
                result += $"Type : {obj[i].BarcodeType}, Value: {obj[i].DisplayValue}{Environment.NewLine}";
            }

            Dispatcher.Dispatch(async () =>
            {
                await DisplayAlert("Barcode", result, "OK");

                cameraView.IsScanning = true;
            });
        }
    }

}
