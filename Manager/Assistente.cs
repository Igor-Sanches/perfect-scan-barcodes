using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Media.SpeechSynthesis;
using Windows.UI.Xaml.Controls;
using ZXing;

namespace Perfect_Scan.Manager
{
    public class Assistente
    {
        private SpeechSynthesizer synthesizer;
        private MediaElement mediaPlayer;

        public Assistente(MediaElement mediaPlayer)
        {
            synthesizer = new SpeechSynthesizer();
            this.mediaPlayer = mediaPlayer;
        }

        public async void Narrar(Result result)
        {
            if(result != null)
            {
                if (!result.Text.Trim().Equals(""))
                {
                    if(mediaPlayer.CurrentState == Windows.UI.Xaml.Media.MediaElementState.Playing)
                    {
                        mediaPlayer.Stop();
                    } 
                        try
                        {
                            string ler = "";
                            switch(new Settings.Settings().GetTipoCodigoValue)
                            {
                                case 0: ler = CodigoRetorno.GetTipeName(CodigoRetorno.GetResultado(result.Text, result.BarcodeFormat.ToString())); break;
                                case 1: ler = CodigoRetorno.GetName(result.BarcodeFormat.ToString()); break;
                                case 2: ler = result.Text; break;
                            }
                            SpeechSynthesisStream synthesisStream = await synthesizer.SynthesizeTextToStreamAsync(ler);
                            mediaPlayer.AutoPlay = true;
                            mediaPlayer.SetSource(synthesisStream, synthesisStream.ContentType);
                            mediaPlayer.Play();
                        }
                        catch
                        {

                        } 
                }
            }
        }
    }
}
