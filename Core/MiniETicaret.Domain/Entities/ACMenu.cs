using MiniETicaret.Domain.Entities.Common;

namespace MiniETicaret.Domain.Entities;

public class ACMenu: BaseEntity //autohorization - controller menu
{
    public string Name { get; set; }
    public ICollection<Endpoint> Endpoints { get; set; }
}