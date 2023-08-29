using UnityEngine;
using System.Collections.Generic;

public static class Global {

	public static PlacementGrid placementGrid;
	public static TimeManager timeManager;
	
	public static float UPDATE_STATS_INTERVAL_IN_YEARS = 1;

	public static bool currentlySelectingOnGrid;

	public static Vector2Int selectionFirstCorner;
	public static Vector2Int selectionSecondCorner;
	
	public static GameObject tabUIBar;
	public static GameObject tabUIBarSeparator;
	public static bool buildingMenuOpen;
	
	public static Dictionary<BuildingEffect,BuildingEffectDesc> buildingEffectDescs = new Dictionary<BuildingEffect,BuildingEffectDesc>(){	
		{BuildingEffect.NONE, new BuildingEffectDesc("+ Undefined","- Undefined")},
		{BuildingEffect.WATERWORKS, new BuildingEffectDesc("+ Population health","- River narrowed")},
		{BuildingEffect.WATERWORKS_INCLUDING_TURN_OF_TIDE, new BuildingEffectDesc("Works even at turn of tide","- River narrowed")},
		{BuildingEffect.FOOD_SHOP, new BuildingEffectDesc("+ Population health","")},
		{BuildingEffect.REDUCE_DISEASE, new BuildingEffectDesc("+ Population health","")},
		{BuildingEffect.ANTI_FIRE, new BuildingEffectDesc("+ Combats fire risk","")},
		{BuildingEffect.IMPROVE_RESOURCE_ANIMAL_PRODUCTS, new BuildingEffectDesc("+ Improved animal trade","")},
		{BuildingEffect.IMPROVE_RESOURCE_FINERY, new BuildingEffectDesc("+ Improved finery trade","- Increased fire risk")},
		{BuildingEffect.IMPROVE_RESOURCE_TOOLS, new BuildingEffectDesc("+ Improved tool trade","")},
		{BuildingEffect.IMPROVE_RESOURCE_SCIENCE, new BuildingEffectDesc("+ Improved science trade","")},
		{BuildingEffect.IMPROVE_RESOURCE_EVERYDAY, new BuildingEffectDesc("+ Improved everyday trade","")},
		{BuildingEffect.IMPROVE_RESOURCE_METAL, new BuildingEffectDesc("+ Improved metalworking trade","- Heavy")},
		{BuildingEffect.CITIZEN_SELF_DEFENSE_UP_IF_IN_MODERATION, new BuildingEffectDesc("+ Improved citizen defense","- Increased citizen unruliness")},
		{BuildingEffect.FASTER_STONE_BUILDINGS, new BuildingEffectDesc("+ Improved stone buildings","- Heavy")},
		{BuildingEffect.FASTER_WOODEN_BUILDINGS, new BuildingEffectDesc("+ Improved wood construction","- Increased fire risk")},
		{BuildingEffect.FASTER_BRICK_BUILDINGS, new BuildingEffectDesc("+ Improved brick construction","- Heavy")},
		{BuildingEffect.LONGER_LASTING_BUILDINGS, new BuildingEffectDesc("+ Improved overall construction","- Heavy")},
		{BuildingEffect.MORE_EFFICIENT_TECHNOLOGY, new BuildingEffectDesc("+ Technology efficiency","- Heavy")},
		{BuildingEffect.IMPROVED_HORSES, new BuildingEffectDesc("+ Horse-drawn traffic turnover","- Increased bridge wear")},
		{BuildingEffect.IMPROVED_VEHICLES, new BuildingEffectDesc("+ Vehicle traffic turnover","- Increased bridge wear")},
		{BuildingEffect.IMPROVED_FOOT_TRAFFIC, new BuildingEffectDesc("+ Pedestrian traffic turnover","- Increased bridge wear")},
		{BuildingEffect.IMPROVED_TRAFFIC_IN_GENERAL, new BuildingEffectDesc("+ Traffic movement increased","- Increased bridge wear")},
		{BuildingEffect.SECURITY_IF_IN_MODERATION, new BuildingEffectDesc("+ Increased security","- Increased intimidation")},
		{BuildingEffect.PLACE_OF_WORSHIP, new BuildingEffectDesc("+ Population calm","")},
		{BuildingEffect.DIPLOMACY, new BuildingEffectDesc("+ Diplomacy","")},
		{BuildingEffect.IMPROVED_EDUCATION, new BuildingEffectDesc("+ Education","")},
		{BuildingEffect.IMPROVED_VALUE_FOR_MONEY, new BuildingEffectDesc("+ Construction value for money","")},
	};
	
