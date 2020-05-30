using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using parking_automation_dotnet_core.Models;

namespace parking_automation_dotnet_core.Controllers
{
    public class ParkingController : Controller
    {
        public ActionResult Index()
        {
            var cars = Parking.GetCars();
            return View(cars);
        }

        public ActionResult Create()
        {
            Random random = new Random();
            int id = random.Next(00000, 99999);
            ViewBag.Submited = false;
            var created = false;

            if (HttpContext.Request.Method == "POST")
            {
                ViewBag.Submited = true;

                var name = Request.Form["name"];
                var color = Request.Form["color"];
                var plaque = Request.Form["plaque"];
                var trusted = false;


                if (Request.Form["trusted"] == "on")
                {
                    trusted = true;
                }

                Parking cars = new Parking()
                {
                    ID = id,
                    Name = name,
                    Color = color,
                    Plaque = plaque,
                    EnterDate = DateTime.Now,
                    Trusted = Convert.ToBoolean(trusted)
                };

                var DummyDataFile = Parking.DummyDataFile;
                var DummyData = System.IO.File.ReadAllText(DummyDataFile);
                List<Parking> CarList = new List<Parking>();
                CarList = JsonConvert.DeserializeObject<List<Parking>>(DummyData);

                if (CarList == null)
                {
                    CarList = new List<Parking>();
                }

                CarList.Add(cars);

                System.IO.File.WriteAllText(DummyDataFile, JsonConvert.SerializeObject(CarList));

                created = true;
            }

            if (created)
            {
                ViewBag.Message = "Car was created succesfully!...";
            }
            else
            {
                ViewBag.Message = "There was an error while creating the car.";
            }
            return View();
        }
    }
}