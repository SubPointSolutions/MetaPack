using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using MetaPack.Core.Consts;
using MetaPack.NuGet.Extensions;
using MetaPack.NuGet.Utils;
using Microsoft.SharePoint.Client;
using NuGet;
using File = Microsoft.SharePoint.Client.File;

namespace MetaPack.NuGet.Services
{
    public class SharePointCSOMFileSystem : IFileSystem
    {
        #region consturctors
        public SharePointCSOMFileSystem(ClientContext context)
        {
            if (!context.Web.IsPropertyAvailable("Url"))
            {
                context.Load(context.Web, w => w.Url);
                context.ExecuteQuery();
            }

            Root = context.Web.Url;
            LibraryUrl = MetaPackConsts.SharePointLibraryUrl;

            _context = context;

            EnsureMetapackLibrary();
        }

        #endregion

        #region properties

        public ILogger Logger
        {
            get
            {
                return this._logger ?? NullLogger.Instance;
            }
            set
            {
                this._logger = value;
            }
        }

        public string Root { get; set; }

        private ILogger _logger { get; set; }

        public string LibraryUrl { get; set; }

        private ClientContext _context;
        private Dictionary<string, List<VirtualFile>> _cachedPathToFiles = new Dictionary<string, List<VirtualFile>>();

        #endregion

        #region methods

        private void InvalidatePathToFilesCache(string path)
        {
            path = path.ToLower();

            if (_cachedPathToFiles.ContainsKey(path))
                _cachedPathToFiles.Remove(path);
        }

        public void AddFile(string path, Action<Stream> writeToStream)
        {
            throw new NotImplementedException();
        }

        public void AddFile(string path, Stream stream)
        {
            var folder = LookupFolderByPath("", true);

            AddFileToSharePoint(folder, path, stream);
        }

        public void AddFiles(IEnumerable<IPackageFile> files, string rootDir)
        {
            var folder = LookupFolderByPath(rootDir, true);

            foreach (var file in files)
            {
                AddFileToSharePoint(folder, file.Path, file.GetStream());
            }
        }

        public static void CopyStream(Stream input, Stream output)
        {
            byte[] buffer = new byte[32768];
            int read;
            while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
            {
                output.Write(buffer, 0, read);
            }
        }

        private void AddFileToSharePoint(Folder rootFolder, string filePath, Stream fileStream)
        {
            if (filePath.ToLower().EndsWith(".nupkg"))
            {

                var clonedStream = new MemoryStream();

                CopyStream(fileStream, clonedStream);

                fileStream.Position = 0;
                clonedStream.Position = 0;

                var package = new ZipPackage(clonedStream);
            }

            //LogHelper.Log("Adding file to SharePoint: " + filePath);

            var context = rootFolder.Context;

            var fileName = Path.GetFileName(filePath);
            var fileFolderPath = Path.GetDirectoryName(filePath);

            InvalidatePathToFilesCache(fileFolderPath);

            var fileFolder = EnsureFolder(context, rootFolder, fileFolderPath);

            fileFolder.Files.Add(new FileCreationInformation
            {
                ContentStream = fileStream,
                Overwrite = true,
                Url = fileName
            });

            context.ExecuteQuery();

            //LogHelper.Log("Added file to SharePoint: " + filePath);
        }
        public Folder EnsureFolder(ClientRuntimeContext context, Folder ParentFolder, string FolderPath)
        {
            if (string.IsNullOrEmpty(FolderPath))
                return ParentFolder;

            //Split up the incoming path so we have the first element as the a new sub-folder name 
            //and add it to ParentFolder folders collection
            string[] PathElements = FolderPath.Split(new char[] { '/', '\\' }, StringSplitOptions.RemoveEmptyEntries);
            string Head = PathElements[0];

            var NewFolder = ParentFolder.Folders.GetByUrl(Head);

            try
            {
                context.ExecuteQuery();
            }
            catch (Exception e)
            {
                NewFolder = ParentFolder.Folders.Add(Head);
                context.Load(NewFolder);
                try
                {
                    context.ExecuteQuery();
                }
                catch (Exception eee)
                {
                    throw;
                }
            }

            //If we have subfolders to create then the length of PathElements will be greater than 1
            if (PathElements.Length > 1)
            {
                //If we have more nested folders to create then reassemble the folder path using what we have left i.e. the tail
                string Tail = string.Empty;
                for (int i = 1; i < PathElements.Length; i++)
                    Tail = Tail + "/" + PathElements[i];

                //Then make a recursive call to create the next subfolder
                return EnsureFolder(context, NewFolder, Tail);
            }
            else
                //This ensures that the folder at the end of the chain gets returned
                return NewFolder;
        }

