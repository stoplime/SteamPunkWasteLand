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
using Sce.PlayStation.Core.Input;

namespace SteamPunkWasteLand
{
	public enum States {
		MainMenu,
		Play,
		HighScore,
		Name
	}
	
	public static class Game
	{
		public static bool Running;
		public static HUD hud;
		public static GraphicsContext Graphics;
		public static Random Rand;
		public static States GameState;
		public static Spawner Spawner;
		public static float TimeSpeed;
		public static int Level;
		
		public static Background BgSky,BgGround,BgCloud;
		public static Player Player1;
		public static long Score;
		public static int Money;
		
		public static List<Loot> Loots;
		public static List<Enemy> Enemies;
		public static List<Bullet> PBullets;
		public static List<Bullet> EBullets;
		public static List<Coins> AnimatedMoney;
		
		public static List<Texture2D> Textures;
		public static List<Weapon> ObtainedWeapons;
	}
}