	public class BuildingEffectDesc {
		public string pos;
		public string neg;
		public BuildingEffectDesc(string pos, string neg){
			this.pos = pos;
			this.neg = neg;
		}
	}
	
	public static void toggleBuildingUIBar(){				
		placementGrid.toggleBuildBarOpen();
	}
	
	public static BuildingTemplateMenuOption buildingTemplateOptionBasis;
	
	private static List<BuildingTemplate> healthBuildingTemplates = new List<BuildingTemplate>();
	private static List<BuildingTemplate> craftBuildingTemplates = new List<BuildingTemplate>();
	private static List<BuildingTemplate> constructionBuildingTemplates = new List<BuildingTemplate>();
	private static List<BuildingTemplate> transportBuildingTemplates = new List<BuildingTemplate>();
	private static List<BuildingTemplate> socialBuildingTemplates = new List<BuildingTemplate>();
	
	public static void registerBuildingTemplates(){
		
		buildingTemplateOptionBasis = GameObject.Find("BuildingTemplateUIImage").GetComponent<BuildingTemplateMenuOption>();
		
		//HEALTH
		//Food
		registerBuildingTemplate(new BuildingTemplate ("Grocers", 			BuildingCategory.HEALTH, new Money(0,0,0), 1f, 300f, BuildingEffect.FOOD_SHOP, "HouseParts/houseSingular"));
		registerBuildingTemplate(new BuildingTemplate ("Bakers",  			BuildingCategory.HEALTH, new Money(0,0,0), 1f, 300f, BuildingEffect.FOOD_SHOP,"HouseParts/houseSingular"));
		registerBuildingTemplate(new BuildingTemplate ("Fruiterers",		BuildingCategory.HEALTH, new Money(0,0,0), 1f, 300f, BuildingEffect.FOOD_SHOP,"HouseParts/houseSingular"));
		//Meat & Fish
		registerBuildingTemplate(new BuildingTemplate ("Fishmongers",		BuildingCategory.HEALTH, new Money(0,0,0), 1f, 300f, BuildingEffect.FOOD_SHOP,"HouseParts/houseSingular"));
		registerBuildingTemplate(new BuildingTemplate ("Poulters",			BuildingCategory.HEALTH, new Money(0,0,0), 1f, 300f, BuildingEffect.FOOD_SHOP,"HouseParts/houseSingular"));
		registerBuildingTemplate(new BuildingTemplate ("Butchers",			BuildingCategory.HEALTH, new Money(0,0,0), 1f, 300f, BuildingEffect.FOOD_SHOP,"HouseParts/houseSingular"));
		//Drink
		registerBuildingTemplate(new BuildingTemplate ("Distillers",		BuildingCategory.HEALTH, new Money(0,0,0), 1f, 300f, BuildingEffect.REDUCE_DISEASE,"HouseParts/houseSingular"));
		registerBuildingTemplate(new BuildingTemplate ("Brewers",			BuildingCategory.HEALTH, new Money(0,0,0), 1f, 300f, BuildingEffect.REDUCE_DISEASE,"HouseParts/houseSingular"));
		registerBuildingTemplate(new BuildingTemplate ("Vintners",			BuildingCategory.HEALTH, new Money(0,0,0), 1f, 300f, BuildingEffect.REDUCE_DISEASE,"HouseParts/houseSingular"));
		//Medicine
		registerBuildingTemplate(new BuildingTemplate ("Salters",  			BuildingCategory.HEALTH, new Money(0,0,0), 1f, 300f, BuildingEffect.REDUCE_DISEASE,"HouseParts/houseSingular"));
		registerBuildingTemplate(new BuildingTemplate ("Apothecary",		BuildingCategory.HEALTH, new Money(0,0,0), 1f, 300f, BuildingEffect.REDUCE_DISEASE,"HouseParts/houseSingular"));
		registerBuildingTemplate(new BuildingTemplate ("Barber",			BuildingCategory.HEALTH, new Money(0,0,0), 1f, 300f, BuildingEffect.REDUCE_DISEASE,"HouseParts/houseSingular"));
		registerBuildingTemplate(new BuildingTemplate ("Spectacle-makers",	BuildingCategory.HEALTH, new Money(0,0,0), 1f, 300f, BuildingEffect.REDUCE_DISEASE,"HouseParts/houseSingular"));
		//Practical
		registerBuildingTemplate(new BuildingTemplate ("Waterworks",		BuildingCategory.HEALTH, new Money(0,0,0), 1f, 300f, BuildingEffect.WATERWORKS,"HouseParts/houseSingular"));
		registerBuildingTemplate(new BuildingTemplate ("Waterworks w/ Steam Engine",BuildingCategory.HEALTH, new Money(0,0,0), 1f, 300f, BuildingEffect.WATERWORKS_INCLUDING_TURN_OF_TIDE,"HouseParts/houseSingular"));
		registerBuildingTemplate(new BuildingTemplate ("Plumber",			BuildingCategory.HEALTH, new Money(0,0,0), 1f, 300f, BuildingEffect.REDUCE_DISEASE,"HouseParts/houseSingular"));
		registerBuildingTemplate(new BuildingTemplate ("Firefighters",		BuildingCategory.HEALTH, new Money(0,0,0), 1f, 300f, BuildingEffect.ANTI_FIRE,"HouseParts/houseSingular"));
		
		
		//CRAFT
		registerBuildingTemplate(new BuildingTemplate ("Skinners",			BuildingCategory.TRADE, new Money(0,0,0), 1f, 300f, BuildingEffect.IMPROVE_RESOURCE_ANIMAL_PRODUCTS,"HouseParts/houseSingular"));
		registerBuildingTemplate(new BuildingTemplate ("Woolmen",			BuildingCategory.TRADE, new Money(0,0,0), 1f, 300f, BuildingEffect.IMPROVE_RESOURCE_ANIMAL_PRODUCTS,"HouseParts/houseSingular"));
		registerBuildingTemplate(new BuildingTemplate ("Leathersellers",	BuildingCategory.TRADE, new Money(0,0,0), 1f, 300f, BuildingEffect.IMPROVE_RESOURCE_ANIMAL_PRODUCTS,"HouseParts/houseSingular"));
		
		registerBuildingTemplate(new BuildingTemplate ("Haberdashers",		BuildingCategory.TRADE, new Money(0,0,0), 1f, 300f, BuildingEffect.IMPROVE_RESOURCE_FINERY,"HouseParts/houseSingular"));
		registerBuildingTemplate(new BuildingTemplate ("Dyers",				BuildingCategory.TRADE, new Money(0,0,0), 1f, 300f, BuildingEffect.IMPROVE_RESOURCE_FINERY,"HouseParts/houseSingular"));
		registerBuildingTemplate(new BuildingTemplate ("Embroiderers",		BuildingCategory.TRADE, new Money(0,0,0), 1f, 300f, BuildingEffect.IMPROVE_RESOURCE_FINERY,"HouseParts/houseSingular"));
		registerBuildingTemplate(new BuildingTemplate ("Wire-drawers",		BuildingCategory.TRADE, new Money(0,0,0), 1f, 300f, BuildingEffect.IMPROVE_RESOURCE_FINERY,"HouseParts/houseSingular"));
		
		registerBuildingTemplate(new BuildingTemplate ("Needlemakers",		BuildingCategory.TRADE, new Money(0,0,0), 1f, 300f, BuildingEffect.IMPROVE_RESOURCE_TOOLS,"HouseParts/houseSingular"));
		registerBuildingTemplate(new BuildingTemplate ("Basketmakers",		BuildingCategory.TRADE, new Money(0,0,0), 1f, 300f, BuildingEffect.IMPROVE_RESOURCE_TOOLS,"HouseParts/houseSingular"));
		registerBuildingTemplate(new BuildingTemplate ("Painter-stainers",	BuildingCategory.TRADE, new Money(0,0,0), 1f, 300f, BuildingEffect.IMPROVE_RESOURCE_TOOLS,"HouseParts/houseSingular"));
		
		registerBuildingTemplate(new BuildingTemplate ("Candlemakers",		BuildingCategory.TRADE, new Money(0,0,0), 1f, 300f, BuildingEffect.IMPROVE_RESOURCE_EVERYDAY,"HouseParts/houseSingular"));
		registerBuildingTemplate(new BuildingTemplate ("Furniture-makers",	BuildingCategory.TRADE, new Money(0,0,0), 1f, 300f, BuildingEffect.IMPROVE_RESOURCE_EVERYDAY,"HouseParts/houseSingular"));
		registerBuildingTemplate(new BuildingTemplate ("Booksellers",		BuildingCategory.TRADE, new Money(0,0,0), 1f, 300f, BuildingEffect.IMPROVE_RESOURCE_EVERYDAY,"HouseParts/houseSingular"));
		
		registerBuildingTemplate(new BuildingTemplate ("Clockmakers",		BuildingCategory.TRADE, new Money(0,0,0), 1f, 300f, BuildingEffect.IMPROVE_RESOURCE_SCIENCE,"HouseParts/houseSingular"));
		registerBuildingTemplate(new BuildingTemplate ("Telescopy",			BuildingCategory.TRADE, new Money(0,0,0), 1f, 300f, BuildingEffect.IMPROVE_RESOURCE_SCIENCE,"HouseParts/houseSingular"));
		registerBuildingTemplate(new BuildingTemplate ("Cartography",		BuildingCategory.TRADE, new Money(0,0,0), 1f, 300f, BuildingEffect.IMPROVE_RESOURCE_SCIENCE,"HouseParts/houseSingular"));

		registerBuildingTemplate(new BuildingTemplate ("Ironmongers",		BuildingCategory.TRADE, new Money(0,0,0), 1f, 300f, BuildingEffect.IMPROVE_RESOURCE_METAL,"HouseParts/houseSingular"));
		registerBuildingTemplate(new BuildingTemplate ("Goldsmiths",		BuildingCategory.TRADE, new Money(0,0,0), 1f, 300f, BuildingEffect.IMPROVE_RESOURCE_METAL,"HouseParts/houseSingular"));
		registerBuildingTemplate(new BuildingTemplate ("Blacksmiths",		BuildingCategory.TRADE, new Money(0,0,0), 1f, 300f, BuildingEffect.IMPROVE_RESOURCE_METAL,"HouseParts/houseSingular"));
		registerBuildingTemplate(new BuildingTemplate ("Pewterers",			BuildingCategory.TRADE, new Money(0,0,0), 1f, 300f, BuildingEffect.IMPROVE_RESOURCE_METAL,"HouseParts/houseSingular"));
		registerBuildingTemplate(new BuildingTemplate ("Founders",			BuildingCategory.TRADE, new Money(0,0,0), 1f, 300f, BuildingEffect.IMPROVE_RESOURCE_METAL,"HouseParts/houseSingular"));
				
		registerBuildingTemplate(new BuildingTemplate ("Cutlers",			BuildingCategory.TRADE, new Money(0,0,0), 1f, 300f, BuildingEffect.CITIZEN_SELF_DEFENSE_UP_IF_IN_MODERATION,"HouseParts/houseSingular"));
		registerBuildingTemplate(new BuildingTemplate ("Armourers",			BuildingCategory.TRADE, new Money(0,0,0), 1f, 300f, BuildingEffect.CITIZEN_SELF_DEFENSE_UP_IF_IN_MODERATION,"HouseParts/houseSingular"));
		registerBuildingTemplate(new BuildingTemplate ("Bowyers",			BuildingCategory.TRADE, new Money(0,0,0), 1f, 300f, BuildingEffect.CITIZEN_SELF_DEFENSE_UP_IF_IN_MODERATION,"HouseParts/houseSingular"));	
		registerBuildingTemplate(new BuildingTemplate ("Fletchers",			BuildingCategory.TRADE, new Money(0,0,0), 1f, 300f, BuildingEffect.CITIZEN_SELF_DEFENSE_UP_IF_IN_MODERATION,"HouseParts/houseSingular"));	
		registerBuildingTemplate(new BuildingTemplate ("Gunmakers",			BuildingCategory.TRADE, new Money(0,0,0), 1f, 300f, BuildingEffect.CITIZEN_SELF_DEFENSE_UP_IF_IN_MODERATION,"HouseParts/houseSingular"));
		
		registerBuildingTemplate(new BuildingTemplate ("Bank",				BuildingCategory.TRADE, new Money(0,0,0), 1f, 300f, BuildingEffect.IMPROVED_VALUE_FOR_MONEY,"HouseParts/houseSingular"));
		

		//CONSTRUCTION
		//Exterior
		registerBuildingTemplate(new BuildingTemplate ("Masons",		BuildingCategory.CONSTRUCTION, new Money(0,0,0), 1f, 300f, BuildingEffect.FASTER_STONE_BUILDINGS,"HouseParts/houseSingular"));	
		registerBuildingTemplate(new BuildingTemplate ("Carpenters",	BuildingCategory.CONSTRUCTION, new Money(0,0,0), 1f, 300f, BuildingEffect.FASTER_WOODEN_BUILDINGS,"HouseParts/houseSingular"));	
		registerBuildingTemplate(new BuildingTemplate ("Bricklayers",	BuildingCategory.CONSTRUCTION, new Money(0,0,0), 1f, 300f, BuildingEffect.FASTER_BRICK_BUILDINGS,"HouseParts/houseSingular"));	
		registerBuildingTemplate(new BuildingTemplate ("Pavers",		BuildingCategory.CONSTRUCTION, new Money(0,0,0), 1f, 300f, BuildingEffect.IMPROVED_TRAFFIC_IN_GENERAL,"HouseParts/houseSingular"));	
		//Interior
		registerBuildingTemplate(new BuildingTemplate ("Plasterers",	BuildingCategory.CONSTRUCTION, new Money(0,0,0), 1f, 300f, BuildingEffect.LONGER_LASTING_BUILDINGS,"HouseParts/houseSingular"));	
		registerBuildingTemplate(new BuildingTemplate ("Upholstery",	BuildingCategory.CONSTRUCTION, new Money(0,0,0), 1f, 300f, BuildingEffect.LONGER_LASTING_BUILDINGS,"HouseParts/houseSingular"));	
		registerBuildingTemplate(new BuildingTemplate ("Joiners",		BuildingCategory.CONSTRUCTION, new Money(0,0,0), 1f, 300f, BuildingEffect.LONGER_LASTING_BUILDINGS,"HouseParts/houseSingular"));	
		registerBuildingTemplate(new BuildingTemplate ("Glaziers & Glass-Painters",	BuildingCategory.CONSTRUCTION, new Money(0,0,0), 1f, 300f, BuildingEffect.LONGER_LASTING_BUILDINGS,"HouseParts/houseSingular"));	

		//Other construction
		registerBuildingTemplate(new BuildingTemplate ("Architect",		BuildingCategory.CONSTRUCTION, new Money(0,0,0), 1f, 300f, BuildingEffect.LONGER_LASTING_BUILDINGS,"HouseParts/houseSingular"));	
		registerBuildingTemplate(new BuildingTemplate ("Engineer",		BuildingCategory.CONSTRUCTION, new Money(0,0,0), 1f, 300f, BuildingEffect.MORE_EFFICIENT_TECHNOLOGY,"HouseParts/houseSingular"));
		
		
		//TRANSPORT
		registerBuildingTemplate(new BuildingTemplate ("Stables",		BuildingCategory.TRANSPORT, new Money(0,0,0), 1f, 300f, BuildingEffect.IMPROVED_HORSES,"HouseParts/houseSingular"));
		registerBuildingTemplate(new BuildingTemplate ("Saddler",		BuildingCategory.TRANSPORT, new Money(0,0,0), 1f, 300f, BuildingEffect.IMPROVED_HORSES,"HouseParts/houseSingular"));
		registerBuildingTemplate(new BuildingTemplate ("Bridlemakers",	BuildingCategory.TRANSPORT, new Money(0,0,0), 1f, 300f, BuildingEffect.IMPROVED_HORSES,"HouseParts/houseSingular"));
		registerBuildingTemplate(new BuildingTemplate ("Horseshoe-makers",BuildingCategory.TRANSPORT, new Money(0,0,0), 1f, 300f, BuildingEffect.IMPROVED_HORSES,"HouseParts/houseSingular"));
		registerBuildingTemplate(new BuildingTemplate ("Shoemakers",	BuildingCategory.TRANSPORT, new Money(0,0,0), 1f, 300f, BuildingEffect.IMPROVED_FOOT_TRAFFIC,"HouseParts/houseSingular"));
		registerBuildingTemplate(new BuildingTemplate ("Gloversmakers",	BuildingCategory.TRANSPORT, new Money(0,0,0), 1f, 300f, BuildingEffect.IMPROVED_FOOT_TRAFFIC,"HouseParts/houseSingular"));
		registerBuildingTemplate(new BuildingTemplate ("Feltmakers",	BuildingCategory.TRANSPORT, new Money(0,0,0), 1f, 300f, BuildingEffect.IMPROVED_FOOT_TRAFFIC,"HouseParts/houseSingular"));
		registerBuildingTemplate(new BuildingTemplate ("Carriagemen",	BuildingCategory.TRANSPORT, new Money(0,0,0), 1f, 300f, BuildingEffect.IMPROVED_VEHICLES,"HouseParts/houseSingular"));
		registerBuildingTemplate(new BuildingTemplate ("Shipwrights",	BuildingCategory.TRANSPORT, new Money(0,0,0), 1f, 300f, BuildingEffect.IMPROVED_VEHICLES,"HouseParts/houseSingular"));
		registerBuildingTemplate(new BuildingTemplate ("Coachmakers",	BuildingCategory.TRANSPORT, new Money(0,0,0), 1f, 300f, BuildingEffect.IMPROVED_VEHICLES,"HouseParts/houseSingular"));
		registerBuildingTemplate(new BuildingTemplate ("Wheelwrights",	BuildingCategory.TRANSPORT, new Money(0,0,0), 1f, 300f, BuildingEffect.IMPROVED_VEHICLES,"HouseParts/houseSingular"));
		registerBuildingTemplate(new BuildingTemplate ("Mariners",		BuildingCategory.TRANSPORT, new Money(0,0,0), 1f, 300f, BuildingEffect.IMPROVED_TRAFFIC_IN_GENERAL,"HouseParts/houseSingular"));
		
		
		//SOCIAL
		registerBuildingTemplate(new BuildingTemplate ("Scriveners",	BuildingCategory.SOCIAL, new Money(0,0,0), 1f, 300f, BuildingEffect.IMPROVED_EDUCATION,"HouseParts/houseSingular"));
		registerBuildingTemplate(new BuildingTemplate ("Stationers",	BuildingCategory.SOCIAL, new Money(0,0,0), 1f, 300f, BuildingEffect.IMPROVED_EDUCATION,"HouseParts/houseSingular"));
		registerBuildingTemplate(new BuildingTemplate ("Newspaper Office",BuildingCategory.SOCIAL, new Money(0,0,0), 1f, 300f, BuildingEffect.IMPROVED_EDUCATION,"HouseParts/houseSingular"));
		registerBuildingTemplate(new BuildingTemplate ("Booksellers",	BuildingCategory.SOCIAL, new Money(0,0,0), 1f, 300f, BuildingEffect.IMPROVED_EDUCATION,"HouseParts/houseSingular"));
		registerBuildingTemplate(new BuildingTemplate ("Monument",	BuildingCategory.SOCIAL, new Money(0,0,0), 1f, 300f, BuildingEffect.IMPROVED_EDUCATION,"HouseParts/houseSingular"));				
		registerBuildingTemplate(new BuildingTemplate ("Nonsuch House",	BuildingCategory.SOCIAL, new Money(0,0,0), 1f, 300f, BuildingEffect.DIPLOMACY,"HouseParts/houseSingular"));
		
		registerBuildingTemplate(new BuildingTemplate ("Gatehouse",	BuildingCategory.SOCIAL, new Money(0,0,0), 1f, 300f, BuildingEffect.CITIZEN_SELF_DEFENSE_UP_IF_IN_MODERATION,"HouseParts/houseSingular"));
		registerBuildingTemplate(new BuildingTemplate ("Tavern",	BuildingCategory.SOCIAL, new Money(0,0,0), 1f, 300f, BuildingEffect.CITIZEN_SELF_DEFENSE_UP_IF_IN_MODERATION,"HouseParts/houseSingular"));
		registerBuildingTemplate(new BuildingTemplate ("Bridge Wardens",	BuildingCategory.SOCIAL, new Money(0,0,0), 1f, 300f, BuildingEffect.CITIZEN_SELF_DEFENSE_UP_IF_IN_MODERATION,"HouseParts/houseSingular"));
		//OPEN SPACE is also an unspoken factor in the social sphere
		
		registerBuildingTemplate(new BuildingTemplate ("Chapel",	BuildingCategory.SOCIAL, new Money(0,0,0), 1f, 300f, BuildingEffect.PLACE_OF_WORSHIP,"HouseParts/chapel"));
		registerBuildingTemplate(new BuildingTemplate ("Church",	BuildingCategory.SOCIAL, new Money(0,0,0), 1f, 300f, BuildingEffect.PLACE_OF_WORSHIP,"HouseParts/chapel"));
		registerBuildingTemplate(new BuildingTemplate ("Cathedral",	BuildingCategory.SOCIAL, new Money(0,0,0), 1f, 300f, BuildingEffect.PLACE_OF_WORSHIP,"HouseParts/chapel"));
	}
	
