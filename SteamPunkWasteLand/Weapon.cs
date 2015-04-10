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
	public abstract class Weapon
	{
		#region Private Fields
		private const float phi = 0.588f;
		private const float ArmLenght = 15f;
		
		private int spriteIndex;
		#endregion
		
		#region Properties
		private Sprite sprite;
		protected Sprite WSprite
		{
			get{return sprite;}
			set{sprite = value;}
		}
		private Vector3 pos;
		protected Vector3 Pos
		{
			get{return pos;}
			set{pos = value;}
		}
		private float aim;
		protected float Aim
		{
			get{return aim;}
			set{aim = value;}
		}
		#endregion
		
		#region Constructor
		public Weapon ()
		{
			pos = new Vector3();
			aim = 0;
			spriteIndex = 0;
		}
		#endregion
		
		#region Additional Methods
		protected Vector3 worldToSprite ()
		{
			return WorldCoord.WorldToView(new Vector3(pos.X,pos.Y+sprite.Height/2,0));
		}
		
		public abstract void Fire ();
		
		#endregion
		
		#region Original Methods
		//for the player
		public virtual void Update (float time, float aim, Vector3 sholderPos, int index)
		{
			this.aim = aim;
			spriteIndex = index;
			
			float angle = aim+(spriteIndex==0?-phi:phi);
			pos.X = sholderPos.X+FMath.Cos(angle)*ArmLenght*(spriteIndex==0?1:-1);
			pos.Y = sholderPos.Y+FMath.Sin(angle)*ArmLenght*(spriteIndex==0?1:-1);
			
			sprite.Position = worldToSprite();
			sprite.Rotation = -aim;
		}
		
		public virtual void Render ()
		{
			sprite.SetTextureCoord(sprite.Width*spriteIndex,0,(spriteIndex+1)*sprite.Width,sprite.Height);
			sprite.Render();
		}
		#endregion
	}
}

