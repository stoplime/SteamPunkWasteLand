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
		private static Vector3 ViewZero = new Vector3(Game.Graphics.Screen.Width/2,Game.Graphics.Screen.Height*7/8f,0);
		public static Vector3 WorldZero = ViewZero;//Center of view in Graphics coordinates
		public static Vector3 FocusObject;//Should be the player position in World Coordinates
		public const float WIDTH_DEVIATION = 250f;//distance from center until camera starts focusing
		//public const float HEIGHT_DEVIATION = 350f;
		public static float Hwidth = Game.Graphics.Screen.Width/2;
		public static float Hheight = Game.Graphics.Screen.Height/2;
		public static float ViewSpd;
		
		public static Vector3 WorldToView (Vector3 worldPos){
			return new Vector3(WorldZero.X + worldPos.X,WorldZero.Y - worldPos.Y,0);
		}
		
		public static void UpdateFocus (float time){
			float Hwidth = Game.Graphics.Screen.Width/2;
			float Hheight = Game.Graphics.Screen.Height/2;
			
			ViewSpd = FMath.Abs(FocusObject.X+WorldZero.X-Hwidth);
			
			if (FocusObject.X+WorldZero.X > Hwidth+WIDTH_DEVIATION){
				if(WorldZero.X > -Hwidth+10){
					WorldZero.X -= ViewSpd*time*Game.TimeSpeed;
				}
			}
			else if (FocusObject.X+WorldZero.X < +Hwidth-WIDTH_DEVIATION){
				if(WorldZero.X < Hwidth*3-10){
					WorldZero.X += ViewSpd*time*Game.TimeSpeed;
				}
			}
//			if (FocusObject.Y > WorldZero.Y+Hheight+HEIGHT_DEVIATION) 
//				WorldZero.Y += VIEW_SPEED*time;
//			else if (FocusObject.Y < WorldZero.Y+Hheight-HEIGHT_DEVIATION) 
//				WorldZero.Y -= VIEW_SPEED*time;
			
//			Vector3 f = ViewZero;
//			f.X -= FocusObject.X;
//			f.Y += FocusObject.Y;
//			WorldZero = f;
			
		}
	}
}

