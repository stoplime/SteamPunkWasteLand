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
using Sce.PlayStation.Core;
using Sce.PlayStation.Core.Environment;
using Sce.PlayStation.Core.Graphics;

namespace SteamPunkWasteLand
{
	public class Background
	{
		private Sprite sprite;
		private Vector3 center;
		
		public Background (Texture2D texture)
		{
			float width = Game.Graphics.Screen.Width;
			float height = Game.Graphics.Screen.Height;
			sprite = new Sprite(Game.Graphics,texture);
			sprite.Scale = new Vector2(width/1280f,height/800f);
			center = new Vector3(width/2,height/2,0);
		}
		
		public void Update()
		{
			sprite.Position = WorldCoord.WorldToView(-center);
			sprite.Position.Y = 0;
		}
		public void Render()
		{
			sprite.Render();
		}
		
	}
}

