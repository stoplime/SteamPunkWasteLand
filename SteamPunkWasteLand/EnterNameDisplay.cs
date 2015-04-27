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
using System.IO;
using System.Collections.Generic;
using Sce.PlayStation.Core;
using Sce.PlayStation.Core.Input;
using Sce.PlayStation.HighLevel.UI;

namespace SteamPunkWasteLand
{
	public class EnterNameDisplay
	{
		// 9*5 TABLE
		public const string ABC = "ABCDEFGHIJKLMNOPQRSTUVWXYZ.1234567890_";
		private string name;
		private int selection;
		private bool typed;
		private ButtonSet letters;
		private ButtonSet letters2;
		private ButtonSet extras;
		private Text nameDisplay;
		
		private Sprite newHighscorePromp;
		
		public EnterNameDisplay ()
		{
			typed = false;
			name = "NAME";
			selection = 0;
			
			float X = Game.Graphics.Screen.Width / 2f;
			
			newHighscorePromp = new Sprite(Game.Graphics,Game.Textures[38]);
			newHighscorePromp.Center = new Vector2(0.5f,0);
			newHighscorePromp.Position = new Vector3(Game.Graphics.Screen.Width/2f,20,0);
			
			letters = new ButtonSet (Game.Textures [23], new Vector2 (64, 64));
			letters2 = new ButtonSet (Game.Textures [24], new Vector2 (64, 64));
			extras = new ButtonSet (Game.Textures [25], new Vector2 (192, 64));
			
			for (int j = 0; j < 5; j++) {
				for (int i = 0; i < 9; i++) {
					if (i > 1 && j == 4) {
						break;
					}
					if (letters.Count < 19) {
						letters.AddButton (new Vector3 ((X - 256) + (i * 64), (232) + (j * 64), 0));
					} else {
						letters2.AddButton (new Vector3 ((X - 256) + (i * 64), (232) + (j * 64), 0));
					}
				}
			}
			extras.AddButton (new Vector3 (X - 64, 488, 0));
			extras.AddButton (new Vector3 (X + 96, 488, 0));
			extras.AddButton (new Vector3 (X + 224, 488, 0));
			
			nameDisplay = new Text (X - 288, 200, 576, 64, -1, 1, name);
			nameDisplay.TextSize = 64;
			nameDisplay.TextColor = new UIColor (240 / 256f, 155 / 256f, 33 / 256f, 1);
		}
		
		private void selectUp ()
		{
			selection -= 9;
			if (selection < 0) {
				selection = 45 + selection;
			}
		}
		
		private void selectDown ()
		{
			selection += 9;
			if (selection >= 45) {
				selection -= 45;
			}
		}
		
		private void selectLeft ()
		{
			if (selection < 38) {
				selection--;
			} else if (selection >= 38 && selection < 41) {
				selection = 37;
			} else if (selection >= 41 && selection < 43) {
				selection = 40;
			} else if (selection >= 43) {
				selection = 42;
			}
			if (selection % 9 == 8 || selection < 0) {
				selectDown ();
			}
		}
		
		private void selectRight ()
		{
			if (selection < 38) {
				selection++;
			} else if (selection >= 38 && selection < 41) {
				selection = 41;
			} else if (selection >= 41 && selection < 43) {
				selection = 43;
			} else if (selection >= 43) {
				selection = 0;
			}
			if (selection % 9 == 0 || selection >= 45) {
				selectUp ();
			}
		}
		
		private void TypedOnce ()
		{
			if (!typed) {
				typed = true;
				name = "";
			}
		}
		
		public bool Update (GamePadData gamePadData)
		{
			//movement
			if ((gamePadData.Buttons & GamePadButtons.Up) != 0 && (gamePadData.ButtonsPrev & GamePadButtons.Up) == 0) {
				selectUp ();
			}
			if ((gamePadData.Buttons & GamePadButtons.Down) != 0 && (gamePadData.ButtonsPrev & GamePadButtons.Down) == 0) {
				selectDown ();
			}
			if ((gamePadData.Buttons & GamePadButtons.Left) != 0 && (gamePadData.ButtonsPrev & GamePadButtons.Left) == 0) {
				selectLeft ();
			}
			if ((gamePadData.Buttons & GamePadButtons.Right) != 0 && (gamePadData.ButtonsPrev & GamePadButtons.Right) == 0) {
				selectRight ();
			}
			
			//selecting
			if ((gamePadData.Buttons & GamePadButtons.Cross) != 0 && (gamePadData.ButtonsPrev & GamePadButtons.Cross) == 0) {
				if (selection < 38) {
					TypedOnce ();
					name += ABC [selection].ToString ();
				} else if (selection >= 38 && selection < 41) {
					TypedOnce ();
					name += " ";
				} else if (selection >= 41 && selection < 43) {
					if (name.Length > 1) {
						name = name.Substring (0, name.Length - 1);
					}
				} else if (selection >= 43) {
					//submit name
					HighScore h = new HighScore (name, Game.Score.ToString ());
					Game.HighScores.Add (h);
					Game.HighScores.Sort ();
					Game.HighScores.Reverse ();
					Game.HighScores.RemoveAt (10);
					return true;
				}
			}
			
			letters.Select = selection;
			letters2.Select = selection - 19;
			if (selection >= 38 && selection < 41) {
				extras.Select = 0;
			} else if (selection >= 41 && selection < 43) {
				extras.Select = 1;
			} else if (selection >= 43) {
				extras.Select = 2;
			} else {
				extras.Select = -1;
			}
			
			nameDisplay.Update (name);
			
			letters.Update ();
			letters2.Update ();
			extras.Update ();
			
			return false;
		}
		
		public void Render ()
		{
			newHighscorePromp.Render();
			
			nameDisplay.Render ();
			
			letters.Render ();
			letters2.Render ();
			extras.Render ();
		}
	}
}

