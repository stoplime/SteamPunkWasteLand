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
	public class E_Guard : Enemy
	{
		public E_Guard (Vector3 initPos)
			:base(initPos)
		{
			Weapon = new W_CrossBow();
			
			Sprite = new Sprite(Game.Graphics,Game.Textures[5],48,70);
			Sprite.Center = new Vector2(0.5f,0.5f);
			Sprite.Position = worldToSprite();
			
			FireSpeed = 1f;
		}
		
		public override void Update (float time)
		{
			Weapon.Update(time,Aim,new Vector3(Pos.X+((SpriteIndex == 1)? 10:-10),Pos.Y,0),SpriteIndex);
			
			float Vx = ((Target.X-Pos.X >= 0)?1:-1)*Speed;
			
			Vel = new Vector3(Vx,Vel.Y-9.8f*time,0);
			Physics(time);
			
			base.Update (time);
		}
	}
}

