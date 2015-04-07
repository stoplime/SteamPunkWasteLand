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
		private Sprite sprite;
		protected Sprite Sprite
		{
			get{return sprite;}
			set{sprite = value;}
		}
		
		private Vector3 pos;
		private Vector3 vel;
		
		private Weapon weapon;
		private Vector3 target;
		
		public Enemy ()
		{
		}
		
		public abstract void Update();
		
		public virtual void Render()
		{
			sprite.Render();
		}
		
	}
}

