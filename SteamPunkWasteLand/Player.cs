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
		#region Constants
		public const float SPEED = 435f;
		public const float ACCEL = 10f;
		#endregion
		
		#region Private Fields
		private float Swidth = Game.Graphics.Screen.Width;
		
		private Vector3 vel;
		
		private int spriteIndex;
		private int spriteIndexY;
		private float timer;
		
		private float ScreenWidth = Game.Graphics.Screen.Width;
		private float ScreenHeight = Game.Graphics.Screen.Height;
		#endregion
		
		#region Properties
		private Sprite sprite;
		private Vector3 worldPos;
		public Vector3 WorldPos {
			get {return worldPos;}
			set {worldPos = value;}
		}
		#endregion
		
		#region Constructor
		public Player ()
		{
			sprite = new Sprite(Game.Graphics,Game.Textures[3],48,70);
			worldPos = Vector3.Zero;
			vel = Vector3.Zero;
			
			spriteIndex = 0;
			timer = 0;
			sprite.Position = worldToSprite();
			sprite.Center = new Vector2(0.5f,0.5f);
			
		}
		#endregion
		
		#region Methods
		public Vector3 worldToSprite ()
		{
			//Vector3 spriteVector = new Vector3(worldPos.X+ScreenWidth/2,-worldPos.Y+(ScreenHeight*7/8f)-sprite.Height/2,0);
			return WorldCoord.WorldToView(new Vector3(worldPos.X,worldPos.Y+sprite.Height/2,0));
		}
		
		public void Physics (float time)
		{
			//gravity
			vel.Y -= 9.8f*time;
			
			//friction
			if (worldPos.Y > 0) {
				vel.X *= FMath.Pow(0.2f,time);
			}else{
				vel.X *= FMath.Pow(0.001f,time);
			}
			if (FMath.Abs(vel.X) < 0.05f) {
				vel.X = 0;
			}
			
			//ground level
			if(worldPos.Y < 0){
				vel.Y = 0;
				worldPos.Y = 0;
			}
			
		}
		#endregion
		
		#region Original Methods
		public void Update(GamePadData gpd, float time)
		{
			timer+= time;
			//movements
			if ((gpd.Buttons & GamePadButtons.Right) != 0) {
				vel.X = SPEED/1*time;
				spriteIndexY = 0;
			}
			if ((gpd.Buttons & GamePadButtons.Left) != 0) {
				vel.X = -SPEED/1*time;
				spriteIndexY = 1;
			}
			if ((gpd.Buttons & GamePadButtons.Up) != 0) {
				if (worldPos.Y < 1)
					vel.Y = SPEED*time;
			}
			
			Physics(time);
			
			//update pos
			worldPos += vel;
			//limit game world
			float screenMax = Swidth*1.5f-sprite.Width/2;
			float screenMin = -Swidth*1.5f+sprite.Width/2;
			if (worldPos.X > screenMax) {
				worldPos.X = screenMax;
			}
			if (worldPos.X < screenMin) {
				worldPos.X = screenMin;
			}
			//set sprite indexer
			if (vel.X > 0.1 || vel.X < -0.1) {
				if (timer > 0.1){
					timer = 0;
					if (spriteIndex < 3) {
						spriteIndex++;
					}else
					spriteIndex = 0;
				}
			}else{
				spriteIndex = 0;
			}
			if (worldPos.Y > 0.1) {
				spriteIndex = 4;
			}
			
			sprite.Position = worldToSprite();
		}
		
		public void Render()
		{
			sprite.SetTextureCoord(sprite.Width*spriteIndex,sprite.Height*spriteIndexY,(spriteIndex+1)*sprite.Width,(spriteIndexY+1)*sprite.Height);
			sprite.Render();
		}
		#endregion
	}
}

