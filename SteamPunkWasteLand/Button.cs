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
using Sce.PlayStation.Core.Graphics;

namespace SteamPunkWasteLand
{
	public class Button
	{
		private Sprite sprite;
		private ButtonState bs;
		
		private Vector2 size;
		public Vector2 Size
		{
			get{return size;}
			set{size = value;}
		}
		private Vector4 textureCoord;
		public Vector4 TextureCoord
		{
			get{return textureCoord;}
			set{textureCoord = value;}
		}
		
		public Vector3 Pos
		{
			get{return sprite.Position;}
			set{sprite.Position = value;}
		}
		
		private enum ButtonState
		{
			normal,
			selected
		}
		
		public Button (Texture2D text, Vector4 textureCoord, Vector3 initPos)
		{
			this.textureCoord = textureCoord;
			this.size = new Vector2(textureCoord.Z-textureCoord.X,textureCoord.W-textureCoord.Y);
			
			sprite = new Sprite(Game.Graphics,text);
			sprite.Center = new Vector2(0.5f,0.5f);
			sprite.Position = initPos;
			sprite.Width = size.X;
			sprite.Height = size.Y;
		}
		
		public void Update (bool selected)
		{
			if (selected) {
				bs = ButtonState.selected;
			}else{
				bs = ButtonState.normal;
			}
		}
		
		public void Render ()
		{
			switch (bs) {
			case ButtonState.normal:
				sprite.SetTextureCoord(textureCoord.X,textureCoord.Y,textureCoord.Z,textureCoord.W);
				break;
			case ButtonState.selected:
				sprite.SetTextureCoord(textureCoord.X,textureCoord.Y+size.Y,textureCoord.Z,textureCoord.W+size.Y);
				break;
			}
			
			sprite.Render();
		}
	}
}

