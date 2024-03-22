namespace LibreriaKennyXD
{
    public enum BarcodeEnums
    {
        Unknown = 0,
        ContactInfo = 1,
        Email = 2,
        Isbn = 3,
        Phone = 4,
        Product = 5,
        Sms = 6,
        Text = 7,
        Url = 8,
        WiFi = 9,
        GeographicCoordinates = 10,
        CalendarEvent = 11,
        DriversLicense = 12
    }

    [Flags]
    public enum BarcodeFormats
    {
        Code128 = 1,
        QRCode = 256,
    }

    public enum CameraFacing
    {
        Back = 0,
        Front = 1
    }
}
