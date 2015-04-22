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
using System.IO;
using System.Collections.Generic;
using Sce.PlayStation.Core;

namespace SteamPunkWasteLand
{
	public class HighScoresDisplay
	{
		private string[] names;
		private string[] scores;
		
		private Text[] textNames;
		private Text[] textScores;
		
		public HighScoresDisplay ()
		{
			names = new string[10];
			scores = new string[10];
			
			textNames = new Text[10];
			textScores = new Text[10];
			
			StreamReader sr = null;
			try {
				sr = new StreamReader("Documents/highscores.txt");
				loadHighscores(sr);
			} catch (FileNotFoundException) {
				File.CreateText("Documents/highscores.txt");
				StreamWriter sw = new StreamWriter("Documents/highscores.txt");
				for (int i = 0; i < 10; i++) {
					sw.WriteLine("---,--");
					names[i] = "---";
					scores[i] = "--";
				}
				sw.Close();
			} finally {
				if (sr != null) {
					sr.Close();
				}
			}
			
			float X = Game.Graphics.Screen.Width/2f;
			float Y = Game.Graphics.Screen.Height/2f;
			
			float leftX = X-325;
			float topY = Y-125;
			
			float Nx = X+175;
			float dy = 35;
			
			for (int i = 0; i < 10; i++) {
				textNames[i] = new Text(leftX,(i*dy)+topY,500,dy,-1,-1,(i+1)+". "+names[i]);
				textScores[i] = new Text(Nx,(i*dy)+topY,150,dy,-1,-1,scores[i]);
			}
			
		}
		
		private void loadHighscores(StreamReader sr)
		{
			for (int i = 0; i < 10; i++) {
				string[] line = sr.ReadLine().Split(',');
				if (line[0] != null || line[0] != "") {
					names[i] = line[0];
				}else{
					names[i] = "---";
				}
				if (line[1] != null || line[1] != "") {
					scores[i] = line[1];
				}else{
					scores[i] = "--";
				}
			}
		}
		
		public void Render ()
		{
			foreach (Text t in textNames) {
				t.Render();
			}
			
			foreach (Text t in textScores) {
				t.Render();
			}
		}
		
	}
}

