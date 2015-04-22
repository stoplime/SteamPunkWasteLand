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
using System.IO;
using Sce.PlayStation.Core;

namespace SteamPunkWasteLand
{
	public class Spawner
	{
		private float deltaTime;
		private string[] line;
		private Queue<string> queue;
		private bool endGame;
		private float wait;
		
		public Spawner ()
		{
			deltaTime = 0;
			endGame = false;
			wait = 0.5f;
			line = new string[10]; // 10 levels
			queue = new Queue<string>();
			
			StreamReader sr = null;
			try{
				sr = new StreamReader("Documents/levels.txt");
				for (int i = 0; i < Game.Level; i++) {
					if (!sr.EndOfStream) {
						string s = sr.ReadLine();
						if (s.Length > 0) {
							while(s.Contains("//")){
								s = sr.ReadLine();
							}
						}
					}
				}
				if (!sr.EndOfStream) {
					line = sr.ReadLine().Split(';');
					
				}else{
					endGame = true;
				}
			}catch(FileNotFoundException){
				File.CreateText("Documents/levels.txt");
				StreamWriter sw = new StreamWriter("Documents/levels.txt");
				sw.WriteLine("// insert levels here");
				sw.WriteLine("// Format: type, posX, posY, sleep;");
				sw.WriteLine("// One level per line");
				sw.WriteLine("// ");
				sw.WriteLine("// Type:");
				sw.WriteLine("// Wb = spawn cross bow");
				sw.WriteLine("// Wf = spawn flame thrower");
				sw.WriteLine("// Wc = spawn cannon");
				sw.WriteLine("// Eg = spawn imperial guard");
				sw.WriteLine("// Ed = spawn dragon");
				sw.WriteLine("// Ez = spawn zeppelin");
				sw.WriteLine("// Ea = spawn air ship");
				sw.WriteLine("// ");
				sw.WriteLine("// Pos Key words:");
				sw.WriteLine("// left = left most posX");
				sw.WriteLine("// right = right most posX");
				sw.WriteLine("// top = top most posY");
				sw.WriteLine("// skip one line after this one, very important");
				sw.Close();
				Console.WriteLine("Please build levels first before running the game.");
			}catch(Exception e){
				Console.WriteLine(e);
			}finally{
				if (sr != null) {
					sr.Close();
				}
			}
			
			if(line.Length > 0){
				if (line[0] == null) {
					endGame = true;
				}
			}
			
			if(!endGame){
				for (int i = 0; i < line.Length-1; i++) {
					queue.Enqueue(line[i]);
					Console.WriteLine(line[i]);
				}
			}
			
		}
		
		public bool Update(float time)
		{
			deltaTime += time;
			
			
			if (!endGame) {
				if(deltaTime > wait){
					deltaTime = 0;
					if (queue.Count > 0) {
						string action = queue.Dequeue();
						if (action == null) {
							return false;
						}
						// action, posX, posY, delay;
						string[] spwn = action.Split(',');
						string item = spwn[0];
						int posX,posY;
						if (spwn[1].Contains("left")) {
							posX = -Game.Graphics.Screen.Width*2;
						}else if (spwn[1].Contains("right")) {
							posX = Game.Graphics.Screen.Width*2;
						}else{
							posX = Int32.Parse(spwn[1]);
						}
						if (spwn[2].Contains("top")) {
							posY = Game.Graphics.Screen.Height;
						}else{
							posY = Int32.Parse(spwn[2]);
						}
						
						wait = float.Parse(spwn[3]);
						
						
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
							L_CrossBow Wb = new L_CrossBow(new Vector3(posX,posY,0));
							Game.Loots.Add(Wb);
							break;
						case "Wf":
							L_Flamethrower Wf = new L_Flamethrower(new Vector3(posX,posY,0));
							Game.Loots.Add(Wf);
							break;
						case "Wc":
							L_Cannon Wc = new L_Cannon(new Vector3(posX,posY,0));
							Game.Loots.Add(Wc);
							break;
						case "Eg":
							E_Guard Eg = new E_Guard(new Vector3(posX,posY,0));
							Game.Enemies.Add(Eg);
							break;
						case "Ed":
							E_Dragon Ed = new E_Dragon(new Vector3(posX,posY,0));
							Game.Enemies.Add(Ed);
							break;
						case "Ez":
							E_Zeppelin Ez = new E_Zeppelin(new Vector3(posX,posY,0));
							Game.Enemies.Add(Ez);
							break;
						case "Ea":
							E_AirShip Ea = new E_AirShip(new Vector3(posX,posY,0));
							Game.Enemies.Add(Ea);
							break;
						}
					}else{
						endGame = true;
					}
				}
			}
			return endGame;
		}
	}
}

