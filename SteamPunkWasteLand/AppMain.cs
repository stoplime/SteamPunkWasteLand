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
using System.Diagnostics;
using System.Collections.Generic;
using System.IO;
using Sce.PlayStation.Core;
using Sce.PlayStation.Core.Environment;
using Sce.PlayStation.Core.Graphics;
using Sce.PlayStation.Core.Input;
using Sce.PlayStation.HighLevel.UI;

namespace SteamPunkWasteLand
{
	public class AppMain
	{
		
		public static void Main (string[] args)
		{
			Stopwatch s = new Stopwatch ();
			s.Start ();
			Initialize ();

			while (Game.Running) {
				SystemEvents.CheckEvents ();
				float time = s.ElapsedMilliseconds / 1000f;
				s.Reset ();
				s.Start ();
				Update (time * Game.TimeSpeed);
				Render ();
				Console.WriteLine("FPS: "+(int)(1/time));
			}
		}
		
		#region Main Methods
		public static void Initialize ()
		{
			Game.Graphics = new GraphicsContext ();
			UISystem.Initialize (Game.Graphics);
			Game.Running = true;
			Game.Rand = new Random ();
			Game.GameState = States.MainMenu;
			Game.Music = 1f;
			Game.Sound = 1f;
			Game.MusicBox = new MusicBox();
			Game.Textures = new List<Texture2D> ();
			Game.HighScores = new List<HighScore> ();
			InitTextures ();
			InitHighScores ();
			
			NewMenu ();
		}

		public static void Update (float time)
		{
			var gamePadData = GamePad.GetData (0);
			
			Game.MusicBox.Update();
			switch (Game.GameState) {
			case States.MainMenu:
				MenuUpdate (gamePadData);
				break;
			case States.Play:
				InGameUpdate (time, gamePadData);
				break;
			case States.HighScore:
				HighScoreUpdate (gamePadData);
				break;
			case States.Name:
				NamingUpdate (gamePadData);
				break;
			default:
				break;
			}
			
		}
		
		public static void Render ()
		{
			Game.Graphics.SetClearColor (0.0f, 0.0f, 0.0f, 0.0f);
			Game.Graphics.Clear ();
			
			switch (Game.GameState) {
			case States.MainMenu:
				MenuRender ();
				break;
			case States.Play:
				InGameRender ();
				break;
			case States.HighScore:
				HighScoreRender ();
				break;
			case States.Name:
				NamingRender ();
				break;
			default:
				break;
			}
			
			Game.Graphics.SwapBuffers ();
		}
		#endregion
		
		#region Instanciate States
		public static void NewMenu ()
		{
			Game.BgMenu = new Background (Game.Textures [0]);
			
			float X = Game.Graphics.Screen.Width / 2f;
			float Y = Game.Graphics.Screen.Height / 2f;
			
			Game.MenuButtons = new ButtonSet (Game.Textures [22], new Vector2 (200, 75));
			Game.MenuButtons.AddButton (new Vector3 (X, Y, 0));				//play
			Game.MenuButtons.AddButton (new Vector3 (X, Y + 100, 0));		//high score
			Game.MenuButtons.AddButton (new Vector3 (X, Y + 200, 0));		//quit
		}
		
		public static void NewGame ()
		{
			Game.TimeSpeed = 1f;
			Game.Level = 0;
			Game.Score = 0;
			Game.Money = 100000;//TODO: fix this
			
			Game.BgSky = new Background (Game.Textures [0]);
			Game.BgGround = new BackgroundGround (Game.Textures [1]);
			Game.BgCloud = new BackgroundClouds (Game.Textures [2]);
			
			Game.Upgrades = new float[3, 5];
			for (int i = 0; i < 5; i++) {
				Game.Upgrades [0, i] = 0;
			}
			Game.Upgrades [1, 0] = 20;
			Game.Upgrades [1, 1] = 1;
			Game.Upgrades [1, 2] = 0.5f;
			Game.Upgrades [1, 3] = 0.1f;
			Game.Upgrades [1, 4] = 50;
			
			Game.Upgrades [2, 0] = 300;
			Game.Upgrades [2, 1] = 1000;
			Game.Upgrades [2, 2] = 500;
			Game.Upgrades [2, 3] = 500;
			Game.Upgrades [2, 4] = 500;
			
			Game.PBullets = new List<Bullet> ();
			Game.ObtainedWeapons = new List<Weapon> ();
			Game.Enemies = new List<Enemy> ();
			Game.EBullets = new List<Bullet> ();
			Game.Loots = new List<Loot> ();
			Game.AnimatedMoney = new List<Coins> ();
			
			Game.Pause = new Pause ();
			
			NextLevel ();
		}

