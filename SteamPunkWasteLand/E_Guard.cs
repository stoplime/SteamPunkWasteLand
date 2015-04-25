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
	public class E_Guard : Enemy
	{
		private const float SPEED = 30f;
		
		public E_Guard (Vector3 initPos)
			:base(initPos)
		{
			MaxHp = 20;
			Hp = MaxHp;
			Weapon = new W_CrossBow();
			
			Sprite = new Sprite(Game.Graphics,Game.Textures[7],48,70);
			Sprite.Center = new Vector2(0.5f,0.5f);
			Sprite.Position = worldToSprite();
			HitRadius = (Sprite.Width+Sprite.Height)/4f;
			
			FireSpeed = 1f;
			
			MoneyLoot = Game.Rand.Next(20,41);
			Score = MoneyLoot;
		}
		
		public override void Update (float time)
		{
			
			bool right = false;
			if (Target.X-Pos.X >= 0) {
				SpriteIndex = 0;
				right = true;
			}else{
				SpriteIndex = 1;
			}
			
			float distSq = Target.DistanceSquared(Pos);
			if(distSq > 900 && distSq < 250000){
				Firing = true;
				if (SpriteIndex == 1) {
					Aim -= 0.004f*distSq/2500f;
				}else{
					Aim += 0.004f*distSq/2500f;
				}
			}else{
				Firing = false;
			}
			Weapon.Update(time,((SpriteIndex == 1)? Aim+FMath.PI:Aim),
			              new Vector3(Pos.X+((SpriteIndex == 1)? 10:-10),Pos.Y+Sprite.Height/2,0),SpriteIndex);
			float Vx = 0;
			if(distSq > 25000){
				Vx = ((right)?1:-1)*SPEED*time;
			}
			foreach (Enemy e in Game.Enemies) {
				if (e is E_Guard) {
					if (e != this) {
						float dist = e.Pos.X-Pos.X; 
						if(dist < Sprite.Width && dist > 0){
							Vx -= SPEED*time/2f;
						}
						if(dist > -Sprite.Width && dist < 0){
							Vx += SPEED*time/2f;
						}
					}
				}
			}
			
			Vel = new Vector3(Vx,Vel.Y-9.8f*time,0);
			Physics(time,9.8f,false);
			
			base.Update (time);
		}
		
		public override void Render ()
		{
			
			base.Render ();
			if(Hp > 0){
				Weapon.Render();
			}
		}
	}
}

