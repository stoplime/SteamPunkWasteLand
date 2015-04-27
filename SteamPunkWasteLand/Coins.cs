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
	public class Coins
	{
		private Sprite sprite;
		private Vector3 pos, vel;
		private Vector3 target;
		private float delay;
		
		private bool despawn;
		public bool Despawn
		{
			get{return despawn;}
			set{despawn = value;}
		}
		
		public Coins (Vector3 initPos)
		{
			this.pos = WorldCoord.WorldToView(initPos);
			despawn = false;
			
			float scale = Game.Graphics.Screen.Height/800f;
			delay = 1f;
			
			sprite = new Sprite(Game.Graphics,Game.Textures[19]);
			sprite.Center = new Vector2(0.5f,0.5f);
			target = new Vector3(30*scale,Game.Graphics.Screen.Height-30*scale,0);
			
			float angle = (float)(Math.PI*Game.Rand.NextDouble());
			float speed = (float)(4*Game.Rand.NextDouble());
			vel = new Vector3(speed*FMath.Cos(angle),speed*FMath.Sin(angle)-5f,0);
		}
		
		public void Update(float time)
		{
			delay -= time;
			
			if (delay <= 0) {
				vel = target.Subtract(pos);
				vel = vel.Normalize()*10;
			}else{
				//gravity
				vel.Y += 2.5f*time;
				
				//groud
//				float ground = Game.Graphics.Screen.Height*(7.1f/8f);
//				if(pos.Y > ground){
//					vel = Vector3.Zero;
//					pos.Y = ground;
//				}
			}
			
			pos += vel;
			
			if (pos.DistanceSquared(target) < 100) {
				despawn = true;
			}
			sprite.Position = pos;
		}
		
		public void Render()
		{
			sprite.Render();
		}
		
	}
}

