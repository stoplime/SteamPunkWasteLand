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
	public class E_Zeppelin : Enemy
	{
		private float speed;
		
		public E_Zeppelin (Vector3 initPos)
			:base(initPos)
		{
			MaxHp = 200;
			Hp = MaxHp;
			Weapon = new W_Cannon();
			//Its initial pos in the Y needs to be between 90% to 50% the height of screen
			
			Sprite = new Sprite(Game.Graphics,Game.Textures[6],344,174);
			Sprite.Center = new Vector2(0.5f,0.5f);
			Sprite.Position = worldToSprite();
			HitRadius = (Sprite.Width+Sprite.Height)/4f;
			
			FireSpeed = 2f;
			speed = 100f;
			Score = 1000;
			MoneyLoot = Game.Rand.Next(200,401);
			
			SpriteIndex = 0;
		}
		
		public override void Update (float time)
		{
			float Time = time/Game.TimeSpeed;
			Vector3 tempVel = Vel;
			if (SpriteIndex == 0) {
				//go right
				tempVel.X += (speed*Time-tempVel.X)/10f;
			}else{
				tempVel.X += (-speed*Time-tempVel.X)/10f;
			}
			
			if (Pos.X > Game.Graphics.Screen.Width*2f) {
				SpriteIndex = 1;
				tempVel.X = -speed*Time;
			}
			else if(Pos.X < -Game.Graphics.Screen.Width*2f){
				SpriteIndex = 0;
				tempVel.X = speed*Time;
			}
			
			Vel = tempVel;
			
			if (FMath.Abs(Pos.X-Target.X) < Game.Graphics.Screen.Width/2f) {
				Firing = true;
			}else{
				Firing = false;
			}
			
			Aim += 0.0005f*(Target.X-Pos.X);
			
			Weapon.Update(time,(SpriteIndex==0?Aim:Aim+FMath.PI),Pos,SpriteIndex);
			
			base.Update (time);
		}
		
		public override void Render ()
		{
			Weapon.Render();
			base.Render ();
		}
	}
}

