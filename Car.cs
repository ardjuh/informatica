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
        public string picture;

        public Car(
            string _brand, 
            string _type, 
            string _color, 
            int _numberOfDoors, 
            double _price, 
            string _picture
        )
        {
            brand = _brand;
            type = _type;
            color = _color;
            numberOfDoors = _numberOfDoors;
            price = _price;
            picture = _picture;
        }

        public string getName() => brand + " " + type;

        public bool containsPicture() => picture.Length > 0;

        public void setPrice(double newPrice) => price = newPrice;

        public string getPrice()
        {
            double roundedPrice = Math.Round(price, 2);
            string textPrice = "€ " + roundedPrice.ToString();
            return textPrice;
        }
    }
}
