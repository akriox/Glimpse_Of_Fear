using UnityEngine;
using System.Collections;

public class Inventory : MonoBehaviour {

	public static Inventory Instance { get; private set; }
	public int glowStickStash { get; private set; }
	public bool hasPentacle;

	public void Awake(){
		Instance = this;
		glowStickStash = 10;
	}

	public void addGlowStick(int qty){
		glowStickStash += qty;
	}

	public void removeGlowStick(int qty){
		glowStickStash -= qty;
	}
}