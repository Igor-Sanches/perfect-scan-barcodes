using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Contacts;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace Perfect_Scan.Models
{
    public class Contatos
    {
        private Contact contato;
        private ImageSource imageContato;

        public Contatos(Contact contato)
        {
            this.contato = contato;
        }

        public async void SetImageAsync()
        {
            var ThumbnailReference = contato.Thumbnail;
            var thumbnailImage = new BitmapImage();
            if (ThumbnailReference != null)
            {
                using (IRandomAccessStreamWithContentType thumbnailStream = await ThumbnailReference.OpenReadAsync())
                {
                    thumbnailImage.SetSource(thumbnailStream);
                }
            }
            else
            {
                thumbnailImage = new BitmapImage(new Uri("ms-appx:///Assets/ContatoDefault.png", UriKind.Absolute));
            }
            this.imageContato = thumbnailImage;
        }

        public string DisplayName { get { return contato.DisplayName; } }
        public Contact Contato { get { return contato; } }
        public string Number
        {
            get
            {
                string numeros = "";
                for(int i =0;i< contato.Phones.Count; i++)
                {
                    numeros = contato.Phones[i].Number + ", ";
                }
                return numeros.EndsWith(", ") ? numeros.TrimEnd().Replace(",", "") : numeros;
            }
        }
        public ImageSource ContactImage
        {
            get
            {
                return imageContato;
            }
        }
        public string Id { get { return contato.Id; } }
    }
}
