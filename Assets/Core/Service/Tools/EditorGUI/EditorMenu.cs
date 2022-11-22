#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


public class EditorMenu : Editor
{
	class ProjectPath
	{
		public const string header = "Service";
	}



	[MenuItem(ProjectPath.header + "/Data/Update Tsv")]
	public static void OnTsv()
	{
		TsvDataEditor.ShowWindow();
	}
	[MenuItem(ProjectPath.header + "/Editor/DeleteAll EditorPrefs")]
	public static void OnDeleteAll()
	{
		EditorPrefs.DeleteAll();
	}
	[MenuItem(ProjectPath.header + "/Editor/ExternalEditor")]
	public static void OnExternalEditor()
	{
		ExternalEditor.ExternalEditorInspector.ShowWindow();
	}
	[MenuItem(ProjectPath.header + "/Editor/Log/LogSetting")]
	public static void OnLogSetting()
	{
		LogService.LogEditor.UILogSetting.OnSelection();
		//LogToolsUI.OnSelection();
	}
	[MenuItem(ProjectPath.header + "/Editor/Log/LogEditor")]
	public static void OnLogEditor()
	{
		LogService.LogEditor.LogEditor.ShowWindow();
		//LogEditor.LogEditor.ShowWindow();
	}


	[MenuItem(ProjectPath.header + "/Formula/FormulaToolsWindows")]
	public static void OnFormulaToolsWindows()
	{
		FormulaToolsWindows.ShowWindow();
	}


	//ByProduct
	[MenuItem(ProjectPath.header + "/UIMaker")]
	public static void OnUIMakerTools()
	{
		UIMakerToolsWindowEditor.ShowWindow();
	}
	[MenuItem(ProjectPath.header + "/Sound/Playlist")]
	public static void OnPlaylist()
	{
		Sound.MenuSetup();
	}
	[MenuItem(ProjectPath.header + "/AssetsBundle/EditorAssetsBundleHandle")]
	public static void OnEditorAssetsBundleHandle()
	{
		EditorAssetsBundleHandle.ShowWindow();
	}

	[MenuItem(ProjectPath.header + "/AssetsBundle/SourceFinding")]
	public static void OnSourceFinding()
	{
		EditorSourceFinding.ShowWindow();
	}


}
#endif