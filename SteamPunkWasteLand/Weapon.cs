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
	public abstract class Weapon
	{
		#region Private Fields
		private const float phi = 0.588f;
		private const float ArmLenght = 15f;
		
		#endregion
		
		#region Properties
		private Sprite sprite;
		protected Sprite Sprite
		{
			get{return sprite;}
			set{sprite = value;}
		}
		private Vector3 pos;
		protected Vector3 Pos
		{
			get{return pos;}
			set{pos = value;}
		}
		private float aim;
		protected float Aim
		{
			get{return aim;}
			set{aim = value;}
		}
		private int spriteIndex;
		protected int SpriteIndex
		{
			get{return spriteIndex;}
			set{spriteIndex = value;}
		}
		private float fireSpd;
		protected float FireSpd
		{
			get{return fireSpd;}
			set{fireSpd = value;}
		}
		private float deltaTime;
		protected float DeltaTime
		{
			get{return deltaTime;}
			set{deltaTime = value;}
		}
		private WeaponType type;
		public WeaponType Type
		{
			get{return type;}
			set{type = value;}
		}
		#endregion
		
		#region Constructor
		public Weapon ()
		{
			pos = new Vector3();
			aim = 0;
			spriteIndex = 0;
			deltaTime = 10;
			
		}
		#endregion
		
		#region Additional Methods
		protected Vector3 worldToSprite ()
		{
			return WorldCoord.WorldToView(new Vector3(pos.X,pos.Y,0));
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
		
		public abstract Bullet Fire (Vector3 vel);
		
		public int Delay ()
		{
			return (int)FMath.Floor(deltaTime/fireSpd);
		}
		
		private void adjustSholder(float aim, Vector3 sholderPos)
		{
			pos.X = ExtendArc(sholderPos.X,ArmLenght,aim,phi,spriteIndex,true);
			pos.Y = ExtendArc(sholderPos.Y,ArmLenght,aim,phi,spriteIndex,false);
		}
		
		#endregion
		
		#region Original Methods
		//for the player
		public virtual void Update (float time, float aim, Vector3 sholderPos, int index)
		{
			this.Update(time,aim,sholderPos,index,false);
		}
		
		public virtual void Update (float time, float aim, Vector3 sholderPos, int index, bool isEnemy)
		{
			deltaTime += time;
			this.aim = aim;
			spriteIndex = index;
			
			if(!isEnemy){
				adjustSholder(aim,sholderPos);
			}else{
				pos = sholderPos;
			}
			
			sprite.Position = worldToSprite();
			sprite.Rotation = -aim;
		}
		
		public virtual void Render ()
		{
			sprite.SetTextureCoord(0,sprite.Height*spriteIndex,sprite.Width,(spriteIndex+1)*sprite.Height);
			sprite.Render();
		}
		#endregion
	}
}

