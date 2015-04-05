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
	public class BackgroundGround : Background
	{
		public Sprite sprite2,sprite3;
		
		public BackgroundGround (Texture2D texture) : base(texture)
		{
			init (out sprite2, texture);
			init (out sprite3, texture);
		}
		
		public override void Update()
		{
			Sprite.Position = WorldCoord.WorldToView(new Vector3(-width/2,height*7/8f,0));
			sprite2.Position = Sprite.Position+ new Vector3(width,0,0);
			sprite3.Position = Sprite.Position+ new Vector3(-width,0,0);
		}
		
		public override void Render ()
		{
			sprite2.Render();
			sprite3.Render();
			base.Render ();
		}
		
	}
}

