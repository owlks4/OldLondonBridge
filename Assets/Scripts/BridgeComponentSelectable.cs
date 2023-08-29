using System.Collections.Generic;
using UnityEngine;

public class BridgeComponentSelectable : MonoBehaviour {

	public BridgeComponent bridgeComponent;
	
	Renderer[] getRends(){
		if (bridgeComponent.GetType() == typeof(Drawbridge)){			
				return new Renderer[]{((Drawbridge)bridgeComponent).drawbridgePart.gameObject.GetComponent<Renderer>()};
			 }
			 else {
				 return new Renderer[]{
						bridgeComponent.eastSide.GetComponent<Renderer>(),
						bridgeComponent.westSide.GetComponent<Renderer>()};
			 }
	}
	
	void OnMouseEnter(){
		if (!Global.buildingMenuOpen){
			Global.changeEmissionHighlight(getRends(), new Color(0.007843143f, 0.3346648f, 0.5960784f), true);
			}
	}
	
	void OnMouseUpAsButton(){
		if (!Global.buildingMenuOpen){
			Debug.Log(bridgeComponent.name);
			}
	}
	
	void OnMouseExit(){
		if (!Global.buildingMenuOpen){
			Global.changeEmissionHighlight(getRends(), new Color(0f, 0f, 0f), false);
		}
	}
}