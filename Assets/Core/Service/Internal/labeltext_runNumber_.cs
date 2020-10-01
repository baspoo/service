using UnityEngine;
using System.Collections;

public class labeltext_runNumber_ : MonoBehaviour {
	public UILabel _uitext;
	public string textBegin;
	public string textEnd;
	public int _Number_Start;
	public int _Number_End;
	 int SumRuntime;
	 int PerRuntime;
	 int ShowRuntime;
	public int _speedCount;
	public float _speedTime;
	float _TimeRun;
	void Start () {
		if(_uitext==null) _uitext = gameObject.GetComponent<UILabel>();
		if(_uitext==null) Destroy(this);
		if(Auto)setRun(_Number_Start,_Number_End);
	}
	public bool Auto=false;
	public bool AutoDes=false;
	 bool isRun=false;
	public void setRun(int _start,int _end)
	{
		_TimeRun=0.0f;
		_Number_Start = _start;
		_Number_End = _end;
		ShowRuntime = _Number_Start;
		_uitext.text = textBegin+_Number_Start.ToString("#,##0")+textEnd;
		isRun = true;
		SumRuntime = _Number_End - _Number_Start;
		PerRuntime = SumRuntime/_speedCount;
		if(PerRuntime<1)PerRuntime=1;
	}
	void fixText()
	{

		if(ShowRuntime < SumRuntime){
			ShowRuntime+=PerRuntime;
			if(ShowRuntime >= _Number_End)ShowRuntime=_Number_End;
			_uitext.text = textBegin+ShowRuntime.ToString("#,##0")+textEnd;
		}
		else
		{
			//Debug.Log("FIX");
			isRun = false;
			_uitext.text = textBegin+_Number_End.ToString("#,##0")+textEnd;
			if(AutoDes)Destroy(this);
		}
	}
	void Update () {
		if(isRun)
		{
			if(_TimeRun< _speedTime) _TimeRun+= Time.deltaTime;
			else
			{
				_TimeRun=0.0f;
				fixText();
			}
		}
	}
}
