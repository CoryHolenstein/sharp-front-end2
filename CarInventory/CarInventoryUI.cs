﻿using System;
using System.Windows.Forms;
using System.Web;
using System.Net.Http;
using System.Net.Http.Json;
using System.Collections.Generic;
using System.Text.Json;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Linq;
using System.Web.Script.Serialization;
using static UserInformation.UserInformation;

namespace CarInventory
{
    public partial class CarInventoryUI : Form
    {
        int inventoryID = 0;
        String carBrand = "", carName = "", carColor = "", carType = "";

        public class CarInfo {
            public int inventoryID { get; set; }
            public String carBrand { get; set; }
            public String carName { get; set; }
            public String carColor { get; set; }
            public String carType { get; set; }

            public CarInfo(int inventoryID, String carBrand, String carName, String carColor, String carType) 
            {
                this.inventoryID = inventoryID;
                this.carBrand = carBrand;
                this.carName = carName;
                this.carColor = carColor;
                this.carType = carType;
            }
            public CarInfo(int inventoryID)
            {
                this.inventoryID = inventoryID;
          
            }
        }

       
       

        public CarInventoryUI()
        {
            InitializeComponent();
       
        }

        private void List_CarResults_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

    

        private async void Button_GetAllCars_Click(object sender, EventArgs e)
        {


            label7.Text = Stored_UserName;

            var client = new HttpClient();

            var carInfo = new CarInfo(inventoryID, carBrand, carName,carColor,carType);
            var requestContent = JsonContent.Create(carInfo);

            var response = await client.PostAsync("http://localhost:3000/inventory/getcars", requestContent);
            var contents = await response.Content.ReadAsStringAsync();

            //credits :https://stackoverflow.com/questions/15726197/parsing-a-json-array-using-json-net 
            //
             JArray contentsArray = JArray.Parse(contents);
            foreach (JObject o in contentsArray.Children<JObject>())
		{
			foreach (JProperty p in o.Properties())
			{
				string name = p.Name;
				//string value = (string)p.Value;
				Console.WriteLine(name + " -- " + p.Value);
                     List_CarResults.Items.Add(p.Value);
			}
		}
        }

        private async void Button_UpdateCar_Click(object sender, EventArgs e)
        {
           
           
            if (List_CarResults.SelectedIndex >= 0)
            {
                Console.WriteLine("selected item");
                //  Console.WriteLine(List_CarResults.SelectedItem.ToString());
                // if (List_CarResults.SelectedItem.ToString().Contains("InventoryID"))
                if (int.TryParse(List_CarResults.SelectedItem.ToString(), out int value))
                {
                    //call
                    Console.WriteLine("api call time baby");
                    var client = new HttpClient();


                    var inventoryID = List_CarResults.SelectedItem.ToString();

                    if (TextBox_CarBrand.Text == "" || TextBox_CarName.Text == "" || TextBox_CarColor.Text == "" || TextBox_CarType.Text == "")
                    {
                        Console.WriteLine("Please input all fields!");
                    }
                    else
                    {
                        CarInfo updateCar = new CarInfo(int.Parse(inventoryID), TextBox_CarBrand.Text, TextBox_CarName.Text, TextBox_CarColor.Text, TextBox_CarType.Text);
                        var requestContent = JsonContent.Create(updateCar);
                        var response = await client.PostAsync("http://localhost:3000/inventory/updatecar", requestContent);
                        var contents = await response.Content.ReadAsStringAsync();
                        emptylistbox(this, e);
                      Button_GetAllCars_Click(this, e);
                    }
          
                    //int inventoryIDToSend = int.Parse(inventoryID);


                }
            }
            else if (List_CarResults.SelectedIndex < 0)
            {
                Console.WriteLine("no item selected.");
            }
        }

        private async void Button_RemoveCar_Click(object sender, EventArgs e)
        {
            if (List_CarResults.SelectedIndex >= 0)
            {
                Console.WriteLine("selected item");
                //  Console.WriteLine(List_CarResults.SelectedItem.ToString());
                // if (List_CarResults.SelectedItem.ToString().Contains("InventoryID"))
                if (int.TryParse(List_CarResults.SelectedItem.ToString(), out int value))
                {
                    //call
                    Console.WriteLine("api call time baby");
                    var client = new HttpClient();


                    var inventoryID = List_CarResults.SelectedItem.ToString();
                    CarInfo deleteCar = new CarInfo(int.Parse(inventoryID));
                    //int inventoryIDToSend = int.Parse(inventoryID);

                    var requestContent = JsonContent.Create(deleteCar);

                    var response = await client.PostAsync("http://localhost:3000/inventory/deletecar", requestContent);
                    var contents = await response.Content.ReadAsStringAsync();
                
                    emptylistbox(this, e);
                    Button_GetAllCars_Click(this, e);

                }
            }
            else
            {
                Console.WriteLine("no item selected.");
            }

        }
        private async void button1_Click(object sender, EventArgs e)
        {
            Random rnd = new Random();
            var client = new HttpClient();

            int card = rnd.Next(100);
            if (TextBox_CarBrand.Text == "" || TextBox_CarName.Text == "" || TextBox_CarColor.Text == "" || TextBox_CarType.Text == "")
            {
                Console.WriteLine("Please input all fields!");
            }
            else
            {
                CarInfo newCar = new CarInfo(card, TextBox_CarBrand.Text, TextBox_CarName.Text, TextBox_CarColor.Text, TextBox_CarType.Text);
                Console.WriteLine(newCar.ToString());
                var requestContent = JsonContent.Create(newCar);

                var response = await client.PostAsync("http://localhost:3000/inventory/addcar", requestContent);
                var contents = await response.Content.ReadAsStringAsync();
                emptylistbox(this, e);
                Button_GetAllCars_Click(this, e);
            }
        }


        private void emptylistbox(object sender, EventArgs e)
        {
            List_CarResults.Items.Clear();
        }
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            Button_GetAllCars_Click(this, e);
        }

        protected void Button_UpdateCar_MouseHover(object sender, EventArgs e)
        {
            System.Windows.Forms.ToolTip ToolTip1 = new System.Windows.Forms.ToolTip();
            label8.Text = "Ok";
            ToolTip1.SetToolTip(this.Button_UpdateCar, this.Button_UpdateCar.Text);
        }
        protected void Button_UpdateCar_MouseLeave(object sender, EventArgs e)
        {
            label8.Text = "okokokk";
        }
    }
}
