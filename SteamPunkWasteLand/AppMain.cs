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
		private static GraphicsContext graphics;
		
		public static void Main (string[] args)
		{
			Stopwatch s = new Stopwatch();
			s.Start();
			Initialize ();

			while (true) {
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
			
			Game.GameState = States.MainMenu;
		}

		public static void Update (float time)
		{
			var gamePadData = GamePad.GetData (0);
			
		}

		public static void Render ()
		{
			graphics.SetClearColor (0.0f, 0.0f, 0.0f, 0.0f);
			graphics.Clear ();
			
			
			
			graphics.SwapBuffers ();
		}
	}
}
