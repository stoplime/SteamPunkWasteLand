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
using System.Diagnostics;
using System.Collections.Generic;
using Sce.PlayStation.Core;
using Sce.PlayStation.Core.Environment;
using Sce.PlayStation.Core.Graphics;
using Sce.PlayStation.Core.Input;

namespace SteamPunkWasteLand
{
	public class AppMain
	{
		
		public static void Main (string[] args)
		{
			Stopwatch s = new Stopwatch();
			s.Start();
			Initialize ();

			while (Game.Running) {
				SystemEvents.CheckEvents ();
				float time = s.ElapsedMilliseconds/1000f;
				s.Reset();
				s.Start();
				Update (time);
				Render ();
			}
		}

		public static void Initialize ()
		{
			Game.Graphics = new GraphicsContext ();
			Game.Running = true;
			Game.GameState = States.MainMenu;
			Game.Textures = new List<Texture2D>();
			InitTextures();
			
			Game.BgSky = new Background(Game.Textures[0]);
			Game.BgGround = new BackgroundGround(Game.Textures[1]);
			Game.BgCloud = new BackgroundClouds(Game.Textures[2]);
			
			Game.Player1 = new Player();
			
		}

		public static void InitTextures ()
		{
			Game.Textures.Add(new Texture2D("/Application/assets/Backgrounds/Sky2.png",false));
			Game.Textures.Add(new Texture2D("/Application/assets/Backgrounds/Ground1.png",false));
			Game.Textures.Add(new Texture2D("/Application/assets/Backgrounds/Cloud2.png",false));
			
			Game.Textures.Add(new Texture2D("/Application/assets/Player/Tophat_Sheet.png",false));
		}

		public static void Update (float time)
		{
			var gamePadData = GamePad.GetData (0);
			Game.Player1.Update(gamePadData,time);
			WorldCoord.FocusObject = Game.Player1.WorldPos;
			
			WorldCoord.UpdateFocus(time);
			Game.BgCloud.Update();
			Game.BgGround.Update();
		}

		public static void Render ()
		{
			Game.Graphics.SetClearColor (0.0f, 0.0f, 0.0f, 0.0f);
			Game.Graphics.Clear ();
			
			Game.BgSky.Render();
			Game.BgGround.Render();
			Game.BgCloud.Render();
			
			Game.Player1.Render();
			
			Game.Graphics.SwapBuffers ();
		}
	}
}
