using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TimeManager : MonoBehaviour {

	public float year = 1200;
	
	private TextMeshProUGUI centuryUI;
	private RectTransform centuryProgressBar;
	
	private TextMeshProUGUI moneyUI;
	
	private float TIME_STEP = 2f;	//a good default seems to be 2f at the moment?
	
	private string[] monthStrings = new string[]{"None","January","February","March","April","May","June","July","August","September","October","November","December"};
	
	private float mostRecentCenturyFloor = 12;
	
	private Transform river;
	private Renderer riverRenderer;
	private float TIDE_MIN_HEIGHT = -1.8f;
	private float TIDE_MAX_HEIGHT = -0.5f;
	private float TIDE_SPEED = 0.1f;
	
	public void Start(){
		centuryUI = GameObject.Find("CenturyUI").GetComponent<TextMeshProUGUI>();	
		centuryProgressBar = GameObject.Find("CenturyProgressBar").GetComponent<RectTransform>();	
		moneyUI = GameObject.Find("MoneyUI").GetComponent<TextMeshProUGUI>();
		river = GameObject.Find("River").transform;
		riverRenderer = river.gameObject.GetComponent<Renderer>();
	}
	
	public void Update(){
		
		year += TIME_STEP * Time.deltaTime;
		
		calculateMoneyOverflow(); //the money overflow calculations are also done here
		
		float centuryFloor = Mathf.Floor(year / 100f) * 100;		
		
		if (centuryFloor != mostRecentCenturyFloor){ //if we have changed century, update the display string
			centuryUI.text = getCenturyString();
			mostRecentCenturyFloor = centuryFloor;			
		}		
		
		centuryProgressBar.localScale = new Vector3((year - centuryFloor) / 100f,1,1);
				
		float tideValueClamped = (Mathf.Sin(year*TIDE_SPEED) / 2f) + 0.5f; //move the tide in and out according to a sine function
		river.transform.position = new Vector3(river.transform.position.x,TIDE_MAX_HEIGHT - (tideValueClamped * (TIDE_MAX_HEIGHT - TIDE_MIN_HEIGHT)),river.transform.position.z);
		Material m = riverRenderer.materials[0];
		m.SetTextureOffset("_MainTex", new Vector2(0, tideValueClamped/4f));
		riverRenderer.materials[0] = m;
		
		moneyUI.text = Stats.money.getString();
	}
	
	public void calculateMoneyOverflow(){
		Stats.money.checkOverflowAndUpdate();
	}
	
	public string getCenturyString(){
		string centuryString = (Mathf.Floor(year / 100f) + 1).ToString();
		
		if (centuryString[0] == '1' && centuryString.Length > 1){	//Append the appropriate suffix to the century number. The length check is to account for the edge case of '1st century', should it ever come up.
			centuryString += "th century"; //for 11th, 12th, etc. centuries
		} else {
			switch (centuryString[1]){
				case '1':
					centuryString += "st century";
					break;
				case '2':
					centuryString += "nd century";
					break;
				case '3':
					centuryString += "rd century";
					break;
				default:
					centuryString += "th century";
					break;
			}
		}
		return centuryString;
	}
}