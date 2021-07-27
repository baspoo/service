using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace ToolsSounds{
	public class SoundPlaylist : MonoBehaviour {



		public Backgrounds backgroundmusic = new Backgrounds();
		[System.Serializable]
		public class Backgrounds{

			public SoundData bgm_mainmenu; 
		}

		[Space(10)]


		public Playlist playlist = new Playlist();
		[System.Serializable]
		public class Playlist{

			[Header("Interface ------------------------------------------")]
			public SoundData sfx_click;
			public SoundData sfx_select;
	
		}

	






		AudioSource audioSource = null;
		public void Play(AudioClip s){
			audioSource = GetComponent<AudioSource> ();
			audioSource.PlayOneShot (s);
		}
	}
}
