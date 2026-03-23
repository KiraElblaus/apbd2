namespace APBD_TASK2.Models;

public abstract class Equipment
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public string Name { get; set; }
    public bool IsAvailable { get; set; } = true;
    public decimal DailyRentalPrice { get; set; }

    protected Equipment(string name, decimal dailyRentalPrice)
    {
        Name = name;
        DailyRentalPrice = dailyRentalPrice;
    }
}

public class Laptop : Equipment
{
    public int RamGB { get; set; }
    public string OperatingSystem { get; set; }

    public Laptop(string name, decimal price, int ramGb, string os) : base(name, price)
    {
        RamGB = ramGb;
        OperatingSystem = os;
    }
}

public class Projector : Equipment
{
    public string Resolution { get; set; }
    public int Lumens { get; set; }

    public Projector(string name, decimal price, string resolution, int lumens) : base(name, price)
    {
        Resolution = resolution;
        Lumens = lumens;
    }
}

public class Camera : Equipment
{
    public int Megapixels { get; set; }
    public bool IncludesLens { get; set; }

    public Camera(string name, decimal price, int megapixels, bool includesLens) : base(name, price)
    {
        Megapixels = megapixels;
        IncludesLens = includesLens;
    }
}