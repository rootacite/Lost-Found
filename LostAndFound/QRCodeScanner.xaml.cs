using BarcodeScanner.Mobile.Maui;

namespace LostAndFound;

public partial class QRCodeScanner : ContentPage
{

    private bool ExitFlag = false;
    private readonly QRCodeScannerDataModel qRCodeScannerDataModelDataModel;
    private string Result = "Null";

    public QRCodeScanner()
	{
		InitializeComponent();

        NavigationPage.SetHasNavigationBar(this, false);
        NavigationPage.SetBackButtonTitle(this, null);

        BindingContext = new QRCodeScannerDataModel();
        qRCodeScannerDataModelDataModel = BindingContext as QRCodeScannerDataModel;

        Loaded += (e, v) =>
        {
            
        };

        Unloaded += (e, v) =>
        {
            ExitFlag = true;
        };
    }

    public async Task<string> WaitResult()
    {
        return await Task.Run(async () =>
        {
            while (!ExitFlag)
                await Task.Delay(1);

            return Result;
        });
    }

    private void Camera_OnDetected(object sender, BarcodeScanner.Mobile.OnDetectedEventArg e)
    {
        Result = e.BarcodeResults[0].RawValue;
        ExitFlag = true;
    }
}