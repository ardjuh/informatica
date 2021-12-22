using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _8._12_eindopdracht
{
    public partial class carGarageForm : System.Windows.Forms.Form
    {
        private string settingsPath = "";
        private string directory = "";

        public carGarageForm()
        {
            InitializeComponent();

            // Get path of stock.settings
            directory = Directory.GetCurrentDirectory();
            directory = directory.Substring(0, directory.IndexOf("bin"));
            settingsPath = $"{directory}stock.settings";
        }

        private void addCarButton_Click(object sender, EventArgs e)
        {
            String picturePath = pictureTextBox.Text;
            string picture = picturePath.Substring(picturePath.LastIndexOf("\\"));
            Bitmap bitmap = new Bitmap(picturePath);
            bitmap.Save(directory + picture);

            Car newCar = new Car(
                brandTextBox.Text,
                typeTextbox.Text,
                colorTextBox.Text,
                int.Parse(numberOfDoorsTextBox.Text),
                double.Parse(priceTextBox.Text),
                picture
            );

            carComboBox.Items.Add(newCar);
            carComboBox.SelectedItem = newCar;
        }

        private void selectPictureButton_Click(object sender, EventArgs e)
        {
            if (openPictureDialog.ShowDialog() == DialogResult.OK)
            {
                pictureTextBox.Text = openPictureDialog.FileName;
            }

        }

        private void carComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            Car selectedCar = (Car)carComboBox.SelectedItem;

            brandAndTypeLabel.Text = selectedCar.getName();
            colorLabel.BackColor = Color.FromName(selectedCar.color);
            numberOfDoorsLabel.Text = selectedCar.numberOfDoors.ToString();
            priceLabel.Text = selectedCar.getPrice();

            if (selectedCar.containsPicture())
            {
                try
                {
                    Bitmap bit = new Bitmap(directory + selectedCar.picture);
                    carPictureBox.Image = bit;
                    carPictureBox.Show();
                }
                catch (Exception)
                {
                    Console.WriteLine($"{selectedCar.picture} doesn't exist");
                }
            }
            else
            {
                carPictureBox.Hide();
            }
        }

        private void deleteCarButton_Click(object sender, EventArgs e)
        {
            if (checkSelectedCar() && MessageBox.Show(
                "Weet u het zeker?",
                "Controle",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Asterisk
            ) == DialogResult.Yes)
            {
                Car selectedCar = (Car)carComboBox.SelectedItem;
                carComboBox.Items.Remove(selectedCar);
            }
        }

        private void changePriceButton_Click(object sender, EventArgs e)
        {
            if (checkSelectedCar())
            {
                Car selectedCar = (Car)carComboBox.SelectedItem;
                selectedCar.setPrice(double.Parse(changePriceTextBox.Text));
            }
        }

        private void carGarageForm_Load(object sender, EventArgs e)
        {
            if (File.ReadAllText(settingsPath).Length > 0)
            {
                List<string> cars = File.ReadLines(settingsPath).ToList();
                foreach (string car in cars)
                {
                    List<string> y = new List<string>();
                    //foreach (string word in car.Split(';'))
                    //{
                    //    y.AddRange(word.Split('='));
                  //  }
                    y.AddRange(car.Split(';', '='));

                    string brand = y[1];
                    string type = y[3];
                    string color = y[5];
                    int numberOfDoors = int.Parse(y[7]);
                    double price = double.Parse(y[9]);
                    string picture = "";
                    if (y[11] != "")
                    {
                        picture = y[11];
                    }
                    Car newCar = new Car(
                        brand,
                        type,
                        color,
                        numberOfDoors,
                        price,
                        picture
                    );
                    carComboBox.Items.Add(newCar);
                }
            }
        }

        private void carGarageForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            string x = "";
            foreach (object car in carComboBox.Items)
            {
                Car auto = (Car)car;
                x += "brand=" + auto.brand +
                    ";type=" + auto.type +
                    ";color=" + auto.color +
                    ";numberOfDoorsn=" + auto.numberOfDoors +
                    ";price=" + auto.price +
                    ";picture=" + auto.picture +
                    "\n";
            }
            File.WriteAllText(settingsPath, x);
        }

        private bool checkSelectedCar() => carComboBox.SelectedIndex > -1;
    }
}
