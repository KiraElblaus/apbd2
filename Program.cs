using APBD_TASK2.Data;
using APBD_TASK2.Exceptions;
using APBD_TASK2.Models;
using APBD_TASK2.Services;


var dataStore = new InMemoryDataStore();
var penaltyCalculator = new StandardPenaltyCalculator();
var rentalManager = new RentalManager(dataStore, penaltyCalculator);

Console.WriteLine("Starting University Equipment Rental Demonstration...\n");

var laptop = new Laptop("Dell XPS 15", 25.0m, 16, "Windows 11");
var projector = new Projector("Epson 1080p", 15.0m, "1920x1080", 3000);
var camera = new Camera("Canon EOS R5", 40.0m, 45, true);
dataStore.Equipment.AddRange(new Equipment[] { laptop, projector, camera });

var student = new Student("Alice", "Smith");
var employee = new Employee("Dr. Bob", "Jones");
dataStore.Users.AddRange(new User[] { student, employee });

Console.WriteLine("Correct Rental");
var validRental = rentalManager.RentEquipment(student.Id, laptop.Id, 3);
Console.WriteLine($"Success: {student.FirstName} rented {laptop.Name}. Due: {validRental.DueDate.ToShortDateString()}");

Console.WriteLine("\n Invalid Operation (Unavailable)");
try
{
    rentalManager.RentEquipment(employee.Id, laptop.Id, 2);
}
catch (EquipmentUnavailableException ex)
{
    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine($"Blocked: {ex.Message}");
    Console.ResetColor();
}

Console.WriteLine("\n Invalid Operation (Limit Exceeded)");
try
{
    rentalManager.RentEquipment(student.Id, projector.Id, 2);
    rentalManager.RentEquipment(student.Id, camera.Id, 1); 
}
catch (RentalLimitExceededException ex)
{
    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine($"Blocked: {ex.Message}");
    Console.ResetColor();
}

Console.WriteLine("\nOn-Time Return");
rentalManager.ReturnEquipment(validRental.Id, validRental.DueDate.AddDays(-1));
Console.WriteLine($"Success: {laptop.Name} returned on time. Penalty: ${validRental.PenaltyFee}");

Console.WriteLine("\nLate Return");
var lateRental = rentalManager.RentEquipment(employee.Id, camera.Id, 2);

rentalManager.ReturnEquipment(lateRental.Id, lateRental.DueDate.AddDays(3));
Console.WriteLine($"Success: {camera.Name} returned late. Base Fee: ${lateRental.BaseFee}. Penalty Fee: ${lateRental.PenaltyFee}. Total: ${lateRental.TotalFee}");

rentalManager.GenerateSummaryReport();