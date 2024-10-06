using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

namespace VehicleSearchApp.Pages.Vehicles
{
    public class SearchVehicleEstimateModel : PageModel
    {
        public List<Vehicle> Vehicles { get; set; } = new List<Vehicle>();
        public List<Vehicle> FilteredVehicles { get; set; } = new List<Vehicle>();

        [BindProperty] public required string Make { get; set; } = string.Empty;
        [BindProperty] public required string Model { get; set; } = string.Empty;
        [BindProperty] public int? Year { get; set; }

        public void OnGet()
        {
            LoadVehicles();
            FilteredVehicles = new List<Vehicle>(); // Initialize to an empty list
        }

        public void OnPost()
        {
            LoadVehicles();
            FilteredVehicles = Vehicles; // Start with all vehicles

            // Apply filters based on user input
            if (!string.IsNullOrEmpty(Make))
            {
                FilteredVehicles = FilteredVehicles
                    .Where(v => v.Make.Contains(Make, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            if (!string.IsNullOrEmpty(Model))
            {
                FilteredVehicles = FilteredVehicles
                    .Where(v => v.Model.Contains(Model, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            if (Year.HasValue)
            {
                FilteredVehicles = FilteredVehicles
                    .Where(v => v.YearModelId == Year.Value).ToList();
            }
        }

        private void LoadVehicles()
        {
            string filePath = @"C:\Users\keduk\Desktop\SearchVehicle Model\VehicleSearchApp\Data\Vehicle.json"; // Updated path

            if (System.IO.File.Exists(filePath))
            {
                var json = System.IO.File.ReadAllText(filePath);
#pragma warning disable CS8601 // Possible null reference assignment.
                Vehicles = JsonConvert.DeserializeObject<List<Vehicle>>(json) ?? new List<Vehicle>();
#pragma warning restore CS8601 // Possible null reference assignment.
            }
            else
            {
                throw new FileNotFoundException($"The file at {filePath} could not be found.");
            }
        }

        public IActionResult OnPostDelete(int vehicleId)
        {
            LoadVehicles();
            var vehicleToDelete = Vehicles.FirstOrDefault(v => v.VehicleId == vehicleId);
            if (vehicleToDelete != null)
            {
                Vehicles.Remove(vehicleToDelete);
                SaveVehicles();
            }
            return RedirectToPage(); // Redirect to refresh the page and update the vehicle list
        }

        private void SaveVehicles()
        {
            string filePath = @"C:\Users\keduk\Desktop\SearchVehicle Model\VehicleSearchApp\Data\Vehicle.json"; // Updated path

            var json = JsonConvert.SerializeObject(Vehicles, Formatting.Indented);
            System.IO.File.WriteAllText(filePath, json);
        }
    }

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
}
