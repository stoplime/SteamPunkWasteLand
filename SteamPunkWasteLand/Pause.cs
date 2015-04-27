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
using Sce.PlayStation.Core.Input;

namespace SteamPunkWasteLand
{
	public class Pause
	{
		private Sprite outFocus;
		private Sprite backSettings, backShop;
		private bool isSettings;
		private int settingsSelect;
		private float timeSpeedDerived;
		private ButtonSet tabs;
		
		private Text[] settingsValues;
		private ButtonSet settingsButtons;
		
		private ButtonSet shopIcons;
		private float[] upgradedValue;
		private Text[,] shopDescription;
		private Text shopBuyText;
		private Sprite selectedIcon;
		
		private float buttonPressTime;
		
		private bool isPause;
		public bool IsPause
		{
			get{return isPause;}
			set{isPause = value;}
		}
		
		public Pause ()
		{
			isPause = false;
			buttonPressTime = 0;
			outFocus = new Sprite(Game.Graphics,Game.Textures[16],Game.Graphics.Screen.Width,Game.Graphics.Screen.Height);
			outFocus.SetColor(1,1,1,0.3f);
			
			backSettings = new Sprite(Game.Graphics,Game.Textures[30]);
			backSettings.Center = new Vector2(0.5f,0);
			backSettings.Position = new Vector3(Game.Graphics.Screen.Width/2f,50,0);
			
			backShop = new Sprite(Game.Graphics,Game.Textures[31]);
			backShop.Center = new Vector2(0.5f,0);
			backShop.Position = new Vector3(Game.Graphics.Screen.Width/2f,50,0);
			
			isSettings = true;
			settingsSelect = 0;
			
			tabs = new ButtonSet(Game.Textures[32],new Vector2(300,60));
			tabs.AddButton(new Vector3(-150,30,0)+backSettings.Position);
			tabs.AddButton(new Vector3(150,30,0)+backSettings.Position);
			
			upgradedValue = new float[5];
			UpdateUpgradedValues();
			
			//Settings
			settingsValues = new Text[3];
			settingsValues[0] = new Text(250+backSettings.Position.X,109+backSettings.Position.Y,100,40,0,0,FMath.Round(Game.Music*100)+"%");
			settingsValues[1] = new Text(250+backSettings.Position.X,175+backSettings.Position.Y,100,40,0,0,FMath.Round(Game.Sound*100)+"%");
			settingsValues[2] = new Text(250+backSettings.Position.X,243+backSettings.Position.Y,100,40,0,0,FMath.Round(Game.TimeSpeed*100)/100+"X");
			
			timeSpeedDerived = FMath.Sqrt((Game.TimeSpeed-0.5f)/2f);
			
			settingsButtons = new ButtonSet(Game.Textures[33],new Vector2(128,50));
			settingsButtons.AddCustomButton(Game.Textures[34],
			                                new Vector4(0,0,10,30),
			                                new Vector3(-100+300*Game.Music,109,0)+backSettings.Position);
			settingsButtons.AddCustomButton(Game.Textures[34],
			                                new Vector4(0,0,10,30),
			                                new Vector3(-100+300*Game.Sound,175,0)+backSettings.Position);
			settingsButtons.AddCustomButton(Game.Textures[34],
			                                new Vector4(0,0,10,30),
			                                new Vector3(-100+300*timeSpeedDerived,243,0)+backSettings.Position);
			settingsButtons.AddCustomButton(Game.Textures[33],
			                                new Vector4(0,0,128,50),
			                                new Vector3(200,348,0)+backSettings.Position);
			settingsButtons.AddCustomButton(Game.Textures[33],
			                                new Vector4(128,0,256,50),
			                                new Vector3(-200,348,0)+backSettings.Position);
			//Shop
			shopIcons = new ButtonSet(Game.Textures[35],new Vector2(54,54));
			shopIcons.AddButton(backShop.Position + new Vector3(-200,109,0));
			shopIcons.AddButton(backShop.Position + new Vector3(-100,109,0));
			shopIcons.AddButton(backShop.Position + new Vector3(0,109,0));
			shopIcons.AddButton(backShop.Position + new Vector3(100,109,0));
			shopIcons.AddButton(backShop.Position + new Vector3(200,109,0));
			
			float X = backShop.Position.X;
			float Y = backShop.Position.Y;
			
			shopDescription = new Text[5,5];//[Text type, select number]
			//level display
			shopDescription[0,0] = new Text(-240+X,330+Y,70f,26f,0,-1,"Lvl:"+Game.Upgrades[0,0]);
			shopDescription[0,1] = new Text(-240+X,330+Y,70f,26f,0,-1,"Lvl:"+Game.Upgrades[0,1]);
			shopDescription[0,2] = new Text(-240+X,330+Y,70f,26f,0,-1,"Lvl:"+Game.Upgrades[0,2]);
			shopDescription[0,3] = new Text(-240+X,330+Y,70f,26f,0,-1,"Lvl:"+Game.Upgrades[0,3]);
			shopDescription[0,4] = new Text(-240+X,330+Y,70f,26f,0,-1,"Lvl:"+Game.Upgrades[0,4]);
			//Upgrade Title
			shopDescription[1,0] = new Text(-200+X,273+Y,250f,26f,-1,-1,"Better Protection!");
			shopDescription[1,1] = new Text(-200+X,273+Y,250f,26f,-1,-1,"More Damage!");
			shopDescription[1,2] = new Text(-200+X,273+Y,250f,26f,-1,-1,"Rapid Arrows!");
			shopDescription[1,3] = new Text(-200+X,273+Y,250f,26f,-1,-1,"Hotter Flames!");
			shopDescription[1,4] = new Text(-200+X,273+Y,250f,26f,-1,-1,"Bigger Explosions!");
			//Description Box
			shopDescription[2,0] = new Text(-200+X,300+Y,250f,70f,-1,-1,"Some not important description!");
			shopDescription[2,1] = new Text(-200+X,300+Y,250f,70f,-1,-1,"Some not important description!");
			shopDescription[2,2] = new Text(-200+X,300+Y,250f,70f,-1,-1,"Some not important description!");
			shopDescription[2,3] = new Text(-200+X,300+Y,250f,70f,-1,-1,"Some not important description!");
			shopDescription[2,4] = new Text(-200+X,300+Y,250f,70f,-1,-1,"Some not important description!");
			//Upgrade Value
			shopDescription[3,0] = new Text(70+X,273+Y,250f,26f,-1,-1,"HP: "+Game.Upgrades[1,0]+" → "+upgradedValue[0]);
			shopDescription[3,1] = new Text(70+X,273+Y,250f,26f,-1,-1,"Dmg Mult: "+Game.Upgrades[1,1]+" → "+upgradedValue[1]);
			shopDescription[3,2] = new Text(70+X,273+Y,250f,26f,-1,-1,"Fire/Sec: "+Game.Upgrades[1,2]+" → "+upgradedValue[2]);
			shopDescription[3,3] = new Text(70+X,273+Y,250f,26f,-1,-1,"Fire Dmg: "+Game.Upgrades[1,3]+" → "+upgradedValue[3]);
			shopDescription[3,4] = new Text(70+X,273+Y,250f,26f,-1,-1,"Dmg Radius: "+Game.Upgrades[1,4]+" → "+upgradedValue[4]);
			//Cost
			shopDescription[4,0] = new Text(70+X,300+Y,250f,26f,-1,-1,"Cost: $"+Game.Upgrades[2,0]);
			shopDescription[4,1] = new Text(70+X,300+Y,250f,26f,-1,-1,"Cost: $"+Game.Upgrades[2,1]);
			shopDescription[4,2] = new Text(70+X,300+Y,250f,26f,-1,-1,"Cost: $"+Game.Upgrades[2,2]);
			shopDescription[4,3] = new Text(70+X,300+Y,250f,26f,-1,-1,"Cost: $"+Game.Upgrades[2,3]);
			shopDescription[4,4] = new Text(70+X,300+Y,250f,26f,-1,-1,"Cost: $"+Game.Upgrades[2,4]);
			
			for (int i = 0; i < 5; i++) {
				for (int j = 0; j < 5; j++) {
					shopDescription[i,j].TextSize = 20;
				}
			}
			selectedIcon = new Sprite(Game.Graphics,Game.Textures[35],54,54);
			selectedIcon.Center = new Vector2(0.5f,0.5f);
			selectedIcon.Position = new Vector3(-240,300,0)+backShop.Position;
			shopBuyText = new Text(70+X,370+Y,190,30,-1,1,"Press 'W' to Buy");
		}
		
