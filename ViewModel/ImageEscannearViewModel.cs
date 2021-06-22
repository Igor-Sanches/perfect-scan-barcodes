using Microsoft.Toolkit.Uwp.UI.Controls;
using Perfect_Scan.Manager;
using Perfect_Scan.Tools;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.ApplicationModel.Resources;
using Windows.Graphics.Imaging;
using Windows.Media.Capture;
using Windows.Networking.BackgroundTransfer;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using ZXing;

namespace Perfect_Scan.ViewModel
{
    public class ImageEscannearViewModel : ViewModelBase
    {
        private ResourceLoader loader = new ResourceLoader();
        private StorageFile arquivo;
        private bool btnEnabledForScan = false;
        private string cropLayout = "Collapsed", scanLayout = "Visible", dialgProgress = "Collapsed", visibleLayoutTxt = "Visible", visibleLayout = "Collapsed";
        private RelayCommand CancelCrop, saveCrop, imageCrop, escanearommand, cameraCommand, galeriaCommand;
        private ImageSource image;
        private ImageCropper imageCropper; 
        private WriteableBitmap writeable; 

        public ICommand SaveCropCommand
        {
            get
            {
                if (saveCrop == null)
                {
                    saveCrop = new RelayCommand(p => OnSalvarCorte());
                }
                return saveCrop;
            }
        }

        public ICommand CancelCropCommand
        {
            get
            {
                if (CancelCrop == null)
                {
                    CancelCrop = new RelayCommand(p => OnCancelarCorte());
                }
                return CancelCrop;
            }
        }

        private void OnCancelarCorte()
        {
            ScanView();
            CropperImage = null;
        }

        private async void OnSalvarCorte()
        {
            try
            {
                var file = await AppStorage.GetAppDataTemporaryFileAsync("Crop.png", true);
                var stream = await file.OpenAsync(FileAccessMode.ReadWrite);
                await imageCropper.SaveAsync(stream, BitmapFileFormat.Png, true);
                arquivo = file;
                Carregar();
                ScanView();
            }
            catch(Exception x) { Paginas.Root.RootApp.Instance.GetToast(x.Message); }
        }

        public WriteableBitmap CropperImage
        {
            get {
                if (arquivo != null && cropLayout.Equals("Visible"))
                {
                    return writeable;
                }
                else return null;
            }
            private set
            {
                if (writeable != value)
                {
                    writeable = value;
                    RaisePropertyChanged();
                }
            }
        }

        public ICommand ImageCropCommand
        {
            get
            {
                if (imageCrop == null)
                {
                    imageCrop = new RelayCommand(p => OnCrop());
                }
                return imageCrop;
            }
        }

        private async void OnCrop()
        {
            DialogProgress = "Visible";
            var stream = await arquivo.OpenAsync(FileAccessMode.Read);
            BitmapDecoder decoder = await BitmapDecoder.CreateAsync(stream);
            SoftwareBitmap software = await decoder.GetSoftwareBitmapAsync();
            SoftwareBitmap bi = SoftwareBitmap.Convert(software, BitmapPixelFormat.Bgra8, BitmapAlphaMode.Premultiplied);

            WriteableBitmap wb = new WriteableBitmap(bi.PixelWidth, bi.PixelHeight);
             
            await wb.SetSourceAsync(stream);
            DialogProgress = "Collapsed";
            CropperImage = wb;
            imageCropper.Source = wb;
            CropperView();
        }
         
        private void CropperView()
        {
            ScanLayout = "Collapsed";
            CropLayout = "Visible";
        }

        private void ScanView()
        {
            ScanLayout = "Visible";
            CropLayout = "Collapsed";
        }

        public string ScanLayout
        {
            get { return scanLayout; }
            private set
            {
                if (scanLayout != value)
                {
                    scanLayout = value;
                    RaisePropertyChanged();
                }
            }
        }
        public string CropLayout
        {
            get { return cropLayout; }
            private set
            {
                if (cropLayout != value)
                {
                    cropLayout = value;
                    RaisePropertyChanged();
                }
            }
        }