		public static void NewHighScore ()
		{
			Game.BgMenu = new Background (Game.Textures [0]);
			Game.HSD = new HighScoresDisplay ();
		}
		
		public static void NewName ()
		{
			Game.BgMenu = new Background (Game.Textures [0]);
			Game.NameDisplay = new EnterNameDisplay ();
		}
		#endregion
		
		#region Other Methods
		public static void InitTextures ()
		{
			Game.Textures.Add (new Texture2D ("/Application/assets/Backgrounds/Sky1.png", false));				//0		Sky
			Game.Textures.Add (new Texture2D ("/Application/assets/Backgrounds/Ground1.png", false));			//1		Ground
			Game.Textures.Add (new Texture2D ("/Application/assets/Backgrounds/Cloud1.png", false));			//2		Clouds
			
			Game.Textures.Add (new Texture2D ("/Application/assets/Player/Tophat_Sheet.png", false));			//3		Player
			Game.Textures.Add (new Texture2D ("/Application/assets/Player/arm.png", false));					//4		Player arm
			
			Game.Textures.Add (new Texture2D ("/Application/assets/Enemies/Zeppelin.png", false));				//5		Zeppelin
			Game.Textures.Add (new Texture2D ("/Application/assets/Enemies/Dragon.png", false));				//6		Dragon
			Game.Textures.Add (new Texture2D ("/Application/assets/Enemies/Guard.png", false));					//7		Imperial Guards
			
			Game.Textures.Add (new Texture2D ("/Application/assets/Weapons/Cannon.png", false));				//8		Cannon
			Game.Textures.Add (new Texture2D ("/Application/assets/Weapons/Flamethrower.png", false));			//9		Flamethrower
			Game.Textures.Add (new Texture2D ("/Application/assets/Weapons/Crossbow.png", false));				//10	Crossbow
			
			Game.Textures.Add (new Texture2D ("/Application/assets/Weapons/CannonBall.png", false));			//11	Cannon Ball
			Game.Textures.Add (new Texture2D ("/Application/assets/Weapons/Flame.png", false));					//12	Flame Particle
			Game.Textures.Add (new Texture2D ("/Application/assets/Weapons/Arrow.png", false));					//13	Arrows
			
			Game.Textures.Add (new Texture2D ("/Application/assets/Weapons/explosion.png", false));				//14	Explosions
			Game.Textures.Add (new Texture2D ("/Application/assets/Enemies/DragonHead.png", false));			//15	DragonHead
			Game.Textures.Add (new Texture2D ("/Application/assets/Other/White.png", false));					//16	white pixel
			
			Game.Textures.Add (new Texture2D ("/Application/assets/Other/BetterGear.png", false));				//17	HUD gear
			Game.Textures.Add (new Texture2D ("/Application/assets/Other/hpTube.png", false));					//18	HP Tube
			Game.Textures.Add (new Texture2D ("/Application/assets/Other/Coin.png", false));					//19	Coin
			
			Game.Textures.Add (new Texture2D ("/Application/assets/Backgrounds/Sky2.png", false));				//20	Sky2
			Game.Textures.Add (new Texture2D ("/Application/assets/Backgrounds/Cloud2.png", false));			//21	Cloud2
			
			Game.Textures.Add (new Texture2D ("/Application/assets/Menu/MenuButtonsSheet.png", false));			//22	Menu Buttons
			Game.Textures.Add (new Texture2D ("/Application/assets/Menu/ABC1.png", false));						//23	ABC Buttons 1
			Game.Textures.Add (new Texture2D ("/Application/assets/Menu/ABC2.png", false));						//24	ABC Buttons 2
			Game.Textures.Add (new Texture2D ("/Application/assets/Menu/EnterKeys.png", false));				//25	Extra Keys
			
			Game.Textures.Add (new Texture2D ("/Application/assets/Enemies/AirShip.png", false));				//26	AirShip
			Game.Textures.Add (new Texture2D ("/Application/assets/Enemies/E_CrossBow.png", false));			//27	E_CrossBow
			Game.Textures.Add (new Texture2D ("/Application/assets/Enemies/E_Cannon.png", false));				//28	E_cannon
			Game.Textures.Add (new Texture2D ("/Application/assets/Enemies/GuardArms.png", false));				//29	Guard Arms
			
			Game.Textures.Add (new Texture2D ("/Application/assets/PauseMenu/SettingsBackground.png", false));	//30	Pause settings backgound
			Game.Textures.Add (new Texture2D ("/Application/assets/PauseMenu/ShopBackground.png", false));		//31	Pause shop backgound
			Game.Textures.Add (new Texture2D ("/Application/assets/PauseMenu/PauseTab.png", false));			//32	Pause settings and shop tabs
			
			Game.Textures.Add (new Texture2D ("/Application/assets/PauseMenu/SettingsButtons.png", false));		//33	Resume / Exit
			Game.Textures.Add (new Texture2D ("/Application/assets/PauseMenu/Slider.png", false));				//34	slider
			Game.Textures.Add (new Texture2D ("/Application/assets/PauseMenu/Icons.png", false));				//35	Upgrade Icons
		}

