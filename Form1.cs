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
            setCars();
        }

        private void carGarageForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            string cars = "";
            foreach (Car car in carComboBox.Items)
            {
                cars += car.getDescription();
            }
            File.WriteAllText(settingsPath, cars);
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
            if (File.ReadAllText(settingsPath).Length > 0)
            {
                carComboBox.Items.Clear();
                List<string> cars = File.ReadLines(settingsPath).ToList();
                List<Car> autos = new List<Car>();
                if (location == "")
                {
                    autos = cars.Select(car => new Car(car)).ToList();
                }
                else
                {
                    foreach(string car in cars)
                    {
                        Car auto = new Car(car);
                        if (auto.location == location)
                        {
                            autos.Add(auto);
                        }
                    }
                }
                carComboBox.Items.AddRange(autos.ToArray());
            }
        }
    }
}