        public ICommand Escanear
        {
            get
            {
                if (escanearommand == null)
                {
                    escanearommand = new RelayCommand(p => OnEscanear());
                }
                return escanearommand;
            }
        }

        private async void OnEscanear()
        {
            DialogProgress = "Visible";
            try
            {
                if (arquivo != null)
                {

                    using (var stream = await arquivo.OpenAsync(FileAccessMode.Read))
                    {
                        BitmapDecoder decoder = await BitmapDecoder.CreateAsync(stream);
                        SoftwareBitmap software = await decoder.GetSoftwareBitmapAsync();
                        var size = software.PixelWidth * software.PixelHeight * 4;
                        var buffer = new Windows.Storage.Streams.Buffer((uint)size);
                        software.CopyToBuffer(buffer);
                        byte[] bytes = buffer.ToArray();
                        BarcodeReader reader = new BarcodeReader();
                         
                        Result result = reader.Decode(bytes, software.PixelWidth, software.PixelHeight, RGBLuminanceSource.BitmapFormat.RGB32);
                        
                        if (result != null)
                        {
                            if (BarcodeFormatSupport.IsFormate(result.BarcodeFormat))
                            {
                                DialogProgress = "Collapsed";
                                HistoricoManager.Escaneado(result);
                            }
                            else
                            {
                                ShowErrorMessage("support");
                            }
                        }
                        else
                        {
                            ShowErrorMessage("");
                        }
                    }
                }
                else
                {
                    DialogProgress = "Collapsed";
                }
            }
            catch
            {
                ShowErrorMessage("support");
            }
        }

        internal void Shot(ImageCropper imageCropper)
        {
            this.imageCropper = imageCropper;
        }

        internal void ShowNavigationFile(StorageFile file)
        {
            DialogProgress = "Visible";
            arquivo = file;
            Carregar();
        }
        internal async void ShowNavigationFile(RandomAccessStreamReference _stream)
        {
            DialogProgress = "Visible";
            try
            {
                var stream_ = await _stream.OpenReadAsync(); 
                BitmapDecoder decoder = await BitmapDecoder.CreateAsync(stream_);
                SoftwareBitmap software = await decoder.GetSoftwareBitmapAsync();
                SoftwareBitmap bi = SoftwareBitmap.Convert(software, BitmapPixelFormat.Bgra8, BitmapAlphaMode.Premultiplied);

                WriteableBitmap wb = new WriteableBitmap(bi.PixelWidth, bi.PixelHeight);

                await wb.SetSourceAsync(stream_);
                if (bi != null)
                {
                    using (var stream = await arquivo.OpenAsync(FileAccessMode.ReadWrite))
                    {
                        BitmapEncoder encoder = await BitmapEncoder.CreateAsync(new Guid(GetGuid), stream);
                        Stream pixelStream = wb.PixelBuffer.AsStream();
                        byte[] pixels = new byte[pixelStream.Length];
                        await pixelStream.ReadAsync(pixels, 0, pixels.Length);
                        encoder.SetPixelData(BitmapPixelFormat.Bgra8, BitmapAlphaMode.Ignore, (uint)wb.PixelWidth, (uint)wb.PixelHeight, 96.0, 96.0, pixels);
                        await encoder.FlushAsync();
                    }
                    BtnEnabled = true;
                    VisibleLayout = "Visible";
                    VisibleLayoutText = "Collapsed"; 
                }
            }
            catch
            {
            }
            DialogProgress = "Collapsed"; 
        }

        private async void ShowErrorMessage(string v)
        {
            try
            {

                DialogProgress = "Collapsed";

                ContentDialog dialog = new ContentDialog();
                dialog.Title = loader.GetString("AlertError");
                dialog.Content = v.Equals("") ? loader.GetString("MessageError") : loader.GetString("MessageErrorSupport");
                dialog.Foreground = ((Brush)App.Current.Resources["Texto"]);
                dialog.Background = ((Brush)App.Current.Resources["DialogoBackground"]);
                dialog.CloseButtonStyle = ((Style)App.Current.Resources["ButtonStyleContent"]);
                dialog.CloseButtonText = loader.GetString("ButtonClose");
                await dialog.ShowAsync();
            }
            catch { }
            
        }

