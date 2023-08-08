using MiniETicaret.Domain.Entities.Common;
using MiniETicaret.Domain.Entities.Identity;

namespace MiniETicaret.Domain.Entities;

public class Endpoint: BaseEntity
{
    public Endpoint()
    {
        Roles = new HashSet<AppRole>();
    }
    public string ActionType { get; set; }
    public string HttpType { get; set; }
    public string Definition { get; set; }
    public string Code { get; set; }
    public ACMenu AcMenu { get; set; }
    public ICollection<AppRole> Roles { get; set; }
}