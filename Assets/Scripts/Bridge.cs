using System.Collections.Generic;
using UnityEngine;

public static class Bridge {

	public static List<BridgeComponent> bridgeComponents = new List<BridgeComponent>();
	public static GameObject bridgeParentObject;
	static float bridgeLength;

	public static void generateBridgeInWorld(){
		
		float cumuPosition = 0;
		
		foreach (BridgeComponent component in bridgeComponents){
			cumuPosition += component.generateInWorld(cumuPosition,bridgeParentObject.transform);
		}
		
		bridgeLength = cumuPosition;
	}
	
	public static void updateBridgeStats(float yearsSinceLastUpdate){
		double obstructedness = 0;
		
		foreach (BridgeComponent component in bridgeComponents){
			obstructedness += component.starlingBoundsX;
			Debug.Log("I guess there should also be a check for waterwheels in the obstructedness accumulation here?");
			
			//check the placement grid squares above the position of this bridgecomponent to factor in the weight of the buildings above, as well as footfall etc according to the proportion of traffic types
			component.durability -= (float)Random.Range(0.50f,1.50f) * yearsSinceLastUpdate * (float)Stats.archFlowRate;	
			if (component.durability <= 0){
				Debug.Log(component.name + " collapses! in the year "+Global.timeManager.year);
				}
			}
		
		Stats.archFlowRate = obstructedness / bridgeLength;	//obstructiveness as fraction of total width. Higher obstruction means higher flow through the arches, which is what this variable represents
		Stats.riverFlowRate = 1.0 - Stats.archFlowRate; //the flow of the river: the opposite of the flow through the arches. If arch flow rate is high, then the river must be obstructed, so river flow is slooooow.
	}
}