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
	public class W_Flamethrower : Weapon
	{
		private float deviation = 0.05f;
		private bool isEnemy;
		
		public W_Flamethrower()
			:this(false)
		{}
		
		public W_Flamethrower (bool isEnemy)
		{
			Type = WeaponType.Flamethrower;
			this.isEnemy = isEnemy;
			
			if(isEnemy){
				Sprite = new Sprite(Game.Graphics,Game.Textures[15],60,50);
			}else{
				Sprite = new Sprite(Game.Graphics,Game.Textures[9],52,14);
			}
			Sprite.Center = new Vector2(0.5f,0.5f);
			FireSpd = 0.002f;
		}
		
		public override void Update (float time, float aim, Vector3 sholderPos, int index, bool isEnemy)
		{
			base.Update (time, aim, sholderPos, index, isEnemy);
			
			if (isEnemy) {
				Sprite.Rotation = -aim + 0.32f;
			}
		}
		
		public override Bullet Fire (Vector3 vel)
		{
			DeltaTime = 0;
			Vector3 firePos;
			if(isEnemy){
				firePos = new Vector3(
					ExtendArc(Pos.X,32.4f,Aim,(SpriteIndex==1?0.406f:0.406f+FMath.PI/4),SpriteIndex,true),
					ExtendArc(Pos.Y,32.4f,Aim,(SpriteIndex==1?0.406f:0.406f+FMath.PI/4),SpriteIndex,false),0);
			}else{
				firePos = new Vector3(
					ExtendArc(Pos.X,25.3f,Aim,-0.161f,SpriteIndex,true),
					ExtendArc(Pos.Y-8,25.3f,Aim,-0.161f,SpriteIndex,false),0);
			}
			float unSteady = -Aim+deviation*(Game.Rand.Next(2)==0?1:-1)*(float)Game.Rand.NextDouble();
			float relativeVel = 200f+vel.Length()*50f*FMath.Cos(FMath.Atan2(-vel.Y,vel.X)-((SpriteIndex==0)?unSteady:unSteady-FMath.PI));
			B_Flame b = new B_Flame(unSteady, relativeVel, firePos, SpriteIndex, 0.3f, isEnemy);
			return b;
		}
	}
}

