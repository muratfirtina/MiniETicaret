using System.ComponentModel.DataAnnotations.Schema;
using MiniETicaret.Domain.Entities.Common;

namespace MiniETicaret.Domain.Entities;

public class File:BaseEntity
{
    public string FileName { get; set; }
    public string Path { get; set; }
    public string Storage { get; set; }
    [NotMapped]
    public override DateTime UpdatedDate { get; set; }
}