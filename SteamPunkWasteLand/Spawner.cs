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
using System.IO;
using Sce.PlayStation.Core;

namespace SteamPunkWasteLand
{
	public class Spawner
	{
		private float deltaTime;
		private List<string> line;
		private Queue<string> queue;
		private bool endGame;
		private float wait;
		private bool endless;
		private Queue<SpawnObject> randomLevel;
		
		public Spawner ()
		{
			endless = false;
			deltaTime = 0;
			endGame = false;
			wait = 0.5f;
			line = new List<string> (); // 10 levels
			queue = new Queue<string> ();
			randomLevel = new Queue<SpawnObject> ();
			
			StreamReader sr = null;
			try {
				sr = new StreamReader ("Documents/levels.txt");
				for (int i = 0; i < Game.Level; i++) {
					if (!sr.EndOfStream) {
						string s = sr.ReadLine ();
						if (s.Length > 0) {
							while (s.Contains("//")) {
								s = sr.ReadLine ();
							}
						}
					}
				}
				if (!sr.EndOfStream) {
					string[] oneLine = sr.ReadLine ().Split (';');
					for (int i = 0; i < oneLine.Length; i++) {
						line.Add (oneLine [i]);
					}
				} else {
					endless = true;
				}
			} catch (FileNotFoundException) {
				File.CreateText ("Documents/levels.txt");
				StreamWriter sw = new StreamWriter ("Documents/levels.txt");
				CreateLevelsTextFile (sw);
				sw.Close ();
				Console.WriteLine ("Please build levels first before running the game.");
			} catch (Exception e) {
				Console.WriteLine (e);
			} finally {
				if (sr != null) {
					sr.Close ();
				}
			}
			
//			if(line.Count > 0){
//				if (line[0] == null) {
//					endGame = true;
//				}
//			}
			
			if (!endless) {
				for (int i = 0; i < line.Count-1; i++) {
					queue.Enqueue (line [i]);
					Console.WriteLine (line [i]);
				}
			} else {
				CreateNewLevels ();
			}
		}
		
		private void CreateLevelsTextFile (StreamWriter sw)
		{
			sw.WriteLine ("// insert levels here");
			sw.WriteLine ("// Format: type, posX, posY, sleep;");
			sw.WriteLine ("// One level per line");
			sw.WriteLine ("// ");
			sw.WriteLine ("// Type:");
			sw.WriteLine ("// Wb = spawn cross bow");
			sw.WriteLine ("// Wf = spawn flame thrower");
			sw.WriteLine ("// Wc = spawn cannon");
			sw.WriteLine ("// Eg = spawn imperial guard");
			sw.WriteLine ("// Ed = spawn dragon");
			sw.WriteLine ("// Ez = spawn zeppelin");
			sw.WriteLine ("// Ea = spawn air ship");
			sw.WriteLine ("// ");
			sw.WriteLine ("// Pos Key words:");
			sw.WriteLine ("// left = left most posX");
			sw.WriteLine ("// right = right most posX");
			sw.WriteLine ("// top = top most posY");
			sw.WriteLine ("// ");
			
			sw.WriteLine ("// skip one line after this one, very important");
		}
		
		public void AddObjects (string item, int posX, int posY)
		{
			/*	Wb = spawn cross bow
			 * 	Wf = spawn flame thrower
			 * 	Wc = spawn cannon
			 * 	Eg = spawn imperial guard
			 * 	Ed = spawn dragon
			 * 	Ez = spawn zeppelin
			 * 	Ea = spawn air ship        
			*/
			switch (item) {
			case "Wb":
				L_CrossBow Wb = new L_CrossBow (new Vector3 (posX, posY, 0));
				Game.Loots.Add (Wb);
				break;
			case "Wf":
				L_Flamethrower Wf = new L_Flamethrower (new Vector3 (posX, posY, 0));
				Game.Loots.Add (Wf);
				break;
			case "Wc":
				L_Cannon Wc = new L_Cannon (new Vector3 (posX, posY, 0));
				Game.Loots.Add (Wc);
				break;
			case "Eg":
				E_Guard Eg = new E_Guard (new Vector3 (posX, posY, 0));
				Game.Enemies.Add (Eg);
				break;
			case "Ed":
				E_Dragon Ed = new E_Dragon (new Vector3 (posX, posY, 0));
				Game.Enemies.Add (Ed);
				break;
			case "Ez":
				E_Zeppelin Ez = new E_Zeppelin (new Vector3 (posX, posY, 0));
				Game.Enemies.Add (Ez);
				break;
			case "Ea":
				E_AirShip Ea = new E_AirShip (new Vector3 (posX, posY, 0));
				Game.Enemies.Add (Ea);
				break;
			}
		}
		
		public void CreateNewLevels ()
		{
			/*	Eg = spawn imperial guard
			 * 	Ed = spawn dragon
			 * 	Ez = spawn zeppelin
			 * 	Ea = spawn air ship   
			 */
			for (int i = -3; i < Game.Level/5; i++) {
				string enemy;
				int posX, posY;
				float delay;
				int left = (int)(-Game.Graphics.Screen.Width * 1.5f);
				int right = (int)(Game.Graphics.Screen.Width * 1.5f);
				
				double enemyProbability = Game.Rand.NextDouble () * 100;
				if (enemyProbability < 72.73) {
					enemy = "Eg";
					posX = (Game.Rand.Next (2) == 0 ? left : right);
					posY = 0;
				} else if (enemyProbability < 90.91) {
					enemy = "Ez";
					posX = (Game.Rand.Next (2) == 0 ? left : right);
					posY = Game.Rand.Next (250, (int)(Game.Graphics.Screen.Height * 3f / 4f));
				} else if (enemyProbability < 98.18) {
					enemy = "Ed";
					posX = (Game.Rand.Next (2) == 0 ? left : right);
					posY = 50;
				} else {
					enemy = "Ea";
					posX = Game.Rand.Next (-1500, 1500);
					posY = Game.Graphics.Screen.Height;
				}
				delay = (float)Game.Rand.NextDouble () * 4 + 1;
				
				SpawnObject obj = new SpawnObject (enemy, posX, posY, delay);
				randomLevel.Enqueue (obj);
			}
		}
		
		public bool Update (float time)
		{
			deltaTime += time;
			
			
			if (!endGame) {
				if (deltaTime > wait) {
					deltaTime = 0;
					if (!endless) {
						if (queue.Count > 0) {
							string action = queue.Dequeue ();
							if (action == null) {
								return false;
							}
							// action, posX, posY, delay;
							string[] spwn = action.Split (',');
							string item = spwn [0];
							int posX, posY;
							if (spwn [1].Contains ("left")) {
								posX = (int)(-Game.Graphics.Screen.Width * 1.5f);
							} else if (spwn [1].Contains ("right")) {
								posX = (int)(Game.Graphics.Screen.Width * 1.5f);
							} else {
								posX = Int32.Parse (spwn [1]);
							}
							if (spwn [2].Contains ("top")) {
								posY = Game.Graphics.Screen.Height;
							} else {
								posY = Int32.Parse (spwn [2]);
							}
							
							wait = float.Parse (spwn [3]);
							
							AddObjects (item, posX, posY);
							
						} else {
							endGame = true;
						}
					} else {
						if (randomLevel.Count > 0) {
							SpawnObject obj = randomLevel.Dequeue ();
							wait = obj.Delay;
							AddObjects (obj.Type, obj.PosX, obj.PosY);
						} else {
							endGame = true;
						}
					}
				}
				
			}
			
			return endGame;
		}
	}
}

