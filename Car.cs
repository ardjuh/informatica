using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _8._12_eindopdracht
{
    public class Car
    {
        public string brand;
        public string type;
        public string color;
        public int numberOfDoors;
        public double price;
        public string location;
        public string picture;

        public Car(
            string _brand, 
            string _type, 
            string _color, 
            int _numberOfDoors, 
            double _price,
            string _location,
            string _picture
        )
        {
            brand = _brand;
            type = _type;
            color = _color;
            numberOfDoors = _numberOfDoors;
            price = _price;
            location = _location;
            picture = _picture;
        }

        public Car(string _car) // Second contstructor
        {
            List<string> car = _car.Split(';', '=').ToList();

            brand = car[1];
            type = car[3];
            color = car[5];
            numberOfDoors = int.Parse(car[7]);
            price = double.Parse(car[9]);
            location = car[11];
            picture = "";
            if (car[13] != "")
            {
                picture = car[13];
            }
        }

        public string getName() => brand + " " + type;

        public string getDescription() => 
            "brand=" + brand
            + ";type=" + type
            + ";color=" + color
            + ";numberOfDoorsn=" + numberOfDoors
            + ";price=" + price
            + ";location=" + location
            + ";picture=" + picture
            + "\n";

        public bool containsPicture() => picture.Length > 0;

        public void setPrice(double newPrice) => price = newPrice;

        public string getPrice()
        {
            double roundedPrice = Math.Round(price, 2);
            string textPrice = "€ " + roundedPrice.ToString();
            return textPrice;
        }

        public override string ToString() => getName(); // Combobox accesses this function
    }
}
