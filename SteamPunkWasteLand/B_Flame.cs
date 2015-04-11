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
	public class B_Flame : Bullet
	{
		public B_Flame (float direction, float speed, Vector3 initPos)
			:this(direction,speed,initPos,0)
		{}
		public B_Flame (float direction, float speed, Vector3 initPos, int spriteIndex)
			:base(direction,speed,initPos,spriteIndex)
		{
			Sprite = new Sprite(Game.Graphics,Game.Textures[12]);
		}
	}
}

