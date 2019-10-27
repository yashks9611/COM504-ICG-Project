using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
	//Configuration Parameters
	[Header("Player Attributes")]
	[SerializeField] float moveSpeed = 15f;
	[SerializeField] float padding = 1f;
	[SerializeField] int health = 200;

	[Header("Projectile")]
	[SerializeField] GameObject lazer1;
	[SerializeField] GameObject lazer2;
	[SerializeField] GameObject missile;
	[SerializeField] float lazerSpeed = 18f;
	[SerializeField] float missileSpeed = 12f;
	[SerializeField] float lazerFiringPeriod = 0.1f;
	[SerializeField] float missileFiringPeriod = 0.1f;

	[Header("SoundEffects")]
	[SerializeField] AudioClip deathSFX;
	[SerializeField] [Range(0, 1)] float deathSFXVolume = 0.75f;
	[SerializeField] AudioClip lazerSFX;
	[SerializeField] [Range(0, 1)] float lazerSFXVolume = 0.75f;	
	[SerializeField] AudioClip missileSFX;
	[SerializeField] [Range(0, 1)] float missileSFXVolume = 0.5f;

	float xMin;
	float xMax;

	float yMin;
	float yMax;

	Coroutine fireLazerCoroutine;
	Coroutine fireMissileCoroutine;
	Boolean missileFiring = false;
	Boolean lazerFiring = false;
	// Start is called before the first frame update
	void Start()
    {
		SetUpMovementBounds();   
    }

	// Update is called once per frame
	void Update()
    {
		Move();
		Fire();
    }

	private void Fire()
	{
		if (Input.GetButtonDown("Fire1"))
		{
			if (missileFiring)
			{
				StopCoroutine(fireMissileCoroutine);
			}
			
			fireLazerCoroutine = StartCoroutine(FireLazerContinuously());
		}
		if (Input.GetButtonDown("Fire2"))
		{
			if (lazerFiring)
			{
				StopCoroutine(fireLazerCoroutine);
			}
			fireMissileCoroutine = StartCoroutine(FireMissileContinuously());
		}
		if (Input.GetButtonUp("Fire1"))
		{
			StopCoroutine(fireLazerCoroutine);
		}
		if (Input.GetButtonUp("Fire2"))
		{
			StopCoroutine(fireMissileCoroutine);
		}
	}

	IEnumerator FireLazerContinuously()
	{
		lazerFiring = true;
		while (true)
		{
			var lazer1Pos = new Vector3(transform.position.x - 0.5f, transform.position.y + 0.4f, transform.position.z);
			var lazer2Pos = new Vector3(transform.position.x + 0.5f, transform.position.y + 0.4f, transform.position.z);

			GameObject playerLazer1 = Instantiate(lazer1, lazer1Pos, Quaternion.identity) as GameObject;
			GameObject playerLazer2 = Instantiate(lazer2, lazer2Pos, Quaternion.identity) as GameObject;

			playerLazer1.GetComponent<Rigidbody2D>().velocity = new Vector2(0, lazerSpeed);
			playerLazer2.GetComponent<Rigidbody2D>().velocity = new Vector2(0, lazerSpeed);

			AudioSource.PlayClipAtPoint(lazerSFX, Camera.main.transform.position, lazerSFXVolume);

			yield return new WaitForSeconds(lazerFiringPeriod);
		}
	}

	IEnumerator FireMissileContinuously()
	{
		missileFiring = true;
		while (true)
		{
			var missilePos = new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z);

			GameObject playerMissile = Instantiate(missile, missilePos, Quaternion.identity) as GameObject;

			playerMissile.GetComponent<Rigidbody2D>().velocity = new Vector2(0, missileSpeed);

			AudioSource.PlayClipAtPoint(missileSFX, Camera.main.transform.position, missileSFXVolume);

			yield return new WaitForSeconds(missileFiringPeriod);
		}
	}
	private void Move()
	{
		var deltaX = Input.GetAxis("Horizontal") * Time.deltaTime * moveSpeed;
		var deltaY = Input.GetAxis("Vertical") * Time.deltaTime * moveSpeed;

		var newXPos = Mathf.Clamp(transform.position.x + deltaX, xMin, xMax);
		var newYPos = Mathf.Clamp(transform.position.y + deltaY, yMin, yMax);
		transform.position = new Vector2(newXPos, newYPos);
	}

	private void SetUpMovementBounds()
	{
		Camera gameCamera = Camera.main;

		xMin = gameCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).x + padding;
		xMax = gameCamera.ViewportToWorldPoint(new Vector3(1, 0, 0)).x - padding;

		yMin = gameCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).y + padding;
		yMax = gameCamera.ViewportToWorldPoint(new Vector3(0, 1, 0)).y - padding;
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		DamageDealer damageDealt = collision.gameObject.GetComponent<DamageDealer>();
		if (!damageDealt)
		{
			return;
		}
		PlayerHit(damageDealt);
	}

	private void PlayerHit(DamageDealer damageDealt)
	{
		health -= damageDealt.GetDamage();
		damageDealt.Hit();
		if (health <= 0)
		{
			Death();
			Destroy(gameObject);
		}
	}

	private void Death()
	{
		FindObjectOfType<Level>().LoadGameOver();
		Destroy(gameObject);
		AudioSource.PlayClipAtPoint(deathSFX, Camera.main.transform.position, deathSFXVolume);
	}
}
