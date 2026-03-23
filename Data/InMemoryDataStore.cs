using APBD_TASK2.Models;

namespace APBD_TASK2.Data;

public class InMemoryDataStore
{
    public List<User> Users { get;  } = new();
    public List<Equipment> Equipment { get;  } = new();
    public List<Rental> Rentals { get;  } = new();
}