using System.Collections.Generic;
using UnityEngine;

public class BridgeComponent {

	public bool eastSideHasCollapsed;
	public bool westSideHasCollapsed;
	
	public Transform eastSide;
	public Transform westSide;
	
	public Transform starling;
	
	public float starlingBoundsX = 0;
	
	public string name;
	public float lengthCoefficient = 1f;
	public float durability = 400f;		//measured in terms of the number of years it can last before falling down, depending on other things too
	public float starlingRelativeWidth = -1f;
	
	public BridgeComponent(float lengthCoefficient, float durability, string name, float starlingRelativeWidth){
		this.lengthCoefficient = lengthCoefficient;
		this.durability = durability;
		this.name = name;
		this.starlingRelativeWidth = starlingRelativeWidth;
	}

	public float generateInWorld(float position, Transform parent){
		GameObject component = null;
		
		if (this.GetType() == typeof(Pier)){					//This would've been a switch statement, but the compiler doesn't accept typeof(Pier) as a constant (even though it effectively should be) so it won't accept it
			component = Resources.Load("Bridge/PIER") as GameObject;
		} else if (this.GetType() == typeof(Arch)){							
			component = Resources.Load("Bridge/ARCH") as GameObject;
		} else if (this.GetType() == typeof(Drawbridge)){							
			component = Resources.Load("Bridge/DRAWBRIDGE") as GameObject;
			((Drawbridge)this).spawnDrawbridgeElement(position,parent);
		} 
		else {
			Debug.Log("UNRECOGNISED BRIDGE COMPONENT TYPE...?");
		}
		
		eastSide = GameObject.Instantiate(component, parent).transform;
		westSide = GameObject.Instantiate(component, parent).transform;
		eastSide.transform.localPosition = new Vector3(position,0,0);
		westSide.transform.localPosition = new Vector3(position,0,0);
		eastSide.transform.localEulerAngles = new Vector3(-90,0,0);
		westSide.transform.localEulerAngles = new Vector3(-90,0,0);
		
		eastSide.localScale = new Vector3(lengthCoefficient*100f,100f,100f);
		westSide.localScale = new Vector3(lengthCoefficient*100f,-100f,100f);
		
		eastSide.gameObject.AddComponent<BridgeComponentSelectable>().bridgeComponent = this;
		westSide.gameObject.AddComponent<BridgeComponentSelectable>().bridgeComponent = this;
		
		float boundsX = eastSide.gameObject.GetComponent<MeshFilter>().sharedMesh.bounds.size.x;
		
		if (starlingRelativeWidth > 0){
			starling = GameObject.Instantiate(Resources.Load("Bridge/STARLING") as GameObject, eastSide.transform).transform;			
			starling.transform.localPosition = new Vector3(boundsX/2f,0f,0.0295f);
			starling.transform.localEulerAngles = new Vector3(0,0,90);
			starling.transform.localScale = new Vector3(1f,starlingRelativeWidth,1f);
			starlingBoundsX = eastSide.gameObject.GetComponent<MeshFilter>().sharedMesh.bounds.size.y * starling.transform.localScale.y * 100f;
			starling.gameObject.AddComponent<BridgeComponentSelectable>().bridgeComponent = this;
		}
		
		return boundsX * 100f * lengthCoefficient;
	}
}