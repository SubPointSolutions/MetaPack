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
using System.IO.Compression;
using System.Text;
using System.Xml.Linq;
using MetaPack.Core.Exceptions;

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
            NuGetLogUtils.Verbose(string.Format("Saved file [{0}] to SharePoint", filePath));

            MemoryStream clonedNuPkgStream = null;
            MemoryStream clonedZipNuPkgStream = null;

            try
            {

                if (filePath.ToLower().EndsWith(".nupkg"))
                {
                    clonedNuPkgStream = new MemoryStream();
                    clonedZipNuPkgStream = new MemoryStream();

                    CopyStream(fileStream, clonedNuPkgStream);
                    fileStream.Position = 0;

                    CopyStream(fileStream, clonedZipNuPkgStream);
                    fileStream.Position = 0;

                    clonedNuPkgStream.Position = 0;
                    clonedZipNuPkgStream.Position = 0;
                }
                else
                {
                    return;
                }

                var context = rootFolder.Context;

                var fileName = Path.GetFileName(filePath);
                var fileFolderPath = Path.GetDirectoryName(filePath);

                InvalidatePathToFilesCache(fileFolderPath);

                var fileFolder = EnsureFolder(context, rootFolder, fileFolderPath);

                var file = fileFolder.Files.Add(new FileCreationInformation
                {
                    ContentStream = fileStream,
                    Overwrite = true,
                    Url = fileName
                });

                context.ExecuteQuery();

                if (clonedNuPkgStream != null)
                {
                    NuGetLogUtils.Verbose("Saving .nupkg metadata to SharePoint...");

                    var zipPackage = new ZipPackage(clonedNuPkgStream);
                    var zipArchive = new System.IO.Compression.ZipArchive(clonedZipNuPkgStream);

                    NuGetLogUtils.Info("Fetching nuspec file...");

                    var manifestFileName = string.Format("{0}.nuspec", zipPackage.Id);
                    var manifestFile = zipArchive.GetEntry(manifestFileName);

                    var nuspecXmlValue = string.Empty;

                    using (var stramReader = new StreamReader(manifestFile.Open()))
                    {
                        nuspecXmlValue = stramReader.ReadToEnd();
                    }

                    NuGetLogUtils.Verbose("Updating nuspec and version values...");
                    if (!string.IsNullOrEmpty(nuspecXmlValue))
                    {
                        // update properties
                        var fileItem = file.ListItemAllFields;

                        context.Load(fileItem);

                        fileItem.ParseAndSetFieldValue("PackageNuspecXml", nuspecXmlValue);
                        fileItem.ParseAndSetFieldValue("PackageVersion", zipPackage.Version.ToString());
                        fileItem.ParseAndSetFieldValue("PackageId", zipPackage.Id);

                        fileItem.Update();
                        context.ExecuteQuery();
                    }
                }

                NuGetLogUtils.Verbose(string.Format("Saved file [{0}] to SharePoint", filePath));
            }
            finally
            {
                if (clonedNuPkgStream != null)
                {
                    clonedNuPkgStream.Dispose();
                    clonedNuPkgStream = null;
                }

                if (clonedZipNuPkgStream != null)
                {
                    clonedZipNuPkgStream.Dispose();
                    clonedZipNuPkgStream = null;
                }
            }
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

            NuGetLogUtils.Verbose("Fetching folders for path: " + path);

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

            NuGetLogUtils.Verbose("Fetched folders for path: " + path);

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

        public bool ShadowFileExists(string path)
        {
            var targetFile = _shadowFiles.FirstOrDefault(f => f.Path == path);

            if (targetFile != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool FileExists(string path)
        {
            return ShadowFileExists(path);

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
            InitShadowFileSystem();
            return GetShadowFoldersForPath(path);

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

        protected virtual void InitShadowFileSystem()
        {
            // essentially, a cache over list items to avoid repeable queries
            WithSPContext(context =>
            {
                var web = context.Web;
                var list = LoadListByUrl(context.Web, LibraryUrl);

                var camlQuery = new CamlQuery()
                {
                    ViewXml = string.Format("<View Scope='RecursiveAll'>{0}</View>", Strings.NuPkgCamlQuery)
                };

                var files = list.GetItems(camlQuery);

                context.Load(files);
                context.ExecuteQuery();

                foreach (var file in files)
                {
                    var filePath = (string)file["FileRef"];
                    var created = (DateTime)file["Created"];
                    var version = (string)file["PackageVersion"];
                    var id = (string)file["PackageId"];
                    //var nuspecXml = (string)file["NuspecXml"];

                    //filePath = filePath.Replace("/" + LibraryUrl, string.Empty)
                    //                   .ToLower();
                    filePath = filePath.ToLower();

                    var existingShadowFile = _shadowFiles.FirstOrDefault(f => f.Path == filePath);

                    if (existingShadowFile == null)
                    {
                        existingShadowFile = new VirtualFile
                        {
                            Path = filePath,
                            Created = created,
                            PackageVersion = version,
                            PackageId = id,
                            PackageNuSpecXml = string.Empty,
                        };

                        _shadowFiles.Add(existingShadowFile);
                    }
                    else
                    {
                        existingShadowFile.Path = filePath;
                        existingShadowFile.Created = created;
                    }
                }
            });

            var packageIds = _shadowFiles.GroupBy(f => f.PackageId)
                                         .Where(s => !string.IsNullOrEmpty(s.Key))
                                         .Select(s => s.Key)
                                         .Distinct();

            var topPackages = new List<VirtualFile>();

            foreach (var packageId in packageIds)
            {
                var topPackage = _shadowFiles.Where(f => f.PackageId == packageId)
                                              .OrderByDescending(f =>
                                              {
                                                  if (!string.IsNullOrEmpty(f.PackageVersion))
                                                  {
                                                      return new SemanticVersion(f.PackageVersion);
                                                  }

                                                  return new SemanticVersion("0.0.0.0");
                                              })
                                              .FirstOrDefault();

                if (topPackage != null)
                    topPackages.Add(topPackage);
            }

            topPackages = topPackages.OrderBy(p => p.PackageId)
                                     .ToList();

            // trim to get only latest versions
            _shadowFiles = topPackages.ToList();
        }

        public List<string> GetShadowFoldersForPath(string path)
        {
            var result = new List<string>();

            path = path.ToLower();

            foreach (var shadowFilePath in _shadowFiles.Select(p => p.Path))
            {
                //if (!shadowFilePath.StartsWith(path))
                //    continue;

                //var localPath = shadowFilePath;

                //if (!string.IsNullOrEmpty(path))
                //    localPath = shadowFilePath.Replace(path, string.Empty);

                //var localFolderPath = localPath.Trim('/')
                //                               .Split('/')
                //                               .FirstOrDefault();

                //if (!string.IsNullOrEmpty(localFolderPath))
                //    result.Add(localFolderPath);

                if (shadowFilePath.Contains('/'))
                {
                    var parts = shadowFilePath.Split('/').ToList();
                    parts.RemoveAt(parts.Count - 1);

                    result.Add(string.Join("/", parts));
                }
                else
                {
                    result.Add(shadowFilePath);
                }
            }

            result = result.Distinct()
                           .OrderBy(s => s)
                           .ToList();


            return result;
        }

        public List<string> GetShadowFiles(string path, string filter, bool recursive)
        {
            if (!path.StartsWith("/"))
                path = "/" + path;

            var result = new List<string>();

            var paths = _shadowFiles.Select(f => f.Path);
            var targetPaths = paths.Where(p => p.StartsWith(path));


            foreach (var targetPath in targetPaths)
            {
                var fileName = targetPath;
                var fileMatch = false;

                if (filter.Contains("*"))
                    fileMatch = Regex.IsMatch(fileName.ToLower(), WildCardToRegular(filter).ToLower());
                else
                    fileMatch = Regex.IsMatch(fileName.ToLower(), filter.ToLower());

                if (fileMatch)
                {
                    //result.Add(path + "/" + Path.GetFileName(targetPath));
                    result.Add(targetPath);
                }
            }

            result = result.OrderBy(s => s)
                           .ToList();

            //var subPaths = _shadowFiles.Where(p => p.)

            return result;
        }

        private List LoadListByUrl(Web web, string listUrl)
        {
            var ctx = web.Context;
            var listFolder = web.GetFolderByServerRelativeUrl(listUrl);

            ctx.Load(listFolder.Properties);
            ctx.ExecuteQuery();

            var listId = new Guid(listFolder.Properties["vti_listname"].ToString());
            var list = web.Lists.GetById(listId);

            ctx.Load(list);
            ctx.ExecuteQuery();

            return list;
        }

        public IEnumerable<string> GetFiles(string path, string filter, bool recursive)
        {
            return GetShadowFiles(path, filter, recursive);

            var hasFolderMatch = Regex.IsMatch(path.ToLower(), WildCardToRegular(filter.ToLower().Replace(".nupkg", string.Empty)).ToLower());

            if (!hasFolderMatch)
            {
                return Enumerable.Empty<string>();
            }

            NuGetLogUtils.Verbose(string.Format("Fetching files. Path:[{0}] Filter:[{1}] Recursive:[{2}]", path, filter, recursive));

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

            NuGetLogUtils.Verbose("Fetched files for path with filter: " + filter + " : " + path);

            return result;
        }


        private List<VirtualFile> _shadowFiles = new List<VirtualFile>();

        private class VirtualFile
        {
            public string Path { get; set; }
            public DateTime LastModified { get; set; }
            public DateTime Created { get; set; }

            public string RelativePath { get; set; }
            public string PackageVersion { get; set; }
            public string PackageId { get; set; }

            public string PackageNuSpecXml { get; set; }
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

        public DateTimeOffset GetLastShadowModified(string path)
        {
            var targetFile = _shadowFiles.FirstOrDefault(f => f.Path.Contains(path));

            if (targetFile != null)
            {
                return new DateTimeOffset(targetFile.Created);
            }
            else
            {
                throw new ArgumentException("Can't find file for the path: " + path);
            }
        }

        public DateTimeOffset GetLastModified(string path)
        {
            return GetLastShadowModified(path);


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
            NuGetLogUtils.Verbose("Opening file for path: " + path);

            Stream result = null;

            WithSPContext(context =>
            {
                var web = context.Web;

                if (!web.IsPropertyAvailable("ServerRelativeUrl"))
                {
                    context.Load(web, w => w.ServerRelativeUrl);
                    context.ExecuteQuery();
                }

                var rootFolderUrl = UrlUtility.CombineUrl(web.ServerRelativeUrl, LibraryUrl).ToLower();

                // /sites/ci-121/metapack-gallery/metapack.sharepointpnp.ci.1.2017.0414.0657/metapack.sharepointpnp.ci.1.2017.0414.0657.nupkg
                var folderRelatedFileUrl = path;

                if (folderRelatedFileUrl.Contains(rootFolderUrl))
                    folderRelatedFileUrl = folderRelatedFileUrl.Split(new string[] { rootFolderUrl }, StringSplitOptions.None)[1];

                var fileUrl = UrlUtility.CombineUrl(rootFolderUrl, folderRelatedFileUrl);

                // try file item first, shadow this for optimization

                NuGetLogUtils.Verbose("Opening file for path: " + fileUrl);
                var file = web.GetFileByServerRelativeUrl(fileUrl);
                var item = file.ListItemAllFields;

                context.Load(item);
                context.ExecuteQuery();

                if (item == null || (item.ServerObjectIsNull.HasValue && item.ServerObjectIsNull.Value))
                {
                    throw new MetaPackException(string.Format(
                        "Cannot find file by server relative path:[{0}]",
                        fileUrl));
                }

                var nuspecXml = (string)item["PackageNuspecXml"];

                var useNuspecXml = true;

                // faking nuget package here to improve performance
                // PackageNuspecXml stores nuget package manifest
                // in that case, NuGet does not download the whole package from SharePoint!
                if (!string.IsNullOrEmpty(nuspecXml) && useNuspecXml)
                {
                    NuGetLogUtils.Verbose("Using PackageNuspecXml...");

                    var nuspecXmlDoc = XDocument.Parse(nuspecXml);
                    var packagingService = new FakeNuGetSolutionPackageService();

                    using (var stream = new MemoryStream())
                    {
                        using (var writer = new StreamWriter(stream))
                        {
                            var fakeSolutionPackageBase = (new Core.Packaging.SolutionPackageBase
                            {
                                Authors = GetNuGetPackageMetadataValue(nuspecXmlDoc, "Authors"),
                                Company = GetNuGetPackageMetadataValue(nuspecXmlDoc, "Company"),
                                Copyright = GetNuGetPackageMetadataValue(nuspecXmlDoc, "Copyright"),
                                Description = GetNuGetPackageMetadataValue(nuspecXmlDoc, "Description"),
                                IconUrl = GetNuGetPackageMetadataValue(nuspecXmlDoc, "IconUrl"),
                                Id = GetNuGetPackageMetadataValue(nuspecXmlDoc, "Id"),
                                LicenseUrl = GetNuGetPackageMetadataValue(nuspecXmlDoc, "LicenseUrl"),
                                Name = GetNuGetPackageMetadataValue(nuspecXmlDoc, "Name"),
                                Owners = GetNuGetPackageMetadataValue(nuspecXmlDoc, "Owners"),
                                ProjectUrl = GetNuGetPackageMetadataValue(nuspecXmlDoc, "ProjectUrl"),
                                ReleaseNotes = GetNuGetPackageMetadataValue(nuspecXmlDoc, "ReleaseNotes"),
                                Summary = GetNuGetPackageMetadataValue(nuspecXmlDoc, "Summary"),
                                Tags = GetNuGetPackageMetadataValue(nuspecXmlDoc, "Tags"),
                                Title = GetNuGetPackageMetadataValue(nuspecXmlDoc, "Title"),
                                Version = GetNuGetPackageMetadataValue(nuspecXmlDoc, "Version")
                            });

                            // TODO, dependencies

                            result = packagingService.Pack(fakeSolutionPackageBase);
                        }
                    }

                    return;
                }

                NuGetLogUtils.Verbose("Using raw file content...");

                // fallback on the actual file
                // that's slow, NuGet download the whole package to get the metadata 
                var q = file.OpenBinaryStream();

                context.Load(file);
                context.ExecuteQuery();

                result = q.Value;

                var memoryStream = new MemoryStream();

                CopyStream(q.Value, memoryStream);
                memoryStream.Position = 0;

                result = memoryStream;
            });

            NuGetLogUtils.Verbose("Opened file for path: " + path);

            return result;
        }

        #endregion

        #region gallery model

        private string GetNuGetPackageMetadataValue(XDocument nuspecXmlDoc, string name)
        {
            var node = nuspecXmlDoc.Root.Descendants()
                                        .FirstOrDefault(n => n.Name.LocalName.ToLower() == name.ToLower());

            if (node != null)
                return node.Value;

            return string.Empty;
        }

        public class FakeNuGetSolutionPackageService : NuGetSolutionPackageService
        {

        }

        protected virtual void EnsureMetapackLibrary()
        {
            NuGetLogUtils.Verbose("Detecting MetaPack library on target site...");

            var web = _context.Web;
            var list = WebExtensions.LoadListByUrl(web, MetaPackConsts.SharePointLibraryUrl);

            if (list == null)
            {
                NuGetLogUtils.Verbose(string.Format("MetaPack library does not exist. Creating..."));

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

                // ensure schema

                NuGetLogUtils.Info("MetaPack library created");
                EnsureLibrarySchema(web);
            }
            else
            {
                NuGetLogUtils.Verbose("MetaPack library exists");
                EnsureLibrarySchema(web);
            }
        }

        private string schemaKey = "metapack.schema.version";

        private void EnsureLibrarySchema(Web web)
        {
            NuGetLogUtils.Verbose(string.Format("Ensuring metapack library schema version:[{0}]", metaPackShemaVersion));

            var context = web.Context;

            var list = WebExtensions.LoadListByUrl(web, MetaPackConsts.SharePointLibraryUrl);

            var rootFolder = list.RootFolder;

            context.Load(rootFolder);
            context.Load(rootFolder, f => f.Properties);

            context.ExecuteQuery();

            var properties = rootFolder.Properties;

            var currentSchemaObj = properties.FieldValues.ContainsKey(schemaKey) ? properties[schemaKey] : null; ;
            var currentSchemaValue = currentSchemaObj != null ? currentSchemaObj.ToString() : string.Empty;

            var currentSchema = new System.Version(0, 0, 0, 0);

            if (!string.IsNullOrEmpty(currentSchemaValue))
            {
                try
                {
                    currentSchema = new Version(currentSchemaValue);
                    NuGetLogUtils.Verbose(string.Format("Current schema value:[{0}]", currentSchema));
                }
                catch (Exception e)
                {
                    NuGetLogUtils.Verbose(string.Format("Can't parse schema version value:[{0}]", currentSchemaValue));
                }
            }

            if (currentSchema < metaPackShemaVersion)
            {
                NuGetLogUtils.Verbose(string.Format("Schema update is required. Required schema version is :[{0}]", metaPackShemaVersion));

                UpdateToVersion(list, metaPackShemaVersion);
            }
            else
            {
                NuGetLogUtils.Verbose(string.Format("No schema update is required"));
            }
        }

        private System.Version metaPackShemaVersion = new System.Version(0, 1, 5, 0);

        private void UpdateToVersion(List list, System.Version metaPackShemaVersion)
        {
            var context = list.Context;

            if (metaPackShemaVersion <= new System.Version(0, 1, 5, 0))
            {
                AddTextField(list, "PackageId", "Package Id");
                AddTextField(list, "PackageVersion", "Version");
                AddMultiLineTextField(list, "PackageNuspecXml", "NuspecXml");
            }

            var rootFolder = list.RootFolder;

            context.Load(rootFolder);
            context.Load(rootFolder, f => f.Properties);

            context.ExecuteQuery();

            var properties = rootFolder.Properties;

            properties[schemaKey] = metaPackShemaVersion.ToString();
            rootFolder.Update();

            context.ExecuteQuery();
        }

        private void AddTextField(List list, string internalName, string displayName)
        {
            AddField(list, internalName, displayName, "Text");
        }

        private void AddMultiLineTextField(List list, string internalName, string displayName)
        {
            AddField(list, internalName, displayName, "Note");
        }

        private void AddField(List list, string internalName, string displayName, string fieldType)
        {
            var context = list.Context;

            // field exist?
            var fields = list.Fields;

            context.Load(fields);
            context.ExecuteQuery();

            if (fields.ToArray().Any(f => f.InternalName == internalName))
                return;

            var schema = string.Format(
                                "<Field Type='{2}' Name='{0}' StaticName='{0}' DisplayName='{1}' />",
                                internalName, displayName, fieldType);

            if (fieldType == "Note")
            {
                schema = string.Format(
                               "<Field Type='{2}' Name='{0}' StaticName='{0}' DisplayName='{1}' UnlimitedLengthInDocumentLibrary='TRUE' />",
                               internalName, displayName, fieldType);
            }

            var field = list.Fields.AddFieldAsXml(schema, true, AddFieldOptions.AddFieldInternalNameHint);

            context.ExecuteQuery();
        }

        #endregion
    }
}
