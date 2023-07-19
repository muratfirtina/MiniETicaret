using MiniETicaret.Domain.Entities.Common;

namespace MiniETicaret.Domain.Entities;

public class CompletedOrder: BaseEntity
{ 
        public Guid OrderId { get; set; }

        public Order Order { get; set; }
}