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
		private float speed;
		private float dir;
		
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
		
		
		public Bullet (float direction, float speed)
		{
			this.speed = speed;
			this.dir = direction;
			
			sprite.Rotation = direction;
		}
		
		public virtual void Update (float time)
		{
			pos.X += speed*time*FMath.Cos(dir);
			pos.Y += speed*time*FMath.Sin(dir);
			
			sprite.Position = pos;
		}
		
		public virtual void Render ()
		{
			
		}
	}
}

