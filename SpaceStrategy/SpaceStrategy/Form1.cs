﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SpaceStrategy
{
    public partial class Form1 : Form
    {
        // Form and main events
        private List<Planet> planetsList = new List<Planet>();
        public static List<Resource> resourceTypes;

        public Form1()
        {
            InitializeComponent();
            Resource[] resourceTypesRaw = { new Wood("wood"), new Stone("stone"), new Food("food")};
            resourceTypes = new List<Resource>(resourceTypesRaw);
            Console.WriteLine(resourceTypes);
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string planetName = PlanetsInput.Text;
            PlanetsInput.Text = "";
            CreatePlanet(planetName);
            //string text = PlanetsSelectList.GetItemText(PlanetsSelectList.SelectedItem);
            //label1.Text = text;
            //PlanetsSelectList.getSelectedItem();
            //planetsList.Add(planetName);
            //label1.Text += ;
            //PlanetsSelectList.Items.Add(planetName);
        }
        private void CreatePlanet(string name)
        {
            if (planetsList.All(x => x.Name != name))
            {
                Planet tempPlanet = new Planet(name);
                planetsList.Add(tempPlanet);
                UpdateWindowPlanetsList();
            }
            else
            {
                //Console.WriteLine("Planet with this name already exists");
                ShowStatus("Planet with this name already exists");
            }
        }
        private void UpdateWindowPlanetsList()
        {
            //PlanetsSelectList.DataSource = null;
            PlanetsSelectList.Items.Clear();
            for (int i = 0; i < planetsList.Count(); i++)
            {
                PlanetsSelectList.Items.Add(planetsList[i].Name);
            }

        }

        private void RemovePlanetButton_Click(object sender, EventArgs e)
        {
            string text = PlanetsSelectList.GetItemText(PlanetsSelectList.SelectedItem);
            
            int index = planetsList.FindIndex(i => i.Name == text);
            planetsList.RemoveAt(index);
            UpdateWindowPlanetsList();
        }

        private void PlanetsInput_TextChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void ColoniesSelectList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ColoniesSelectList.SelectedItem != null)
            {
                string text = ColoniesSelectList.SelectedItem.ToString();
                string tempPlanetName = PlanetsSelectList.SelectedItem.ToString();
                Planet tempPlanet = DefinePlanetByName(tempPlanetName);
                Colony tempColony = DefineColonyByName(text, tempPlanet.GetColonies());
                if (tempColony.Name != "error")
                {
                    ShowBuildings(tempColony);
                }
                ShowColoniesData(tempColony);
                //Console.WriteLine(text);
            }
        }
        private void ShowBuildings(Colony colony)
        {
            BuildingsSelectList.Items.Clear();
            List<Building> list = colony.GetBuildings();
            for (int i = 0; i < list.Count; i++)
            {
                BuildingsSelectList.Items.Add(list[i].Id);
            }
        }
        private Colony DefineColonyByName(string name, List<Colony> list)
        {
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].Name == name)
                {
                    return list[i];
                }
            }
            //Console.WriteLine("error");
            ShowStatus("error");
            return new Colony("error");
        }


        private void PlanetsSelectList_SelectedIndexChanged(object sender, EventArgs e)
        {
            BuildingsSelectList.Items.Clear(); // clear buildings list
            if (PlanetsSelectList.SelectedItem != null)
            {
                string text = PlanetsSelectList.SelectedItem.ToString();
                Planet planet = DefinePlanetByName(text);
                if (planet.Name != "error")
                {
                    ShowColonies(planet);
                }
                ShowPlanetData(planet);
                //Console.WriteLine(text);   
            }
        }
        private Planet DefinePlanetByName(string name)
        {
            for (int i = 0; i < planetsList.Count; i++)
            {
                if (planetsList[i].Name == name)
                {
                    return planetsList[i];
                }
            }
            //Console.WriteLine("error");
            ShowStatus("error");
            return new Planet("error");
        }
        private void ShowColonies(Planet planet)
        {
            ColoniesSelectList.Items.Clear();
            List<Colony> list = planet.GetColonies();
            for (int i = 0; i < list.Count; i++)
            {
                ColoniesSelectList.Items.Add(list[i].Name);
            }
        }
        private void CreateColonyButton_Click(object sender, EventArgs e)
        {
            if (PlanetsSelectList.SelectedIndex == -1)
            {
                //Console.WriteLine("Select at least one planet");
                ShowStatus("Select at least one planet");
            }
            else
            {
                string planetName = PlanetsSelectList.SelectedItem.ToString();
                Planet planet = DefinePlanetByName(planetName);
                string colonyName = ColonyInput.Text;
                ColonyInput.Text = "";
                planet.CreateColony(colonyName);
                UpdateWindowColoniesList(planet);
            }
        }
        private void UpdateWindowColoniesList(Planet planet)
        {
            ColoniesSelectList.Items.Clear();
            List<Colony> tempList = planet.GetColonies();
            for (int i = 0; i < tempList.Count(); i++)
            {
                ColoniesSelectList.Items.Add(tempList[i].Name);
            }
        }

        private void RemoveColonyButton_Click(object sender, EventArgs e)
        {
            string planetName = PlanetsSelectList.SelectedItem.ToString();
            string colonyName = ColoniesSelectList.SelectedItem.ToString();
            Planet tempPlanet = DefinePlanetByName(planetName);
            tempPlanet.RemoveColony(colonyName);
            UpdateWindowColoniesList(tempPlanet);
        }
        private void ShowStatus(string text)
        {
            StatusBar.Text = text;
        }

        private void BuildingsSelectList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (BuildingsSelectList.SelectedItem != null)
            {
                string text = ColoniesSelectList.SelectedItem.ToString();
                string tempPlanetName = PlanetsSelectList.SelectedItem.ToString();
                Planet tempPlanet = DefinePlanetByName(tempPlanetName);
                Colony tempColony = DefineColonyByName(text, tempPlanet.GetColonies());

                string idText = BuildingsSelectList.SelectedItem.ToString();

                if (int.TryParse(idText, out int id))
                {
                    Building tempBuilding = DefineBuildingByID(id, tempColony.GetBuildings());
                    ShowBuildingsData(tempBuilding);
                }
            }
        }

        private Building DefineBuildingByID(int id, List<Building> list)
        {
            for (int i = 0; i < list.Count(); i++)
            {
                if (list[i].Id == id)
                {
                    return list[i];
                }
            }
            return new Building("error", 0);
        }

        private void CreateBuildingButton_Click(object sender, EventArgs e)
        {
            if (ColoniesSelectList.SelectedIndex == -1)
            {
                //Console.WriteLine("Select at least one planet");
                ShowStatus("Select at least one planet");
            }
            else
            {
                string planetName = PlanetsSelectList.SelectedItem.ToString();
                Planet tempPlanet = DefinePlanetByName(planetName);
                string colonyName = ColoniesSelectList.SelectedItem.ToString();
                Colony tempColony = DefineColonyByName(colonyName, tempPlanet.GetColonies());
                string buildingType = BuildingInput.Text;
                BuildingInput.Text = "";
                tempColony.CreateBuilding(buildingType);
                UpdateWindowBuildingsList(tempColony);
            }
        }
        private void UpdateWindowBuildingsList(Colony colony)
        {
            BuildingsSelectList.Items.Clear();
            List<Building> tempList = colony.GetBuildings();
            for (int i = 0; i < tempList.Count(); i++)
            {
                BuildingsSelectList.Items.Add(tempList[i].Id);
            }
        }

        private void BuildingInput_TextChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click_1(object sender, EventArgs e)
        {

        }

        private void ShowPlanetData(Planet planet)
        {
            string data = "";
            string nameData = "Name : " + planet.Name + "\n";
            string radiusData = "Radius : " + planet.Radius + "\n";
            string coordinatesData = "Coordinates : x : " + planet.coordinates.X + ", y : " + planet.coordinates.Y + "\n";
            string coloniesData = "Number of colonies: " + planet.GetColonies().Count();
            data = nameData + radiusData + coordinatesData + coloniesData;
            PlanetInfoData.Text = data;
        }
        private void ShowColoniesData(Colony colony)
        {
            string data = "";
            string nameData = "Name : " + colony.Name + "\n";
            string moneyData = "Money : " + colony.Money + "\n";
            string buildingsData = "Number of buildings : " + colony.GetBuildings().Count() + "\n";
            data = nameData + moneyData + buildingsData;
            ColonyInfoData.Text = data;
        }
        private void ShowBuildingsData(Building building)
        {
            string data = "";
            string idData = "ID : " + building.Id + "\n";
            string typeData = "Type : " + building.Type + "\n";
            //string buildingsData = "Number of buildings : " + colony.GetBuildings().Count() + "\n";
            data = idData + typeData;
            BuildingInfoData.Text = data;
        }

        private void RemoveBuildingButton_Click(object sender, EventArgs e)
        {
            // need fix
            string planetName = PlanetsSelectList.SelectedItem.ToString();
            string colonyName = ColoniesSelectList.SelectedItem.ToString();
            string idStr = BuildingsSelectList.SelectedItem.ToString();
            Planet tempPlanet = DefinePlanetByName(planetName);
            Colony tempColony = DefineColonyByName(colonyName, tempPlanet.GetColonies());
            if (int.TryParse(idStr, out int id))
            {
                tempColony.RemoveBuilding(id);
                UpdateWindowBuildingsList(tempColony);
            }
        }
    }
}
