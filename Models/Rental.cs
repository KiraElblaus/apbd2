namespace APBD_TASK2.Models;

public class Rental
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public Guid UserId { get; set; }
    public Guid EquipmentId { get; set; }
    public DateTime RentalDate { get; set; }
    public DateTime DueDate { get; set; }
    public DateTime? ReturnDate { get; set; }
    public decimal BaseFee { get; set; }
    public decimal PenaltyFee { get; set; }

    public bool IsActive => ReturnDate == null;
    public decimal TotalFee => BaseFee + PenaltyFee;

    public Rental(Guid userId, Guid equipmentId, DateTime rentalDate, int daysRented, decimal dailyPrice)
    {
        UserId = userId;
        EquipmentId = equipmentId;
        RentalDate = rentalDate;
        DueDate = rentalDate.AddDays(daysRented);
        BaseFee = daysRented * dailyPrice;
    }
}