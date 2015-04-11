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
		public const float JUMP = 6.0f;
		public const float ACCEL = 10f;
		
		public const float GRAVITY = 9.8f;
		public const float ARM_SPD = 8f;
		#endregion
		
		#region Private Fields
		private float Swidth = Game.Graphics.Screen.Width;
		private Sprite sprite;
		private Sprite armSprite;
		
		private Vector3 vel;
		
		private int spriteIndex;
		private int spriteIndexY;
		private float timer;
		
		private Weapon weapon;
		
		#endregion
		
		#region Properties
		private Vector3 worldPos;
		public Vector3 WorldPos {
			get {return worldPos;}
			set {worldPos = value;}
		}
		
		private float aim;
		public float Aim{
			get{return aim;}
			set{aim = value;}
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
			
			weapon = new W_CrossBow();
			
			armSprite = new Sprite(Game.Graphics,Game.Textures[4],48,70);
			armSprite.Center = sprite.Center;
			armSprite.Position = new Vector3(sprite.Position.X-10,sprite.Position.Y,0);
		}
		#endregion
		
		#region Methods
		public Vector3 worldToSprite ()
		{
			return WorldCoord.WorldToView(new Vector3(worldPos.X,worldPos.Y,0));
		}
		
		public void Physics (float time)
		{
			//gravity
			vel.Y -= GRAVITY*time;
			
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
			if(worldPos.Y < sprite.Height/2){
				vel.Y = 0;
				worldPos.Y = sprite.Height/2;
			}
		}
		#endregion
		
		#region Original Methods
		public void Update(GamePadData gpd, float time)
		{
			timer += time*FMath.Abs(vel.X)/3;
			//movements
			if ((gpd.Buttons & GamePadButtons.Right) != 0) {
				vel.X = SPEED/1*time;
				if (spriteIndexY == 1) {
					spriteIndexY = 0;
					aim *= -1;
				}
			}
			if ((gpd.Buttons & GamePadButtons.Left) != 0) {
				vel.X = -SPEED/1*time;
				if (spriteIndexY == 0) {
					spriteIndexY = 1;
					aim *= -1;
				}
			}
			if ((gpd.Buttons & GamePadButtons.Up) != 0) {
				if (worldPos.Y < 1+sprite.Height/2)
					vel.Y = JUMP;
			}
			if ((gpd.Buttons & GamePadButtons.Circle) != 0) {
				aim -= ARM_SPD*time;
			}
			if ((gpd.Buttons & GamePadButtons.Square) != 0) {
				aim += ARM_SPD*time;
			}
			if ((gpd.Buttons & GamePadButtons.Cross) != 0) {
				weapon.Fire();
			}
			
			Physics(time);
			
			//update pos
			worldPos += vel */* time*65;//*/Game.TimeSpeed;
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
			if (FMath.Abs(vel.X) > 0.15) {
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
			if (worldPos.Y > 0.1+sprite.Height/2) {
				spriteIndex = 4;
			}
			
			sprite.Position = worldToSprite();
			armSprite.Position = sprite.Position;
			armSprite.Position.X+=(spriteIndexY == 1)? 10:-10;
			armSprite.Rotation = -aim;
			
			weapon.Update(time, aim, new Vector3(worldPos.X+((spriteIndexY == 1)? 10:-10),worldPos.Y-sprite.Height/2,0),spriteIndexY);
		}
		
		public void Render()
		{
			sprite.SetTextureCoord(sprite.Width*spriteIndex,sprite.Height*spriteIndexY,(spriteIndex+1)*sprite.Width,(spriteIndexY+1)*sprite.Height);
			armSprite.SetTextureCoord(armSprite.Width*spriteIndexY,0,(spriteIndexY+1)*armSprite.Width,armSprite.Height);
			sprite.Render();
			armSprite.Render();
			weapon.Render();
		}
		#endregion
	}
}

