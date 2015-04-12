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
		
		private float deviation;
		#endregion
		
		#region Properties
		private Sprite sprite;
		protected Sprite Sprite
		{
			get{return sprite;}
			set{sprite = value;}
		}
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
		private Vector3 pos;
		protected Vector3 Pos
		{
			get{return pos;}
			set{pos = value;}
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
		private float aim;
		protected float Aim
		{
			get{return aim;}
			set{aim = value;}
		}
		private float speed;
		protected float Speed
		{
			get{return speed;}
			set{speed = value;}
		}
		#endregion
		
		#region Constructor and Init
		public Enemy (Vector3 initPos)
		{
			pos = initPos;
			vel = Vector3.Zero;
			deltaTime = 0;
			spriteIndex = 0;
			deviation = 0.2f;
		}
		#endregion
		
		#region Additional Methods
		public Vector3 worldToSprite ()
		{
			return WorldCoord.WorldToView(new Vector3(pos.X,pos.Y+sprite.Height/2,0));
		}
		
		protected virtual void Physics (float time)
		{
			vel -= 9.8f*time;
			
			//friction
			if (pos.Y > 0) {
				vel.X *= FMath.Pow(0.2f,time);
			}else{
				vel.X *= FMath.Pow(0.001f,time);
			}
			if (FMath.Abs(vel.X) < 0.05f) {
				vel.X = 0;
			}
			
			//ground level
			if(pos.Y < 0){
				vel.Y = 0;
				pos.Y = 0;
			}
		}
		#endregion
		
		#region Original Methods
		public virtual void Update(float time)
		{
			deltaTime += time;
			target = Game.Player1.WorldPos;
			aim = FMath.Atan2(target.Y-pos.Y,target.X-pos.X)+(float)(deviation*Game.Rand.NextDouble()*(Game.Rand.Next(2)==0?1:-1));
			
			if (deltaTime > fireSpeed) {
				deltaTime = 0;
				weapon.Fire();
			}
			
			pos += vel * Game.TimeSpeed;
			
			sprite.Position = worldToSprite();
		}
		
		public virtual void Render()
		{
			sprite.Render();
			weapon.Render();
		}
		#endregion
	}
}

