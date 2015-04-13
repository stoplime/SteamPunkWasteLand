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
		private Sprite head;
		private float deltaTime;
		public E_Dragon (Vector3 initPos)
			:base(initPos)
		{
			Hp = 500;
			Weapon = new W_Flamethrower();
			
			Sprite = new Sprite(Game.Graphics,Game.Textures[6],344,174);
			Sprite.Center = new Vector2(0.5f,0.5f);
			Sprite.Position = worldToSprite();
			
			head = new Sprite(Game.Graphics,Game.Textures[15],60,50);
			head.Center = new Vector2(0.5f,0.5f);
			
			deltaTime = 0;
			FireSpeed = 0f;
			SpriteIndex = 1;
		}
		
		public float ExtendArc (float initPos, float extention, float angle, float phi, int mirror, bool cos)
		{
			float a = angle+(mirror==0?-phi:phi);
			float s;
			if (cos) 
				s = initPos+FMath.Cos(a)*extention*(mirror==0?1:-1);
			else
				s = initPos+FMath.Sin(a)*extention*(mirror==0?1:-1);
			return s;
		}
		
		public override void Update (float time)
		{
			deltaTime += time;
			Vector3 tempVel = Vel;
			if (SpriteIndex == 0) {
				//go right
				tempVel.X = 5f;
			}else{
				tempVel.X = -5f;
			}
			
			//sin wave flying
			tempVel.Y = FMath.Sin(deltaTime)*1.5f;//*0.8f;
			
			Vel = tempVel;
			
			if (Pos.X > Game.Graphics.Screen.Width*2f) {
				SpriteIndex = 1;
			}
			else if(Pos.X < -Game.Graphics.Screen.Width*2f){
				SpriteIndex = 0;
			}
			
			if(Target.DistanceSquared(Pos) < 250000){
				Firing = true;
			}else{
				Firing = false;
			}
			
			Sprite.Rotation = FMath.Atan2(-tempVel.Y,tempVel.X)+((SpriteIndex==1)?FMath.PI:0);
			
			Vector3 weaponPos = new Vector3(
				ExtendArc(Pos.X,143.087f,-Sprite.Rotation,-0.02951f,SpriteIndex,true),
				ExtendArc(Pos.Y+Sprite.Height/2,143.087f,-Sprite.Rotation,-0.02951f,SpriteIndex,false),0);
			
			Aim = FMath.Atan2(Target.Y-(weaponPos.Y),Target.X-(weaponPos.X));
			if (SpriteIndex == 0) {
				if (Aim > FMath.PI/2) {
					Aim = FMath.PI/2;
				}
				if (Aim < -FMath.PI/2) {
					Aim = -FMath.PI/2;
				}
			}else{
				if (Aim < FMath.PI/2 && Aim > 0) {
					Aim = FMath.PI/2;
				}
				if (Aim > -FMath.PI/2 && Aim < 0) {
					Aim = -FMath.PI/2;
				}
			}
			
			Weapon.Update(time,((SpriteIndex == 1)? Aim+FMath.PI:Aim), weaponPos,SpriteIndex);
			
			head.Position = new Vector3(
				ExtendArc(Sprite.Position.X,143.087f,Sprite.Rotation,0.03951f,SpriteIndex,true),
				ExtendArc(Sprite.Position.Y,143.087f,Sprite.Rotation,0.03951f,SpriteIndex,false),0);
			
			head.Rotation = ((SpriteIndex==1)?-Aim+FMath.PI+0.45f:-Aim-0.45f);
			
			base.Update (time);
		}
		
		public override void Render ()
		{
			//Weapon.Render();
			base.Render();
			head.SetTextureCoord(0,SpriteIndex*head.Height,head.Width,(SpriteIndex+1)*head.Height);
			head.Render();
		}
	}
}

