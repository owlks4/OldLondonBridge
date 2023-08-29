using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

public static class SaveLoadManager {

	private static SaveData currentlyLoadedFile;		//The currently loaded save file

	[Serializable]
	public class SaveData {
		//meta info
		public DateTime dateSaved;
		public int slotIndex;
		//game info
		public float year;
		public Money money;
		public BuildingTemplate[] availableBuildingTemplates;
		public Plot[,] grid;
		public List<BridgeComponent> bridge;
	}

	private static SaveData startingSaveFile = new SaveData {		//The default save file, should the player choose not to load an existing one
		slotIndex = -1, 	//-1, so the game will ask which slot you want to save it in
		dateSaved = DateTime.Now,
		year = 1200,
		money = new Money(100,0,0),
		grid = Global.placementGrid.getGrid(),
		availableBuildingTemplates = new BuildingTemplate[]{
			Global.getBuildingTemplateByName("Grocers"),
			Global.getBuildingTemplateByName("Bakers"),
			Global.getBuildingTemplateByName("Fishmongers"),
			Global.getBuildingTemplateByName("Haberdashers"),
			Global.getBuildingTemplateByName("Embroiderers"),
			Global.getBuildingTemplateByName("Carpenters"),
			Global.getBuildingTemplateByName("Stables"),
			Global.getBuildingTemplateByName("Wheelwrights"),
			Global.getBuildingTemplateByName("Gatehouse"),
			Global.getBuildingTemplateByName("Tavern"),
			Global.getBuildingTemplateByName("Chapel")
			},
		bridge = new List<BridgeComponent>(){
			new Pier(0.76f,UnityEngine.Random.Range(100f,600f),"Pier",1.09f),
			new Arch(0.8f,UnityEngine.Random.Range(100f,600f),"Surrey Lock",-1),
			new Pier(0.93f,UnityEngine.Random.Range(100f,600f),"Pier",1f),
			new Arch(0.93f,UnityEngine.Random.Range(100f,600f),"Borough Lock",-1),
			new Pier(1.08f,UnityEngine.Random.Range(100f,600f),"Pier",0.99f),
			new Arch(1.26f,UnityEngine.Random.Range(100f,600f),"Rock Lock",-1),
			new Pier(1.4f,UnityEngine.Random.Range(100f,600f),"Pier",0.68f),
			new Arch(0.72f,UnityEngine.Random.Range(100f,600f),"Roger Lock",-1),
			new Pier(1.34f,UnityEngine.Random.Range(100f,600f),"Pier",0.69f),
			new Drawbridge(0.95f,UnityEngine.Random.Range(100f,600f),"Draw Lock",-1),
			new Pier(1.7f,UnityEngine.Random.Range(100f,600f),"Pier",0.71f),
			new Arch(0.83f,UnityEngine.Random.Range(100f,600f),"Nonesuch Lock",-1),
			new Pier(1.58f,UnityEngine.Random.Range(100f,600f),"Pier",0.66f),
			new Arch(0.75f,UnityEngine.Random.Range(100f,600f),"Pedlar's Lock",-1),
			new Pier(2.4f,UnityEngine.Random.Range(100f,600f),"Great Pier",0.65f),
			new Arch(0.82f,UnityEngine.Random.Range(100f,600f),"Chapel Lock",-1),
			new Pier(1.32f,UnityEngine.Random.Range(100f,600f),"Pier",0.9f),
			new Arch(1.00f,UnityEngine.Random.Range(100f,600f),"St. Mary's Lock",-1),
			new Pier(1.44f,UnityEngine.Random.Range(100f,600f),"Pier",0.78f),
			new Arch(1.06f,UnityEngine.Random.Range(100f,600f),"Queen's Lock",-1),	
			new Pier(0.72f,UnityEngine.Random.Range(100f,600f),"Pier",1.36f),
			new Arch(1.1f,UnityEngine.Random.Range(100f,600f),"King's Lock",-1),	
			new Pier(1.7f,UnityEngine.Random.Range(100f,600f),"Pier",0.72f),
			new Arch(1.03f,UnityEngine.Random.Range(100f,600f),"3rd Lock",-1),	
			new Pier(1.43f,UnityEngine.Random.Range(100f,600f),"Pier",0.78f),
			new Arch(1.01f,UnityEngine.Random.Range(100f,600f),"2nd Lock",-1),	
			new Pier(1.97f,UnityEngine.Random.Range(100f,600f),"Pier",0.68f),
			new Arch(0.53f,UnityEngine.Random.Range(100f,600f),"London Shore",-1),
			new Pier(1.66f,UnityEngine.Random.Range(100f,600f),"Pier",0.74f)
		}
	};
	
	public static void init(){
		loadFile(startingSaveFile);
		saveGameToFile(-1);	//temp
	}
	
	public static void loadFile(SaveData file){
		
		currentlyLoadedFile = file;
		
		Global.placementGrid.setGrid(file.grid);
		Global.timeManager.year = currentlyLoadedFile.year;
		Stats.money = currentlyLoadedFile.money;
		Bridge.bridgeComponents = currentlyLoadedFile.bridge; //set the current layout of the bridge arches, piers etc		
		
		//create all the building type menu options based on how many are currently available to the player
		
		Global.fillOutTabBarWithAvailableBuildingTemplates(BuildingCategory.HEALTH);
	}
	
	public static SaveData getCurrentlyLoadedFile(){
		return currentlyLoadedFile;
	}
	
	public static void saveGameToFile(int saveIndex){
		if (saveIndex == -1){
			Debug.Log("You should NOT be saving a file with an index of -1. The -1 indicates that you should have prompted the user to choose a slot before reaching this point.");			
		}
		string json = JsonUtility.ToJson(currentlyLoadedFile);
		string base64json = Convert.ToBase64String(Encoding.UTF8.GetBytes(json));
		File.WriteAllText(Path.Combine(Application.streamingAssetsPath, "savefile"+saveIndex+".savegame"),base64json);

	}
	
	public static void loadGameFromFile(string filePath){
		string json = Encoding.UTF8.GetString(Convert.FromBase64String(File.ReadAllText(filePath)));
		loadFile(JsonUtility.FromJson<SaveData>(json));
	}
}