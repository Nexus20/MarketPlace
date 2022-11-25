using MarketPlace.Domain.Entities.Abstract;

namespace MarketPlace.Domain.Entities;

public class Buyer : BaseEntity
{
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public DateTime? BirthDate { get; set; }
    public string? DeliveryAddress { get; set; }
    
    public string UserId { get; set; } = null!;
    public virtual User User { get; set; } = null!;
    public virtual List<Order>? Orders { get; set; }
}