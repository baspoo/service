﻿#region copyright
// ------------------------------------------------------
// Copyright (C) Dmitriy Yukhanov [https://codestage.net]
// ------------------------------------------------------
#endregion

namespace CodeStage.AntiCheat.EditorCode.PropertyDrawers
{
	using ObscuredTypes;

	using UnityEditor;
	using UnityEngine;

	[CustomPropertyDrawer(typeof(ObscuredShort))]
	internal class ObscuredShortDrawer : PropertyDrawer
	{
		public override void OnGUI(Rect position, SerializedProperty prop, GUIContent label)
		{
			var hiddenValue = prop.FindPropertyRelative("hiddenValue");

			var cryptoKey = prop.FindPropertyRelative("currentCryptoKey");
			var inited = prop.FindPropertyRelative("inited");
			var fakeValue = prop.FindPropertyRelative("fakeValue");
			var fakeValueActive = prop.FindPropertyRelative("fakeValueActive");

			var currentCryptoKey = (short)cryptoKey.intValue;
			short val = 0;

			if (!inited.boolValue)
			{
				if (currentCryptoKey == 0)
				{
					currentCryptoKey = ObscuredShort.GenerateKey();
					cryptoKey.intValue = currentCryptoKey;
				}
				hiddenValue.intValue = ObscuredShort.Encrypt(0, currentCryptoKey);
				inited.boolValue = true;
			}
			else
			{
				val = ObscuredShort.Decrypt((short)hiddenValue.intValue, currentCryptoKey);
			}

			label = EditorGUI.BeginProperty(position, label, prop);

			EditorGUI.BeginChangeCheck();
			val = (short)EditorGUI.DelayedIntField(position, label, val);
			if (EditorGUI.EndChangeCheck())
			{
				hiddenValue.intValue = ObscuredShort.Encrypt(val, currentCryptoKey);
				fakeValue.intValue = val;
				fakeValueActive.boolValue = true;
			}
			EditorGUI.EndProperty();
		}
	}
}