		public void UpdateUpgradedValues ()
		{
			upgradedValue[0] = Game.Upgrades[1,0]+5;
			upgradedValue[1] = Game.Upgrades[1,1]+FMath.Pow(2,Game.Upgrades[0,1]-2);
			upgradedValue[2] = Game.Upgrades[1,2]*0.8f;
			upgradedValue[3] = Game.Upgrades[1,3]*2f;
			upgradedValue[4] = Game.Upgrades[1,4]+10;
		}
		
		public void SettingsArrowUpdate (bool left, bool right, bool up, bool down)
		{
			if (down) {
				if (settingsSelect < 3) {
					settingsSelect++;
				}
			}
			if (up) {
				if (settingsSelect > 0) {
					if (settingsSelect == 4) {
						settingsSelect = 2;
					}else{
						settingsSelect--;
					}
				}
			}
			if (left) {
				switch (settingsSelect) {
				case 0:
					if (Game.Music > 0) {
						Game.Music -= 0.01f;
					}else{
						Game.Music = 0;
					}
					break;
				case 1:
					if (Game.Sound > 0) {
						Game.Sound -= 0.01f;
					}else{
						Game.Sound = 0;
					}
					break;
				case 2:
					if (Game.TimeSpeed > 0.5f) {
						timeSpeedDerived -= 0.01f;
					}
					break;
				case 3:
					settingsSelect = 4;
					break;
				default:
					break;
				}
			}
			if (right) {
				switch (settingsSelect) {
				case 0:
					if (Game.Music < 1f) {
						Game.Music += 0.01f;
					}else{
						Game.Music = 1;
					}
					break;
				case 1:
					if (Game.Sound < 1f) {
						Game.Sound += 0.01f;
					}else{
						Game.Sound = 1;
					}
					break;
				case 2:
					if (Game.TimeSpeed < 2.5f) {
						timeSpeedDerived += 0.01f;
					}
					break;
				case 4:
					settingsSelect = 3;
					break;
				default:
					break;
				}
			}
		}
		