	private static void registerBuildingTemplate(BuildingTemplate bt){
		switch (bt.category){
			case BuildingCategory.HEALTH:
				healthBuildingTemplates.Add(bt);
			break;
			case BuildingCategory.TRADE:
				craftBuildingTemplates.Add(bt);
			break;
			case BuildingCategory.CONSTRUCTION:
				constructionBuildingTemplates.Add(bt);
			break;
			case BuildingCategory.TRANSPORT:
				transportBuildingTemplates.Add(bt);
			break;
			case BuildingCategory.SOCIAL:
				socialBuildingTemplates.Add(bt);
			break;
			default:
				Debug.Log("SANITY CHECK FAILED: BUILDING TEMPLATE BEING REGISTERED WAS NOT OF A RECOGNISED TYPE!");
			break;
		}		
	}
	
	public static BuildingTemplate getBuildingTemplateByName(string name){
		foreach (BuildingTemplate bt in healthBuildingTemplates){
			if (bt.name == name){
				return bt;
			}
		}
		foreach (BuildingTemplate bt in craftBuildingTemplates){
			if (bt.name == name){
				return bt;
			}
		}
		foreach (BuildingTemplate bt in constructionBuildingTemplates){
			if (bt.name == name){
				return bt;
			}
		}
		foreach (BuildingTemplate bt in transportBuildingTemplates){
			if (bt.name == name){
				return bt;
			}
		}
		foreach (BuildingTemplate bt in socialBuildingTemplates){
			if (bt.name == name){
				return bt;
			}
		}
		Debug.Log("Failed to get building template by name");
		return null;
	}
	
