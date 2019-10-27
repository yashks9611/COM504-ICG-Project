using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
	[Header("EnemyAttributes")]
	[SerializeField] float health = 100;
	[SerializeField] int scoreValue = 150;

	[Header("EnemyProjectile")]
	[SerializeField] float shotCounter;
	[SerializeField] float minTimeBetweenShots = 0.3f;
	[SerializeField] float maxTimeBetweenShots = 3f;
	[SerializeField] GameObject enemyLazerPrefab;
	[SerializeField] float enemyLazerSpeed = 10f;
	[SerializeField] AudioClip weaponSFX;
	[SerializeField] [Range(0, 1)] float weaponSFXVolume = 0.75f;

	[Header("EnemyDeathAttributes")]
	[SerializeField] GameObject explodeVFX;
	[SerializeField] float explodeDuration = 1f;
	[SerializeField] AudioClip deathSFX;
	[SerializeField] [Range(0, 1)] float deathSFXVolume = 0.75f;

	// Start is called before the first frame update
	void Start()
    {
		shotCounter = UnityEngine.Random.Range(minTimeBetweenShots, maxTimeBetweenShots);
    }

    // Update is called once per frame
    void Update()
    {
		CountDownToShoot();
    }

	private void CountDownToShoot()
	{
		shotCounter -= Time.deltaTime;
		if (shotCounter <= 0)
		{
			Fire();
			shotCounter = UnityEngine.Random.Range(minTimeBetweenShots, maxTimeBetweenShots);
		}
	}

	private void Fire()
	{
		GameObject enemyLazer = Instantiate(enemyLazerPrefab, transform.position, Quaternion.identity) as GameObject;
		enemyLazer.GetComponent<Rigidbody2D>().velocity = new Vector2(0, -enemyLazerSpeed);
		AudioSource.PlayClipAtPoint(weaponSFX, Camera.main.transform.position, weaponSFXVolume);
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		DamageDealer damageDealt = collision.gameObject.GetComponent<DamageDealer>();
		if (!damageDealt)
		{
			return;
		}
		EnemyHit(damageDealt);
	}

	private void EnemyHit(DamageDealer damageDealt)
	{
		health -= damageDealt.GetDamage();
		damageDealt.Hit();
		if (health <= 0)
		{
			EnemyDestroy();
		}
	}

	private void EnemyDestroy()
	{
		FindObjectOfType<GameSession>().AddToScore(scoreValue);
		Destroy(gameObject);
		GameObject explosion = Instantiate(explodeVFX, transform.position, transform.rotation);
		Destroy(explosion, explodeDuration);

		AudioSource.PlayClipAtPoint(deathSFX, Camera.main.transform.position, deathSFXVolume);
	}
}
