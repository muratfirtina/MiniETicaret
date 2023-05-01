using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using MiniETicaret.Application.Services;
using MiniETicaret.Infrastructure.Operations;

namespace MiniETicaret.Infrastructure.Services;

public class FileService : IFileService
{
    readonly IWebHostEnvironment _webHostEnvironment;
    public FileService(IWebHostEnvironment webHostEnvironment)
    {
        _webHostEnvironment = webHostEnvironment;
    }
    
    public async Task<List<(string fileName, string path)>> UploadAsync(string path,IFormFileCollection files)
    {
        string uploadPath = Path.Combine(_webHostEnvironment.WebRootPath, path);
        if (!Directory.Exists(uploadPath))
        {
            Directory.CreateDirectory(uploadPath);
        }

        List<(string fileName, string path)> datas = new ();
        List<bool> results = new ();
        foreach (IFormFile file in files)
        {
            string fileNewName = await FileRenameAsync(uploadPath,file.FileName);
            bool result = await CopyFileAsync(Path.Combine(uploadPath, fileNewName), file);
            datas.Add((fileNewName, Path.Combine(path, fileNewName)));
            results.Add(result);
        }

        if (results.TrueForAll(r => r.Equals(true)))
        
            return datas;
        return null;

        //todo: Eğer ki if geçerli değilse hata fırlatılması gerekiyor!

    }

    private async Task<string> FileRenameAsync(string path,string fileName)
    {
            string extension = Path.GetExtension(fileName);
            string oldName = Path.GetFileNameWithoutExtension(fileName);
            oldName = oldName.ToLower().Trim('-', ' '); //harfleri küçültür ve baştaki ve sondaki - ve boşlukları siler
            char[] invalidChars = { '$', ':', ';', '@', '+', '-', '_', '=', '(', ')', '{', '}', '[', ']' ,'∑','€','®','₺','¥','π','¨','~','æ','ß','∂','ƒ','^','∆','´','¬','Ω','√','∫','µ','≥','÷','|'}; //geçersiz karakterleri belirler.
            oldName = oldName.TrimStart(invalidChars).TrimEnd(invalidChars); //baştaki ve sondaki geçersiz karakterleri siler
            //Regex regex = new Regex("[*'\",+._&#^@|/<>~]");
            string regulatedFileName = NameOperation.CharacterRegulatory(oldName);
            //string newFileName = regex.Replace(regulatedFileName, string.Empty);//geçersiz karakterleri siler ve yeni dosya ismi oluşturur.
            DateTime datetimenow = DateTime.UtcNow;
            string datetimeutcnow = datetimenow.ToString("yyyyMMddHHmmss");//dosya isminin sonuna eklenen tarih bilgisi
            string fullName = $"{regulatedFileName}-{datetimeutcnow}{extension}";//dosya ismi ve uzantısı birleştirilir ve yeni dosya ismi oluşturulur.
            
        
        return fullName;
    }

    public async Task<bool> CopyFileAsync(string path, IFormFile file)
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