	public static void fillOutTabBarWithAvailableBuildingTemplates(BuildingCategory bc){
		
		foreach (Transform child in Global.tabUIBarSeparator.transform){
			if (child.gameObject != buildingTemplateOptionBasis.transform.parent.gameObject){ //don't destroy the template that we keep disabled here!! but clear everything else
			GameObject.Destroy(child.gameObject);	
			}
		}
				
		SaveLoadManager.SaveData curFile = SaveLoadManager.getCurrentlyLoadedFile();
		
		int indexInVisualArrangement = 0;
		
		for (int i = 0; i < curFile.availableBuildingTemplates.Length; i++){
			if (curFile.availableBuildingTemplates[i].category != bc){
				continue;
			}
			GameObject newBuildingTemplateMenuOption = GameObject.Instantiate(buildingTemplateOptionBasis.transform.parent.gameObject, Global.tabUIBarSeparator.transform);
			newBuildingTemplateMenuOption.transform.Find("BuildingTemplateUIImage").GetComponent<BuildingTemplateMenuOption>().setFromBuildingTemplate(curFile.availableBuildingTemplates[i]);
			newBuildingTemplateMenuOption.GetComponent<RectTransform>().anchoredPosition = new Vector2(122.5f+(180f*indexInVisualArrangement),-27.54993f);
			newBuildingTemplateMenuOption.SetActive(true);
			indexInVisualArrangement++;
		}
		
		buildingTemplateOptionBasis.transform.parent.gameObject.SetActive(false);
	}

	public static bool canAffordCost(Money cost){
		if (cost.inPennies() > Stats.money.inPennies()){
			return false;
		}
		return true;
	}
	
	public static void changeEmissionHighlight(Renderer[] rends, Color col, bool enable){	
		foreach (Renderer r in rends){
			Material[] mats = r.materials;	
				
			foreach (Material m in mats){
				if (enable){
					m.EnableKeyword("_EMISSION");
				}
				else {
					m.DisableKeyword("_EMISSION");
				}
				
				m.SetColor ("_EmissionColor", col);
			}
			r.materials = mats;
		}	
	}
	
	public static void changeEmissionHighlight(Renderer rend, Color col, bool enable){	
		changeEmissionHighlight(new Renderer[]{rend}, col, enable);
	}
}