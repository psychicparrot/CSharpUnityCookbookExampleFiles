using UnityEngine;
namespace GPC
{
	[AddComponentMenu("CSharpBookCode/Base/Enemy Controller")]

	public class BaseEnemyStatsController : BaseAIController
	{
		[Header("Enemy stats")]
		public int myID;
		private static int enemyUniqueID;

		public int defaultHealthAmount = 1;

		public int enemyHealth;
		public int enemyScore;
		public string enemyName;

		public override void Init()
		{
			base.Init();

			myID = enemyUniqueID;
			enemyUniqueID++;

			enemyScore = 0;
			enemyHealth = defaultHealthAmount;
		}


		public virtual void AddScore(int anAmount)
		{
			enemyScore += anAmount;
		}

		public virtual void AddHealth(int anAmount)
		{
			enemyHealth += anAmount;
		}

		public virtual void LoseScore(int anAmount)
		{
			enemyScore -= anAmount;
		}

		public virtual void ReduceHealth(int anAmount)
		{
			enemyHealth -= anAmount;
		}

		public virtual void SetScore(int anAmount)
		{
			enemyScore = anAmount;
		}

		public virtual void SetHealth(int anAmount)
		{
			enemyHealth = anAmount;
		}

		public int GetHealth()
		{
			return enemyHealth;
		}

		public int GetScore()
		{
			return enemyScore;
		}

		public void SetName(string aName)
		{
			enemyName = aName;
		}
	}
}