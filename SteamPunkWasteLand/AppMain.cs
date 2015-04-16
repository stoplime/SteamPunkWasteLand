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
using System.Diagnostics;
using System.Collections.Generic;
using Sce.PlayStation.Core;
using Sce.PlayStation.Core.Environment;
using Sce.PlayStation.Core.Graphics;
using Sce.PlayStation.Core.Input;

namespace SteamPunkWasteLand
{
	public class AppMain
	{
		
		public static void Main (string[] args)
		{
			Stopwatch s = new Stopwatch();
			s.Start();
			Initialize ();

			while (Game.Running) {
				SystemEvents.CheckEvents ();
				float time = s.ElapsedMilliseconds/1000f;
				s.Reset();
				s.Start();
				Update (time*Game.TimeSpeed);
				Render ();
				Console.WriteLine("FPS: "+(int)(1/time));
			}
		}

		public static void Initialize ()
		{
			Game.Graphics = new GraphicsContext ();
			Game.Running = true;
			Game.TimeSpeed = 1f;
			Game.Rand = new Random();
			Game.GameState = States.MainMenu;
			Game.Textures = new List<Texture2D>();
			InitTextures();
			
			Game.BgSky = new Background(Game.Textures[0]);
			Game.BgGround = new BackgroundGround(Game.Textures[1]);
			Game.BgCloud = new BackgroundClouds(Game.Textures[2]);
			
			Game.Player1 = new Player();
			Game.PBullets = new List<Bullet>();
			Game.ObtainedWeapons = new List<Weapon>();
			
			//test add
			Game.Loots = new List<Loot>();
			L_CrossBow l = new L_CrossBow(new Vector3(-200, 300,0));
			L_Cannon ll = new L_Cannon(new Vector3(200, 300,0));
			L_Flamethrower lll = new L_Flamethrower(new Vector3(0,600,0));
			Game.Loots.Add(l);
			Game.Loots.Add(ll);
			Game.Loots.Add(lll);
			
			Game.EBullets = new List<Bullet>();
			Game.Enemies = new List<Enemy>();
			E_Zeppelin g = new E_Zeppelin(new Vector3(500,300,0));
			//E_Dragon g = new E_Dragon(new Vector3(500,50,0));
			Game.Enemies.Add(g);
		}

		public static void InitTextures ()
		{
			Game.Textures.Add(new Texture2D("/Application/assets/Backgrounds/Sky2.png",false));		//0		Sky
			Game.Textures.Add(new Texture2D("/Application/assets/Backgrounds/Ground1.png",false));	//1		Ground
			Game.Textures.Add(new Texture2D("/Application/assets/Backgrounds/Cloud1.png",false));	//2		Clouds
			
			Game.Textures.Add(new Texture2D("/Application/assets/Player/Tophat_Sheet.png",false));	//3		Player
			Game.Textures.Add(new Texture2D("/Application/assets/Player/arm.png",false));			//4		Player arm
			
			Game.Textures.Add(new Texture2D("/Application/assets/Player/Tophat_Sheet.png",false));	//5		Zeppelin
			Game.Textures.Add(new Texture2D("/Application/assets/Enemies/Dragon.png",false));		//6		Dragon
			Game.Textures.Add(new Texture2D("/Application/assets/Player/Tophat_Sheet.png",false));	//7		Imperial Guards
			
			Game.Textures.Add(new Texture2D("/Application/assets/Weapons/Cannon.png",false));		//8		Cannon
			Game.Textures.Add(new Texture2D("/Application/assets/Weapons/Flamethrower.png",false));	//9		Flamethrower
			Game.Textures.Add(new Texture2D("/Application/assets/Weapons/Crossbow.png",false));		//10	Crossbow
			
			Game.Textures.Add(new Texture2D("/Application/assets/Weapons/CannonBall.png",false));	//11	Cannon Ball
			Game.Textures.Add(new Texture2D("/Application/assets/Weapons/Flame.png",false));		//12	Flame Particle
			Game.Textures.Add(new Texture2D("/Application/assets/Weapons/Arrow.png",false));		//13	Arrows
			
			Game.Textures.Add(new Texture2D("/Application/assets/Weapons/explosion.png",false));	//14	Explosions
			Game.Textures.Add(new Texture2D("/Application/assets/Enemies/DragonHead.png",false));	//15	DragonHead
		}

