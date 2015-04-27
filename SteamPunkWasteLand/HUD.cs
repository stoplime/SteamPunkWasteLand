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
using Sce.PlayStation.HighLevel.UI;

namespace SteamPunkWasteLand
{
	public class HUD
	{
		private float time;
		private float scale;
		private float height;
		
		private Sprite gear;
		private float gearSpeed;
		
		private Sprite playerHp;
		private Sprite hpTube;
		private int hpWidth = 1000;
		private int hpHeight = 20;
		
		private Text score;
		private Text money;
		private Text hpText;
		
		private Text intro;
		private Text outro;
		private float outroBlink;
		
		public HUD ()
		{
			height = Game.Graphics.Screen.Height;
			scale = height/800f;
			hpWidth = (int)(hpWidth*scale);
			hpHeight = (int)(hpHeight*scale);
			
			intro = new Text(Game.Graphics.Screen.Width/2f,height/2f,1000,80,0,0,"LEVEL "+Game.Level+", Start!");
			intro.TextColor = new UIColor(240/256f,155/256f,33/256f,1);
			intro.TextSize = 60;
			outro = new Text(Game.Graphics.Screen.Width/2f,height/4f,1000,80,0,0,"Press 'X' to continue next level!");
			outro.TextColor = new UIColor(240/256f,155/256f,33/256f,1);
			outro.TextSize = 60;
			outroBlink = 0;
			
			initGear();
			
			initHP();
			score = new Text(Game.Graphics.Screen.Width-20,height-20,300,40,1,1,Game.Score.ToString());
			money = new Text(25*scale+7,height-20*scale,75*scale,75*scale,0,0,Game.Money.ToString());
			hpText = new Text(60*scale+15+hpWidth/2f,height-hpHeight/2f-(3*scale),300,30,0,0,FMath.Round(Game.Player1.Hp*100)/100f+" / "+Game.Player1.MaxHp);
			money.TextSize = 18;
			hpText.TextSize = 18;
			hpText.TextColor = new UIColor(0,0,0,1);
		}
		
		private void initGear()
		{
			gear = new Sprite(Game.Graphics,Game.Textures[17],150,150);
			gear.Scale = new Vector2(scale,scale);
			gear.Center = new Vector2(0.5f,0.5f);
			gear.Position = new Vector3(15,height,0);
			//gear.SetColor(202/265f,140/265f,50/256f,1);
			gearSpeed = 0.62f;
		}
		
		private void initHP()
		{
			playerHp = new Sprite(Game.Graphics,Game.Textures[16]);
			playerHp.Center = new Vector2(0,1);
			playerHp.Position = new Vector3(60*scale+15,height-(5*scale),0);
			hpTube = new Sprite(Game.Graphics,Game.Textures[18],hpWidth+(50*scale),hpHeight+(10*scale));
			hpTube.Center = new Vector2(0,1);
			hpTube.Position = new Vector3(60*scale+15,height,0);
			hpTube.SetColor(162/265f,100/265f,10/256f,1);
			hpTube.SetTextureUV(0,0,1,1);
		}
		
		private void hpUpdate()
		{
			float hp = Game.Player1.Hp;
			float hpMax = Game.Player1.MaxHp;
			
			playerHp.Width = (hp/hpMax)*hpWidth;
			playerHp.Height = hpHeight;
			if (hp < hpMax/5) {
				playerHp.SetColor(1f,0f,0f,1f);
			}else if (hp < hpMax/2) {
				playerHp.SetColor(1f,0.5f,0f,1f);
			}else{
				playerHp.SetColor(0f,1f,0f,1f);
			}
			
			if (Game.Player1.Hp < Game.Player1.MaxHp/2f) {
				hpText.TextColor = new UIColor(1,1,1,1);
			}
			hpText.Update(Game.Player1.Hp+" / "+Game.Player1.MaxHp);
		}

		private void introOutro ()
		{
			if (time < 2) {
				intro.TextAlpha = 2-time;
				intro.Render();
			}
			if (Game.LevelFinished) {
				outro.TextAlpha = (1+FMath.Cos((time-outroBlink)*4))/2f;
				outro.Render();
			}else{
				outroBlink = time;
			}
		}
		
		public void Update(float t)
		{
			this.time += t;
			
			gear.Rotation += gearSpeed*t;
			
			hpUpdate();
			
			score.Update(Game.Score.ToString());
			money.Update(Game.Money.ToString());
		}
		
		public void Render()
		{
			
			hpTube.Render();
			playerHp.Render();
			gear.Render();
			score.Render();
			money.Render();
			hpText.Render();
			
			introOutro();
		}
		
	}
}

