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
	public class E_Dragon : Enemy
	{
		private float deltaTime;
		public E_Dragon (Vector3 initPos)
			:base(initPos)
		{
			MaxHp = 500;
			Hp = MaxHp;
			Weapon = new W_Flamethrower(true);
			
			Sprite = new Sprite(Game.Graphics,Game.Textures[6],344,174);
			Sprite.Center = new Vector2(0.5f,0.5f);
			Sprite.Position = worldToSprite();
			HitRadius = (Sprite.Width+Sprite.Height)/4f;
			
			
			
			deltaTime = 0;
			FireSpeed = 0.01f;
			SpriteIndex = 1;
		}
		
		public override void CollideWithB (Bullet b)
		{
			if(!(b is B_Flame)){
				//float y = Vel.Y;
				base.CollideWithB (b);
				//Vel = new Vector3(Vel.X,y,0);
			}
		}
		
//		public override void FireMethod ()
//		{
//			int numbFire = (int)FMath.Floor(DeltaTime/FireSpeed);
//			DeltaTime = 0;
//			for (int i = 0; i < numbFire; i++) {
//				Game.EBullets.Add(Weapon.Fire(Vel));
//			}
//		}
		
		public override void Update (float time)
		{
			float Time = time/Game.TimeSpeed;
			deltaTime += time;
			Vector3 tempVel = Vel;
			if (SpriteIndex == 0) {
				//go right
				tempVel.X += (300f*Time-tempVel.X)/10f;
			}else{
				tempVel.X += (-300f*Time-tempVel.X)/10f;
			}
			
			//sin wave flying
			tempVel.Y += (FMath.Sin(deltaTime)*90f*Time-tempVel.Y)/10f;//*0.8f;
			
			if (Pos.X > Game.Graphics.Screen.Width*2f) {
				SpriteIndex = 1;
				tempVel.X = -300f*Time;
			}
			else if(Pos.X < -Game.Graphics.Screen.Width*2f){
				SpriteIndex = 0;
				tempVel.X = 300f*Time;
			}
			
			Vel = tempVel;
			
			if(Target.DistanceSquared(Pos) < 250000){
				Firing = true;
			}else{
				Firing = false;
			}
			
			Sprite.Rotation = FMath.Atan2(-tempVel.Y,tempVel.X)+((SpriteIndex==1)?FMath.PI:0);
			
			Vector3 weaponPos = new Vector3(
				ExtendArc(Pos.X,143.087f,-Sprite.Rotation,-0.03951f,SpriteIndex,true),
				ExtendArc(Pos.Y+Sprite.Height/2,143.087f,-Sprite.Rotation,-0.03951f,SpriteIndex,false),0);
			
			Aim = FMath.Atan2(Target.Y-(weaponPos.Y),Target.X-(weaponPos.X));
			float adjustAim = Aim;
			if (SpriteIndex == 0) {
				if (adjustAim > FMath.PI/2) {
					adjustAim = FMath.PI/2;
				}
				if (adjustAim < -FMath.PI/2) {
					adjustAim = -FMath.PI/2;
				}
			}else{
				if (adjustAim < FMath.PI/2 && adjustAim > 0) {
					adjustAim = FMath.PI/2;
				}
				if (adjustAim > -FMath.PI/2 && adjustAim < 0) {
					adjustAim = -FMath.PI/2;
				}
			}
			Aim = adjustAim + ((SpriteIndex==1)?(-FMath.PI/16f):(FMath.PI/8f));
			
			Weapon.Update(time,((SpriteIndex == 1)? Aim+FMath.PI:Aim), weaponPos,SpriteIndex,true);
			
//			head.Position = new Vector3(
//				ExtendArc(Sprite.Position.X,143.087f,Sprite.Rotation,0.03951f,SpriteIndex,true),
//				ExtendArc(Sprite.Position.Y,143.087f,Sprite.Rotation,0.03951f,SpriteIndex,false),0);
//			
//			head.Rotation = ((SpriteIndex==1)?-Aim+FMath.PI+0.45f:-Aim-0.45f);
			
			base.Update (time);
		}
		
		public override void Render ()
		{
			
			base.Render();
			Weapon.Render();
		}
	}
}

