using UnityEngine;

public class BuildingTemplate {
	
	public string name;
	public BuildingCategory category;
	public Money costPerPlot = new Money(0,0,0);
	public float weight = 1; //KG
	public float durability = 100f;
	public Money rent = new Money(0,0,1);
	public BuildingEffect effect = BuildingEffect.NONE;
	public string assetPath;

	public BuildingTemplate(string name, BuildingCategory category, Money costPerPlot, float weight, float durability, BuildingEffect effect, string assetPath){
		//Debug.Log("IDEALLY, Building SHOULD HAVE NO REFERENCE WITHIN IT TO THIS BuildingTemplate CLASS, AS WE'RE DECIDING THAT ALL PLOTS HAVE THE SAME BUILDING TYPE ALL THE WAY UP. BUILDING TYPE SHOULD ONLY BE USED AT THE POINT OF CREATION OF A BUILDING, RATHER THAN BEING ATTACHED PERMANENTLY. MAYBE RENAME IT TO BUILDINGTEMPLATE TO STRESS THIS");
		
		this.name = name;
		this.category = category;
		this.costPerPlot = costPerPlot;
		this.weight = weight;				
		this.durability = durability;
		this.rent = costPerPlot.multipliedBy(0.1f); //RENT IS A FUNCTION OF COSTPERPLOT
		this.effect = effect;
		this.assetPath = assetPath;
		
		//int hash = name.GetHashCode();
		//mat = new Material(Global.placementGrid.defaultMaterial);
		//mat.SetColor("_Color", new Color((hash & 0xFF)/255f, ((hash & 0xFF00) >> 8)/255f, ((hash & 0xFF0000) >> 16)/255f, 1f));	  //this is just to get a fairly unique colour for each building type
	}
}