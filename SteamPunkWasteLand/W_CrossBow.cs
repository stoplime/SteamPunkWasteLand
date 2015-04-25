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
	public class W_CrossBow : Weapon
	{
		private const float deviation = 0.05f;
		private bool isEnemy;
		
		public W_CrossBow ()
			:this(false)
		{}
		
		public W_CrossBow (bool isEnemy)
		{
			this.isEnemy = isEnemy;
			Type = WeaponType.CrossBow;
			
			if (!isEnemy) {
				Sprite = new Sprite(Game.Graphics,Game.Textures[10],76,24);
			}else{
				Sprite = new Sprite(Game.Graphics,Game.Textures[27],34,34);
			}
			
			Sprite.Center = new Vector2(0.5f,0.5f);
			FireSpd = 0.5f;
		}
		
		public override Bullet Fire (Vector3 vel)
		{
			DeltaTime = 0;
			Vector3 firePos = new Vector3(
				ExtendArc(Pos.X,32f,Aim,0f,SpriteIndex,true),
				ExtendArc(Pos.Y-3,32f,Aim,0f,SpriteIndex,false),0);
			float unSteady = -Aim+deviation*(Game.Rand.Next(2)==0?1:-1)*(float)Game.Rand.NextDouble();
			float relativeVel = 500f+vel.Length()*50f*FMath.Cos(FMath.Atan2(-vel.Y,vel.X)-((SpriteIndex==0)?unSteady:unSteady-FMath.PI));
			B_Arrow b = new B_Arrow(unSteady, relativeVel, firePos,SpriteIndex);
			return b;
			
		}
	}
}

