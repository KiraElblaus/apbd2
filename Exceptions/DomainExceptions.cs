namespace APBD_TASK2.Exceptions;

public class EquipmentUnavailableException : Exception
{
    public EquipmentUnavailableException(string message) : base(message) { }
}

public class RentalLimitExceededException : Exception
{
    public RentalLimitExceededException(string message) : base(message) { }
}