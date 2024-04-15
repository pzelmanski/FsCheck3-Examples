namespace FsCheck3PBTests.ComplexTypes;

public record Order(Guid Id, String Name, OrderStatus Status, int Quantity, decimal Price);

public enum OrderStatus
{
    New,
    InProgress,
    Done
}
