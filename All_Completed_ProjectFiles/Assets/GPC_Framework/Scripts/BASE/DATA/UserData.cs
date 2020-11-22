namespace GPC
{
	[System.Serializable]
	public class UserData
	{
		public int id;
		public string playerName = "Anonymous"; 
		
		public int score;
		public int level;
		public int health;
		public int lives;
		public int type;
		public bool isFinished;
	}
}