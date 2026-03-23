using APBD_TASK2.Models;

namespace APBD_TASK2.Services;

public interface IPenaltyCalculator
{
    decimal CalculatePenalty(Rental rental, DateTime actualReturnDate, decimal dailyRate);
}

public class StandardPenaltyCalculator : IPenaltyCalculator
{
    public decimal CalculatePenalty(Rental rental, DateTime actualReturnDate, decimal dailyRate)
    {
        if (actualReturnDate > rental.DueDate) return 0m;
        
        int daysLate = (actualReturnDate - rental.DueDate).Days;
        return daysLate * (dailyRate * 2);
    }
}