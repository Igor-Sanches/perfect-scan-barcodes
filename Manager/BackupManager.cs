using Perfect_Scan.Data;
using Perfect_Scan.Database.Sql;
using Perfect_Scan.Models;
using Perfect_Scan.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.ApplicationModel.Resources;
using Windows.Security.ExchangeActiveSyncProvisioning;
using Windows.Storage;
using Windows.System.Profile;
using Windows.UI.Core;

namespace Perfect_Scan.Manager
{
    public class BackupManager
    {
        private ResourceLoader loader = new ResourceLoader();
        public static string RegexReplace = "_..._forreplace_..._";
        public static string RegexReplace2 = "_..._forreinNextplace_..._";
        private ControleEscaneados escaneados = new ControleEscaneados();
        public bool IsGeradosSelected;
        public bool IsEscaneadosSelected;
        private ControleGerados gerados = new ControleGerados();
         
        public async Task<bool> AdicionarAoExistentes(string path, bool isG, bool isE)
        {
            bool result = false;
            try
            {
                var file = await StorageFile.GetFileFromPathAsync(path);
                if (file != null)
                {
                    BackupHelper helper = new BackupHelper(file);
                    var xDoc = await helper.GetDocumentAsync();
                    if(xDoc != null)
                    {
                        if (isE)
                        {
                            List<Escaneados> lista_ = await helper.GetListaEscaneadosAsync();
                            if(lista_.Count > 0)
                            {
                                 await HistoricoManager.EscaneadoAsync(lista_, false);
                                result = true;
                            }
                        }
                        if (isG)
                        {
                            List<Gerados> lista_ = await helper.GetListaGeradosAsync();
                            if (lista_.Count > 0)
                            {
                                await HistoricoManager.GeradoAsync(lista_, false);
                                result = true;
                            }
                        }
                    }
                }
            }
            catch { }
            return result;
        }


        public async Task<bool> AdicionarRestaurar(string path, bool isG, bool isE)
        {
            bool result = false;
            try
            {
                var file = await StorageFile.GetFileFromPathAsync(path);
                if (file != null)
                {
                    BackupHelper helper = new BackupHelper(file);
                    var xDoc = await helper.GetDocumentAsync();
                    if (xDoc != null)
                    {
                        if (isE)
                        {
                            List<Escaneados> lista_ = await helper.GetListaEscaneadosAsync();
                            if (lista_.Count > 0)
                            {
                                await HistoricoManager.EscaneadoAsync(lista_, true);
                                result = true;
                            }
                        }
                        if (isG)
                        {
                            List<Gerados> lista_ = await helper.GetListaGeradosAsync();
                            if (lista_.Count > 0)
                            {
                                await HistoricoManager.GeradoAsync(lista_, true);
                                result = true;
                            }
                        }
                    }
                }
            }
            catch { }
            return result;
        }

        private string backupNameDefault
        {
            get
            {
                string data = DateTime.Now.ToString("dd_MM_yyyy HH mm");
                return $"Backup {data}";
            }
        }

        private int countGerados
        {
            get
            {
                var lista = gerados.ListaGerados();
                return lista.Count;
            }
        }

        private int countEscaneados
        {
            get
            {
                var lista = escaneados.ListaEscaneados();
                return lista.Count;
            }
        }

        private string Database()
        {
            string softSystem = GetInfoSoft("System");
            string softVersion = GetInfoSoft("Version");
            string deviceInfo = GetDevice();
            string data = DateTime.Now.ToString("dd/MM/yyyy HH:mm");
            string xmlInfoBackup = $"<{Key.ELEMENT_KEY_INFO} {Key.ELEMENT_KEY_SYSTEM}=\"{softSystem}\" {Key.ELEMENT_KEY_SYSTEM_VERSION}=\"{softVersion}\" {Key.ELEMENT_KEY_DATA}=\"{data}\" {Key.ELEMENT_KEY_DEVICE}=\"{deviceInfo}\" {Key.ELEMENT_KEY_COUNT_GERADOS}=\"{CountGeradosForEnabled}\" {Key.ELEMENT_KEY_COUNT_ESCANEADOS}=\"{CountEscaneadosForEnabled}\"/>";
            string xml =
                $"<{Key.ELEMENT_KEY_RAIZ}>\n" +
                xmlInfoBackup +
                $"<{Key.ELEMENT_KEY_DATA_BASE} {Key.ELEMENT_KEY_ESCANEADOS}=\"{IsEscaneadosSelected}\" {Key.ELEMENT_KEY_GERADOS}=\"{IsGeradosSelected}\">\n" +
                DatabaseEscaneados() +
                DatabaseGerados() +
                $"</{Key.ELEMENT_KEY_DATA_BASE}>\n" +
                $"</{Key.ELEMENT_KEY_RAIZ}>";

            return xml;
        }

