using UnityEngine;

namespace SlimUI.ModernMenu{
	public class CheckSFXVolume : MonoBehaviour {
		private AudioSource _audioSource;

		public void Start(){
			_audioSource = GetComponent<AudioSource>();
			_audioSource.volume = PlayerPrefs.GetFloat("SFXVolume");
		}

		public void UpdateVolume(){
			_audioSource.volume = PlayerPrefs.GetFloat("SFXVolume");
		}
	}
}
