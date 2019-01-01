using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptTriggerBall : MonoBehaviour
{
	public bool playerNearBall = false;

	private GameObject _objectBall, _objectSpriteBall;
	private GameObject _objectPlayer, _objectSpritePlayer;

	ScriptBall scriptBall;
	ScriptSpriteBall scriptSpriteBall;
	ScriptPlayer scriptPlayer;
	ScriptSpritePlayer scriptSpritePlayer;

	/// <summary>
	/// //////////////////////////////////////////////////////////////////////////////////////////////
	/// </summary>


	// Use this for initialization
	void Start ()
	{
		_objectBall = GameObject.Find ("Ball");
		scriptBall = _objectBall.GetComponent <ScriptBall> ();

		_objectSpriteBall = GameObject.Find ("SpriteBall");
		scriptSpriteBall = _objectSpriteBall.GetComponent <ScriptSpriteBall> ();

		_objectPlayer = GameObject.Find ("Player");
		scriptPlayer = _objectPlayer.GetComponent <ScriptPlayer> ();

		_objectSpritePlayer = GameObject.Find ("SpritePlayer");
		scriptSpritePlayer = _objectSpritePlayer.GetComponent <ScriptSpritePlayer> ();
	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}

	// This function is called every fixed framerate frame (physics frame)
	void FixedUpdate ()
	{

	}

	public bool SetPlayerHasBall (bool b)
	{
		return scriptBall.playerHasBall = scriptSpriteBall.playerHasBall = scriptPlayer.playerHasBall = b;
	}

	public bool SetPlayerOnBall (bool b)
	{
		return scriptBall.playerOnBall = scriptSpriteBall.playerOnBall = scriptSpritePlayer.playerOnBall = b;
	}

	private void OnTriggerEnter2D (Collider2D other)
	{
		// WHEN PLAYER LANDS ON THE BALL
		if (other.name == "TriggerPlayerFeet" && playerNearBall)
		{
			Debug.Log ("Player LANDS ON BALL");
			
			if (!scriptSpritePlayer.playerInAir || !scriptSpriteBall.ballInAir)
			{
				SetPlayerHasBall (true);
				SetPlayerOnBall (true);
			}
		}
		return;
	}

	private void OnTriggerExit2D (Collider2D other)
	{

	}

	private void OnTriggerStay2D (Collider2D other)
	{
		// WHEN PLAYER ON BALL (TO JUMP AWAY)
		if (other.name == "TriggerPlayerFeet" && Input.GetKeyDown (KeyCode.Space))
		{
			if (!scriptSpritePlayer.playerInAir && !scriptSpriteBall.ballInAir)
			{
				SetPlayerHasBall (true);
				SetPlayerOnBall (true);
			}
		}
		return;
	}

	private void OnCollisionEnter2D (Collision2D collision)
	{

	}

	private void OnCollisionExit2D (Collision2D collision)
	{

	}
	private void OnCollisionStay2D (Collision2D collision)
	{

	}
}