		public static void LootUpdate (float time)
		{
			for (int i = 0; i < Game.Loots.Count; i++) {
				Game.Loots[i].Update(time);
				if (Game.Loots[i].CheckPlayer()) {
					bool has = false;
					foreach(Weapon w in Game.ObtainedWeapons){
						if (w.Type == Game.Loots[i].Type) {
							has = true;
							//resupply ammo
						}
					}
					if (!has) {
						switch (Game.Loots[i].Type) {
						case WeaponType.CrossBow:
							Game.ObtainedWeapons.Add(new W_CrossBow());
							break;
						case WeaponType.Flamethrower:
							Game.ObtainedWeapons.Add(new W_Flamethrower());
							break;
						case WeaponType.Cannon:
							Game.ObtainedWeapons.Add(new W_Cannon());
							break;
						}
					}
				}
				if (Game.Loots[i].Despawn) {
					Game.Loots.RemoveAt(i);
				}
			}
		}

		public static void Update (float time)
		{
			var gamePadData = GamePad.GetData (0);
			if ((gamePadData.Buttons & GamePadButtons.Select) != 0) {
				Game.Running = false;
			}
			Game.Player1.Update(gamePadData,time);
			WorldCoord.FocusObject = Game.Player1.WorldPos;
			
			float pDistSq = Game.Player1.HitRadius*Game.Player1.HitRadius;
			for (int i = 0; i < Game.EBullets.Count; i++) {
				if (!Game.EBullets[i].Hit) {
					if ((Game.Player1.WorldPos).DistanceSquared(Game.EBullets[i].Pos) < pDistSq) 
					{
						Game.EBullets[i].CollideWithPlayer();
						Game.Player1.CollideWithB(Game.EBullets[i]);
					}
				}
			}
			
			for (int i = 0; i < Game.Enemies.Count; i++) {
				Game.Enemies[i].Update(time);
				//bullet collision
				for (int j = 0; j < Game.PBullets.Count; j++) {
					if (!Game.PBullets[j].Hit) {
						float dist = (Game.Enemies[i].Sprite.Width+Game.Enemies[i].Sprite.Height)/4f;
						float distSq = dist * dist;
						if ((Game.Enemies[i].Pos+
						     new Vector3(0,Game.Enemies[i].Sprite.Height/2,0)
						     ).DistanceSquared(Game.PBullets[j].Pos) < distSq) 
						{
							//hit
							Game.Enemies[i].CollideWithB(Game.PBullets[j]);
							Game.PBullets[j].CollideWithE(Game.Enemies[i]);
						}
					}
				}
			}
			
			for (int i = 0; i < Game.EBullets.Count; i++) {
				Game.EBullets[i].Update(time);
				if (Game.EBullets[i].Despawn) {
					Game.EBullets.RemoveAt(i);
				}
			}
			
			for (int i = 0; i < Game.PBullets.Count; i++) {
				Game.PBullets[i].Update(time);
				if (Game.PBullets[i].Despawn) {
					Game.PBullets.RemoveAt(i);
				}
			}
			
			LootUpdate(time);
			
			
			WorldCoord.UpdateFocus(time);
			Game.BgCloud.Update();
			Game.BgGround.Update();
		}

		public static void Render ()
		{
			Game.Graphics.SetClearColor (0.0f, 0.0f, 0.0f, 0.0f);
			Game.Graphics.Clear ();
			
			Game.BgSky.Render();
			Game.BgGround.Render();
			Game.BgCloud.Render();
			
			Game.Player1.Render();
			
			foreach(Enemy e in Game.Enemies){
				e.Render();
			}
			
			foreach(Bullet b in Game.PBullets){
				b.Render();
			}
			
			foreach(Bullet b in Game.EBullets){
				b.Render();
			}
			
			foreach(Loot l in Game.Loots){
				l.Render();
			}
			
			Game.Graphics.SwapBuffers ();
		}
	}
}
