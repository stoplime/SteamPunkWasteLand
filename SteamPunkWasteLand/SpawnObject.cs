/*	
 * Copyright (C) 2015  Steffen Lim and Nicolas Villanueva
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
	public class SpawnObject
	{
		private string type;
		public string Type
		{
			get{return type;}
			set{type = value;}
		}
		
		private int posX;
		public int PosX
		{
			get{return posX;}
			set{posX = value;}
		}
		
		private int posY;
		public int PosY
		{
			get{return posY;}
			set{posY = value;}
		}
		
		private float delay;
		public float Delay
		{
			get{return delay;}
			set{delay = value;}
		}
		
		public SpawnObject (string type, int posX, int posY, float delay)
		{
			this.type = type;
			this.posX = posX;
			this.posY = posY;
			this.delay = delay;
		}
	}
}

