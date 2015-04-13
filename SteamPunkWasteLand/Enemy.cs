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

namespace SteamPunkWasteLand
{
	public abstract class Enemy
	{
		#region Private Fields
		private float deltaTime;
		private bool hit;
		private const float HitDelay = 0.5f;
		private float hitTime;
		#endregion
		
		#region Properties
		private Sprite sprite;
		public Sprite Sprite
		{
			get{return sprite;}
			set{sprite = value;}
		}
		private Vector3 pos;
		public Vector3 Pos
		{
			get{return pos;}
			set{pos = value;}
		}
		private float hp;
		public float Hp
		{
			get{return hp;}
			set{hp = value;}
		}
		
		//protected
		private int spriteIndex;
		protected int SpriteIndex
		{
			get{return spriteIndex;}
			set{spriteIndex = value;}
		}
		private Weapon weapon;
		protected Weapon Weapon
		{
			get{return weapon;}
			set{weapon = value;}
		}
		private Vector3 vel;
		protected Vector3 Vel
		{
			get{return vel;}
			set{vel = value;}
		}
		private Vector3 target;
		protected Vector3 Target
		{
			get{return target;}
			set{target = value;}
		}
		private float fireSpeed;
		protected float FireSpeed
		{
			get{return fireSpeed;}
			set{fireSpeed = value;}
		}
		private bool firing;
		protected bool Firing
		{
			get{return firing;}
			set{firing = value;}
		}
		private float aim;
		protected float Aim
		{
			get{return aim;}
			set{aim = value;}
		}
		
		#endregion
		
		#region Constructor and Init
		public Enemy (Vector3 initPos)
		{
			pos = initPos;
			vel = Vector3.Zero;
			deltaTime = 0;
			spriteIndex = 0;
			firing = false;
			hitTime = HitDelay;
		}
		#endregion
		
		#region Additional Methods
		public Vector3 worldToSprite ()
		{
			return WorldCoord.WorldToView(new Vector3(pos.X,pos.Y+sprite.Height/2,0));
		}
		
		protected virtual void Physics (float time)
		{
			vel.Y -= 9.8f*time;
			
			//friction
//			if (pos.Y > 0) {
//				vel.X *= FMath.Pow(0.2f,time);
//			}else{
//				vel.X *= FMath.Pow(0.001f,time);
//			}
//			if (FMath.Abs(vel.X) < 0.05f) {
//				vel.X = 0;
//			}
			
			//ground level
			if(pos.Y < 0){
				vel.Y = 0;
				pos.Y = 0;
			}
		}
		
		public void CollideWithB (Bullet b)
		{
			hp -= b.Damage;
			hitTime = 0;
			if (pos.Y < 1f) {
				vel.Y = 4f;
			}
		}
		#endregion
		
		#region Original Methods
		public virtual void Update(float time)
		{
			deltaTime += time;
			hitTime += time;
			target = Game.Player1.WorldPos;
			aim = FMath.Atan2(target.Y-pos.Y,target.X-pos.X);
			
			
			//aim += deviation*FMath.Sin(FMath.PI*2*(float)Game.Rand.NextDouble());
			
			if (deltaTime > fireSpeed && firing) {
				deltaTime = 0;
				Game.EBullets.Add(weapon.Fire(vel));
			}
			
			pos += vel * Game.TimeSpeed;
			
			sprite.Position = worldToSprite();
		}
		
		public virtual void Render()
		{
			sprite.SetTextureCoord(0,spriteIndex*sprite.Height,sprite.Width,(spriteIndex+1)*sprite.Height);
			if (hitTime < HitDelay) {
				sprite.SetColor(1,0.5f,0.5f,1);
			}else{
				sprite.SetColor(1,1,1,1);
			}
			sprite.Render();
		}
		#endregion
	}
}

