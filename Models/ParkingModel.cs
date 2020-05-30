using System.IO;
using System.Collections.Generic;
using Newtonsoft.Json;
using System;

namespace parking_automation_dotnet_core.Models
{
    public class Parking
    {
        public static string DummyDataFile = System.IO.Path.GetFullPath("FakeData.json");

        public int ID { get; set; }
        public string Name { get; set; }

        public string Color { get; set; }
        public string Plaque { get; set; }

        public DateTime EnterDate { get; set; }

        public static int carsCount()
        {
            return GetCars().Count;
        }

        public static List<Parking> GetCars()
        {
            List<Parking> cars = new List<Parking>();
            if (File.Exists(DummyDataFile))
            {
                // File exists..
                string content = File.ReadAllText(DummyDataFile);
                // Deserialize the objects 
                cars = JsonConvert.DeserializeObject<List<Parking>>(content);

                // Returns the clients, either empty list or containing the Client(s).
                return cars;
            }
            else
            {
                // Create the file 
                File.Create(DummyDataFile).Close();
                // Write data to it; [] means an array, 
                // List<Client> would throw error if [] is not wrapping text
                File.WriteAllText(DummyDataFile, "[]");

                // Re run the function
                GetCars();
            }

            return cars;
        }
    }
}