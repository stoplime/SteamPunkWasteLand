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
	public class B_Arrow : Bullet
	{
		public B_Arrow (float direction, float speed, Vector3 initPos)
			:this(direction,speed,initPos,0)
		{}
		public B_Arrow (float direction, float speed, Vector3 initPos, int spriteIndex)
			:base(direction,speed,initPos,spriteIndex)
		{
			Sprite = new Sprite(Game.Graphics,Game.Textures[13],38,6);
			Sprite.Position = worldToSprite();
			Sprite.Rotation = direction;
			Sprite.Center = new Vector2(0.9f,0.5f);
		}
		
		public override void Update (float time)
		{
			//gravity
			float Vy = Speed*FMath.Sin(Dir)+500f*time*(SpriteIndex==0?1:-1);
			float Vx = Speed*FMath.Cos(Dir);
			Speed = FMath.Sqrt(Vy*Vy+Vx*Vx);
			Dir = FMath.Atan2(Vy,Vx);
			
			//ground
			if (Pos.Y < -Sprite.Width/2f) {
				Hit = true;
			}
			
			base.Update (time);
			
			//checks if it despawns
			if(DeltaTime > 10){
				Despawn = true;
			}
		}
		
	}
}

