using UnityEngine;
using UnityEngine.UI;

public class TooltipUI : MonoBehaviour {

	public Text OutlineText;
	public Text ContentText;

	public void UpdateTooltip(string text){
		OutlineText.text = text;
		ContentText.text = text;
	}
	public void show()
	{
		gameObject.SetActive (true);
	}
	public void hidden()
	{
		gameObject.SetActive (false);
	}
    public void SetLocalPosition(Vector2 position) {
        transform.localPosition = position;
    }
}
