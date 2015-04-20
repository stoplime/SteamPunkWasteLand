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
	public abstract class Enemy
	{
		#region Private Fields
		private const float HitDelay = 0.5f;
		private float hitTime;
		private Sprite hpSprite;
		private Sprite hpBoxSprite;
		#endregion
		
		#region Properties
		private Sprite sprite;
		public Sprite Sprite
		{
			get{return sprite;}
			set{sprite = value;}
		}
		private Vector3 pos;
		public Vector3 Pos
		{
			get{return pos;}
			set{pos = value;}
		}
		private float hp;
		public float Hp
		{
			get{return hp;}
			set{hp = value;}
		}
		private float hitRadius;
		public float HitRadius
		{
			get{return hitRadius;}
			set{hitRadius = value;}
		}
		private int score;
		public int Score
		{
			get{return score;}
			set{score = value;}
		}
		
		//protected
		private float maxHp;
		protected float MaxHp
		{
			get{return maxHp;}
			set{maxHp = value;}
		}
		private int spriteIndex;
		protected int SpriteIndex
		{
			get{return spriteIndex;}
			set{spriteIndex = value;}
		}
		private Weapon weapon;
		protected Weapon Weapon
		{
			get{return weapon;}
			set{weapon = value;}
		}
		private Vector3 vel;
		protected Vector3 Vel
		{
			get{return vel;}
			set{vel = value;}
		}
		private Vector3 target;
		protected Vector3 Target
		{
			get{return target;}
			set{target = value;}
		}
		private float deltaTime;
		protected float DeltaTime
		{
			get{return deltaTime;}
			set{deltaTime = value;}
		}
		private float fireSpeed;
		protected float FireSpeed
		{
			get{return fireSpeed;}
			set{fireSpeed = value;}
		}
		private bool firing;
		protected bool Firing
		{
			get{return firing;}
			set{firing = value;}
		}
		private float aim;
		protected float Aim
		{
			get{return aim;}
			set{aim = value;}
		}
		private int moneyLoot;
		protected int MoneyLoot
		{
			get{return moneyLoot;}
			set{moneyLoot = value;}
		}
		
		
		#endregion
		
		#region Constructor and Init
		public Enemy (Vector3 initPos)
		{
			pos = initPos;
			vel = Vector3.Zero;
			deltaTime = 0;
			spriteIndex = 0;
			firing = false;
			hitTime = HitDelay;
			
			hpSprite = new Sprite(Game.Graphics,Game.Textures[16]);
			hpBoxSprite = new Sprite(Game.Graphics,Game.Textures[16]);
			hpBoxSprite.SetColor(0,0,0,0.5f);
		}
		#endregion
		
		#region Additional Methods
		public Vector3 worldToSprite ()
		{
			return WorldCoord.WorldToView(new Vector3(pos.X,pos.Y+sprite.Height/2,0));
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
		
		protected virtual void Physics (float time)
		{
			vel.Y -= 9.8f*time;
			
			//friction
//			if (pos.Y > 0) {
//				vel.X *= FMath.Pow(0.2f,time);
//			}else{
//				vel.X *= FMath.Pow(0.001f,time);
//			}
//			if (FMath.Abs(vel.X) < 0.05f) {
//				vel.X = 0;
//			}
			
			//ground level
			if(pos.Y < 0){
				vel.Y = 0;
				pos.Y = 0;
			}
		}
		
		public virtual void CollideWithB (Bullet b)
		{
			hp -= b.Damage * Game.Player1.DamgeMultiplier;
			hitTime = 0;
			if (pos.Y < 1f) {
				vel.Y = 4f;
			}
		}
		
		public virtual void FireMethod ()
		{
			if (deltaTime > fireSpeed) {
				deltaTime = 0;
				Game.EBullets.Add(weapon.Fire(vel));
			}
		}
		
		public void HpDisp (float hpMax, float hp, Vector3 pos, float width, float height, int offsetY)
		{
			hpSprite.Width = ((float)hp/hpMax)*width;
			hpSprite.Height = height;
			if (hp < (float)hpMax/5) {
				hpSprite.SetColor(1f,0f,0f,1f);
			}else if (hp < (float)hpMax/2) {
				hpSprite.SetColor(1f,0.5f,0f,1f);
			}else{
				hpSprite.SetColor(0f,1f,0f,1f);
			}
			hpSprite.Position = pos;
			hpSprite.Position.Y -= offsetY;
			
			hpBoxSprite.Width = width+4;
			hpBoxSprite.Height = height+4;
			hpBoxSprite.Position = hpSprite.Position;
			hpBoxSprite.Position.X -= 2;
			hpBoxSprite.Position.Y -= 2;
		}

		public virtual void animateDeath ()
		{
			sprite.Rotation = FMath.PI/2f;
			
		}
		
		public bool DeathUpdate (float time)
		{
			if (hp <= 0) {
				if (moneyLoot <= 0) {
					return true;
				}else{
					for (int i = 0; i < 10; i++) {
						if (moneyLoot > 0) {
							moneyLoot--;
							Coins c = new Coins(pos);
							Game.AnimatedMoney.Add(c);
						}
					}
					//animate death
					animateDeath();
				}
			}
			return false;
		}
		
		#endregion
		
		#region Original Methods
		public virtual void Update(float time)
		{
			deltaTime += time;
			hitTime += time;
			target = Game.Player1.WorldPos;
			aim = FMath.Atan2(target.Y-pos.Y,target.X-pos.X);
			
			
			//aim += deviation*FMath.Sin(FMath.PI*2*(float)Game.Rand.NextDouble());
			
			if(firing){
				FireMethod();
			}
			
			pos += vel * Game.TimeSpeed;
			
			sprite.Position = worldToSprite();
			
			HpDisp(maxHp,hp,new Vector3(sprite.Position.X-sprite.Width/2,sprite.Position.Y,0),sprite.Width,0.05f*sprite.Height,(int)(sprite.Height/2));
		}
		
		public virtual void Render()
		{
			sprite.SetTextureCoord(0,spriteIndex*sprite.Height,sprite.Width,(spriteIndex+1)*sprite.Height);
			if (hitTime < HitDelay) {
				sprite.SetColor(1,0.5f,0.5f,1);
			}else{
				sprite.SetColor(1,1,1,1);
			}
			sprite.Render();
			hpBoxSprite.Render();
			hpSprite.Render();
		}
		#endregion
	}
}

