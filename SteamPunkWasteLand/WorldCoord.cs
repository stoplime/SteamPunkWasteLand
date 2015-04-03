/*	
 * Copyright (C) 2015  Steffen Lim
 * 
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published 
 * the Free Software Foundation, either version 3 of the License, 
 * (at your option) any later 
 * 
 * This program is distributed in the hope that it will be 
 * but WITHOUT ANY WARRANTY; without even the implied warranty 
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See 
 * GNU General Public License for more 
 * 
 * You should have received a copy of the GNU General Public 
 * along with this program.  If not, see <http://www.gnu.org/licenses/>.
 */

using System;
using System.Collections.Generic;
using Sce.PlayStation.Core;
using Sce.PlayStation.Core.Environment;
using Sce.PlayStation.Core.Graphics;

namespace SteamPunkWasteLand
{
	public static class WorldCoord
	{
		public static Vector3 WorldZero;//Center of view in Graphics coordinates
		public static Vector3 FocusObject;//Should be the player position in World Coordinates
		public const float WIDTH_DEVIATION = 230f;//distance from center until camera starts focusing
		public const float HEIGHT_DEVIATION = 350f;
		public const float VIEW_SPEED = 150f;
		
		public static Vector3 WorldToView (Vector3 worldPos){
			return WorldZero + worldPos;
		}
		
		public static void UpdateFocus (float time){
			float Hwidth = Game.Graphics.Screen.Width/2;
			float Hheight = Game.Graphics.Screen.Height/2;
			
//			if (FocusObject.X > Focus.X+Hwidth+WIDTH_DEVIATION) 
//				Focus.X += VIEW_SPEED*time;
//			else if (FocusObject.X < Focus.X+Hwidth-WIDTH_DEVIATION) 
//				Focus.X -= VIEW_SPEED*time;
//			if (FocusObject.Y > Focus.Y+Hheight+HEIGHT_DEVIATION) 
//				Focus.Y += VIEW_SPEED*time;
//			else if (FocusObject.Y < Focus.Y+Hheight-HEIGHT_DEVIATION) 
//				Focus.Y -= VIEW_SPEED*time;
			
			
			
		}
	}
}

