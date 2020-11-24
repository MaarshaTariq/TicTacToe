using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfoManager : MonoBehaviour {

	public GameObject infoBox;
	public GameObject closeBlock;
	public int infoSoundIndex;

	
	public void InfoHandler()
	{
		if (GameManager.Instance.CanClick ()) {
			StartCoroutine (OpenCloseInfo ());	
		}
	}
	IEnumerator OpenCloseInfo()
	{
		if (!infoBox.activeInHierarchy) {
			infoBox.SetActive (true);
			closeBlock.SetActive (true);
			SoundManager.instance.PlaySound (infoSoundIndex);
		} else {
			infoBox.GetComponent<Fade> ().Fadeout = true;
			yield return new WaitForSeconds (0.6f);
			infoBox.SetActive (false);
			closeBlock.SetActive (false);
			infoBox.GetComponent<Fade> ().Fadeout = false;
		}
	}
}
