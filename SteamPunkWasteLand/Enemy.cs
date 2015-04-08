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
		private Vector3 pos;
		private Vector3 vel;
		
		private Vector3 target;
		#endregion
		
		#region Properties
		private Sprite sprite;
		protected Sprite ESprite
		{
			get{return sprite;}
			set{sprite = value;}
		}
		
		private Weapon weapon;
		protected Weapon EWeapon
		{
			get{return weapon;}
			set{weapon = value;}
		}
		#endregion
		
		#region Constructor and Init
		public Enemy ()
		{
		}
		
		public virtual void Init ()
		{
			sprite.Center = new Vector2(0.5f,0.5f);
			
		}
		#endregion
		
		#region Additional Methods
		public Vector3 worldToSprite ()
		{
			return WorldCoord.WorldToView(new Vector3(pos.X,pos.Y+sprite.Height/2,0));
		}
		
		public virtual void Physics (float time)
		{
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
			target = Game.Player1.WorldPos;
			
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

