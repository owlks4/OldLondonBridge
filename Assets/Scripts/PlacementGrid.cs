using System.Collections.Generic;
using UnityEngine;

public class PlacementGrid : MonoBehaviour {

	//when the grid was 56 by three, a good value for GRID_SCALE was 0.43f

	private int GRID_X = 37;				
	private int GRID_Y = 2;				
	private float GRID_SCALE = 0.6f;			
	
	private Plot[,] grid;
	public int numPlotsCurrentlyPreSelected;
	public int numPlotsCurrentlySelected;
	public BuildingTemplateMenuOption selectedBuildingTemplateMenuOption;
	public Material defaultMaterial;
	
	public int getGridYMax(){
		return GRID_Y;
	}
	
	public float getGridScale(){
		return GRID_SCALE;		
	}
	
	public Plot[,] getGrid(){
		return grid;
	}
	
	public void setGrid(Plot[,] grid){
		this.grid = grid;
	}
	
	public void Start(){
		grid = new Plot[GRID_X,GRID_Y];
		Global.placementGrid = this;
		Global.timeManager = this.GetComponent<TimeManager>();
		Global.registerBuildingTemplates();
		Stats.lastYearUpdated = Global.timeManager.year;
		
		//initialise building type option bar and its options
		
		Global.tabUIBar = GameObject.Find("TabBackgroundBar");
		Global.tabUIBarSeparator = GameObject.Find("TabBarSeparator");
		
		//generate placement grid on bridge
		
		generateGrid();
		
		//init save data
		SaveLoadManager.init();
		
		//spawn bridge		
		Bridge.bridgeParentObject = GameObject.Find("Bridge");
		Bridge.generateBridgeInWorld();
		
		//initial stat update
		Stats.updateStats();
	}
	
	public void Update(){
		if (Global.timeManager.year - Stats.lastYearUpdated > Global.UPDATE_STATS_INTERVAL_IN_YEARS){
			Stats.updateStats();
		}				
	}
	
	private void generateGrid(){
		
		
		for (int i = 0; i < grid.GetLength(0); i++){
			for (int j = 0; j < grid.GetLength(1); j++){
				if (grid[i,j] != null){
					Destroy(grid[i,j].gameObject);
				}
				GameObject newSlot = GameObject.CreatePrimitive(PrimitiveType.Plane);				
				newSlot.transform.parent = this.transform;
				newSlot.name = i + "," + j;
				newSlot.transform.localScale = new Vector3(GRID_SCALE,GRID_SCALE,GRID_SCALE);
				newSlot.transform.localPosition = new Vector3(i*10*GRID_SCALE,0,j*10*GRID_SCALE);
				
				grid[i,j] = newSlot.AddComponent<Plot>();
				Plot newPlot = grid[i,j];
				newPlot.placementGrid = this;
				newPlot.positionInGrid = new Vector2Int(i,j);				
			}
		}
	}
	
	public float getAmountOfTrafficSpace(){	//in square metres
		
		//each plot is 10 square metres
		
		float freeSpaceSquareMetres = 0;
		
		for (int i = 0; i < grid.GetLength(0); i++){
			for (int j = 0; j < grid.GetLength(1); j++){
				Plot p = grid[i,j];
				if (p != null && p.building == null){
					freeSpaceSquareMetres += GRID_SCALE * 10;
				}
			}
		}
		
		return freeSpaceSquareMetres * 0.667f; //because you can't walk on the outermost bits of the plots beyond the balustrades
	}
	
	public void updateBuildingStats(float yearsSinceLastUpdate){
		foreach (Plot p in grid){
			p.updateBuildingStats(yearsSinceLastUpdate);
		}
	}
	
	public void toggleBuildBarOpen(){
		Global.buildingMenuOpen = !Global.buildingMenuOpen;
		 
		if (Global.buildingMenuOpen){
			Global.tabUIBar.GetComponent<Animator>().Play("UIbuildBarOpen");	
		} else {
			Global.tabUIBar.GetComponent<Animator>().Play("UIbuildBarClose");
		}
		
		for (int i = 0; i < grid.GetLength(0); i++){
			for (int j = 0; j < grid.GetLength(1); j++){
				grid[i,j].setColliderEnabled(Global.buildingMenuOpen);
				}
			}
	} 
	
	private bool numberWithinRange(int num, int bound1, int bound2){
		
		if (num >= bound1 && num <= bound2){
			return true;
		} else if (num >= bound2 && num <= bound1){
			return true;
		}
		
		return false;
	}
	
