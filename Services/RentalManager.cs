using APBD_TASK2.Data;
using APBD_TASK2.Exceptions;
using APBD_TASK2.Models;

namespace APBD_TASK2.Services;

public class RentalManager
{
    private readonly InMemoryDataStore _dataStore;
    private readonly IPenaltyCalculator _penaltyCalculator;

    public RentalManager(InMemoryDataStore dataStore, IPenaltyCalculator penaltyCalculator)
    {
        _dataStore = dataStore;
        _penaltyCalculator = penaltyCalculator;
    }

    public Rental RentEquipment(Guid userId, Guid equipmentId, int days)
    {
        var user = _dataStore.Users.FirstOrDefault(u => u.Id == userId) 
            ?? throw new ArgumentException("User not found.");
        
        var equipment = _dataStore.Equipment.FirstOrDefault(e => e.Id == equipmentId) 
            ?? throw new ArgumentException("Equipment not found.");

        if (!equipment.IsAvailable)
            throw new EquipmentUnavailableException($"{equipment.Name} is currently marked as unavailable or rented out.");

        int activeRentals = _dataStore.Rentals.Count(r => r.UserId == userId && r.IsActive);
        if (activeRentals >= user.MaxActiveRentals)
            throw new RentalLimitExceededException($"{user.FirstName} has reached their active rental limit of {user.MaxActiveRentals}.");

        var rental = new Rental(userId, equipmentId, DateTime.Now, days, equipment.DailyRentalPrice);
        equipment.IsAvailable = false;
        
        _dataStore.Rentals.Add(rental);
        return rental;
    }
    
    public void ReturnEquipment(Guid rentalId, DateTime actualReturnDate)
    {
        var rental = _dataStore.Rentals.FirstOrDefault(r => r.Id == rentalId) 
            ?? throw new ArgumentException("Rental record not found.");
            
        var equipment = _dataStore.Equipment.First(e => e.Id == rental.EquipmentId);

        rental.ReturnDate = actualReturnDate;
        rental.PenaltyFee = _penaltyCalculator.CalculatePenalty(rental, actualReturnDate, equipment.DailyRentalPrice);
        
        equipment.IsAvailable = true;
    }

    public IEnumerable<Rental> GetOverdueRentals(DateTime currentDate)
    {
        return _dataStore.Rentals.Where(r => r.IsActive && r.DueDate < currentDate);
    }
    
    public void GenerateSummaryReport()
    {
        Console.WriteLine("\nSYSTEM SUMMARY REPORT ");
        Console.WriteLine($"Total Equipment: {_dataStore.Equipment.Count}");
        Console.WriteLine($"Available Equipment: {_dataStore.Equipment.Count(e => e.IsAvailable)}");
        Console.WriteLine($"Total Users: {_dataStore.Users.Count}");
        Console.WriteLine($"Active Rentals: {_dataStore.Rentals.Count(r => r.IsActive)}");
        Console.WriteLine($"Total Revenue (Base + Penalties): ${_dataStore.Rentals.Where(r => !r.IsActive).Sum(r => r.TotalFee)}");
    }
}