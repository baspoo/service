using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SongPart  : MonoBehaviour {



	[System.Serializable]
	public class Segment
	{
		public float TotalBar;
		public float HeadBar;
		public float TailBar;
		public AudioClip AudioClip;


		//public float PercentFade {
		//	get{ return (TotalBar - HeadBar - TailBar) / TotalBar; }
		//}

		public float PercentFade( Segment nextSegment ) {
			float HeadBar = 0.0f;
			if (nextSegment != null)
				HeadBar = nextSegment.HeadBar;
			 return (TotalBar - HeadBar - TailBar) / TotalBar; 
		}
	}

	public string PartName;
	[Space(10)]


	[Header ("Bridge -----------")]
	public Segment BridgeIN;
	public Segment BridgeOut;





	[Header ("Loops -----------")]
	public List<Segment> Loops;








	[HideInInspector]
	public string AlbumName;
	[Range(0.0f,1.0f)]
	[HideInInspector]
	public float PercentFade;
	[HideInInspector]
	public List<AudioClip> AudioClips;




	public Segment AudioSegment{ 
		get{


			if (isFinishingPart)
				return null;

			if (isEnding)
			if (BridgeOut.AudioClip != null)
				return  BridgeOut;

			if (!isStarted)
			if (BridgeIN.AudioClip != null)
				return  BridgeIN;

			if (!isEnding)
			if (isStarted)
			if(Loops [SegmentIndex]!=null)
				return Loops [SegmentIndex];

			return null;
		} 
	}
	public Segment NextAudioSegment{ 
		get{

			int nextSegmentIndex = SegmentIndex;
			bool nextisStarted = isStarted;

			if (!nextisStarted) 
			{
				nextisStarted = true;
			} 
			else 
			{
				nextSegmentIndex++;
			}
			if (nextSegmentIndex >= Loops.Count) {
				// Re-AlbumIndex
				nextSegmentIndex = 0;
			}



			if (isFinishingPart)
				return null;
			
			if (isEnding)
			if (BridgeOut.AudioClip != null)
				return  BridgeOut;
			
			if (!nextisStarted)
			if (BridgeIN.AudioClip != null)
				return  BridgeIN;
			
			if (!isEnding)
			if (nextisStarted)
			if(Loops [nextSegmentIndex]!=null)
				return Loops [nextSegmentIndex];

			return null;
		} 
	}




	public bool IsEnding{ 
		get{
			return isEnding;
		}
	}
	public bool IsFinishingPart{ 
		get{
			//**--- Fix Finish On Not Have BridgeOut.AudioClip !
			if (isEnding)
			if (isStarted)
			if (BridgeOut.AudioClip == null)
				return true;
			return isFinishingPart;
		}
	}
	public int GetSegmentIndex{ 
		get{
			return SegmentIndex;
		}
	}
	int SegmentIndex = 0;
	bool isStarted = false;
	bool isEnding = false;
	bool isFinishingPart = false;

	public void Init(){
		isStarted = false;
		isEnding = false;
		isFinishingPart = false;
		SegmentIndex = 0;
	}
	public void Next(){
		if (isEnding)
			return;
		if (!isStarted) {
			isStarted = true;
		} else {
			SegmentIndex++;
		}

		if (SegmentIndex >= Loops.Count) {
			// Re-AlbumIndex
			SegmentIndex = 0;
		}
	}
	public void OnEnding(){
		isEnding = true;
	}
	public void OnFinishingPart(){
		isFinishingPart = true;
	}
}