	public List<Plot> highlightConnectedOfSameType(Building orig){ //and if built in the same year; i.e. part of the same building, even if it has something next to it with the same trade
		
		List<Plot> output = new List<Plot>(){orig.myPlot};
		
		for (int i = orig.myPlot.positionInGrid.x; i < GRID_X; i++){ //go counting up from building position
			bool foundLiterallyAnythingAtAll = false;
			for (int j = 0; j < GRID_Y; j++){
				Plot p = grid[i,j];
				if (arePartOfSameBuilding(orig.myPlot,p)){
					foundLiterallyAnythingAtAll = true;
					if (!output.Contains(p)){
						output.Add(p);
						}
				}
			}
			if (!foundLiterallyAnythingAtAll){
				break;
			}
		}
		
		for (int i = orig.myPlot.positionInGrid.x; i >= 0; i--){	//go counting down from building position
			bool foundLiterallyAnythingAtAll = false;
			for (int j = GRID_Y-1; j >= 0; j--){
				Plot p = grid[i,j];
				if (arePartOfSameBuilding(orig.myPlot,p)){
					foundLiterallyAnythingAtAll = true;
					if (!output.Contains(p)){
						output.Add(p);
						}
				}
			}
			if (!foundLiterallyAnythingAtAll){
				break;
			}
		}
		
		return output;
	}
	
	private bool arePartOfSameBuilding(Plot first, Plot potential){
		if (potential.building == null){
			return false;
		}
		
		if (first.businessID == potential.businessID){
			return true;
			}

		return false;	
	}
	
	public void updatePreselection(){
		numPlotsCurrentlyPreSelected = 0;
		for (int i = 0; i < grid.GetLength(0); i++){
			for (int j = 0; j < grid.GetLength(1); j++){		
				Plot b = grid[i,j];
				if (numberWithinRange(i, Global.selectionFirstCorner.x,Global.selectionSecondCorner.x) && numberWithinRange(j, Global.selectionFirstCorner.y,Global.selectionSecondCorner.y)){
					b.preSelected = true;
					numPlotsCurrentlyPreSelected++;
				}  else {
					b.preSelected = false;
				}
				b.evaluateVisual();
			}
		}
	}
	
	public void changePreselectedToSelected(){
		numPlotsCurrentlySelected = 0;
		for (int i = 0; i < grid.GetLength(0); i++){
			for (int j = 0; j < grid.GetLength(1); j++){		
				Plot b = grid[i,j];
				if (b.preSelected){
					b.selected = true;
					numPlotsCurrentlySelected++;
				}  else {
					b.selected = false;
				}
				b.preSelected = false;
				b.evaluateVisual();
			}
		}
		
		if (selectedBuildingTemplateMenuOption != null){
			buildBuildingInSelectedArea(selectedBuildingTemplateMenuOption.buildingTemplate);					
		}
	}
	
	public void buildBuildingInSelectedArea(BuildingTemplate buildingTemplate){
		
		if (!Global.canAffordCost(buildingTemplate.costPerPlot)){
			Debug.Log("Can't afford that many pllots of that building!");
			return;
		}
		
		string ownerName = NameMaker.getRandomName() + " " + (Random.Range(0,10) < 4 ? (Random.Range(0,2) == 0 ? "& Co.": "& Son"): "");
			
		int businessID = ownerName.GetHashCode();
		
		for (int i = 0; i < grid.GetLength(0); i++){
			for (int j = 0; j < grid.GetLength(1); j++){		
				Plot b = grid[i,j];
				if (b.selected){
					b.selected = false;
					b.ownerName = ownerName;
					b.businessID = businessID;
					b.setBuilding(buildingTemplate);
					b.evaluateVisual();
				}
			}
		}
		
		numPlotsCurrentlyPreSelected = 0;
		numPlotsCurrentlySelected = 0;
	}
	
	public void healthButtonClicked(){
		Global.fillOutTabBarWithAvailableBuildingTemplates(BuildingCategory.HEALTH);		
	}
	
	public void tradeButtonClicked(){
		Global.fillOutTabBarWithAvailableBuildingTemplates(BuildingCategory.TRADE);		
	}
	
	public void constructionButtonClicked(){
		Global.fillOutTabBarWithAvailableBuildingTemplates(BuildingCategory.CONSTRUCTION);		
	}
	
	public void transportButtonClicked(){
		Global.fillOutTabBarWithAvailableBuildingTemplates(BuildingCategory.TRANSPORT);		
	}
	
	public void socialButtonClicked(){
		Global.fillOutTabBarWithAvailableBuildingTemplates(BuildingCategory.SOCIAL);		
	}
}