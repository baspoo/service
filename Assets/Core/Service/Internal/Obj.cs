using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obj : MonoBehaviour {
	public static Obj GetObj(GameObject target)
	{
		return (target).GetComponent<Obj>();
	}
	public static Obj currentobj = null;
	public delegate void onclick (Obj obj = null);
	public delegate void onclickargument (Obj obj = null , GameObject btn = null);

	[Header("Options")]
	[Header("---------------------------------------------")]
	public int index;
	public string data;
	public int value;
	public bool active;

	public UILabel label_header;
	public UILabel label_content;
	public UILabel label_count;
	public UITexture icon;
	public UI2DSprite spr;
	public UIButton btn;
	public GameObject[] objs;
	public Service.Var.Behaviours behaviours;
	public Service.Var.Values Value;
	public List<Texture> Imgs;
	public List<Color> Colors;
	[Header("---------------------------------------------")]
	[Space(20)]
	[TextArea]
	public string status;
	int statuss;

	[HideInInspector]
	public object dataobject;

	[HideInInspector]
	public onclick Onclick = null;
	[HideInInspector]
	public onclickargument Onclickargument = null;
	[HideInInspector]
	public onclick OnUpdate = null;



	public void onClick(){
		currentobj = this;
		if (Onclick != null)
			Onclick (this);
	}
	public void onClickArgument( GameObject btn )
	{
		currentobj = this;
		if (Onclickargument != null)
			Onclickargument(this, btn);
	}
	void Update()
	{
		if (OnUpdate != null)
			OnUpdate (this);
	}





}
