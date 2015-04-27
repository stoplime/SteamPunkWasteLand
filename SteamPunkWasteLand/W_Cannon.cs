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
	public class W_Cannon : Weapon
	{
		private float deviation = 0.05f;
		private bool isEnemy;
		
		private float radius;
		
		public W_Cannon ()
			:this(false)
		{}
		
		public W_Cannon (bool isEnemy)
		{
			this.isEnemy = isEnemy;
			Type = WeaponType.Cannon;
			
			if (!isEnemy) {
				Sprite = new Sprite(Game.Graphics,Game.Textures[8],70,34);
				radius = Game.Upgrades[1,4]/32f;
			}else{
				Sprite = new Sprite(Game.Graphics,Game.Textures[28],40,40);
				radius = Game.Rand.Next(25,80);
			}
			Sprite.Center = new Vector2(0.5f,0.5f);
			FireSpd = 1f;
		}
		
		public override Bullet Fire (Vector3 vel)
		{
			DeltaTime = 0;
			Vector3 firePos = new Vector3(
				ExtendArc(Pos.X,31.4f,Aim,-0.160f,SpriteIndex,true),
				ExtendArc(Pos.Y-4,31.4f,Aim,-0.160f,SpriteIndex,false),0);
			float unSteady = -Aim+deviation*(Game.Rand.Next(2)==0?1:-1)*(float)Game.Rand.NextDouble();
			float relativeVel = 600f+vel.Length()*30f*FMath.Cos(FMath.Atan2(-vel.Y,vel.X)-((SpriteIndex==0)?unSteady:unSteady-FMath.PI));
			B_CannonBall b = new B_CannonBall(unSteady, relativeVel, firePos, SpriteIndex, radius);
			return b;
		}
	}
}

