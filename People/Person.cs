using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Person
{
	private const double waterConsumationModifier = 0.01;
	private const double foodConsumationModifier = 0.01;

	public enum State {working, interracting, home, sleeping}

	public string name { get; set; }
	public Personality personality { get; private set; }
	public State state { get; private set; }
	public Building currentBuilding { get; private set; }

	private Health health;
	private ScienceField scfield;

	private Resource resource;

	public Person(Resource resource){
		this.health = new Health ();
		this.name = name;
		this.scfield = new ScienceField(ScienceField.Scfield.Astronaut);
		this.personality = new Personality ();
		this.state = State.home;
		this.resource = resource;
		this.resource.addPerson (this);
		this.resource.setupDropDownMenu ();
	}
	public void setVariables(ScienceField scf, string name){
		this.scfield = scf;
		this.name = name;
	}

	public void UpdateState ()
	{
		switch (state) {
		case Person.State.home:
			consumeResources ();
			break;
		case Person.State.sleeping:
			break;
		case Person.State.working:
			performInterraction ();
			break;
		default:
			break;
		}
		updateHealth ();
	}

	public Skillset getSkillset(){
		return scfield.getSkillset();
	}

	public void performInterraction ()
	{
		currentBuilding.performWork (this);
	}

	public bool enterBuilding(Building building){
		if (building.enter (this)) {
			currentBuilding = building;
			if (Building.BuildingType.Habitation.Equals(building.getBuildingType())) {
				state = State.home;
			}else { state = State.working; }
			return true;
		} else {
			return false;
		}
	}

	public void consumeResources ()
	{
		if (health.addFood()) {
			resource.consumeResource (Resource.Resources.food, foodConsumationModifier);
		}
		if (health.addWater()) {
			resource.consumeResource (Resource.Resources.water, waterConsumationModifier);
		}
	}

	public void updateHealth ()
	{
		health.update ();
	}

}