        private Folder LookupFolderByPath(string path, bool create = false)
        {

            LogUtils.Log("Fetching folders for path: " + path);

            Folder result = null;

            WithSPContext(context =>
            {
                var web = context.Web;

                var nuGetFolderUrl = UrlUtility.CombineUrl(Root, LibraryUrl);
                var targetFolderUrl = UrlUtility.CombineUrl(nuGetFolderUrl, path);



                try
                {
                    result = web.GetFolderByServerRelativeUrl(targetFolderUrl);

                    context.Load(result);
                    context.ExecuteQuery();

                    if (result.ServerObjectIsNull.HasValue && result.ServerObjectIsNull.Value)
                    {
                        throw new Exception("ServerObjectIsNull is null. Folrder does not exist.");
                    }
                }
                catch (Exception ex)
                {
                    if (create)
                    {
                        var rootFolder = web.GetFolderByServerRelativeUrl(nuGetFolderUrl);
                        rootFolder.Folders.Add(path);

                        context.ExecuteQuery();

                        result = web.GetFolderByServerRelativeUrl(targetFolderUrl);
                        context.ExecuteQuery();
                    }
                    else
                    {
                        result = null;
                    }
                }
            });

            LogUtils.Log("Fetched folders for path: " + path);

            return result;
        }
        public Stream CreateFile(string path)
        {
            throw new NotImplementedException();
        }

        public void DeleteDirectory(string path, bool recursive)
        {
            WithSPContext(context =>
            {
                var isRootFolder = string.IsNullOrEmpty(path);
                var nuGetFolderUrl = UrlUtility.CombineUrl(Root, LibraryUrl);

                if (!isRootFolder)
                    nuGetFolderUrl = UrlUtility.CombineUrl(nuGetFolderUrl, path);

                var web = context.Web;
                var exists = false;

                var folder = web.GetFolderByServerRelativeUrl(nuGetFolderUrl);

                try
                {
                    context.ExecuteQuery();
                    exists = true;
                }
                catch (Exception ex)
                { }

                if (exists)
                {
                    folder.DeleteObject();
                    context.ExecuteQuery();
                }
            });
        }
        public void DeleteFile(string path)
        {
            WithFile(path, spFile =>
            {
                if (spFile != null)
                {
                    spFile.DeleteObject();
                    spFile.Context.ExecuteQuery();
                }
            });
        }

        public void DeleteFiles(IEnumerable<IPackageFile> files, string rootDir)
        {
            foreach (var file in files)
            {
                var fullPath = Path.Combine(rootDir, file.Path);

                DeleteFile(fullPath);

                //var virtualFile = _cachedPathToFiles.Values.First()
            }
        }

        public bool DirectoryExists(string path)
        {
            throw new NotImplementedException();
        }

        public void WithFile(string path, Action<File> action)
        {
            WithSPContext(context =>
            {
                var web = context.Web;

                if (!web.IsPropertyAvailable("ServerRelativeUrl"))
                {
                    context.Load(web, w => w.ServerRelativeUrl);
                    context.ExecuteQuery();
                }

                var rootFolderUrl = UrlUtility.CombineUrl(web.ServerRelativeUrl, LibraryUrl);

                var filePath = UrlUtility.CombineUrl(rootFolderUrl, path);
                var file = web.GetFileByServerRelativeUrl(filePath);

                context.Load(file);
                try
                {
                    context.ExecuteQuery();
                    action(file);
                }
                catch (Exception e)
                {
                    action(null);
                }
            });
        }
        public bool FileExists(string path)
        {
            var result = false;

            WithFile(path, file =>
            {
                result = (file != null) && (file.Exists);
            });

            return result;
        }
        public DateTimeOffset GetCreated(string path)
        {
            throw new NotImplementedException();
        }
        public IEnumerable<string> GetDirectories(string path)
        {
            return GetFoldersForPath(path);
        }
        private List<string> GetFoldersForPath(string path)
        {
            var result = new List<string>();

            WithSPContext(context =>
            {
                var isRootFolder = string.IsNullOrEmpty(path);
                var nuGetFolderUrl = UrlUtility.CombineUrl(Root, LibraryUrl);

                if (!isRootFolder)
                    nuGetFolderUrl = UrlUtility.CombineUrl(nuGetFolderUrl, path);

                var web = context.Web;
                var exists = false;

                var folder = web.GetFolderByServerRelativeUrl(nuGetFolderUrl);

                try
                {
                    context.ExecuteQuery();
                    exists = true;
                }
                catch (Exception ex)
                { }

                if (exists)
                {
                    var folderCollection = folder.Folders;

                    context.Load(folderCollection);
                    context.ExecuteQuery();

                    foreach (var f in folderCollection)
                    {
                        result.Add(f.Name);
                    }

                    if (result.Count == 0 && isRootFolder)
                    {
                        result.Add("/");
                    }
                }
            });

            return result;
        }
        protected void WithSPContext(Action<ClientContext> action)
        {
            if (action != null)
                action(_context);
        }
        public IEnumerable<string> GetFiles(string path, string filter, bool recursive)
        {
            var hasFolderMatch = Regex.IsMatch(path.ToLower(), WildCardToRegular(filter.ToLower().Replace(".nupkg", string.Empty)).ToLower());

            if (!hasFolderMatch)
            {
                return Enumerable.Empty<string>();
            }

            LogUtils.Log("Fetching files for path with filter: " + filter + " : " + path);


            if (recursive)
                throw new NotSupportedException("recursive = true");

            var isRootFolder = string.IsNullOrEmpty(path);

            path = path.ToLower();
            var files = new List<VirtualFile>();

            WithSPContext(context =>
            {
                var nuGetFolderUrl = UrlUtility.CombineUrl(Root, LibraryUrl);

                if (!isRootFolder)
                    nuGetFolderUrl = UrlUtility.CombineUrl(nuGetFolderUrl, path);

                var web = context.Web;
                var exists = false;

                var folder = web.GetFolderByServerRelativeUrl(nuGetFolderUrl);

                try
                {
                    context.ExecuteQuery();
                    exists = true;
                }
                catch (Exception ex)
                {
                }

                if (exists)
                {
                    files = new List<VirtualFile>();

                    if (!_cachedPathToFiles.ContainsKey(path))
                        _cachedPathToFiles.Add(path, files);
                    else
                        files = _cachedPathToFiles[path];

                    var items = folder.Files;

                    context.Load(items);
                    context.ExecuteQuery();

                    foreach (var f in items)
                    {
                        files.Add(new VirtualFile
                        {
                            Path = f.Name,
                            LastModified = f.TimeLastModified,
                            RelativePath = Path.Combine(path, f.Name)
                        });
                    }
                }
                else
                {

                }
            });

            var result = new List<string>();

            foreach (var file in files)
            {
                if (string.IsNullOrEmpty(filter))
                {
                    result.Add(file.Path);
                }
                else
                {
                    var fileName = file.Path;
                    var fileMatch = Regex.IsMatch(fileName.ToLower(), WildCardToRegular(filter).ToLower());

                    if (fileMatch)
                        result.Add(fileName);
                }
            }

            // make relative path
            if (result.Any())
            {
                result = result.Select(p => Path.Combine(path, p)).ToList();
            }

            LogUtils.Log("Fetched files for path with filter: " + filter + " : " + path);

            return result;
        }

