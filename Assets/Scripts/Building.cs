using UnityEngine; 
using System.Collections.Generic;

public class Building : MonoBehaviour {

	public BuildingCategory category;
	public float timeConstructed;
	public float weight = 1f;
	public Money costPerPlot = new Money(0,0,5);
	public float durability = 100f; //in years, but can be reduced in later calculations if there's lots of weight on the plot for example
	public Money rent = new Money(0,0,1);
	public Plot myPlot;

	public void Start(){
		
	}
	
	public void grabValuesFromBuildingTemplate(BuildingTemplate buildingTemplate){
		category = buildingTemplate.category;
		weight = buildingTemplate.weight;
		costPerPlot = rent;
		rent = buildingTemplate.rent;
		durability = buildingTemplate.durability;
	}
	
	void OnMouseUpAsButton(){
		
	}

	void OnMouseEnter(){
		List<Plot> plotsComprisingThisEstablishment = Global.placementGrid.highlightConnectedOfSameType(this);		
		
		string buildingSizeIndicator = "premises";
		
		/*
		if (category == BuildingCategory.SOCIAL){		this doesn't apply anymore because the category is now social rather than security
			if (plotsComprisingThisEstablishment.Count > 10){			tbh I think this stepped naming system might need to be reintegrated somehow, though I'd like to keep it in some way
				buildingSizeIndicator = "Fortress";
			} else if (plotsComprisingThisEstablishment.Count > 6){
				buildingSizeIndicator = "Castle";
			} else if (plotsComprisingThisEstablishment.Count >= 3){
				buildingSizeIndicator = "Gatehouse";
			} else {
				buildingSizeIndicator = "Turnpike";
			}
		}
		
		if (category == BuildingCategory.RELIGION){
			if (plotsComprisingThisEstablishment.Count > 6){
				buildingSizeIndicator = "Cathedral";
			} else if (plotsComprisingThisEstablishment.Count > 3){
				buildingSizeIndicator = "Church";
			} else {
				buildingSizeIndicator = "Chapel";
			}
		}  else {
			if (plotsComprisingThisEstablishment.Count > 10){
				buildingSizeIndicator = "Emporium";
			} else if (plotsComprisingThisEstablishment.Count > 6){
				buildingSizeIndicator = "Market";
			} else if (plotsComprisingThisEstablishment.Count > 3){
				buildingSizeIndicator = "Bazaar";
			} else {
				buildingSizeIndicator = "Shop";
			}
		}	
		
		Debug.Log(myPlot.ownerName + " " + myPlot.business + " " + buildingSizeIndicator);
		*/

		foreach (Plot p in plotsComprisingThisEstablishment){	
			Global.changeEmissionHighlight(p.building.GetComponent<Renderer>(), new Color(0.007843143f, 0.3346648f, 0.5960784f), true);
		}
	}
	
    void OnMouseExit(){
		List<Plot> plotsComprisingThisEstablishment = Global.placementGrid.highlightConnectedOfSameType(this);		
		
		foreach (Plot p in plotsComprisingThisEstablishment){			
			Global.changeEmissionHighlight(p.building.GetComponent<Renderer>(),
				new Color(0f, 0f, 0f), false);
		}
    }
}