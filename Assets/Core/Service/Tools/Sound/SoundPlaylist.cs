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
			public SoundData sfx_onTurn;
            public SoundData sfx_onEndTurn;
            public SoundData sfx_onChangeState;
			public SoundData sfx_timeout;
			public SoundData sfx_timeWarning;
			public SoundData sfx_welcome;
			public SoundData sfx_room_matching;
			public SoundData sfx_room_letgo_onReady;
			public SoundData sfx_room_letgo_onAllReady;
			public SoundData sfx_room_letgo_startEffect;
			public SoundData sfx_room_letgo_stayEffect;
			public SoundData sfx_room_letgo_numbercountdown;
			public SoundData sfx_room_letgo_startgame;
			public SoundData sfx_runingPoints;
			public SoundData sfx_movepage;
			public SoundData sfx_openBag;
			public SoundData sfx_get_lvup;
			public SoundData sfx_get_characterlvup;
			public SoundData sfx_reward_lvup;
            public SoundData sfx_claim_lvup;
            public SoundData sfx_reward_small;
			public SoundData sfx_reward_medium;
			public SoundData sfx_reward_big;
			public SoundData sfx_get_reward;
			public SoundData sfx_get_gem;
			public SoundData sfx_buy;
			public SoundData sfx_reward_exp_update_pilot;
			public SoundData sfx_reward_exp_update_real;

            public SoundData sfx_rankShowup;
            public SoundData sfx_rankRunning;
			public SoundData sfx_rankUp;
			public SoundData sfx_mvpShowup;



            [Header("Tutorial ------------------------------------------")]
            public SoundData sfx_tutorial_open_longdialog;
            public SoundData sfx_tutorial_open_shortdialog;
            public SoundData sfx_tutorial_next;
            public SoundData sfx_tutorial_objective_open;
            public SoundData sfx_tutorial_objective_active;
            public SoundData sfx_tutorial_objective_complete;
            public SoundData sfx_tutorial_objective_completeall;






            [Header("Connecting Network ------------------------------------------")]
			public SoundData sfx_message_onchatbar;
			public SoundData sfx_message_onchatbox;
			public SoundData sfx_ping;
			public SoundData sfx_pin;
			public SoundData sfx_notif;
			public SoundData sfx_loginfinish;
			public SoundData sfx_loginfail;
			public SoundData sfx_disconnect;
			public SoundData sfx_error_network;
			public SoundData sfx_player_join_room;
			public SoundData sfx_player_exit_room;

			[Header("Arena ------------------------------------------")]
			public SoundData[] sfx_hits;
			public SoundData sfx_hit{
				get{ return sfx_hits [Random.Range (0, sfx_hits.Length)];}
			}
			public SoundData[] sfx_hitOpenblocks;
			public SoundData sfx_hitOpeblock{
				get{ return sfx_hitOpenblocks [Random.Range (0, sfx_hitOpenblocks.Length)];}
			}
			public SoundData[] sfx_hitShocks;
			public SoundData sfx_hitShock{
				get{ return sfx_hitShocks [Random.Range (0, sfx_hitShocks.Length)];}
			}
			public SoundData[] sfx_rea_CrowdApplauseOnHits;
			public SoundData sfx_rea_CrowdApplauseOnHit{
				get{ return sfx_rea_CrowdApplauseOnHits [Random.Range (0, sfx_rea_CrowdApplauseOnHits.Length)];}
			}
			public SoundData[] sfx_papers;
			public SoundData sfx_paper{
				get{ return sfx_papers [Random.Range (0, sfx_papers.Length)];}
			}
			public SoundData[] sfx_openBlocks;
			public SoundData sfx_openBlockGetReward;
			public SoundData sfx_movebody;
			public SoundData sfx_throl;
			public SoundData sfx_blockselect;
			public SoundData[] sfx_gameActivitys;
			public SoundData sfx_rampagepoint;
            public SoundData sfx_flagwin;
            public SoundData sfx_flaglose;
            public SoundData sfx_winner;
			public SoundData sfx_loser;
			public SoundData sfx_energyChange;
			public SoundData sfx_energyAccept;
			public SoundData sfx_energyMovefinish;
			public SoundData sfx_skillReady;
			public SoundData sfx_combatSchema;
			public SoundData sfx_combatAnimation;
			public SoundData sfx_leaf;
			public SoundData sfx_starttaunt;
			public SoundData sfx_readyGo;
			public SoundData sfx_groundUp;
			public SoundData sfx_openPocket;
			public SoundData sfx_unhide;
			[Header("Action ------------------------------------------")]
			public SoundData sfx_baseballhit;
			public SoundData sfx_boom;
			public SoundData sfx_airbooming;
			public SoundData sfx_gunatk;
			public SoundData sfx_meleeHit;
			public SoundData sfx_meleeSword;
			public SoundData sfx_heal;
			public SoundData sfx_powerup;
			public SoundData sfx_lighting;
			public SoundData sfx_scaner;
			public SoundData sfx_armor;
			public SoundData sfx_armorDis;
			public SoundData sfx_trapBoom;
			public SoundData sfx_dig;
			public SoundData sfx_earthquake;
		}

	





		[Space(10)]
		public List<SoundGroupOther> SoundOther = new List<SoundGroupOther>();




		AudioSource audioSource = null;
		public void Play(AudioClip s){
			audioSource = GetComponent<AudioSource> ();
			audioSource.PlayOneShot (s);
		}
	}
}
