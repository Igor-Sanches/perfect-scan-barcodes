using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Search;

namespace Perfect_Scan.Tools
{
    public class AppStorage
    {

        public static async Task<List<StorageFile>> GetFilesBackupsAsync()
        {
            try
            {
                List<StorageFile> lista = new List<StorageFile>();

                var folderApp = await KnownFolders.PicturesLibrary.GetFolderAsync("Perfect Scan");
                if(folderApp != null)
                {
                    List<string> filder = new List<string>();
                    filder.Add(".oic");
                    filder.Add(".OIC");

                    QueryOptions options = new QueryOptions(CommonFileQuery.OrderBySearchRank, filder);
                    StorageFileQueryResult result = folderApp.CreateFileQueryWithOptions(options);

                    IReadOnlyList<StorageFile> files = await result.GetFilesAsync();
                    
                    foreach (var file in files)
                    {
                        if (file.DisplayType == "Perfect Scan Backup")
                        {
                            lista.Add(file);
                             
                        }
                    }

                    //Paginas.Root.RootApp.Instance.GetToast("" + files.Count);
                } 
                return lista;
            } 
            catch(Exception x) { Paginas.Root.RootApp.Instance.GetToast(x.Message); return null; }
        }

        public static async Task<StorageFolder> GetBackupFolderAsync()
        {
            try
            {
                var folder_ = await GetAppFolderAsync();
                var folder = await folder_.CreateFolderAsync("Backups", CreationCollisionOption.OpenIfExists);
                return folder_;
            }
            catch { return null; }
        }

        public static async Task<StorageFolder> GetBackupFolderDefaulAsync()
        {
            try
            {
                var folder_ = await GetAppFolderAsync();
                var folder = await folder_.CreateFolderAsync("Backups", CreationCollisionOption.OpenIfExists);
                return folder;
            }
            catch { return null; }
        }

        public static async Task<StorageFolder> GetAppFolderAsync()
        {
            try
            {
                var folder = await KnownFolders.PicturesLibrary.CreateFolderAsync("Perfect Scan", CreationCollisionOption.OpenIfExists);
                return folder;
            }
            catch { return null; }
        }

        public static async Task<StorageFolder> GetAppDataCacherAsync(string cacheName)
        {
            try
            {
                return await ApplicationData.Current.LocalCacheFolder.CreateFolderAsync(cacheName, CreationCollisionOption.OpenIfExists);
            }
            catch { return null; }
        }

        public static async Task<StorageFile> GetAppDataTemporaryFileAsync(string cacheName, bool replace)
        {
            try
            {
                var file = await ApplicationData.Current.TemporaryFolder.CreateFileAsync(cacheName, replace ? CreationCollisionOption.ReplaceExisting : CreationCollisionOption.GenerateUniqueName);
                return file;
            }
            catch { return null; }
        }

        internal static async Task<List<StorageFile>> GetFilesBackupsDefaultAsync()
        {
            try
            {
                List<StorageFile> lista = new List<StorageFile>();
                var folder = await GetBackupFolderAsync();
                var files = await folder.GetFilesAsync();
                foreach (var file in files)
                {
                    if (file.Name.EndsWith(".oic") || file.Name.EndsWith(".OIC"))
                    {
                        foreach (var f in lista)
                        {
                            if (!file.Name.Equals(f.Name))
                            {
                                lista.Add(file);
                            }
                        }
                    }
                }
                return lista;
            }
            catch { return null; }
        }
    }
}
