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
	public class E_AirShip : Enemy
	{
		private List<Weapon> cannons;
		private List<float> fireTime;
		private int cannonIndex;
		private float speed;
		private float downCount;
		
		public E_AirShip (Vector3 initPos)
			:base(initPos)
		{
			MaxHp = 3001;//2000;//TODO: Revert this back
			Hp = MaxHp;
			cannonIndex = 0;
			cannons = new List<Weapon>();
			fireTime = new List<float>();
			for (int i = 0; i < 8; i++) {
				cannons.Add(new W_Cannon());
				fireTime.Add((float)(Game.Rand.NextDouble()*5+2));
			}
			Weapon = cannons[cannonIndex];
			
			downCount = 0;
			
			Sprite = new Sprite(Game.Graphics,Game.Textures[26],512,350);
			Sprite.Center = new Vector2(0.5f,0.5f);
			Sprite.Position = worldToSprite();
			HitRadius = (Sprite.Width+Sprite.Height)/4f;
			HpOffset = true;
			MoneyLoot = Game.Rand.Next(2000,4001);
			Score = MoneyLoot;
			
			FireSpeed = 2f;
			speed = 50f;
		}
		
		public override void Update (float time)
		{
			float Time = time/Game.TimeSpeed;
			downCount += time;
			Vector3 tempVel = Vel;
			//downward movement
			if (Pos.Y-Target.Y > 350) {
				tempVel.Y += (-speed*Time-tempVel.Y)/10f;
			}else{
				tempVel.Y += (-tempVel.Y)/100f;//slowly normalize to zero
			}
			if (SpriteIndex == 0) {
				//go right
				tempVel.X += (speed*Time-tempVel.X)/20f;
			}else{
				tempVel.X += (-speed*Time-tempVel.X)/20f;
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

