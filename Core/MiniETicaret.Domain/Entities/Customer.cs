using MiniETicaret.Domain.Entities.Common;

namespace MiniETicaret.Domain.Entities;

public class Customer : BaseEntity
{
    public string Name { get; set; }
    //public ICollection<Order> Orders { get; set; }
    
}