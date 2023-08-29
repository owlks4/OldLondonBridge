using UnityEngine;

public class Money {

	public int pounds;
	public int shillings;
	public int pence;
	
	public Money (int pounds, int shillings, int pence){
		this.pounds = pounds;
		this.shillings = shillings;
		this.pence = pence;		
	}	
	
	public void checkOverflowAndUpdate(){
		while (pence >= 12){
			shillings++;
			pence -= 12;
		}
		
		while (shillings >= 20){
			pounds++;
			shillings -= 20;
		}							
	}
	
	public void addMoney(Money moneyToAdd){
		this.pounds += moneyToAdd.pounds;
		this.shillings += moneyToAdd.shillings;
		this.pence += moneyToAdd.pence;		
		checkOverflowAndUpdate();
	}
	
	public float getDecimalised(){
		return (float)(Mathf.Round(((float)pounds + ((float)shillings/20f) + ((float) shillings / 12f))*100))/100f;
	}
	
	public string getString(){
		return "£"+pounds+" "+shillings+"s "+pence+"d";
	}
	
	public Money multipliedBy(float multiplier){
		return new Money((int)Mathf.Round(pounds * multiplier), (int)Mathf.Round(shillings * multiplier), (int)Mathf.Round(pence * multiplier));
	}
	
	public static Money fromDecimal(float dec){		
		Money output = new Money((int)Mathf.Floor(dec),0,0);
		dec = dec - output.pounds; //get remainder, which we will now turn into shillings and pence
		
		while (dec >= 0.05f){
			dec -= 0.05f;
			output.shillings++;
		}
		
		while (dec >= (1f/240f)){
			dec -= (1f/240f);
			output.pence++;
		}
		
		return output;		
	}
	
	public int inPennies(){
		return pence + (shillings * 12) + (pounds * 240);
	}
}