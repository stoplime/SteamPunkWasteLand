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
		public const float ARM_SPD = 4f;
		public const float HitDelay = 0.5f;
		#endregion
		
		#region Private Fields
		private float Swidth = Game.Graphics.Screen.Width;
		
		private Sprite armSprite;
		
		private Vector3 vel;
		
		private int spriteIndex;
		private int spriteIndexY;
		private float timer;
		
		private Weapon weapon;
		private int weaponIndex;
		
		private float hitTime;
		#endregion
		
		#region Properties
		private Sprite sprite;
		public Sprite Sprite
		{
			get{return sprite;}
			set{sprite = value;}
		}
		private Vector3 worldPos;
		public Vector3 WorldPos 
		{
			get {return worldPos;}
			set {worldPos = value;}
		}
		
		private float aim;
		public float Aim
		{
			get{return aim;}
			set{aim = value;}
		}
		
		private float maxHp;
		public float MaxHp
		{
			get{return maxHp;}
			set{maxHp = value;}
		}
		
		private float hp;
		public float Hp
		{
			get{return hp;}
			set{hp = value;}
		}
		private float hitRadius;
		public float HitRadius
		{
			get{return hitRadius;}
			set{hitRadius = value;}
		}
		private float damageMultiplier;
		public float DamgeMultiplier
		{
			get{return damageMultiplier;}
			set{DamgeMultiplier = value;}
		}
		#endregion
		
		#region Constructor
		public Player ()
		{
			sprite = new Sprite(Game.Graphics,Game.Textures[3],48,70);
			worldPos = Vector3.Zero;
			vel = Vector3.Zero;
			maxHp = 100;
			hp = maxHp;
			hitTime = HitDelay;
			
			damageMultiplier = 1000;
			
			spriteIndex = 0;
			timer = 0;
			sprite.Position = worldToSprite();
			sprite.Center = new Vector2(0.5f,0.5f);
			hitRadius = (sprite.Width+sprite.Height)/4f;
			
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
				vel.X *= FMath.Pow(0.02f,time);
			}else{
				vel.X *= FMath.Pow(0.00001f,time);
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
		
		public void CollideWithB (Bullet b)
		{
			hp -= b.Damage;
			hitTime = 0;
			
		}
		#endregion
		
		#region Original Methods
		public void Update(GamePadData gpd, float time)
		{
			hitTime += time;
			timer += time*FMath.Abs(vel.X)/3;
			
			if (weapon == null && Game.ObtainedWeapons.Count > 0) {
				weapon = Game.ObtainedWeapons[0];
			}
			
			//movements
			if ((gpd.Buttons & GamePadButtons.Right) != 0) {
				vel.X = SPEED*time/Game.TimeSpeed;
				if (spriteIndexY == 1) {
					spriteIndexY = 0;
					aim *= -1;
				}
			}
			if ((gpd.Buttons & GamePadButtons.Left) != 0) {
				vel.X = -SPEED*time/Game.TimeSpeed;
				if (spriteIndexY == 0) {
					spriteIndexY = 1;
					aim *= -1;
				}
			}
			if ((gpd.Buttons & GamePadButtons.Up) != 0) {
				if (worldPos.Y < 1+sprite.Height/2)
					vel.Y = JUMP;
			}
			//rotate arm
			if ((gpd.Buttons & GamePadButtons.Circle) != 0) {
				if (spriteIndexY == 1) {
					aim -= ARM_SPD*time;	
				}else{
					aim += ARM_SPD*time;
				}
			}
			if ((gpd.Buttons & GamePadButtons.Square) != 0) {
				if (spriteIndexY == 0) {
					aim -= ARM_SPD*time;	
				}else{
					aim += ARM_SPD*time;
				}
			}
			//switch weapons
			if ((gpd.Buttons & GamePadButtons.L) != 0 && (gpd.ButtonsPrev & GamePadButtons.L) == 0) {
				if (weaponIndex > 0) {
					weaponIndex--;
				}else{
					weaponIndex = Game.ObtainedWeapons.Count-1;
				}
			}
			if ((gpd.Buttons & GamePadButtons.R) != 0 && (gpd.ButtonsPrev & GamePadButtons.R) == 0) {
				if (weaponIndex < Game.ObtainedWeapons.Count-1) {
					weaponIndex++;
				}else{
					weaponIndex = 0;
				}
			}
			//fire
			if ((gpd.Buttons & GamePadButtons.Cross) != 0) {
				if (Game.ObtainedWeapons.Count > 0) {
					for (int i = 0; i < weapon.Delay(); i++) {
						Game.PBullets.Add(weapon.Fire(vel));
					}
				}
			}
			
			Physics(time);
			
			//switch weapons
			if (weaponIndex >= 0 && Game.ObtainedWeapons.Count > 0) {
				weapon = Game.ObtainedWeapons[weaponIndex];
			}
			
			//update pos
			worldPos += vel * /*time*65;//*/Game.TimeSpeed;
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
			
			if (weapon != null){
				weapon.Update(time, aim, new Vector3(worldPos.X+((spriteIndexY == 1)? 10:-10),worldPos.Y,0),spriteIndexY);
			}
		}
		
		public void Render()
		{
			if (hitTime < HitDelay){
				sprite.SetColor(1,0.5f,0.5f,1);
			}else{
				sprite.SetColor(1,1,1,1);
			}
			sprite.SetTextureCoord(sprite.Width*spriteIndex,sprite.Height*spriteIndexY,(spriteIndex+1)*sprite.Width,(spriteIndexY+1)*sprite.Height);
			armSprite.SetTextureCoord(armSprite.Width*spriteIndexY,0,(spriteIndexY+1)*armSprite.Width,armSprite.Height);
			sprite.Render();
			armSprite.Render();
			if (weapon != null){
				weapon.Render();
			}
		}
		#endregion
	}
}

