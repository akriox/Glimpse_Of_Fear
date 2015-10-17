using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using XInputDotNetPure;

public class TipsTracker : MonoBehaviour {

	public static TipsTracker Instance {get; private set;}

	public enum Tips {UseFlashlight, SwitchOffFlashlight, UseFlareStick, Sprint, Crouch, Rotate, AvoidWraith, Teleport};

	private float tipDuration = 3.0f;

	private bool useFlashlightTip  = true;
	private bool switchOffFlashlightTip = true;
	private bool useFlareStickTip = true;
	private bool sprintTip = true;
    private bool crouchTip = true;
	private bool rotateTip = true;
    private bool avoidWraithTip = true;

    public Image keyboardTip;
	public Image gamepadTip;
	
	public Sprite[] useFlashlightSprites;
	public Sprite switchOffFlashligthSprite;
	public Sprite[] useFlareStickSprites;
	public Sprite[] sprintSprites;
	public Sprite[] crouchSprites;
	public Sprite[] rotateSprites;
    public Sprite avoidWraithSprite;
	public Sprite teleportSprite;

	public void Awake(){
		Instance = this;
	}

	public void displayTip(Tips tip){

		switch(tip){
			case Tips.UseFlashlight:
				if(useFlashlightTip){
					useFlashlightTip = false; 
					setTipSprite(useFlashlightSprites[0],  useFlashlightSprites[1]);
				}
				break;
			case Tips.SwitchOffFlashlight:
				if(switchOffFlashlightTip){
					switchOffFlashlightTip = false; 
					setTipSprite(switchOffFlashligthSprite, switchOffFlashligthSprite);
				}
				break;
			case Tips.UseFlareStick: 
				if(useFlareStickTip){
					useFlareStickTip = false;
					setTipSprite(useFlareStickSprites[0], useFlareStickSprites[1]);
				}
				break;
			case Tips.Sprint: 
				if(sprintTip){
					sprintTip = false; 
					setTipSprite(sprintSprites[0], sprintSprites[1]);
				}
				break;

			case Tips.Crouch: if(crouchTip){
							  		crouchTip = false; 
									setTipSprite(crouchSprites[0], crouchSprites[1]);
							  }
							  break;

			case Tips.Rotate:	if(rotateTip){
									rotateTip = false; 
									setTipSprite(rotateSprites[0], rotateSprites[1]);
								}
								break;

            case Tips.AvoidWraith:	if (avoidWraithTip){
                    					avoidWraithTip = false;
                    					setTipSprite(avoidWraithSprite,avoidWraithSprite);
                					}
                					break;

			case Tips.Teleport: setTipSprite(teleportSprite, teleportSprite);
								break;
		}
	}

	private void setTipSprite(Sprite kbSprite, Sprite gpSprite){
		keyboardTip.sprite = kbSprite;
		gamepadTip.sprite = gpSprite;

		GamePadState state = GamePad.GetState(PlayerIndex.One);
		StopAllCoroutines();
		if(state.IsConnected) {
			StartCoroutine(enablegamepadTip());
		}
		else {
			StartCoroutine(enablekeyboardTip());
		}
	}
	
	private IEnumerator enablekeyboardTip(){
		keyboardTip.enabled = true;
		yield return new WaitForSeconds(tipDuration);
		keyboardTip.enabled = false;
	}
	
	private IEnumerator enablegamepadTip(){
		gamepadTip.enabled = true;
		yield return new WaitForSeconds(tipDuration);
		gamepadTip.enabled = false;
	}
}
