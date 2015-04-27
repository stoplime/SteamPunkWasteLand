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
	public class E_Zeppelin : Enemy
	{
		private float speed;
		
		public E_Zeppelin (Vector3 initPos)
			:base(initPos)
		{
			MaxHp = 100+8*Game.Level;
			Hp = MaxHp;
			Weapon = new W_Cannon(true);
			//Its initial pos in the Y needs to be between 90% to 50% the height of screen
			
			Sprite = new Sprite(Game.Graphics,Game.Textures[5],200,150);
			Sprite.Center = new Vector2(0.5f,0.5f);
			Sprite.Position = worldToSprite();
			HitRadius = (Sprite.Width+Sprite.Height)/4f;
			
			FireSpeed = 2f;
			speed = 100f;
			
			MoneyLoot = Game.Rand.Next(200,401);
			Score = MoneyLoot;
			
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
			
			Vector3 weaponPos = new Vector3(
				ExtendArc(Pos.X,61.31f,-Sprite.Rotation,0.9126f,SpriteIndex,true),
				ExtendArc(Pos.Y,61.31f,-Sprite.Rotation,0.9126f,SpriteIndex,false)+Sprite.Height/2,0);
			
			Aim = FMath.Atan2(Target.Y-weaponPos.Y,Target.X-weaponPos.X);
			Aim += 0.0005f*(Target.X-weaponPos.X);
			
			Weapon.Update(time,(SpriteIndex==0?Aim:Aim+FMath.PI),weaponPos,SpriteIndex,true);
			
			base.Update (time);
		}
		
		public override void Render ()
		{
			base.Render ();
			if (Hp > 0) {
				Weapon.Render();
			}
		}
	}
}

