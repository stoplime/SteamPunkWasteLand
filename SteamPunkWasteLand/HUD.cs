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
	public class HUD
	{
		private float time;
		private float scale;
		private float height;
		
		private Sprite gear;
		private int gearIndex;
		private float gearCount;
		
		private Sprite playerHp;
		private Sprite hpTube;
		private int hpWidth = 1000;
		private int hpHeight = 20;
		
		private Text score;
		private Text money;
		
		public HUD ()
		{
			height = Game.Graphics.Screen.Height;
			scale = height/800f;
			hpWidth = (int)(hpWidth*scale);
			hpHeight = (int)(hpHeight*scale);
			
			initGear();
			
			initHP();
			score = new Text(Game.Graphics.Screen.Width-20,height-20,300,40,1,1,Game.Score.ToString());
			money = new Text(22*scale,height-22*scale,75*scale,75*scale,0,0,Game.Money.ToString());
			money.TextSize = 18;
		}
		
		private void initGear()
		{
			gear = new Sprite(Game.Graphics,Game.Textures[17],75,75);
			gear.Scale = new Vector2(scale,scale);
			gear.Center = new Vector2(0f,1f);
			gear.Position = new Vector3(0,height,0);
			gearIndex = 0;
			gearCount = 0;
		}
		
		private void initHP()
		{
			playerHp = new Sprite(Game.Graphics,Game.Textures[16]);
			playerHp.Center = new Vector2(0,1);
			playerHp.Position = new Vector3(60*scale,height-(5*scale),0);
			hpTube = new Sprite(Game.Graphics,Game.Textures[18],hpWidth+(50*scale),hpHeight+(10*scale));
			hpTube.Center = new Vector2(0,1);
			hpTube.Position = new Vector3(60*scale,height,0);
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
		}
		
		public void Update(float t)
		{
			this.time += t;
			gearCount += t;
			
			if (gearCount > 0.1f) {
				gearCount = 0;
				if (gearIndex < 3) {
					gearIndex++;
				}else{
					gearIndex = 0;
				}
			}
			
			hpUpdate();
			
			score.Update(Game.Score.ToString());
			money.Update(Game.Money.ToString());
		}
		
		public void Render()
		{
			gear.SetTextureUV(gearIndex/4f,0,(gearIndex+1)/4f,1);
			
			hpTube.Render();
			playerHp.Render();
			gear.Render();
			score.Render();
			money.Render();
		}
		
	}
}

