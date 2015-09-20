using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FlareStickCount : MonoBehaviour {
	
	private static Text count;
	private static Image icon;

	public void Start () {
		count = GetComponentInChildren<Text>();
		icon = GetComponentInChildren<Image>();
		show (false);
	}

	public static void refreshCount(){
		count.text = Inventory.Instance.FlareStickStash.ToString();
	}

	public static void show(bool b){
		icon.enabled = b;
		count.enabled = b;
	}
}
