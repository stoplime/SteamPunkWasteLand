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
	public class B_CannonBall : Bullet
	{
		private float explodeDelay;
		private Sprite explode;
		private bool preHit;
		private float explodeRadius;
		private bool isEnemy;
		
		public B_CannonBall (float direction, float speed, Vector3 initPos)
			:this(direction,speed,initPos,0,0)
		{
		}

		public B_CannonBall (float direction, float speed, Vector3 initPos, int spriteIndex)
			:this(direction,speed,initPos,0,0)
		{
		}

		public B_CannonBall (float direction, float speed, Vector3 initPos, int spriteIndex, float diameter)
			:base(direction,speed,initPos,spriteIndex)
		{
			explodeDelay = 0.5f;//1sec
			spriteIndex = 0;
			preHit = false;
			
			Sprite = new Sprite (Game.Graphics, Game.Textures [11], 9, 9);
			Sprite.Position = worldToSprite ();
			Sprite.Rotation = direction;
			Sprite.Center = new Vector2 (0.5f, 0.5f);
			
			explode = new Sprite (Game.Graphics, Game.Textures [14], 32, 32);
			explode.Center = new Vector2 (0.5f, 0);
			explode.Position = Sprite.Position;
			if (diameter > 10) {
				explode.Scale = new Vector2 (1, 1) * (diameter / 32f);
				explodeRadius = diameter / 2f;
				isEnemy = true;
			} else {
				explode.Scale = new Vector2 (1, 1) * (Game.Upgrades [1, 4] / 32f);
				explodeRadius = Game.Upgrades [1, 4] / 2f;
				isEnemy = false;
			}
			Radius = Sprite.Width / 2f;
			
			Damage = 10;
		}
		
		public override void Update (float time)
		{
			//gravity
			float Vy = Speed * FMath.Sin (Dir) + 500f * time * (SpriteIndex == 0 ? 1 : -1);
			float Vx = Speed * FMath.Cos (Dir);
			Speed = FMath.Sqrt (Vy * Vy + Vx * Vx);
			Dir = FMath.Atan2 (Vy, Vx);
			
			//ground
			if (Pos.Y < -Sprite.Width / 2f) {
				Hit = true;
			}
			
			//explode
			if (Hit) {
				explode.Position = Sprite.Position;
				Sprite = explode;
				explodeDelay -= time;
			}
			
			base.Update (time);
			
			//despawn
			if (explodeDelay < 0) {
				Despawn = true;
			}
			
			if (Hit && !preHit) {
				preHit = true;
				if (!isEnemy) {
					for (int i = 0; i < Game.Enemies.Count; i++) {
						if(HitObj != null && HitObj != Game.Enemies[i]){
							float dist = Game.Enemies [i].HitRadius + explodeRadius;
							float distSq = dist * dist;
							if ((Game.Enemies [i].Pos +
						     new Vector3 (0, Game.Enemies [i].Sprite.Height / 2f, 0)
						     ).DistanceSquared (Pos) < distSq) {
								//hit
								Game.Enemies [i].CollideWithB (this);
							}
						}
					}
				} else {
					float distP = Game.Player1.HitRadius + explodeRadius;
					float distSqP = distP * distP;
					if (Game.Player1.WorldPos.DistanceSquared (Pos) < distSqP) {
						Game.Player1.CollideWithB (this);
					}
				}
			}
		}

		public override void Render ()
		{
			if (Hit) {
				//animate explosion
				Sprite.SetTextureUV ((5 - FMath.Floor (explodeDelay * 10)) / 5f, 0, (6 - FMath.Floor (explodeDelay * 10)) / 5f, 1f);
			}
			Sprite.Render ();
		}
	}
}

