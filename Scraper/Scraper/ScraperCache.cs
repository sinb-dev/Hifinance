using System.IO.Enumeration;

class ScraperCache 
{
    string cacheFolder = string.Empty;
    public void SetCacheFolder(string path) 
    {
        if (!Directory.Exists(path)) 
        {
            try 
            {
                createCacheFolder(path);
            } 
            catch (Exception) 
            {
                throw new IOException($"Cannot set cache path '{path}' is not a directory");
            }
            
        }
        if (!canWriteToFolder(path)) {
            throw new IOException($"Cannot write to path '{path}' check disk space and file permissions");
        }
        cacheFolder = path;
    }
    
    string getCacheFilename(string filename)
    {
        return cacheFolder + Path.DirectorySeparatorChar + filename;
    }

    public bool Check(string filename) 
    {
        return File.Exists( getCacheFilename(filename) );
    }
    public bool Check(Uri uri) 
    {
        return Check(getFileNameFromUrl(uri));
    }

    public async Task<string> Retrieve(string filename) 
    {
        if (Check(filename)) 
        {
            return await File.ReadAllTextAsync(getCacheFilename(filename));
        }
        return string.Empty;
    }
    public async Task<string> Retrieve(Uri uri)
    {
        string filename = getFileNameFromUrl(uri);
        return await Retrieve(filename);

    }
    public async Task Cache(string filename, string content) 
    {
        try 
        {
            FileInfo info = new(filename);
            string relativeFolder = cacheFolder + Path.DirectorySeparatorChar + info.Directory?.Name;
            if (!Directory.Exists(relativeFolder))
            {
                Directory.CreateDirectory(relativeFolder);
            }
            await File.WriteAllTextAsync(getCacheFilename(filename), content);
        }
        catch (IOException e)
        {
            Console.WriteLine(e.Message);
        }
    }
    public async Task Cache(Uri uri, string content) 
    {
        string filename = getFileNameFromUrl(uri);
        await Cache(filename, content);
    }

    string getFileNameFromUrl(Uri url) 
    {
        string filename = url.Host + Path.DirectorySeparatorChar;
        if (url.LocalPath == "/") 
        {
            filename += "index.html";
        }
        else
        {
            filename += url.LocalPath.Substring(1).Replace("/","--");
        }
        filename = filename.Replace("?","_");
        filename = filename.Replace(":","_");
        filename = filename.Replace("+","-");
        return filename;
    }
    void createCacheFolder(string folder) 
    {
        Directory.CreateDirectory(folder);
    }

    private bool canWriteToFolder(string folderPath)
    {
        try
        {
            var tempPath = folderPath + Path.DirectorySeparatorChar + Path.GetRandomFileName();
            File.WriteAllText(tempPath, "!");
            File.Delete(tempPath);
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }
}