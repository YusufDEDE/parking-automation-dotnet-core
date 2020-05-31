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
            return View(Parking.GetCars());
        }

        public ActionResult Create()
        {
            Random random = new Random();
            int id = random.Next(00000, 99999);
            var created = false;

            if (HttpContext.Request.Method == "POST")
            {

                var name = Request.Form["name"];
                var color = Request.Form["color"];
                var plaque = Request.Form["plaque"];

                if (Parking.carsCount() < 5)
                {
                    Parking cars = new Parking()
                    {
                        ID = id,
                        Name = name,
                        Color = color,
                        Plaque = plaque,
                        EnterDate = DateTime.Now,

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
                else
                {
                    ViewBag.Warning = "AutoPark Full | Not car add";
                }

            }

            if (created)
            {
                ViewBag.Message = "Car was created succesfully!...";
                return RedirectToAction("Index", "Parking");
            }


            return View();

        }

        public ActionResult Delete(int id)
        {
            var Cars = Parking.GetCars();
            var deleted = false;

            foreach (Parking car in Cars)
            {
                if (car.ID == id)
                {
                    var index = Cars.IndexOf(car);
                    Cars.RemoveAt(index);

                    System.IO.File.WriteAllText(Parking.DummyDataFile, JsonConvert.SerializeObject(Cars));
                    deleted = true;
                    break;
                }
            }

            if (deleted)
            {
                ViewBag.Message = "Car was deleted successfull.";
            }
            else
            {
                ViewBag.Message = "There was an error while deleting the car.";
            }

            return RedirectToAction("Index", "Parking");
        }
    }

}