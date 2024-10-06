// Vehicle.cs
public class Vehicle
{
    public int VehicleId { get; set; }
    public required string Make { get; set; }
    public required string Model { get; set; }
    public int YearModelId { get; set; }
    public required string Engine { get; set; }
    public required string TransmissionId { get; set; }
    public required string VehicleDifferential { get; set; }
}
