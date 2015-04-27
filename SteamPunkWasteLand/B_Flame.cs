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
	public class B_Flame : Bullet
	{
		public B_Flame (float direction, float speed, Vector3 initPos, float deviation)
			:this(direction,speed,initPos,0,deviation,false)
		{}
		public B_Flame (float direction, float speed, Vector3 initPos, int spriteIndex, float deviation, bool isEnemy)
			:base(direction,speed,initPos,spriteIndex)
		{
			Sprite = new Sprite(Game.Graphics,Game.Textures[12],16,16);
			
			//spread deviation
			Dir += (float)(deviation*Game.Rand.NextDouble()*(Game.Rand.Next(2)==1?1:-1));
			
			Sprite.Position = worldToSprite();
			Sprite.Rotation = Dir;
			Sprite.Center = new Vector2(0.5f,0.5f);
			
			if (!isEnemy) {
				Damage = Game.Upgrades[1,3];
			}else{
				Damage = 0.2f;
			}
			Radius = Sprite.Width/2f;
		}
		
		public override void Update (float time)
		{
			//ground
			if (Pos.Y < -Sprite.Width/2f) {
				Hit = true;
			}
			
			base.Update (time);
			
			//checks despawn
			if (DeltaTime > 1) {
				Despawn = true;
			}
		}
		public override void Render ()
		{
			Sprite.SetColor(1f,1f,1f,1-DeltaTime);
			base.Render ();
		}
	}
}

