using UnityEngine;
using System.Collections;

public class Inventory : MonoBehaviour {

	public static Inventory Instance { get; private set; }
	public int FlareStickStash { get; private set; }
	public bool hasPentacle;
	public bool hasTablet;

	public void Awake(){
		Instance = this;
		FlareStickStash = 100000;
	}
	
	public bool canTake(){
		if (FlareStickStash < 14)
			return true;
		return false;
	}

	public void addFlareStick(int qty){
		FlareStickStash += qty;
	}

	public void removeFlareStick(int qty){
		FlareStickStash -= qty;
	}
}