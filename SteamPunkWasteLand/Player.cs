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
using Sce.PlayStation.Core.Input;

namespace SteamPunkWasteLand
{
	public class Player
	{
		public const float SPEED = 335f;
		public const float ACCEL = 10f;
		
		private Sprite sprite;
		private Vector3 worldPos;
		public Vector3 WorldPos {
			get {return worldPos;}
			set {worldPos = value;}
		}
		private Vector3 vel;
		private bool inMotion;
		
		private float ScreenWidth = Game.Graphics.Screen.Width;
		private float ScreenHeight = Game.Graphics.Screen.Height;
		
		public Player ()
		{
			sprite = new Sprite(Game.Graphics,Game.Textures[3],40,60);
			worldPos = Vector3.Zero;
			vel = Vector3.Zero;
			
			sprite.Position = worldToSprite();
			sprite.Center = new Vector2(0.5f,0.5f);
		}

		public Vector3 worldToSprite ()
		{
			Vector3 spriteVector = new Vector3(worldPos.X+ScreenWidth/2,-worldPos.Y+(ScreenHeight*7/8f)-sprite.Height/2,0);
			return spriteVector;
		}
		
		public void Update(GamePadData gpd, float time)
		{
			inMotion = false;
			if ((gpd.Buttons & GamePadButtons.Right) != 0) {
				inMotion = true;
				vel.X = SPEED/1*time;
			}
			if ((gpd.Buttons & GamePadButtons.Left) != 0) {
				inMotion = true;
				vel.X = -SPEED/1*time;
			}
			if ((gpd.Buttons & GamePadButtons.Up) != 0) {
				inMotion = true;
				if (worldPos.Y < 1)
					vel.Y = SPEED*time;
			}
			Physics(time);
			
			worldPos += vel;
			
			sprite.Position = worldToSprite();
		}
		
		public void Physics (float time)
		{
			//gravity
			vel.Y -= 9.8f*time;
			
			//friction
			//if (!inMotion) {
				if (worldPos.Y > 0) {
					vel.X *= FMath.Pow(0.2f,time);
				}else{
					vel.X *= FMath.Pow(0.05f,time);
				}
			//}
			if (FMath.Abs(vel.X) < 0.05f) {
				vel.X = 0;
			}
			
			//ground level
			if(worldPos.Y < 0){
				vel.Y = 0;
				worldPos.Y = 0;
			}
			
		}
		
		public void Render()
		{
			sprite.Render();
		}
		
	}
}

