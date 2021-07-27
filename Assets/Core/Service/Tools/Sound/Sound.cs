using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
[InitializeOnLoad]
#endif


public class Sound : MonoBehaviour {
	#if UNITY_EDITOR
	[MenuItem ("Sound/Setting")]
	public static void MenuSetup1() {
		Selection.activeObject = p_sound;
		((GameObject)Selection.activeObject).transform.hideFlags = HideFlags.HideInInspector;
		AssetDatabase.OpenAsset (Selection.activeObject);
	}
	#endif
	#if UNITY_EDITOR
	[MenuItem ("Sound/Playlist")]
	public static void MenuSetup2() {
		Selection.activeObject = p_soundlist;
		((GameObject)Selection.activeObject).transform.hideFlags = HideFlags.HideInInspector;
		AssetDatabase.OpenAsset (Selection.activeObject);
	}
	#endif
	static GameObject p_sound {get{return Resources.Load ("Sound") as GameObject;}}
	static GameObject p_soundlist {get{return Resources.Load ("SoundPlaylist") as GameObject;}}

	static Sound m_sound;
	public static Sound sound {
		get{
			if (m_sound == null) {
				m_sound =  Service.GameObj.Created (p_sound).GetComponent<Sound> ();
				m_sound.Init ();
			}
			return m_sound;
		}
	}


	static ToolsSounds.SoundPlaylist.Playlist m_playlist;
	public static ToolsSounds.SoundPlaylist.Playlist Playlist {
		get{
			if (m_playlist == null)
				m_playlist = p_soundlist.GetComponent<ToolsSounds.SoundPlaylist> ().playlist;
			return m_playlist;
		}
	}
	static ToolsSounds.SoundPlaylist.Backgrounds m_backgroundmusic;
	public static ToolsSounds.SoundPlaylist.Backgrounds Backgroundmusic {
		get{
			if (m_backgroundmusic == null)
				m_backgroundmusic = p_soundlist.GetComponent<ToolsSounds.SoundPlaylist> ().backgroundmusic;
			return m_backgroundmusic;
		}
	}








	


	//[Header("[ Volume ]---------------------------------")]
	//[Range(0.0f, 1.0f)]
	public float volume_Bgm {
		get { return 1.0f; }
		set { }
		//get { return PlayerConfigs.GetFloat(PlayerConfigs.key.volume_bgm); }
		//set { PlayerConfigs.SetValue(PlayerConfigs.key.volume_bgm,value);  }
	}
	//[Range(0.0f,1.0f)]
	public float volume_Sfx {
		get { return 1.0f; }
		set { }
		//get { return PlayerConfigs.GetFloat(PlayerConfigs.key.volume_sfx); }
		//set { PlayerConfigs.SetValue(PlayerConfigs.key.volume_sfx, value); }
	}





	public void Init(){

		//Debug.Log("volume_Bgm : " + volume_Bgm);
		//Debug.Log("volume_Sfx : " + volume_Sfx);


		if(Application.isPlaying)
			DontDestroyOnLoad (gameObject);
		if(gameObject.GetComponent<AudioListener>() == null)
		{
			gameObject.AddComponent<AudioListener>();
		}
		if(m_sfx==null)
		{
			GameObject SoundSfx = new GameObject("SoundSfx");
			SoundSfx.transform.parent = transform;
			SoundSfx.AddComponent<AudioSource>();
			m_sfx = SoundSfx.GetComponent<AudioSource>();
			m_sfx.volume = volume_Sfx;
		}

		if(m_bgm==null)
		{
			m_bgm = gameObject.GetComponent<AudioSource>();
			m_bgm.clip = Backgroundmusic.bgm_mainmenu.getclip;
			m_bgm.Play();
			m_bgm.loop = true;
			m_bgm.volume = volume_Bgm;
		}
	}




















	static bool m_is_bgm_mute = false;
	static bool m_is_sfx_mute = false;
	public static bool is_bgm_mute{get{ return m_is_bgm_mute;}} 
	public static bool is_sfx_mute{get{ return m_is_sfx_mute;}} 

	static AudioSource m_sfx,m_bgm;
	public static AudioSource Sfx{get{ return m_sfx;}} 
	public static AudioSource Bgm{get{ return m_bgm;}} 



