using UnityEngine;

public class FallAnimator : MonoBehaviour {

	float timeStarted;
	Vector3 eulerAnglesAtStart;
	Vector3 targetEulerAngles;
	bool falling;
	float eventualFallDuration; //seconds
	
	public void Update(){
		if (falling){
			float timeElapsed = (Time.time - timeStarted);
			float step = (timeElapsed / ((eventualFallDuration - timeElapsed)*2f)) / eventualFallDuration; //this is such that the closer you get to the end of the falling period, the quicker the time elapses
			
			if (step >= 1f) {
				falling = false;
				foreach (Transform child in this.transform){
					Destroy(child.gameObject);
				}
				Destroy(this.gameObject);
				}
			else {				
				transform.eulerAngles = Vector3.Slerp(eulerAnglesAtStart, targetEulerAngles, step);	
				}											
		}
	}

	public void fallEast(){		
		Debug.Log("SOME VARIETY OF FALL IS ABOUT TO BEGIN");
		targetEulerAngles = new Vector3(transform.eulerAngles.x + 90, transform.eulerAngles.y, transform.eulerAngles.z);
		beginFall();
	}
	
	public void fallWest(){		
		Debug.Log("SOME VARIETY OF FALL IS ABOUT TO BEGIN");
		targetEulerAngles = new Vector3(transform.eulerAngles.x - 90, transform.eulerAngles.y, transform.eulerAngles.z);
		beginFall();
	}
	
	public void fallIntoRoadway(){
		Debug.Log("SOME VARIETY OF FALL IS ABOUT TO BEGIN");
		targetEulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z);
		beginFall();
	}
	
	private void beginFall(){
		timeStarted = Time.time;
		eulerAnglesAtStart = transform.eulerAngles;
		eventualFallDuration = Random.Range(2.0f,4.0f);
		falling = true;			
	}
}