        public string DialogProgress
        {
            get { return dialgProgress; }
            private set
            {
                if (dialgProgress != value)
                {
                    dialgProgress = value;
                    RaisePropertyChanged();
                }
            }
        }

        private async void Carregar()
        {
            try
            {
                var stream = await arquivo.OpenAsync(FileAccessMode.Read);
                BitmapDecoder decoder = await BitmapDecoder.CreateAsync(stream);
                SoftwareBitmap software = await decoder.GetSoftwareBitmapAsync();
                SoftwareBitmap bitmap = SoftwareBitmap.Convert(software, BitmapPixelFormat.Bgra8, BitmapAlphaMode.Premultiplied);
                SoftwareBitmapSource bitmapSource = new SoftwareBitmapSource();
                await bitmapSource.SetBitmapAsync(bitmap);
                if (bitmapSource != null)
                { 
                    BtnEnabled = true;
                    VisibleLayout = "Visible";
                    VisibleLayoutText = "Collapsed";
                    CodigoView = bitmapSource;
                }
            }
            catch
            {
            }
            DialogProgress = "Collapsed";
        }

        public ICommand Galeria
        {
            get
            {
                if (galeriaCommand == null)
                {
                    galeriaCommand = new RelayCommand(p => OnGaleria());
                }
                return galeriaCommand;
            }
        }

        private async void OnGaleria()
        {
            try
            {
                DialogProgress = "Visible";
                FileOpenPicker fileOpen = new FileOpenPicker();
                fileOpen.FileTypeFilter.Add(".jpg");
                fileOpen.FileTypeFilter.Add(".png");
                fileOpen.FileTypeFilter.Add(".bmp");
                fileOpen.FileTypeFilter.Add(".jpeg");
                fileOpen.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
                fileOpen.ViewMode = PickerViewMode.List;
                var file = await fileOpen.PickSingleFileAsync();
                if (file != null)
                {
                   arquivo = file;
                    Carregar();
                }
                else
                {
                    DialogProgress = "Collapsed";

                }

            }
            catch { }
        }

        public ICommand Camera
        {
            get
            {
                if(cameraCommand== null)
                {
                    cameraCommand = new RelayCommand(p => OnCamera());
                }
                return cameraCommand;
            }
        }

        private async void OnCamera()
        {
            try
            {

                CameraCaptureUI captureUI = new CameraCaptureUI();
                captureUI.PhotoSettings.Format = CameraCaptureUIPhotoFormat.Jpeg;
                captureUI.PhotoSettings.CroppedSizeInPixels = new Windows.Foundation.Size(250, 250);
                var file = await captureUI.CaptureFileAsync(CameraCaptureUIMode.Photo);
                if (file != null)
                {
                    DialogProgress = "Visible";
 
                    arquivo = file;
                    Carregar();
                }
            }
            catch { }
        }

        public ImageSource CodigoView
        {
            get { return image; }
            private set
            {
                if(image != value)
                {
                    image = value;
                    RaisePropertyChanged();
                }
            }
        } 

        public string VisibleLayoutText
        {
            get { return arquivo == null ? "Visible" : "Collapsed"; }
            private set
            {
                if (visibleLayoutTxt != value)
                {
                    visibleLayoutTxt = value;
                    RaisePropertyChanged();
                }
            }
        }

        public string VisibleLayout
        {
            get { return arquivo != null ? "Visible" : "Collapsed"; }
            private set
            {
                if (visibleLayout != value)
                {
                    visibleLayout = value;
                    RaisePropertyChanged();
                }
            }
        }

        public bool BtnEnabled
        {
            get { return arquivo != null; }
            private set
            {
                if(btnEnabledForScan != value)
                {
                    btnEnabledForScan = value;
                    RaisePropertyChanged();
                }
            }
        }

        public byte[] GetGuid { get { return new byte[] { System.Convert.ToByte(DateTime.Now.ToString("ddMMyyyyHHmmss")) }; } }
    }
}
