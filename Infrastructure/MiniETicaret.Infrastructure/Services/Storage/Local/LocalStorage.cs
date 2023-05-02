using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using MiniETicaret.Application.Abstractions.Storage.Local;

namespace MiniETicaret.Infrastructure.Services.Storage.Local;

public class LocalStorage : ILocalStorage
{
    private readonly IWebHostEnvironment _webHostEnvironment;
    public LocalStorage(IWebHostEnvironment webHostEnvironment)
    {
        _webHostEnvironment = webHostEnvironment;
    }
    public async Task<List<(string fileName, string pathOrContainerName)>> UploadAsync(string path, IFormFileCollection files)
    {
        string uploadPath = Path.Combine(_webHostEnvironment.WebRootPath, path);
        if (!Directory.Exists(uploadPath))
        {
            Directory.CreateDirectory(uploadPath);
        }

        List<(string fileName, string path)> datas = new ();
        foreach (IFormFile file in files)
        {
            await CopyFileAsync(Path.Combine(uploadPath, file.FileName), file);
            datas.Add((file.FileName, Path.Combine(path, file.FileName)));
        }
        return datas;
    }

    public async Task DeleteAsync(string path, string fileName)
    {
        File.Delete(Path.Combine(path, fileName));
    }

    public List<string> GetFiles(string path)
    {
        DirectoryInfo directoryInfo = new DirectoryInfo(path);
        return directoryInfo.GetFiles().Select(f =>f.Name).ToList();
    }

    public bool HasFile(string path, string fileName)
    
        => File.Exists(Path.Combine(path, fileName));
    
    async Task<bool> CopyFileAsync(string path, IFormFile file)
    {
        try
        {
            await using FileStream fileStream = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.None, 1024 * 1024, useAsync:false);
            
            await file.CopyToAsync(fileStream);
            await fileStream.FlushAsync();
            return true;

        }
        catch (Exception e)
        {
            //todo: loglama yapılacak!
            throw e;
        }
        

    }
}