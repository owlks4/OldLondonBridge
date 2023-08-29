using UnityEngine;

public class Plot : MonoBehaviour {

	public PlacementGrid placementGrid;
    public Renderer rend;
	public Vector2Int positionInGrid = new Vector2Int(0,0);
	public bool preSelected; //whether it is highlighted while the user is still drawing their selection
	public bool selected;	//whether it remains selected when they let go of the mouse button; i.e. they have chosen to select this area
	public float weight = 0;
	private Collider col;

	public Building building;
	public string ownerName;
	public int businessID;
	public string business;

    public void Start()
    {
        rend = GetComponent<Renderer>();
		rend.enabled = false;
		col = GetComponent<Collider>();
		col.enabled = false;
    }
	
	public void setColliderEnabled(bool b){
		col.enabled = b;
	}
	
	public void evaluateVisual(){
		
		if (preSelected){
			rend.enabled = true; //should be a different colour
		} else if (selected){
			rend.enabled = true;
		} else {
			rend.enabled = false;
		}
	}
	
	public void setBuilding(BuildingTemplate buildingTemplate){
		
		if (building != null){
			Destroy(building.gameObject);
		}
		building = makeNewBuilding(buildingTemplate);
	}
	
	public Building makeNewBuilding(BuildingTemplate buildingTemplate){

		Building newStorey = GameObject.Instantiate(Resources.Load(buildingTemplate.assetPath) as GameObject).AddComponent<Building>();
		newStorey.myPlot = this;		
	    newStorey.transform.parent = this.transform;
		newStorey.transform.localPosition = new Vector3(0,6.68f*placementGrid.getGridScale(),0);
		newStorey.transform.localEulerAngles = new Vector3(-90,90,0);
		if (buildingTemplate.assetPath == "HouseParts/chapel" && positionInGrid.y == placementGrid.getGridYMax() - 1){
			newStorey.transform.localEulerAngles = new Vector3(-90,0,-90);	
		}
		newStorey.transform.localScale = new Vector3(668*placementGrid.getGridScale(),668*placementGrid.getGridScale(),668*placementGrid.getGridScale());
		newStorey.grabValuesFromBuildingTemplate(buildingTemplate);
		newStorey.timeConstructed = Global.timeManager.year;		
		return newStorey;		
	}
	
	public void updateBuildingStats(float yearsSinceLastUpdate){		
		weight = 0;
		bool collapse = false;
		
		if (building != null){
			weight += building.weight;
			building.durability -= (float)Random.Range(0.50f,1.50f) * yearsSinceLastUpdate * (weight / 2f);
			Stats.money.addMoney(building.rent.multipliedBy(yearsSinceLastUpdate));
							
			if (building.durability <= 0){
				collapse = true;		//at the moment, this means that if ANY of the components on the stack run out of durability, the entire thing falls. I guess that's the risk you run from stacking too many buildings on top of each other. You've got to make sure they ALL stay in good repair, or one of them will betray them ALL.
			}
		}
		
		Debug.Log("Work out fire risk here based on the type of establishment. It's a low chance, but if a fire does start, it will spread to nearby businesses, encouraging you to make a fire break, but this will lose other establishments");
		Debug.Log("Hmm, what if the buildings actually grow over time, and increase in productivity? Also, that way you don't have to deal with an 'add storey' feature");
		
		
		if (collapse){
			FallAnimator pivot = new GameObject().AddComponent<FallAnimator>();
			pivot.transform.position = this.transform.position;
			pivot.transform.rotation = Quaternion.identity;

			building.transform.parent = pivot.transform;
			
			building = null;

			if (positionInGrid.y == placementGrid.getGridYMax()-1){
				//fall east				
				pivot.fallEast();
				Debug.Log("A building collapses east! in the year "+Global.timeManager.year);
			} else if (positionInGrid.y == 0){
				//fall west
				pivot.fallWest();
				Debug.Log("A building collapses west! in the year "+Global.timeManager.year);
			} else {
				pivot.fallIntoRoadway();
				//fall into the roadway
				Debug.Log("A building collapses into the roadway! in the year "+Global.timeManager.year);
			}
		}
	}

	void OnMouseDown(){
		if (Global.buildingMenuOpen){
			if (!Global.currentlySelectingOnGrid){
				Global.selectionFirstCorner = positionInGrid;
				Global.selectionSecondCorner = positionInGrid;
				Global.currentlySelectingOnGrid = true;
				placementGrid.updatePreselection();
			}	
		}
	}

	void OnMouseUp(){
		if (Global.buildingMenuOpen){
			if (Global.currentlySelectingOnGrid){
				placementGrid.changePreselectedToSelected();
				Global.currentlySelectingOnGrid = false;
				
				if (Global.buildingMenuOpen){
					//IF YOU ALREADY HAD THE BUILDING TYPE MENU UP AND ALREADY HAD A BUILDING TYPE SELECTED, THAT BUILDING TYPE INSTANTLY BUILDS IN THE SELECTION INSTEAD (ASSUMING YOU HAVE THE MONEY)
				} else {
					//IF YOU DON'T CURRENTLY HAVE THE BUILDING TYPE MENU UP, THIS IS WHEN IT SHOULD COME UP (FROM THE BOTTOM). ON CLICKING A BUILDING TYPE, IT WILL BE PLACED IN THE SELECTION
					Global.toggleBuildingUIBar();
				}
			}	
		}
	}

	void OnMouseEnter(){
	
		if (Global.buildingMenuOpen){
			if (Global.currentlySelectingOnGrid){
				Global.selectionSecondCorner = positionInGrid;
				placementGrid.updatePreselection();
			}
			else {
				rend.enabled = true;	
			}
		}
	}
	
    void OnMouseExit()
    {
		if (!selected && !preSelected){
			rend.enabled = false;
			}
    }
}