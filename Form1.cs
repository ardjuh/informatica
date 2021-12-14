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
        private string filePath = "";
        public carGarageForm()
        {
            InitializeComponent();
            string directory = Directory.GetCurrentDirectory();
            directory = directory.Substring(0, directory.IndexOf("bin"));
            filePath = $"{directory}stock.settings";
        }

        private void addCarButton_Click(object sender, EventArgs e)
        {
            Car newCar = new Car(
                brandTextBox.Text,
                typeTextbox.Text,
                colorTextBox.Text,
                int.Parse(numberOfDoorsTextBox.Text),
                double.Parse(priceTextBox.Text),
                pictureTextBox.Text
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
            brandAndTypeLabel.Text = selectedCar.giveName();
            colorLabel.Text = selectedCar.color;
            colorRichTextBox.BackColor = Color.FromName(selectedCar.color);
            numberOfDoorsLabel.Text = selectedCar.numberOfDoors.ToString();
            priceLabel.Text = selectedCar.givePrice();

            if (selectedCar.containPicture())
            {
                Bitmap bit = new Bitmap(selectedCar.picture);
                carPictureBox.Image = bit;
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
                selectedCar.changePrice(double.Parse(changePriceTextBox.Text));
            }
        }

        private void carGarageForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            string x = "";
            foreach (object car in carComboBox.Items)
            {
                Car auto = (Car)car;
                x += "Merk=" + auto.brand +
                    ";Type=" + auto.type +
                    ";Kleur=" + auto.color +
                    ";AantalDeuren=" + auto.numberOfDoors +
                    ";Vraagprijs=" + auto.price +
                    ";Afbeelding=" + auto.picture +
                    "\n";
            }
            File.WriteAllText(filePath, x);
        }

        private bool checkSelectedCar() => (carComboBox.SelectedIndex > -1);

        private void carGarageForm_Load(object sender, EventArgs e)
        {
            if (File.ReadAllText(filePath).Length > 0)
            {
                List<string> cars = File.ReadLines(filePath).ToList();
                foreach (string car in cars)
                {
                    List<string> y = new List<string>();
                    foreach (string word in car.Split(';'))
                    {
                        y.AddRange(word.Split('='));
                    }
                    string brand = y[y.IndexOf("Merk") + 1];
                    string type = y[y.IndexOf("Type") + 1];
                    string color = y[y.IndexOf("Kleur") + 1];
                    int numberOfDoors = int.Parse(y[y.IndexOf("AantalDeuren") + 1]);
                    double price = double.Parse(y[y.IndexOf("Vraagprijs") + 1]);
                    string picture = "";
                    if (y.Contains("Afbeelding"))
                    {
                        picture = y[y.IndexOf("Afbeelding") + 1];
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
    }
}