        private class VirtualFile
        {
            public string Path { get; set; }
            public DateTime LastModified { get; set; }

            public string RelativePath { get; set; }
        }

        private static String WildCardToRegular(String value)
        {
            return ("^" + Regex.Escape(value).Replace("\\*", ".*") + "$");
        }

        public string GetFullPath(string path)
        {
            throw new NotImplementedException();
        }

        public DateTimeOffset GetLastAccessed(string path)
        {
            throw new NotImplementedException();
        }

        public DateTimeOffset GetLastModified(string path)
        {
            var targetFile = _cachedPathToFiles.Values.SelectMany(s => s)
                 .FirstOrDefault(f => f.RelativePath.ToLower() == path.ToLower());

            if (targetFile != null)
            {
                return new DateTimeOffset(targetFile.LastModified);
            }
            else
            {
                throw new ArgumentException("Can't find file for the path: " + path);
            }
        }

        public void MakeFileWritable(string path)
        {
            throw new NotImplementedException();
        }

        public void MoveFile(string source, string destination)
        {
            throw new NotImplementedException();
        }

        public Stream OpenFile(string path)
        {
            LogUtils.Log("Opening file for path: " + path);

            Stream result = null;

            WithSPContext(context =>
            {
                var web = context.Web;

                if (!web.IsPropertyAvailable("ServerRelativeUrl"))
                {
                    context.Load(web, w => w.ServerRelativeUrl);
                    context.ExecuteQuery();
                }

                var rootFolderUrl = UrlUtility.CombineUrl(web.ServerRelativeUrl, LibraryUrl);
                var fileUrl = UrlUtility.CombineUrl(rootFolderUrl, path);

                var file = web.GetFileByServerRelativeUrl(fileUrl);
                var q = file.OpenBinaryStream();

                context.Load(file);
                context.ExecuteQuery();

                result = q.Value;

                var memoryStream = new MemoryStream();

                CopyStream(q.Value, memoryStream);
                memoryStream.Position = 0;

                result = memoryStream;

            });

            LogUtils.Log("Opened file for path: " + path);

            return result;
        }

        #endregion

        #region gallery model

        protected virtual void EnsureMetapackLibrary()
        {
            // we may consider using SPMeta2 model provision later on

            LogUtils.Log("Ensuring MetaPack list");

            var web = _context.Web;
            var list = WebExtensions.LoadListByUrl(web, MetaPackConsts.SharePointLibraryUrl);

            if (list == null)
            {
                LogUtils.Log("MetaPack list does not exist. Creating new one..");
                var newList = web.Lists.Add(new ListCreationInformation
                {
                    TemplateType = (int)ListTemplateType.DocumentLibrary,
                    Title = MetaPackConsts.SharePointLibraryTitle,
                    Url = MetaPackConsts.SharePointLibraryUrl,
                    Description = "MetaPack Gallery library. Stores NuGet packages with SharePoint solutions."
                });

                newList.Hidden = true;
                newList.OnQuickLaunch = false;
                newList.NoCrawl = true;

                newList.Update();
                _context.ExecuteQuery();

                LogUtils.Log("MetaPack list created.");
            }
            else
            {
                LogUtils.Log("MetaPack list exists");
            }
        }

        #endregion
    }
}
