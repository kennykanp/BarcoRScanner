using LibreriaKennyXD;

namespace ConFeMLKit8
{
    public partial class MainPage : ContentPage
    {

        public MainPage()
        {
            InitializeComponent();

        }

        private void Camera_OnDetected(object sender, OnDetectedEventArg e)
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
