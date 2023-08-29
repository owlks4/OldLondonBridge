using UnityEngine;

public static class Stats {

	public static Money money = new Money(100,0,0);
	
	public static double archFlowRate; //reflects the extent that the water has to squeeze between the starlings: less arch space means a greater arch flow. This controls the deadliness of shooting the bridge and the rate of damage to the starlings
	public static double riverFlowRate; //riverFlowRate: a function of the arch flow rate; water being obstructed and churned through the arches means a slower overall river flow and greater chance of the river freezing
	public static double security; // low security = more crime, more chance of things like Cade rebellion succeeding
	public static double disease; //resulting from overcrowding, can be mitigated with apothecary-type businesses
	public static double fireRisk; //resulting from wooden buildings, and exacerbated by the presence of oil businesses
	
	public static float lastYearUpdated;
	public static float numPeopleCrossingPerHour;
	
	public static InterpolatedHistoricalStat londonPopulation = new InterpolatedHistoricalStat(
	new InterpolatedHistoricalStat.Keyframe[]{
			new InterpolatedHistoricalStat.Keyframe(1200, 22500),new InterpolatedHistoricalStat.Keyframe(1300, 90000),new InterpolatedHistoricalStat.Keyframe(1350, 37500),
			new InterpolatedHistoricalStat.Keyframe(1500, 75000),new InterpolatedHistoricalStat.Keyframe(1550, 120000),new InterpolatedHistoricalStat.Keyframe(1600, 200000),
			new InterpolatedHistoricalStat.Keyframe(1650, 375000),new InterpolatedHistoricalStat.Keyframe(1700, 575000),new InterpolatedHistoricalStat.Keyframe(1750, 700000),
			new InterpolatedHistoricalStat.Keyframe(1801, 959300),new InterpolatedHistoricalStat.Keyframe(1831, 1655000),new InterpolatedHistoricalStat.Keyframe(1851, 2363000),
			new InterpolatedHistoricalStat.Keyframe(1891, 5572012),new InterpolatedHistoricalStat.Keyframe(1901, 6506954),new InterpolatedHistoricalStat.Keyframe(1911, 7160525),
			new InterpolatedHistoricalStat.Keyframe(1921, 7386848),new InterpolatedHistoricalStat.Keyframe(1931, 8110480),new InterpolatedHistoricalStat.Keyframe(1939, 8615245),
			new InterpolatedHistoricalStat.Keyframe(1951, 8196978),new InterpolatedHistoricalStat.Keyframe(1961, 7992616),new InterpolatedHistoricalStat.Keyframe(1971, 7452520),
			new InterpolatedHistoricalStat.Keyframe(1981, 6805000),new InterpolatedHistoricalStat.Keyframe(1991, 6829300),new InterpolatedHistoricalStat.Keyframe(2001, 7322400),
			new InterpolatedHistoricalStat.Keyframe(2006, 7657300),new InterpolatedHistoricalStat.Keyframe(2011, 8174100),new InterpolatedHistoricalStat.Keyframe(2015, 8615246)
	});
	
	public static InterpolatedHistoricalStat valueOf1GramOfGold = new InterpolatedHistoricalStat(
	new InterpolatedHistoricalStat.Keyframe[]{
		new InterpolatedHistoricalStat.Keyframe(1158, 0.03398208217f), //this one is vaguely approximated from the apparent value of silver at the time and not that concrete, but it's only for a background metric anyway
		new InterpolatedHistoricalStat.Keyframe(1351, 0.0430848772f),
		new InterpolatedHistoricalStat.Keyframe(1412, 0.04786979415f),
		new InterpolatedHistoricalStat.Keyframe(1464, 0.06464124111f),
		new InterpolatedHistoricalStat.Keyframe(1551, 0.09699321047f),
		new InterpolatedHistoricalStat.Keyframe(1717, 0.13656761872f),
		new InterpolatedHistoricalStat.Keyframe(1816, 0.13656761872f), // [sic]. Allegedly, this was the same in 1816 as it was in 1717. I doubt it, wikipedia... but maybe it was?		
		new InterpolatedHistoricalStat.Keyframe(1944, 0.13664067291f),
		new InterpolatedHistoricalStat.Keyframe(1972, 0.75f),
		new InterpolatedHistoricalStat.Keyframe(1982, 6.92f),
		new InterpolatedHistoricalStat.Keyframe(1992, 6.26f),
		new InterpolatedHistoricalStat.Keyframe(2002, 6.68f),
		new InterpolatedHistoricalStat.Keyframe(2008, 21.06f),
		new InterpolatedHistoricalStat.Keyframe(2012, 31.98f),
		new InterpolatedHistoricalStat.Keyframe(2018, 30.00f),
		new InterpolatedHistoricalStat.Keyframe(2020, 47.48f),
		new InterpolatedHistoricalStat.Keyframe(2022, 49.10f)
	});
	
