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
using Sce.PlayStation.Core.Graphics;

namespace SteamPunkWasteLand
{
	public class ButtonSet
	{
		private List<Button> buttons;
		private Texture2D texture;
		private Vector2 size;
		
		private int selecting;
		public int Select
		{
			get{return selecting;}
			set{selecting = value;}
		}
		
		public ButtonSet (Texture2D text, Vector2 size)
		{
			texture = text;
			this.size = size;
			selecting = 0;
			
			buttons = new List<Button>();
		}
		
		public void AddButton (Vector3 initPos)
		{
			buttons.Add(new Button(texture,new Vector4(buttons.Count*size.X,0,(buttons.Count+1)*size.X,size.Y),initPos));
		}
		
		public void SelectPrevious ()
		{
			if (selecting > 0) {
				selecting--;
			}else{
				selecting = buttons.Count-1;
			}
		}
		
		public void SelectNext ()
		{
			if (selecting < buttons.Count-1) {
				selecting++;
			}else{
				selecting = 0;
			}
		}
		
		public void Update ()
		{
			for (int i = 0; i < buttons.Count; i++) {
				if(i == selecting){
					buttons[i].Update(true);
				}else{
					buttons[i].Update(false);
				}
			}
		}
		
		public void Render ()
		{
			foreach (Button b in buttons) {
				b.Render();
			}
		}
	}
}

