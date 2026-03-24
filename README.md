
This is a C# console application I built to manage a university 
equipment rental service. It handles adding equipment, registering 
different types of users, processing rentals and returns, and automatically 
calculating late fees.

# How to Run the Program
1. Clone this repository to your local machine.
2. Open the project folder in your IDE (like Rider or Visual Studio) 
or open your terminal.
3. Run the application using the `dotnet run` command or by pressing the 
"Run" button in your IDE.
4. The console will automatically execute a demonstration scenario 
showing all the working features, including successful rentals, blocked 
invalid actions, and penalty calculations.

   
Models: These are the basic building blocks 
(e.g., `User`, `Equipment`, `Rental`). They only hold data and 
basic properties, not the complex business logic.

Data: I created an `InMemoryDataStore` to act as a mock database.

Services: The `RentalManager` coordinates the rentals, 
and the `StandardPenaltyCalculator` handles the math.

Exceptions: I created custom errors (like `RentalLimitExceededException`) 
so the system can gracefully explain *why* something failed, rather than 
just crashing.


Instead of putting everything inside one giant `RentalService` class, 
I split the responsibilities to keep cohesion high and coupling low:

1. Separating Logic from Data (Cohesion): My `RentalManager` doesn't hold 
the lists of users or equipment itself. It takes in the `InMemoryDataStore` 
via its constructor. This keeps the manager highly cohesive—its only job is 
executing rental rules, not storing data.

2. Handling Penalties (Low Coupling): Rules for late fees change all the time 
in the real world. Instead of hardcoding the math into the return process,
I created an `IPenaltyCalculator` interface. The manager relies on this interface 
rather than a specific class. This means the `RentalManager` is completely 
decoupled from the actual math. If the university changes the penalty rules 
tomorrow, I can just plug in a new calculator without touching the core rental 
code.

3. Managing Business Rules (Polymorphism): The business rules state that 
Students can rent 2 items and Employees can rent 5. I really wanted to avoid 
writing ugly, hardcoded statements like `if (user.Type == "Student") 
{ limit = 2; }` inside my logic layer. Instead, I gave the abstract `User` 
class an abstract `MaxActiveRentals` property. The `Student` and `Employee` 
classes define their own limits. This makes the code much cleaner and easier 
to expand if we ever add a "Guest" user type.

4. Inheritance vs. Composition: I used inheritance for the `Equipment` 
classes (`Laptop`, `Camera`, `Projector`) because they genuinely share a lot of 
common properties like `Id`, `Name`, `Price`, and `IsAvailable`. It made sense to
put those in a base class while keeping specific details (like a camera's megapixels) 
in the specific child classes.