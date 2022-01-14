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
        private List<Car> cars = new List<Car>();

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
            string brandText = brandTextBox.Text;
            string typeText = typeTextbox.Text;
            string colorText = colorTextBox.Text;
            string numberOfDoorsText = numberOfDoorsTextBox.Text;
            string priceText = priceTextBox.Text;
            String picturePath = pictureTextBox.Text;

            string picture = picturePath.Substring(picturePath.LastIndexOf("\\") + 1); // get part after last \
            Bitmap bitmap = new Bitmap(picturePath);
            bitmap.Save(directory + picture);

            Car newCar = new Car(
                brandTextBox.Text,
                typeTextbox.Text,
                colorTextBox.Text,
                int.Parse(numberOfDoorsTextBox.Text),
                double.Parse(priceTextBox.Text),
                "Goes",
                picture
            );

            cars.Add(newCar);
            cars = cars.OrderBy(car => car.getName()).ToList();
            carComboBox.Items.Add(newCar);
            carComboBox.SelectedItem = newCar;

            brandTextBox.Clear();
            typeTextbox.Clear();
            colorTextBox.Clear();
            numberOfDoorsTextBox.Clear();
            priceTextBox.Clear();
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
            Car selectedCar = carComboBox.SelectedItem as Car;

            brandAndTypeLabel.Text = selectedCar.getName();
            colorLabel.BackColor = Color.FromName(selectedCar.color);
            numberOfDoorsLabel.Text = selectedCar.numberOfDoors.ToString();
            priceLabel.Text = selectedCar.getPrice();
            locationlabel.Text = selectedCar.location;

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
                    carPictureBox.Hide();
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
                Car selectedCar = carComboBox.SelectedItem as Car;
                cars.Remove(selectedCar);
                carComboBox.Items.Remove(selectedCar);
            }
        }

        private void changePriceButton_Click(object sender, EventArgs e)
        {
            if (checkSelectedCar())
            {
                Car selectedCar = carComboBox.SelectedItem as Car;
                carComboBox.Items.Remove(selectedCar);
                cars.Remove(selectedCar);
                double price = double.Parse(changePriceTextBox.Text);
                selectedCar.setPrice(price);
                priceLabel.Text = "€ " + changePriceTextBox.Text;
                cars.Add(selectedCar);
                cars = cars.OrderBy(car => car.getName()).ToList();
                carComboBox.Items.Clear();
                carComboBox.Items.AddRange(cars.ToArray());
            }
        }

        private void carGarageForm_Load(object sender, EventArgs e)
        {
            if (File.ReadAllText(settingsPath).Length == 0)
            {
                return;
            }
            List<string> autos = File.ReadLines(settingsPath).ToList();
            cars = autos.Select(car => new Car(car)).ToList();
            cars = cars.OrderBy(car => car.getName()).ToList();
            carComboBox.Items.AddRange(cars.ToArray());
        }

        private void carGarageForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            setCars();
            string settings = "";
            foreach (Car car in cars)
            {
                settings += car.getDescription();
            }
            File.WriteAllText(settingsPath, settings);
        }

        private bool checkSelectedCar() => carComboBox.SelectedIndex > -1;

        private void alleLocatiesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            groupBox1.Text = "Voorraad alle locaties";
            setCars();
        }

        private void goesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            groupBox1.Text = "Voorraad Goes";
            setCars("Goes");
        }

        private void middelburgToolStripMenuItem_Click(object sender, EventArgs e)
        {
            groupBox1.Text = "Voorraad Middelburg";
            setCars("Middelburg");
        }

        private void setCars(string location = "")
        {
            brandAndTypeLabel.Text = "";
            colorLabel.BackColor = Color.Transparent;
            numberOfDoorsLabel.Text = "";
            priceLabel.Text = "";
            locationlabel.Text = "";
            carComboBox.Items.Clear();
            carPictureBox.Hide();
            carComboBox.Text = "";
            if (location == "")
            {
                carComboBox.Items.AddRange(cars.ToArray());
                return;
            }
            List<Car> autos = new List<Car>();
            foreach (Car car in cars)
            {
                if (car.location == location)
                {
                    autos.Add(car);
                }
            }
            carComboBox.Items.AddRange(autos.ToArray());
        }
    }
}