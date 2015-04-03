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
using Sce.PlayStation.Core.Environment;
using Sce.PlayStation.Core.Graphics;
using Sce.PlayStation.HighLevel.UI;

namespace SteamPunkWasteLand
{
	public class Text
	{
		private Scene scene;
		private Label rect;
		private float posX;
		private float posY;
		private string text;
		private float width;
		private float height;
		private int horizontalAlignment;
		private int verticalAlignment;
		
		public UIColor TextColor
		{
			get{return rect.TextColor;}
			set{rect.TextColor = value;}
		}
		public float TextAlpha
		{
			get{return rect.Alpha;}
			set{rect.Alpha = value;}
		}
		public int TextSize
		{
			get{return rect.Font.Size;}
			set{rect.Font.Size = value;}
		}
		
		public Text (float x, float y,string text)
		{
			posX = x;
			posY = y;
			this.text = text;
			this.width = 960;
			this.height = -1;
			horizontalAlignment = -1;
			verticalAlignment = -1;
			
			Initialize(posX,posY,width,height,horizontalAlignment,verticalAlignment,text);
		}
		public Text (float x,float y,float width,float height,string text)
		{
			posX = x;
			posY = y;
			this.text = text;
			this.width = width;
			this.height = height;
			horizontalAlignment = -1;
			verticalAlignment = -1;
			
			Initialize(posX,posY,width,height,horizontalAlignment,verticalAlignment,text);
		}
		/// <summary>
		/// Initializes a new instance of the <see cref="GameBeta.Text"/> class.
		/// </summary>
		/// <param name='x'>
		/// X. positions are based on the alignment ie. if align Right then x is the right pos of the rectangle
		/// </param>
		/// <param name='y'>
		/// Y.
		/// </param>
		/// <param name='width'>
		/// Width.
		/// </param>
		/// <param name='height'>
		/// Height. -1 for default
		/// </param>
		/// <param name='horzAlign'>
		/// Horizontal align. Where -1 = Left, 0 = Center, 1 = Right
		/// </param>
		/// <param name='vertAlign'>
		/// Vertical align. Where -1 = Top, 0 = Middle, 1 = Bottom
		/// </param>
		/// <param name='text'>
		/// Text.
		/// </param>
		public Text (float x,float y,float width,float height,int horzAlign,int vertAlign,string text)
		{
			posX = x;
			posY = y;
			this.text = text;
			this.width = width;
			this.height = height;
			horizontalAlignment = horzAlign;
			verticalAlignment = vertAlign;
			
			Initialize(posX,posY,width,height,horizontalAlignment,verticalAlignment,text);
		}
		
		public void Update(string text)
		{
			this.text = text;
			
			OverloadUpdate(posX,posY,width,height,horizontalAlignment,verticalAlignment,text);
		}
		/// <summary>
		/// Update the specified x, y, width, height, horzAlign, vertAlign and text.
		/// </summary>
		/// <param name='x'>
		/// X. positions are based on the alignment ie. if align Right then x is the right pos of the rectangle
		/// </param>
		/// <param name='y'>
		/// Y.
		/// </param>
		/// <param name='width'>
		/// Width.
		/// </param>
		/// <param name='height'>
		/// Height.
		/// </param>
		/// <param name='horzAlign'>
		/// Horizontal align. Where -1 = Left, 0 = Center, 1 = Right
		/// </param>
		/// <param name='vertAlign'>
		/// Vertical align. Where -1 = Top, 0 = Middle, 1 = Bottom
		/// </param>
		/// <param name='text'>
		/// Text.
		/// </param>
		public void Update(float x,float y,float width,float height,int horzAlign,int vertAlign,string text)
		{
			posX = x;
			posY = y;
			this.text = text;
			this.width = width;
			this.height = height;
			horizontalAlignment = horzAlign;
			verticalAlignment = vertAlign;
			
			OverloadUpdate(posX,posY,width,height,horizontalAlignment,verticalAlignment,text);
		}
		
		public void Render(){
			UISystem.SetScene(scene, null);
			UISystem.Render();
		}
		
		public void Initialize(float x,float y,float width,float height,int horzAlign,int vertAlign,string text)
		{
			scene = new Scene();
			rect = new Label();
			
			OverloadUpdate(x,y,width,height,horzAlign,vertAlign,text);
			
			UISystem.SetScene(scene);
		}
		
		/// <summary>
		/// Overloads the update.
		/// </summary>
		/// <param name='x'>
		/// X. positions are based on the alignment ie. if align Right then x is the right pos of the rectangle
		/// </param>
		/// <param name='y'>
		/// Y. positions are based on the alignment
		/// </param>
		/// <param name='width'>
		/// Width.
		/// </param>
		/// <param name='height'>
		/// Height. -1 for default
		/// </param>
		/// <param name='horzAlign'>
		/// Horizontal align. Where -1 = Left, 0 = Center, 1 = Right
		/// </param>
		/// <param name='vertAlign'>
		/// Vertical align. Where -1 = Top, 0 = Middle, 1 = Bottom
		/// </param>
		/// <param name='text'>
		/// Text.
		/// </param>
		public void OverloadUpdate(float x,float y,float width,float height,int horzAlign,int vertAlign,string text)
		{
			rect.Width = width;
			if(FMath.Round(height) != -1){
				rect.Height = height;
			}
			
			switch(horzAlign){
			case -1:
				rect.X = x;
				rect.HorizontalAlignment = HorizontalAlignment.Left;
				break;
			case 0:
				rect.X = x-width/2;
				rect.HorizontalAlignment = HorizontalAlignment.Center;
				break;
			case 1:
				rect.X = x-width;
				rect.HorizontalAlignment = HorizontalAlignment.Right;
				break;
			}
			switch(vertAlign){
			case -1:
				rect.Y = y;
				rect.VerticalAlignment = VerticalAlignment.Top;
				break;
			case 0:
				rect.Y = y-rect.Height/2;
				rect.VerticalAlignment = VerticalAlignment.Middle;
				break;
			case 1:
				rect.Y = y-rect.Height;
				rect.VerticalAlignment = VerticalAlignment.Bottom;
				break;
			}
			
			rect.Text = text;
			
			scene.RootWidget.AddChildLast(rect);
		}
		
	}
}

