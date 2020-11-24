using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;

public class DelayToPLaySound : MonoBehaviour {
	
	public int AudioToPlay;
	SoundManager Sound;
	public float sec = 1f;
	public bool skipSound = false;

	void OnEnable()
	{
		Sound = GameObject.Find ("SoundManager").GetComponent<SoundManager> ();
		StartCoroutine (wait ());
	}

	IEnumerator wait()
	{
		yield return new WaitForSeconds (sec);
		if (!skipSound)
		{
			SoundManager.instance.LocalAudio.clip = SoundManager.instance.sounds[AudioToPlay];
			SoundManager.instance.LocalAudio.Play();
		}
	
	}
}
