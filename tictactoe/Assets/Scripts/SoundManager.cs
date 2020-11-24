using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour {

	public static SoundManager instance;
	public AudioClip[] sounds;
	public AudioSource LocalAudio;
	// Use this for initialization
	void Start () {
		instance = this;
		LocalAudio = this.gameObject.GetComponent<AudioSource> ();
	}
	
	public void PlaySound(int index)
	{
        if (index < sounds.Length)
        {
			LocalAudio.clip = sounds[index];
			LocalAudio.Play();
        }
	}
	public IEnumerator PopSounds(int count)
	{
			for (int i = 0; i < count; i++) {
			LocalAudio.PlayOneShot (sounds [10],1f);
			yield return new WaitForSeconds (Random.Range(0.03f,0.06f));
		}
	}
}
