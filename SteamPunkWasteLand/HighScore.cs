using System;

namespace SteamPunkWasteLand
{
	public class HighScore : IComparable<HighScore>
	{
		private string name;
		public string Name
		{
			get{return name;}
			set{name = value;}
		}
		
		private int score;
		public int Score
		{
			get{return score;}
			set{score = value;}
		}
		
		public HighScore (string name, string score)
		{
			this.name = name;
			if (!score.StartsWith("--")) {
				this.score = Int32.Parse(score);
			}else{
				this.score = 0;
			}
		}
		
		public int CompareTo (HighScore other)
		{
			if(this.score.CompareTo(other.score) != 0){
				return this.score.CompareTo(other.score);
			}
			if (this.name.StartsWith("--")) {
				return -1;
			}
			return 0;	
		}
	}
}

