using UnityEngine;
using System.Collections.Generic;

public class InterpolatedHistoricalStat {			//to be used for things like inflation and population through time

	public Keyframe[] history = new Keyframe[0];  // This array must ALWAYS be in numerical order. It's hardly worth it to keep checking at runtime so make sure it gets initialised as such
	
	public InterpolatedHistoricalStat(Keyframe[] hist){
		history = hist;		
	}
	
	public class Keyframe{
		public float yearOccurred;
		public float stat;
	
		public Keyframe(float yearOccurred, float stat){
			this.yearOccurred = yearOccurred;
			this.stat = stat;
		}	
	}
	
	public float getValueAtYear(float year){
		
		Keyframe min = null;
		Keyframe max = null;
		
		foreach (Keyframe h in history){
			if (h.yearOccurred < year){
				min = h;
			}
			if (h.yearOccurred > year){
				max = h;
				break;
			}			
		}
		
		if (min == null && max == null){
			Debug.Log("???? Sanity check failed in InterpolatedHistoricalStat? There should be at least ONE value for us to bounce off...");
			return 0; 
		} else if (min == null){ //then our year is off the bottom of the scale, so just give it the lowest value we could find, which is max because min is null
			return max.stat;
		} else if (max == null){ //then our year is off the top of the scale, so just give it the highest value we could find, which is min because max is null
			return min.stat;
		} else { 				 //then return an interpolated value between the min and max years, according to the year we requested
			float step = (year - min.yearOccurred) / (max.yearOccurred - min.yearOccurred);
			return min.stat + ((max.stat - min.stat) * step);
		}
	}
}