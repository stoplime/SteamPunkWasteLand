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
using Sce.PlayStation.Core.Audio;

namespace SteamPunkWasteLand
{
	public class MusicBox
	{
		private States preState;
		
		private List<Bgm> musics;
		private List<Sound> sounds;
		
		private BgmPlayer bgmp;
		public BgmPlayer Bgmp
		{
			get{return bgmp;}
			set{bgmp = value;}
		}
		private SoundPlayer sp;
		public SoundPlayer Sp
		{
			get{return sp;}
			set{sp = value;}
		}
		
		public MusicBox ()
		{
			preState = Game.GameState;
			musics = new List<Bgm>();
			sounds = new List<Sound>();
			
			initMusic();
			initSounds();
			
			bgmp = musics[0].CreatePlayer();
			bgmp.Loop = true;
			bgmp.Play();
		}

		public void initMusic ()
		{
			musics.Add(new Bgm("/Application/assets/Music/MainMenuMusic.mp3"));		//0 	Main Menu
			musics.Add(new Bgm("/Application/assets/Music/InGameMusic.mp3"));		//1		In Game
			
		}

		public void initSounds ()
		{
			
		}
		
		public void Update ()
		{
			bgmp.Volume = Game.Music;
			
			if (preState != Game.GameState) {
				if (Game.GameState == States.Play || preState == States.Play) {
					bgmp.Stop();
					bgmp.Dispose();
					switch (Game.GameState) {
					case States.Play:
						bgmp = musics[1].CreatePlayer();
						break;
					default:
						bgmp = musics[0].CreatePlayer();
						break;
					}
					bgmp.Loop = true;
					bgmp.Play();
				}
				preState = Game.GameState;
			}
		}
	}
}

