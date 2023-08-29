using System.Collections.Generic;
using UnityEngine;

public class Drawbridge : BridgeComponent {
	
	public Transform drawbridgePart;
	
	public Drawbridge(float lengthCoefficient, float durability, string name, float starlingRelativeWidth) : base(lengthCoefficient, durability, name, starlingRelativeWidth) {
	}
	
	public void spawnDrawbridgeElement(float positionX, Transform parent){
		drawbridgePart = GameObject.Instantiate(Resources.Load("Bridge/DRAWBRIDGE_PLATFORM") as GameObject,parent).transform;
		drawbridgePart.transform.localPosition = new Vector3(positionX,8.76f,0);
		drawbridgePart.transform.localEulerAngles = new Vector3(-90,-90,90);
		drawbridgePart.transform.localScale = new Vector3(lengthCoefficient*100f,70,100);
		drawbridgePart.gameObject.AddComponent<BridgeComponentSelectable>().bridgeComponent = this;
	}
}