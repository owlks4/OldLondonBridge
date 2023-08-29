using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class BuildingTemplateMenuOption : MonoBehaviour {
	
	public BuildingTemplate buildingTemplate;
	public TextMeshProUGUI nameText;
	public GameObject backing;

	public void init(){
		Global.BuildingEffectDesc descs = Global.buildingEffectDescs[buildingTemplate.effect];
		backing.transform.Find("BuildingTemplateUIDescPos").GetComponent<TextMeshProUGUI>().text = descs.pos;
		backing.transform.Find("BuildingTemplateUIDescNeg").GetComponent<TextMeshProUGUI>().text = descs.neg;
		hideBacking(null);	
	}

	public void onThisSelected(){

		if (Global.placementGrid.selectedBuildingTemplateMenuOption != this){   //we have just changed our selected building type option to this building type
			Global.placementGrid.selectedBuildingTemplateMenuOption = this;
			if (Global.placementGrid.numPlotsCurrentlySelected > 0){
			//then build it (the placement grid will check if we can afford it)
			Global.placementGrid.buildBuildingInSelectedArea(this.buildingTemplate);	
			}
		} else {															//this was already our selected building type; deselect it
			Global.placementGrid.selectedBuildingTemplateMenuOption = null;
		}
	}
	
	public void showBacking(BaseEventData data)
    {
        backing.SetActive(true);
    }
	
    public void hideBacking(BaseEventData data)
    {
        backing.SetActive(false);
    }
	
	public void setFromBuildingTemplate(BuildingTemplate buildingTemplate){
		bool firstTimeSetup = false;
		if (this.buildingTemplate == null){
			firstTimeSetup = true;
		}
		this.buildingTemplate = buildingTemplate;
		this.nameText.text = buildingTemplate.name;
		if (firstTimeSetup){
			init();
		}
		Outline o = this.gameObject.GetComponent<Outline>();
		switch (buildingTemplate.category){
			case BuildingCategory.HEALTH:
				o.effectColor = new Color(0.75f,0,0);
			break;
			case BuildingCategory.TRADE:
				o.effectColor = Color.green;
			break;
			case BuildingCategory.CONSTRUCTION:
				o.effectColor = Color.magenta;
			break;
			case BuildingCategory.TRANSPORT:
				o.effectColor = Color.yellow;
			break;
			case BuildingCategory.SOCIAL:
				o.effectColor = Color.blue;
			break;
		}
	}
}