	//Success metrics:
	//The player won't explicitly be choosing from these per se, but from building templates that secretly fit into these categories:
	public static float healthSuccess;			//(includes food, medicine, etc)
	public static float transportSuccess;		//(stables, footwear, winterwear, and rather craftily, blank walking space)
	public static float tradeSuccess;			//affected by foreign trade, see tradeSuccessDeciders below too
	public static float constructionSuccess;	//(Buildings are longer lasting, or take less time to construct?)
	public static float socialSuccess;  		//(which includes anything security-related, though with that you have to have a high enough amount for people to be safe, but not so much that they are intimidated)
	
	//See the big livery company google doc for lots of business examples that vaguely fit into these categories and might form unlockable options in the skill tree for each one, unlocked in order of precedence
	
	public static Trade[] tradeSuccessDeciders = new Trade[]{      //this is influenced by which country we might be at war at etc (which is different every game due to random events etc). 
		new TradeFood(),										   //the category that best appeases each foreign country (France, Spain, Holland, Italy?) is different every game, too
		new TradeCloth(),
		new TradeMetal(),
		new TradeWood()
	};
	

	
	public static void updateStats(){		//not conducted every frame, but maybe every 5 seconds or so
		float yearsSinceLastUpdate = Global.timeManager.year - lastYearUpdated;
		
		Bridge.updateBridgeStats(yearsSinceLastUpdate); //this processes both archFlowRate and riverFlowRate, as well as bridge health. Check this function for more 
		
		Global.placementGrid.updateBuildingStats(yearsSinceLastUpdate); //this processes building health and makes things collapse etc
		
		float hypotheticalPeoplePerHour = 0.00159726863f * londonPopulation.getValueAtYear(Global.timeManager.year); 
		//^ TODO: NEED TO GIVE A BUFF TO THIS BASED ON THE NUMBER OF PROPERTIES ON THE BRIDGE; IT'S BASED ON THE 1890s STAT OF 8900 PASSAGES AN HOUR,
		//BUT THAT MAKES IT REALLY LOW IN MEDIEVAL TIMES WITHOUT ACCOMMODATING FOR THE FACT THAT THERE WERE SHOPS BACK THEN
				
		numPeopleCrossingPerHour = hypotheticalPeoplePerHour > 500 ? hypotheticalPeoplePerHour : 500; //includes all kinds of traffic; the proportion of traffic should be worked out elsewhere
		
		float bridgeTrafficCapacitySquareMetres = Global.placementGrid.getAmountOfTrafficSpace();  //we'll say 4 people per square metre is the upper limit
		Debug.Log("Maximum capacity of the bridge: "+ (4 * bridgeTrafficCapacitySquareMetres) + " people");
		Debug.Log("Crossings per hour: "+numPeopleCrossingPerHour);
		Debug.Log("People per square metre: "+numPeopleCrossingPerHour/bridgeTrafficCapacitySquareMetres + ". Above 4 should be a trigger for people to complain");		
		
		//security: are the wardens funded? Is there an adequate gatehouse/drawbridge etc? They won't make you money and take up tiles, but they do keep people safe.
		
		
		//disease: from overcrowding
			
		
		//iterate over buildings to check the different trade types
		
		
		//see if we have hit the year of the next random event, and if so, trigger a random event
		
		
		//update lastYearUpdated to the current year:
		lastYearUpdated = Global.timeManager.year;
	}
}