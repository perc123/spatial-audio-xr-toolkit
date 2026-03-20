using UnityEngine;

namespace SlimUI.ModernMenu{
	public class CheckMusicVolume : MonoBehaviour {
		private AudioSource _audioSource;

		public void Start(){
			_audioSource = GetComponent<AudioSource>();
			_audioSource.volume = PlayerPrefs.GetFloat("MusicVolume");
		}

		public void UpdateVolume(){
			_audioSource.volume = PlayerPrefs.GetFloat("MusicVolume");
		}
	}
}