		public static void InitHighScores ()
		{
			StreamReader sr = null;
			try {
				sr = new StreamReader ("Documents/highscores.txt");
				for (int i = 0; i < 10; i++) {
					string name = "---";
					string score = "--";
					string[] line = sr.ReadLine ().Split (',');
					if (line.Length == 2 && (line [0] != "" || line [1] != "")) {
						name = line [0];
						score = line [1];
					}
					Game.HighScores.Add (new HighScore (name, score));
				}
			} catch (FileNotFoundException) {
				File.CreateText ("Documents/highscores.txt");
				StreamWriter sw = new StreamWriter ("Documents/highscores.txt");
				for (int i = 0; i < 10; i++) {
					sw.WriteLine ("---,--");
					Game.HighScores.Add (new HighScore ("---", "--"));
				}
				sw.Close ();
			} finally {
				if (sr != null) {
					sr.Close ();
				}
			}
		}
		#endregion
		
		#region InGame Methods
		public static void LootUpdate (float time)
		{
			for (int i = 0; i < Game.Loots.Count; i++) {
				Game.Loots [i].Update (time);
				if (Game.Loots [i].CheckPlayer ()) {
					bool has = false;
					foreach (Weapon w in Game.ObtainedWeapons) {
						if (w.Type == Game.Loots [i].Type) {
							has = true;
							//resupply ammo
						}
					}
					if (!has) {
						switch (Game.Loots [i].Type) {
						case WeaponType.CrossBow:
							Game.ObtainedWeapons.Add (new W_CrossBow ());
							break;
						case WeaponType.Flamethrower:
							Game.ObtainedWeapons.Add (new W_Flamethrower ());
							break;
						case WeaponType.Cannon:
							Game.ObtainedWeapons.Add (new W_Cannon ());
							break;
						}
					}
				}
				if (Game.Loots [i].Despawn) {
					Game.Loots.RemoveAt (i);
				}
			}
		}
		
		public static void NextLevel ()
		{
			Game.Level++;
			Game.Player1 = new Player ();
			Game.Spawner = new Spawner ();
			Game.EBullets.Clear ();
			Game.PBullets.Clear ();
			Game.LevelFinished = false;
			Game.hud = new HUD ();
			
			if (Game.Level % 2 == 0) {
				Game.BgSky = new Background (Game.Textures [20]);
			} else {
				Game.BgSky = new Background (Game.Textures [0]);
			}
			if (Game.Level / 3 % 2 == 0) {
				Game.BgCloud = new BackgroundClouds (Game.Textures [2]);
			} else {
				Game.BgCloud = new BackgroundClouds (Game.Textures [21]);
			}
		}
		
