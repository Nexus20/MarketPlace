using MarketPlace.Domain.Entities.Abstract;

namespace MarketPlace.Domain.Entities;

public class User : BaseEntity
{
    public string Phone { get; set; } = null!;
    public string Email { get; set; } = null!;

    public virtual Buyer? Buyer { get; set; }
    public virtual Shop? Shop { get; set; }
}