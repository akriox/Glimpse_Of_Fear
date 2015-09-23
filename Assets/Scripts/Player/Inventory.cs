using UnityEngine;
using System.Collections;

public class Inventory : MonoBehaviour {

	public static Inventory Instance { get; private set; }
	public int FlareStickStash { get; private set; }
	private int maxStash = 15;
	public bool hasPentacle;
	public bool hasTablet;

	public void Start(){
		/*
		hasPentacle = true;
		hasTablet = true;
		*/
	}

	public void Awake(){
		Instance = this;
		FlareStickStash = 5;
	}
	
	public bool canTake(){
		if(FlareStickStash < maxStash){
			return true;
		}
		else{
			StartCoroutine(GameController.Instance.displayTimedDebug("Can't carry more", 1.0f));
			return false;
		}
	}

	public void addFlareStick(int qty){
		FlareStickStash += qty;
		if(FlareStickStash > maxStash) FlareStickStash = maxStash;
		StartCoroutine(displayCount(3.0f));
	}

	public void removeFlareStick(int qty){
		FlareStickStash -= qty;
		StartCoroutine(displayCount(3.0f));
	}

	private IEnumerator displayCount(float duration){
		FlareStickCount.refreshCount();
		FlareStickCount.show(true);
		yield return new WaitForSeconds(duration);
		FlareStickCount.show(false);
	}
}