		public static void InGameUpdate (float time, GamePadData gamePadData)
		{
			Game.hud.Update (time);
			
			if (!Game.Pause.IsPause) {
				//*************************
				if ((gamePadData.Buttons & GamePadButtons.Select) != 0 && (gamePadData.ButtonsPrev & GamePadButtons.Select) == 0) {
					Game.Pause.IsPause = true;
				}
				
				Game.LevelFinished = Game.Spawner.Update (time);//returns true once finished spawning
			
				if (Game.LevelFinished) {
					if (Game.Enemies.Count > 0) {
						Game.LevelFinished = false;
					}
				}
			
				Game.Player1.Update (gamePadData, time);
				WorldCoord.FocusObject = Game.Player1.WorldPos;
			
				if (Game.Player1.Hp <= 0) {
					if (Game.Score > Game.HighScores [Game.HighScores.Count - 1].Score) {
						Game.GameState = States.Name;
						NewName ();
					} else {
						Game.GameState = States.HighScore;
						NewHighScore ();
					}
					PlayDispose ();
					return;
				}
			
				float pDistSq = Game.Player1.HitRadius * Game.Player1.HitRadius;
				for (int i = 0; i < Game.EBullets.Count; i++) {
					if (!Game.EBullets [i].Hit) {
						if ((Game.Player1.WorldPos).DistanceSquared (Game.EBullets [i].Pos) < pDistSq) {
							Game.EBullets [i].CollideWithPlayer ();
							Game.Player1.CollideWithB (Game.EBullets [i]);
						}
					}
				}
			
				for (int i = 0; i < Game.Enemies.Count; i++) {
					if (Game.Enemies [i].Hp > 0) {
						Game.Enemies [i].Update (time);
					} else if (Game.Enemies [i].DeathUpdate (time)) {
						Game.Score += Game.Enemies [i].Score;
						Game.Enemies.RemoveAt (i);
						break;
					}
					//bullet collision
					for (int j = 0; j < Game.PBullets.Count; j++) {
						if (!Game.PBullets [j].Hit) {
							if (!Object.ReferenceEquals (Game.Enemies [i].GetType (), typeof(E_AirShip))) {
								float dist = Game.Enemies [i].HitRadius;
								float distSq = dist * dist;
								if ((Game.Enemies [i].Pos +
							     new Vector3 (0, Game.Enemies [i].Sprite.Height / 2f, 0)
							     ).DistanceSquared (Game.PBullets [j].Pos) < distSq) {
									//hit
									Game.Enemies [i].CollideWithB (Game.PBullets [j]);
									Game.PBullets [j].CollideWithE (Game.Enemies [i]);
								}
							} else {
								float Ex = Game.Enemies [i].Pos.X;
								float Ey = Game.Enemies [i].Pos.Y + Game.Enemies [i].Sprite.Height / 2f;
								Vector3 topLeftBound = new Vector3 (Ex - 221, Ey - 45.5f, 0);
								Vector3 bottomRightBound = new Vector3 (Ex + 221, Ey + 266.5f, 0);
							
								float Bx = Game.PBullets [j].Pos.X;
								float By = Game.PBullets [j].Pos.Y;
								if (Bx > topLeftBound.X && Bx < bottomRightBound.X) {
									if (By < bottomRightBound.Y && By > topLeftBound.Y) {
										//hit
										Game.Enemies [i].CollideWithB (Game.PBullets [j]);
										Game.PBullets [j].CollideWithE (Game.Enemies [i]);
									}
								}
							}
						}
					}
				}
			
				for (int i = 0; i < Game.EBullets.Count; i++) {
					Game.EBullets [i].Update (time);
					if (Game.EBullets [i].Despawn) {
						Game.EBullets.RemoveAt (i);
					}
				}
			
				for (int i = 0; i < Game.PBullets.Count; i++) {
					Game.PBullets [i].Update (time);
					if (Game.PBullets [i].Despawn) {
						Game.PBullets.RemoveAt (i);
					}
				}
			
				for (int i = 0; i < Game.AnimatedMoney.Count; i++) {
					Game.AnimatedMoney [i].Update (time);
					if (Game.AnimatedMoney [i].Despawn) {
						Game.AnimatedMoney.RemoveAt (i);
						Game.Money++;
					}
				}
			
				if (Game.LevelFinished) {
					if ((gamePadData.Buttons & GamePadButtons.Start) != 0 && (gamePadData.ButtonsPrev & GamePadButtons.Start) == 0) {
						NextLevel ();
					}
				}
				LootUpdate (time);
				Game.BgCloud.Update ();
				Game.BgGround.Update ();
				//***************************
			} else {
				Game.Pause.Update (gamePadData);
			}
			
			
			
			WorldCoord.UpdateFocus (time);
		}
		
