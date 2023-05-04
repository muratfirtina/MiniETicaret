using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using MiniETicaret.Infrastructure.Operations;

namespace MiniETicaret.Infrastructure.Services;

public class FileService

{
    protected delegate bool HasFile(string pathOrContainerName, string fileName);
    protected async Task<string> FileRenameAsync(string pathOrContainerName,string fileName, HasFile hasFileMethod)
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

            if (hasFileMethod(pathOrContainerName, fullName))
            {
                int i = 1;
                while (hasFileMethod(pathOrContainerName, fullName))
                {
                    fullName = $"{regulatedFileName}-{datetimeutcnow}-{i}{extension}";
                    i++;
                }
            }

            return fullName;
    }

    
}