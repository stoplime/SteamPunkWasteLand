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
	public enum WeaponType{
		CrossBow,
		Flamethrower,
		Cannon
	}
	
	public abstract class Loot
	{
		private Sprite sprite;
		protected Sprite Sprite
		{
			get{return sprite;}
			set{sprite = value;}
		}
		
		private float deltaTime;
		protected float DeltaTime
		{
			get{return deltaTime;}
			set{deltaTime = value;}
		}
		
		private Vector3 pos;
		public Vector3 Pos
		{
			get{return pos;}
			set{pos = value;}
		}
		
		private Vector3 vel;
		public Vector3 Vel
		{
			get{return vel;}
			set{vel = value;}
		}
		
		private bool despawn;
		public bool Despawn
		{
			get{return despawn;}
			set{despawn = value;}
		}
		private WeaponType type;
		public WeaponType Type
		{
			get{return type;}
			set{type = value;}
		}
		
		public Loot (Vector3 initPos)
		{
			pos = initPos;
			float angle = (float)(Math.PI*Game.Rand.NextDouble());
			float speed = (float)(4*Game.Rand.NextDouble());
			vel = new Vector3(speed*FMath.Cos(angle),speed*FMath.Sin(angle),0);
			despawn = false;
		}
		
		protected Vector3 worldToSprite ()
		{
			//Hover motion
			return WorldCoord.WorldToView(new Vector3(pos.X,pos.Y+sprite.Height/2+(5*(FMath.Sin(2*deltaTime)+1)),0));
		}
		
		public bool CheckPlayer ()
		{
			if (pos.DistanceSquared(Game.Player1.WorldPos) < 2500) {
				despawn = true;
				return true;
			}
			return false;
		}
		
		public virtual void Update (float time)
		{
			deltaTime += time;
			//gravity
			vel.Y -= 2.5f*time;
			
			//groud
			if(pos.Y < 0){
				vel = Vector3.Zero;
				pos.Y = 0;
			}
			
			//friction
			vel.X *= FMath.Pow(0.7f,time);
			if (FMath.Abs(vel.X) < 0.005f) {
				vel.X = 0;
			}
			
			//despawn
			//CheckPlayer();
			
			pos += vel*Game.TimeSpeed;
			
			sprite.Position = worldToSprite();
		}
		
		public virtual void Render ()
		{
			sprite.Render();
		}
	}
}