		public void Update (GamePadData gpd)
		{
			bool up = (gpd.Buttons & GamePadButtons.Up) != 0;
			bool down = (gpd.Buttons & GamePadButtons.Down) != 0;
			bool left = (gpd.Buttons & GamePadButtons.Left) != 0;
			bool right = (gpd.Buttons & GamePadButtons.Right) != 0;
			
			timeSpeedDerived = FMath.Sqrt((Game.TimeSpeed-0.5f)/2f);
			
			//Unpause
			if ((gpd.Buttons & GamePadButtons.Select) != 0 && (gpd.ButtonsPrev & GamePadButtons.Select) == 0) {
				isPause = false;
			}
			
			//tab toggle
			if ((gpd.Buttons & GamePadButtons.L) != 0) {
				isSettings = true;
			}
			if ((gpd.Buttons & GamePadButtons.R) != 0) {
				isSettings = false;
			}
			// arrow keys
			if (left || right || up || down) {
				buttonPressTime++;
			}else{
				buttonPressTime = 0;
			}
			
			if (left && (gpd.ButtonsPrev & GamePadButtons.Left) != 0) {
				left = false;
			}
			if (right && (gpd.ButtonsPrev & GamePadButtons.Right) != 0) {
				right = false;
			}
			if (up && (gpd.ButtonsPrev & GamePadButtons.Up) != 0) {
				up = false;
			}
			if (down && (gpd.ButtonsPrev & GamePadButtons.Down) != 0) {
				down = false;
			}
			
			if(buttonPressTime > 20){
				if ((gpd.Buttons & GamePadButtons.Left) != 0) {
					left = true;
				}
				if ((gpd.Buttons & GamePadButtons.Right) != 0) {
					right = true;
				}
				if ((gpd.Buttons & GamePadButtons.Up) != 0) {
					up = true;
				}
				if ((gpd.Buttons & GamePadButtons.Down) != 0) {
					down = true;
				}
			}
			
			//tabs
			if (isSettings) {
				tabs.Select = 0;
			}else{
				tabs.Select = 1;
			}
			tabs.Update();
			
			//display
			if (isSettings) {
				SettingsArrowUpdate(left,right,up,down);
				
				settingsButtons.UpdatePos(0,new Vector3(-100+300*Game.Music,109,0)+backSettings.Position);
				settingsButtons.UpdatePos(1,new Vector3(-100+300*Game.Sound,175,0)+backSettings.Position);
				settingsButtons.UpdatePos(2,new Vector3(-100+300*timeSpeedDerived,243,0)+backSettings.Position);
				
				if ((gpd.Buttons & GamePadButtons.Cross) != 0) {
					if (settingsSelect == 3) {
						//Resume
						isPause = false;
					}
					if (settingsSelect == 4) {
						//Exit
						if (Game.Score > Game.HighScores[Game.HighScores.Count-1].Score) {
							Game.GameState = States.Name;
							AppMain.NewName();
						}else{
							Game.GameState = States.HighScore;
							AppMain.NewHighScore();
						}
						AppMain.PlayDispose();
						return;
					}
				}
				
				settingsValues[0].Update(FMath.Round(Game.Music*100)+"%");
				settingsValues[1].Update(FMath.Round(Game.Sound*100)+"%");
				settingsValues[2].Update(FMath.Round(Game.TimeSpeed*100)/100+"X");
				
				settingsButtons.Select = settingsSelect;
				settingsButtons.Update();
				Game.TimeSpeed = (timeSpeedDerived*timeSpeedDerived*2f)+0.5f;
			}else{
				//Shop
				shopDescription[0,shopIcons.Select].Update("Lvl:"+Game.Upgrades[0,shopIcons.Select]);
				shopDescription[4,shopIcons.Select].Update("Cost: $"+Game.Upgrades[2,shopIcons.Select]);
				
				shopDescription[3,0].Update("HP: "+Game.Upgrades[1,0]+" → "+upgradedValue[0]);
				shopDescription[3,1].Update("Dmg Mult: "+Game.Upgrades[1,1]+" → "+upgradedValue[1]);
				shopDescription[3,2].Update("Fire/Sec: "+Game.Upgrades[1,2]+" → "+upgradedValue[2]);
				shopDescription[3,3].Update("Fire Dmg: "+Game.Upgrades[1,3]+" → "+upgradedValue[3]);
				shopDescription[3,4].Update("Dmg Radius: "+Game.Upgrades[1,4]+" → "+upgradedValue[4]);
				
				shopIcons.Update();
				
				selectedIcon.SetTextureUV(shopIcons.Select/5f,0,(shopIcons.Select+1)/5f,0.5f);
				
				if (left) {
					shopIcons.SelectPrevious();
				}
				if (right) {
					shopIcons.SelectNext();
				}
				
				if ((gpd.Buttons & GamePadButtons.Triangle) != 0 && (gpd.ButtonsPrev & GamePadButtons.Triangle) == 0) {
					if (Game.Money >= Game.Upgrades[2,shopIcons.Select]) {
						Game.Money -= (int)Game.Upgrades[2,shopIcons.Select];
						
						Game.Upgrades[1,shopIcons.Select] = upgradedValue[shopIcons.Select];
						Game.Upgrades[0,shopIcons.Select]++;
						
						switch (shopIcons.Select) {
						case 0:
							Game.Upgrades[2,0] += 50f;
							break;
						case 1:
							Game.Upgrades[2,1] += (2f*Game.Upgrades[0,shopIcons.Select]-1f)*200f;
							break;
						case 2:
						case 3:
						case 4:
							Game.Upgrades[2,shopIcons.Select] += 500f*Game.Upgrades[0,shopIcons.Select];
							break;
						default:
							break;
						}
						
						UpdateUpgradedValues();
					}
				}
			}
		}
		
		public void Render ()
		{
			outFocus.Render();
			if (isSettings) {
				backSettings.Render();
			}else{
				backShop.Render();
			}
			tabs.Render();
			if (isSettings) {
				foreach (Text s in settingsValues) {
					s.Render();
				}
				settingsButtons.Render();
			}else{
				shopIcons.Render();
				for (int i = 0; i < 5; i++) {
					shopDescription[i,shopIcons.Select].Render();
				}
				selectedIcon.Render();
				shopBuyText.Render();
			}
		}
	}
}

