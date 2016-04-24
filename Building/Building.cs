﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Building {
	const double productionRate = 0.01;
	public enum BuildingType 
	{
		Growhouse,
		Connector,
		Habitation,
		SolarPanel,
		LandingPad,
		ResearchCenter,
		WaterTreatment,
		Drill
	}
	public BuildingType buildingType;

	private double food = 0, water = 0, soil = 0, science = 0, energy = 0, waste = 0;


	LinkedList<double> revenue;
	private LinkedList<Person> occupants;
	private int maxOccupants = 4;
	private int currentOccupants = 0;


	public Building(BuildingType bt){
		this.buildingType = bt;
        occupants = new LinkedList<Person>();
    }

	public int consume (Resource.Resources res)
	{
		int tmp = 0;
		switch (res) {
		case Resource.Resources.food:
			tmp = (int)food;
			food = food % 1;
			return tmp;
		case Resource.Resources.water:
			tmp = (int)water;
			water = water % 1;
			return tmp;
		case Resource.Resources.soil:
			tmp = (int)soil;
			soil = soil % 1;
			return tmp;
		case Resource.Resources.science:
			tmp = (int)science;
			science = science % 1;
			return tmp;
		case Resource.Resources.energy:
			tmp = (int)energy;
			energy = energy % 1;
			return tmp;
		default:
			break;
		}
		return tmp;
	}

	public bool enter (Person person)
	{
        Debug.Log(person.name);
		if (currentOccupants < maxOccupants) {
			occupants.AddLast (person);
            currentOccupants++;

            return true;
		}
		return false;
	}

	public void leave(Person person){
		occupants.Remove (person);
		currentOccupants--;
	}

	public BuildingType getBuildingType(){
		return buildingType;
	}

	public LinkedList<Person> getOccupants(){
		return this.occupants;
	}

	public void performWork (Person person)
	{
		Skillset skillset = person.getSkillset ();
		double DISCfactor = calculateDICSFactor ();
		switch (buildingType) {
		case BuildingType.Growhouse:
			food += (productionRate * skillset.farming * DISCfactor);
			water -= (productionRate / (skillset.farming * DISCfactor));
			soil -= (productionRate / (skillset.farming * DISCfactor));
			energy -= (productionRate / (skillset.engineering * DISCfactor));
			break;
		case BuildingType.SolarPanel:
			energy += productionRate;
		case BuildingType.ResearchCenter:
			science += (productionRate * skillset.science * DISCfactor);
			water -= (productionRate / skillset.science * DISCfactor);
			energy -= (productionRate / skillset.science * DISCfactor);
			break;
		case BuildingType.WaterTreatment:
			food += (productionRate * skillset.farming * DISCfactor);
			water += (productionRate * skillset.farming * DISCfactor);
			soil += (productionRate * skillset.farming * DISCfactor);
			waste -= (productionRate / skillset.engineering * DISCfactor);
			break;
		case BuildingType.Drill:
			science += (productionRate * skillset.engineering * DISCfactor);
			water += (productionRate * skillset.engineering * DISCfactor);
			energy -= (productionRate * skillset.engineering * DISCfactor);
			break;
		default:
			break;
		}
	}

	double calculateDICSFactor ()
	{
		double numD = 0, numI = 0, numS = 0, numC = 0;
		double bonus = 1;
		foreach (Person person in occupants) {
			numD += person.personality.getD();
			numI += person.personality.getI();
			numS += person.personality.getS();
			numC += person.personality.getC();
		}
		if (numD <= 10) {
			bonus -= 0.1;
		} else if (numD <= 15) {
			bonus += 0.1;
		} else if (numD <= 20) {
			bonus -= 0.1;
		} else {
			bonus -= 0.3;
		}
		return 1;
	}

}
