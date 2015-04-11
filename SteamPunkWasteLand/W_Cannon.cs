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
	public class W_Cannon : Weapon
	{
		public W_Cannon ()
		{
			WSprite = new Sprite(Game.Graphics,Game.Textures[9],48,70);
			WSprite.Center = new Vector2(0.5f,0.5f);
			FireSpd = 1f;
		}
		public override void Fire ()
		{
			if (DeltaTime > FireSpd) {
				DeltaTime = 0;
				Vector3 firePos = new Vector3(
					ExtendArc(Pos.X,15f,Aim,0.588f,SpriteIndex,true),
					ExtendArc(Pos.Y,15f,Aim,0.588f,SpriteIndex,false),0);
				B_CannonBall b = new B_CannonBall(-Aim, 500f, firePos);
				Game.PBullets.Add(b);
			}
		}
	}
}

