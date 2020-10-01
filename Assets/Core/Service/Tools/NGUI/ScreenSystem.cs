using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenSystem   {


	public class Setting{
		public static float right_padding = -120.0f;
		public static float left_padding = 120.0f;
	}





	public class Ratio{
		public enum RatioType{
			ipad,		// 4:3    [1.333]
			pal,		// 3:2    [1.5]
			phone16_10,	// 16:10  [1.6]
			phone16_9,	// 16:9   [1.77]
			phoneX		// 19.5:9 [2.166]
		}
		static double ipad 			= 1.335;	
		static double pal 			= 1.5;		
		static double phone16_10 	= 1.65;		
		static double phone16_9 	= 1.8;		
		static double phoneX 		= 2.166;		
		public static RatioType GetRatio{
			get{ 
				RatioType ratio = RatioType.ipad;
				double currentRatio =   (double)Screen.width / (double)Screen.height;
				if (currentRatio <= ipad)				ratio = RatioType.ipad;
				else if (currentRatio <= pal)			ratio = RatioType.pal;
				else if (currentRatio <= phone16_10)	ratio = RatioType.phone16_10;
				else if (currentRatio <= phone16_9)		ratio = RatioType.phone16_9;
				else if (currentRatio <= phoneX)		ratio = RatioType.phoneX;
				return ratio;
			}
		}
	}













}
