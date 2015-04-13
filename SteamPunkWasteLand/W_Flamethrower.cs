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
	public class W_Flamethrower : Weapon
	{
		private float deviation = 0.05f;
		
		public W_Flamethrower ()
		{
			Type = WeaponType.Flamethrower;
			
			Sprite = new Sprite(Game.Graphics,Game.Textures[9],52,14);
			Sprite.Center = new Vector2(0.5f,0.5f);
			FireSpd = 0.02f;
		}
		
		public override Bullet Fire (Vector3 vel)
		{
			DeltaTime = 0;
			Vector3 firePos = new Vector3(
				ExtendArc(Pos.X,25.3f,Aim,-0.161f,SpriteIndex,true),
				ExtendArc(Pos.Y-8,25.3f,Aim,-0.161f,SpriteIndex,false),0);
			float unSteady = -Aim+deviation*(Game.Rand.Next(2)==0?1:-1)*(float)Game.Rand.NextDouble();
			float relativeVel = 200f+vel.Length()*50f*FMath.Cos(FMath.Atan2(-vel.Y,vel.X)-((SpriteIndex==0)?unSteady:unSteady-FMath.PI));
			B_Flame b = new B_Flame(unSteady, relativeVel, firePos, SpriteIndex, 0.3f);
			return b;
		}
	}
}

