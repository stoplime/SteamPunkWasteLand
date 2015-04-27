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
	public class E_AirShip : Enemy
	{
		private List<Weapon> weapons;
		private List<float> fireSpeeds;
		private List<float> deltaTimes;
		private int weaponIndex;
		private float speed;
		private float downCount;
		
		private float guardSpawnTime;
		
		public E_AirShip (Vector3 initPos)
			:base(initPos)
		{
			MaxHp = 1600+50*Game.Level;
			Hp = MaxHp;
			weaponIndex = 0;
			guardSpawnTime = (float)Game.Rand.NextDouble()*5f+10f;
			weapons = new List<Weapon>();
			fireSpeeds = new List<float>();
			deltaTimes = new List<float>();
			
			weapons.Add(new W_CrossBow(true));
			weapons.Add(new W_CrossBow(true));
			
			weapons.Add(new W_Cannon(true));
			weapons.Add(new W_Cannon(true));
			weapons.Add(new W_Cannon(true));
			
			for (int i = 0; i < 5; i++) {
				fireSpeeds.Add((float)(Game.Rand.NextDouble()*5+2));
				deltaTimes.Add(0);
			}
			
			Weapon = weapons[weaponIndex];
			
			downCount = 0;
			
			Sprite = new Sprite(Game.Graphics,Game.Textures[26],512,350);
			Sprite.Center = new Vector2(0.5f,0.7843f);
			Sprite.Position = worldToSprite();
			HitRadius = (Sprite.Width+Sprite.Height)/4f;
			HpOffset = true;
			MoneyLoot = Game.Rand.Next(2000,4001);
			Score = MoneyLoot;
			
			FireSpeed = 2f;
			speed = 50f;
		}
		
		public override void animateDeath (float time)
		{
			base.animateDeath (time);
			
			float angle = FMath.Atan2(Vel.Y,Vel.X);
			Sprite.Rotation = (angle-Sprite.Rotation)/20f*(SpriteIndex==0?-1:1);
			
			if(Pos.Y <= -100 && Pos.Y > -500){
				WorldCoord.EarthQuake(time);
			}
		}
		
		public Vector3 GetWeaponPos (int index, float angle)
		{
			float extend = 0;
			float phi = 0;
			switch (index) {
			case 0://Left crossbow
				extend = 80.5f;
				phi = FMath.PI;
				break;
			case 1://Right crossbow
				extend = 80.5f;
				phi = 0;
				break;
			case 2://Left Cannon
				extend = 140.88f;
				phi = -3.467f;
				break;
			case 3://Middle Cannon
				extend = 53;
				phi = FMath.PI/2f;
				break;
			case 4://Right Cannon
				extend = 147.36f;
				phi = 0.3679f;
				break;
			default:
				break;
			}
			return new Vector3(
				ExtendArc(Pos.X,extend,angle,phi,SpriteIndex,true),
				ExtendArc(Pos.Y+Sprite.Height/2f,extend,angle,phi,SpriteIndex,false),0);
		}
		
		public override void Update (float time)
		{
			guardSpawnTime -= time;
			
			for (int i = 0; i < deltaTimes.Count; i++) {
				deltaTimes[i] += time;
			}
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
			
			for (int i = 0; i < weapons.Count; i++) {
				Vector3 weaponPos = GetWeaponPos(i,-Sprite.Rotation);
				float aim = FMath.Atan2(Target.Y-weaponPos.Y,Target.X-weaponPos.X);
				aim += 0.0005f*(Target.X-Pos.X);
				weapons[i].Update(time,(SpriteIndex==0?aim:aim+FMath.PI),weaponPos,SpriteIndex,true);
			}
			
			if (FMath.Abs(Pos.X-Target.X) < Game.Graphics.Screen.Width/2f) {
				Firing = true;
			}else{
				Firing = false;
			}
			
			for (int i = 0; i < 4; i++) {
				DeltaTime = deltaTimes[i];
				FireSpeed = fireSpeeds[i];
				FireMethod();
				deltaTimes[i] = DeltaTime;
				Weapon = weapons[(++weaponIndex)];
			}
			
			//Spawn guards
			if (guardSpawnTime <= 0) {
				guardSpawnTime = (float)Game.Rand.NextDouble()*5f+10f;
				E_Guard g = new E_Guard(Pos);
				Game.Enemies.Add(g);
			}
			
			base.Update (time);
			
			weaponIndex = 0;
			Weapon = weapons[weaponIndex];
			
		}
		
		public override void Render ()
		{
			base.Render ();
			if(Hp > 0){
				foreach (Weapon w in weapons) {
					w.Render();
				}
			}
		}
	}
}

