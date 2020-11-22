using UnityEngine;
using System.Collections.Generic;

namespace GPC
{
	[AddComponentMenu("CSharpBookCode/Base/Base User Manager")]

	public class BaseUserManager : MonoBehaviour
	{
		public static List<UserData> global_userDatas;
		public bool didInit;

		public void Init()
		{
			if (global_userDatas == null)
				global_userDatas = new List<UserData>();

			didInit = true;
		}

		public void ResetUsers()
		{
			if (!didInit)
				Init();

			global_userDatas = new List<UserData>();
		}

		public List<UserData> GetPlayerList()
		{
			if (global_userDatas == null)
				Init();

			return global_userDatas;
		}

		public int AddNewPlayer()
		{
			if (!didInit)
				Init();

			UserData newUser = new UserData();

			newUser.id = global_userDatas.Count;
			newUser.playerName = "Anonymous";
			newUser.score = 0;
			newUser.level = 1;
			newUser.health = 3;
			newUser.lives = 3;
			newUser.isFinished = false;

			global_userDatas.Add(newUser);

			return newUser.id;
		}

		public string GetName(int id)
		{
			if (!didInit)
				Init();

			return global_userDatas[id].playerName;
		}

		public void SetName(int id, string aName)
		{
			if (!didInit)
				Init();

			global_userDatas[id].playerName = aName;
		}

		public int GetLevel(int id)
		{
			if (!didInit)
				Init();

			return global_userDatas[id].level;
		}

		public void SetLevel(int id, int num)
		{
			if (!didInit)
				Init();

			global_userDatas[id].level = num;
		}

		public int GetScore(int id)
		{
			if (!didInit)
				Init();

			return global_userDatas[id].score;
		}

		public virtual void AddScore(int id, int anAmount)
		{
			if (!didInit)
				Init();

			global_userDatas[id].score += anAmount;
		}

		public void ReduceScore(int id, int num)
		{
			if (!didInit)
				Init();

			global_userDatas[id].score -= num;
		}

		public void SetScore(int id, int num)
		{
			if (!didInit)
				Init();

			global_userDatas[id].score = num;
		}

		public int GetHealth(int id)
		{
			if (!didInit)
				Init();

			return global_userDatas[id].health;
		}

		public int GetLives(int id)
		{
			if (!didInit)
				Init();

			return global_userDatas[id].lives;
		}

		public void SetLives(int id, int num)
		{
			if (!didInit)
				Init();

			global_userDatas[id].lives = num;
		}

		public void AddLives(int id, int num)
		{
			if (!didInit)
				Init();

			global_userDatas[id].lives += num;
		}

		public void ReduceLives(int id, int num)
		{
			if (!didInit)
				Init();

			global_userDatas[id].lives -= num;
		}

		public void AddHealth(int id, int num)
		{
			if (!didInit)
				Init();

			global_userDatas[id].health += num;
		}

		public void ReduceHealth(int id, int num)
		{
			if (!didInit)
				Init();

			global_userDatas[id].health -= num;
		}

		public void SetHealth(int id, int num)
		{
			if (!didInit)
				Init();

			global_userDatas[id].health = num;
		}

		public bool GetIsFinished(int id)
		{
			if (!didInit)
				Init();

			return global_userDatas[id].isFinished;
		}

		public void SetType(int id, int theType)
		{
			if (!didInit)
				Init();

			global_userDatas[id].type = theType;
		}

		public int GetType(int id)
		{
			if (!didInit)
				Init();

			return global_userDatas[id].type;
		}

		public void SetIsFinished(int id, bool aVal)
		{
			if (!didInit)
				Init();

			global_userDatas[id].isFinished = aVal;
		}
	}
}