        private string DatabaseGerados()
        {
            string xml = "";
            string id, formato, titulo, data, tipo, codigo;
            foreach(var x in gerados.ListaGerados())
            {
                id = x.ID.ToString();
                formato = x.Formato;
                data = x.Data;
                titulo = x.DisplayName.Replace("\"", RegexReplace);
                tipo = x.Tipo;
                codigo = x.Codigo.Replace("<", RegexReplace).Replace("&", RegexReplace2); xml = $"<{Key.ELEMENT_KEY_DATABASE_GERADOS} {Key.ELEMENT_KEY_DATABASE_ID}=\"{id}\" {Key.ELEMENT_KEY_DATABASE_TYPE}=\"{tipo}\" {Key.ELEMENT_KEY_DATABASE_FORMAT}=\"{formato}\" {Key.ELEMENT_KEY_DATABASE_TITULO}=\"{titulo}\" {Key.ELEMENT_KEY_DATABASE_DATA}=\"{data}\">{codigo}</{Key.ELEMENT_KEY_DATABASE_GERADOS}>\n";
            }
            return IsGeradosSelected ? xml : "";
        }
        private string DatabaseEscaneados()
        {
            string xml = "";
            string id, formato, titulo, data, tipo, codigo;
            foreach (var x in escaneados.ListaEscaneados())
            {
                id = x.ID.ToString();
                formato = x.Formato;
                data = x.Data;
                titulo = x.DisplayName.Replace("\"", RegexReplace);
                tipo = x.Tipo;
                codigo = x.Codigo.Replace("<", RegexReplace).Replace("&", RegexReplace2);
                xml = $"<{Key.ELEMENT_KEY_DATABASE_ESCANEADOS} {Key.ELEMENT_KEY_DATABASE_ID}=\"{id}\" {Key.ELEMENT_KEY_DATABASE_TYPE}=\"{tipo}\" {Key.ELEMENT_KEY_DATABASE_FORMAT}=\"{formato}\" {Key.ELEMENT_KEY_DATABASE_TITULO}=\"{titulo}\" {Key.ELEMENT_KEY_DATABASE_DATA}=\"{data}\">{codigo}</{Key.ELEMENT_KEY_DATABASE_ESCANEADOS}>\n";
            }
            return IsEscaneadosSelected ? xml : "";
        }

        private int CountGeradosForEnabled
        {
            get
            {
                return IsGeradosSelected ? countGerados : 0;
            }
        }

        private int CountEscaneadosForEnabled
        {
            get
            {
                return IsEscaneadosSelected ? countEscaneados : 0;
            }
        }

        private string GetDevice()
        {
            EasClientDeviceInformation eas = new EasClientDeviceInformation();
            return $"{eas.SystemProductName}";//$"{eas.SystemManufacturer} {eas.SystemProductName}";
        }

        private string GetInfoSoft(string str)
        {
            EasClientDeviceInformation eas = new EasClientDeviceInformation();
            var sv = AnalyticsInfo.VersionInfo.DeviceFamilyVersion;
            ulong v = ulong.Parse(sv);
            ulong v1 = (v & 0xFFFF000000000000L) >> 48;
            ulong v2 = (v & 0x0000FFFF00000000L) >> 32;
            ulong v3 = (v & 0x00000000FFFF0000L) >> 16;
            ulong v4 = (v & 0x000000000000FFFFL);

            return str.Equals("System") ? $"{eas.OperatingSystem}" : $"{v1}.{v2}.{v3}.{v4}";
        }

        public BackupManager()
        {

        }

        public bool IsGerados
        {
            get
            {
                var lista = gerados.ListaGerados();
                return lista.Count > 0;
            }
        }

        public bool IsEscaneados
        {
            get
            {
                var lista = escaneados.ListaEscaneados();
                return lista.Count > 0;
            }
        }

       
        internal async void CriarBakup(string backupName, bool isCheckedG, bool isCheckedS, ViewModel.Backups.BackupsCreateViewModel vm)
        {
            IsGeradosSelected = isCheckedG;
            IsEscaneadosSelected = isCheckedS;
            string xml = Database();
            if(xml != null)
            {
                var folder = await AppStorage.GetBackupFolderDefaulAsync();
                if(folder != null)
                {
                    StorageFile file = null;
                    try
                    {
                        file = await folder.CreateFileAsync($"{backupName}.oic", CreationCollisionOption.GenerateUniqueName);
                    }
                    catch
                    {
                        file = await folder.CreateFileAsync($"{backupNameDefault}.oic", CreationCollisionOption.GenerateUniqueName);
                    }

                    if(file != null)
                    {
                        try
                        {
                            await FileIO.WriteTextAsync(file, xml).AsTask().ContinueWith(async p =>
                            {
                                if (p.IsCompleted || p.IsCompletedSuccessfully)
                                {
                                    await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                                    {
                                        Toast(loader.GetString("backupSuccefuly"), 0);
                                        vm.BtnState(); 
                                    });
                                }else if (p.IsFaulted || p.IsCanceled)
                                {
                                    await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                                    {
                                        Toast(loader.GetString("backupFalha"), 1);
                                        vm.BtnState();
                                    });

                                }
                            }); 
                        }
                        catch (Exception x)
                        {
                            Toast(x.Message, 1);
                        }
                    }
                    else
                    {
                        Toast(loader.GetString("backupFalha"), 1);

                    }
                }
                else
                {
                    Toast(loader.GetString("backupFalha"), 1);

                }
            }
            else
            {
                Toast(loader.GetString("backupFalha"), 1);

            }
        }

        private void Toast(string str, int modo)
        {
            Paginas.Root.RootApp.Instance.GetToast(str, modo == 0 ? ModoColor.Succes : ModoColor.Error);
        }
    }
}