		public static void InGameRender ()
		{
			Game.BgSky.Render ();
			Game.BgCloud.Render ();
			
			foreach (Enemy e in Game.Enemies) {
				e.Render ();
			}
			
			Game.Player1.Render ();
			
			foreach (Bullet b in Game.PBullets) {
				b.Render ();
			}
			
			foreach (Bullet b in Game.EBullets) {
				b.Render ();
			}
			
			foreach (Loot l in Game.Loots) {
				l.Render ();
			}
			
			Game.BgGround.Render ();
			
			foreach (Coins c in Game.AnimatedMoney) {
				c.Render ();
			}
			
			Game.hud.Render ();
			
			if (Game.Pause.IsPause) {
				Game.Pause.Render ();
			}
		}
		
		public static void PlayDispose ()
		{
			Game.hud = null;
			Game.BgSky = null;
			Game.BgGround = null;
			Game.BgCloud = null;
			
			Game.Player1 = null;
			Game.Spawner = null;
			
			Game.Loots.Clear ();
			Game.Enemies.Clear ();
			Game.PBullets.Clear ();
			Game.EBullets.Clear ();
			Game.ObtainedWeapons.Clear ();
			
			for (int i = Game.AnimatedMoney.Count-1; i >= 0; i--) {
				Game.Money++;
				Game.AnimatedMoney.RemoveAt (i);
			}
		}
		#endregion
		
		#region Main Menu Methods
		public static void MenuUpdate (GamePadData gamePadData)
		{
			int selection = -1;
			
			//selection movement
			if ((gamePadData.Buttons & GamePadButtons.Up) != 0 && (gamePadData.ButtonsPrev & GamePadButtons.Up) == 0) {
				Game.MenuButtons.SelectPrevious ();
			}
			if ((gamePadData.Buttons & GamePadButtons.Down) != 0 && (gamePadData.ButtonsPrev & GamePadButtons.Down) == 0) {
				Game.MenuButtons.SelectNext ();
			}
			
			//selecting
			if ((gamePadData.Buttons & GamePadButtons.Cross) != 0) {
				selection = Game.MenuButtons.Select;
			}
			
			Game.MenuButtons.Update ();
			
			switch (selection) {
			case 0://play
				Game.GameState = States.Play;
				MenuDispose ();
				NewGame ();
				break;
			case 1://high score
				Game.GameState = States.HighScore;
				MenuDispose ();
				NewHighScore ();
				break;
			case 2://quit
				Game.Running = false;
				break;
			default:
				break;
			}
		}
		
		public static void MenuRender ()
		{
			Game.BgMenu.Render ();
			Game.MenuButtons.Render ();
		}
		
		public static void MenuDispose ()
		{
			Game.BgMenu = null;
			Game.MenuButtons = null;
		}
		#endregion
		
		#region High Score
		public static void HighScoreUpdate (GamePadData gamePadData)
		{
			//check if clicked back
			if ((gamePadData.Buttons & GamePadButtons.Back) != 0 || (gamePadData.Buttons & GamePadButtons.Start) != 0) {
				Game.GameState = States.MainMenu;
				HighScoreDispose ();
				NewMenu ();
			}
		}
		
		public static void HighScoreRender ()
		{
			Game.BgMenu.Render ();
			Game.HSD.Render ();
		}
		
		public static void HighScoreDispose ()
		{
			Game.HSD = null;
			Game.BgMenu = null;
		}
		#endregion
		
		#region Naming
		public static void NamingUpdate (GamePadData gamePadData)
		{
			
			if (Game.NameDisplay.Update (gamePadData)) {
				Game.GameState = States.HighScore;
				NamingDispose ();
				NewHighScore ();
			}
			
		}
		
		public static void NamingRender ()
		{
			Game.BgMenu.Render ();
			Game.NameDisplay.Render ();
		}
		
		public static void NamingDispose ()
		{
			Game.BgMenu = null;
			Game.NameDisplay = null;
		}
		#endregion
	}
}
