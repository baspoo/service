using UnityEngine;
using System;
using System.Collections;
using System.Runtime.InteropServices;
public class TopOnAlway : MonoBehaviour {
	public string WindowsName;
	public static string sWindowsName ="";
	private static readonly IntPtr HWND_TOPMOST = new IntPtr(-1);
	private const UInt32 SWP_NOSIZE = 0x0001;
	private const UInt32 SWP_NOMOVE = 0x0002;
	private const UInt32 TOPMOST_FLAGS = SWP_NOMOVE | SWP_NOSIZE;

	#if UNITY_STANDALONE_WIN || UNITY_EDITOR
	[DllImport("user32.dll", EntryPoint = "SetWindowPos")]
	private static extern bool SetWindowPos(IntPtr hwnd, int hWndInsertAfter, int x, int Y, int cx, int cy, int wFlags);
	[DllImport("user32.dll", EntryPoint = "FindWindow")]
	public static extern IntPtr FindWindow(System.String className, System.String windowName);
	public static void SetPosition(int x, int y, int resX = 0, int resY = 0) 
	{
		SetWindowPos(FindWindow(null, sWindowsName), -1, x, y, resX, resY, resX * resY == 0 ? 1 : 0);
	}



	public bool isSetScreen;
	public int Height;
	public int Width;
	public bool Full;

	void Awake () 
	{
		if(isSetScreen)
			Screen.SetResolution (Width, Height , Full);
		sWindowsName = WindowsName;
	}
	float Timer = 0.0f;
	public float TimerRefresh = 1.0f;
	void Update()
	{
		if (Timer < TimerRefresh)
			Timer += Time.deltaTime;
		else {
			Timer = 0.0f;
			SetPosition (0, 0);
			Debug.Log ("SetPosition");
		}
	}
	#endif
}
