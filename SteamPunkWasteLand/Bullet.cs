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
	public abstract class Bullet
	{
		private int spriteIndex;
		protected int SpriteIndex
		{
			get{return spriteIndex;}
			set{spriteIndex = value;}
		}
		private float speed;
		protected float Speed
		{
			get{return speed;}
			set{speed = value;}
		}
		
		private float dir;
		protected float Dir
		{
			get{return dir;}
			set{dir = value;}
		}
		
		private float deltaTime;
		protected float DeltaTime
		{
			get{return deltaTime;}
			set{deltaTime = value;}
		}
		
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
		private bool despawn;
		public bool Despawn
		{
			get{return despawn;}
			set{despawn = value;}
		}
		private bool hit;
		public bool Hit
		{
			get{return hit;}
			set{hit = value;}
		}
		
		public Bullet (float direction, float speed, Vector3 initPos)
			:this(direction,speed,initPos,0)
		{}
		public Bullet (float direction, float speed, Vector3 initPos, int spriteIndex)
		{
			this.spriteIndex = spriteIndex;
			this.pos = initPos;
			this.speed = speed;
			this.dir = direction;
			deltaTime = 0;
			despawn = false;
			hit = false;
		}
		
		protected Vector3 worldToSprite ()
		{
			return WorldCoord.WorldToView(new Vector3(pos.X,pos.Y+sprite.Height/2,0));
		}
		
		public virtual void Update (float time)
		{
			deltaTime += time;
			
			if(hit == false){
				pos.X += speed*time*FMath.Cos(-dir)*(spriteIndex==0?1:-1);
				pos.Y += speed*time*FMath.Sin(-dir)*(spriteIndex==0?1:-1);
				sprite.Rotation = dir;
			}
			sprite.Position = worldToSprite();
		}
		
		public virtual void Render ()
		{
			sprite.SetTextureCoord(0,sprite.Height*spriteIndex,sprite.Width,(spriteIndex+1)*sprite.Height);
			sprite.Render();
		}
	}
}

