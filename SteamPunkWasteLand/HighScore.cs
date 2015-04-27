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

namespace SteamPunkWasteLand
{
	public class HighScore : IComparable<HighScore>
	{
		private string name;

		public string Name {
			get{ return name;}
			set{ name = value;}
		}
		
		private int score;

		public int Score {
			get{ return score;}
			set{ score = value;}
		}
		
		public HighScore (string name, string score)
		{
			this.name = name;
			if (!score.StartsWith ("--")) {
				this.score = Int32.Parse (score);
			} else {
				this.score = 0;
			}
		}
		
		public int CompareTo (HighScore other)
		{
			if (this.score.CompareTo (other.score) != 0) {
				return this.score.CompareTo (other.score);
			}
			if (this.name.StartsWith ("--")) {
				return -1;
			}
			return 0;	
		}
	}
}