	static float lasttime_sound;
	static AudioClip last_clip;
	public static void Play(SoundData sd, float wait = 0.0f)
	{
		if (wait == 0.0f) Play(sd.getclip);
		else Play(sd.getclip, wait);
	}
	public static void Play( AudioClip clip ){
		if(Sound.sound == null)
			Sound.sound.enabled = true;

		if (clip == null)
			return;

		Log.WriteButton("sound","SOUND : " + clip.name,()=> {
			m_sfx.PlayOneShot(clip);
		});


		//Debug.LogError((lastdete_sound == System.DateTime.Now).ToString() + lastdete_sound +" || "+ Time.time + "      : " + clip.name);

		if (lasttime_sound == Time.time)
		if (last_clip == clip) {
				Debug.LogWarning("["+clip.name + "]   Playing of the same sound at the same time. !!");
			return;
		}			

		

		if(!m_is_sfx_mute)
			m_sfx.PlayOneShot (clip);
		lasttime_sound = Time.time;
		last_clip = clip;
	}









	public static AddOn.Timmer Play( AudioClip clip , float wait){
		return Service.Timmer.Wait ( wait , () => { Play(clip); });
	}




	public static AudioSource CreatedAudio(AudioClip _sound , float duration = 0.0f ){
		var audioSource = CreatedAudio (_sound,duration,false,null);
		return audioSource;
	}
	public static AudioSource CreatedAudio(SoundData _sound , float duration , bool loop , Transform transform ){
		var audioSource = CreatedAudio (_sound.getclip,duration,loop,transform);
		return audioSource;
	}
	static List<AudioSource> AudioSources = new List<AudioSource> ();
	public static AudioSource CreatedAudio(AudioClip _sound , float duration , bool loop , Transform transform ){

		if (transform == null)
			return null;

		//GameObject newSoundSfx = new GameObject("aloneSfx");
		//newSoundSfx.AddComponent<AudioSource>();
		//if (transform != null) {
		//	Service.GameObj.Parent (newSoundSfx,transform);
		//}
		//AudioSource Sfx = newSoundSfx.GetComponent<AudioSource>();




		AudioSource Sfx = transform.gameObject.AddComponent<AudioSource>();

		//-----------------------------------------------------------------------------
		// Update List AudioSources.
		//-----------------------------------------------------------------------------
		List<AudioSource> newlist = new List<AudioSource> ();
		newlist.Add (Sfx);
		foreach(AudioSource a in AudioSources){
			if (a != null)
				newlist.Add (a);
		}
		AudioSources = newlist;



		Sfx.clip = _sound;
		Sfx.volume = sound.volume_Sfx;
		Sfx.loop = loop;
		Sfx.Play();
		if (duration != 0.0f) {
			if(Sfx!=null)
				Service.Timmer.Wait (duration,()=>{
					Destroy(Sfx);
			});
		}
		return Sfx;
	}
	public static void ClearAndStopAllCreatedAudio() {
		foreach (AudioSource a in AudioSources)
		{
			if (a != null)
				Destroy(a);
		}
		AudioSources.Clear();
	}



	public delegate void callback();
	public static void OnBGMMute( callback on_play ,callback on_mute , bool isIgnoreChange = false){
		if(!isIgnoreChange)
			m_is_bgm_mute = !m_is_bgm_mute;
		if (!m_is_bgm_mute) {
			m_bgm.mute = false;
			on_play ();
		} else {
			m_bgm.mute = true;
			on_mute ();
		}
	}
	public static void OnSFXMute( callback on_play ,callback on_mute , bool isIgnoreChange = false){
		if(!isIgnoreChange)
			m_is_sfx_mute = !m_is_sfx_mute;
		if (!m_is_sfx_mute) {
			m_sfx.mute = false;
			on_play ();
		} else {
			m_sfx.mute = true;
			on_mute ();
		}
	}
	public static void OnResetBgmVolume( ){
		OnChangeBgmVolume (sound.volume_Bgm);
	}

	public static void OnDimBgmVolume()
	{
		if (Sound.sound.volume_Bgm > 0.1f)
			Sound.OnChangeBgmVolume(0.1f, true);
		else
			Sound.OnChangeBgmVolume(0.0f, true);
	}
	public static void OnChangeBgmVolume( float volume , bool isTemporaryVolume = false){

		//Debug.Log( "volume : " + volume);




		if(!isTemporaryVolume)
			sound.volume_Bgm = volume;
		Bgm.volume = volume;
		//if (GameScene.Songtools != null)
			//GameScene.Songtools.volume = volume;
	}
	public static void OnUpdateSfxVolume( float volume ){
		sound.volume_Sfx = volume;
		Sound.Sfx.volume = Sound.sound.volume_Sfx;
		Sfx.volume = Sound.sound.volume_Sfx;
		foreach(AudioSource a in AudioSources){
			if (a != null)
				a.volume = volume;
		}
	}





}
