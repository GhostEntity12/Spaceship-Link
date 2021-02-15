using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
	public int health;
	public int damage;
	public int points;
	public int moveSpeed;

	public void DestroyEnemy()
	{

	}
}
