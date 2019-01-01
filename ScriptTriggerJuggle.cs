using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptTriggerJuggle : MonoBehaviour
{
	private bool _triggerJuggle, _triggerJuggleExit, _triggerPlayerHead;
	private float _shootForce, _juggleForce, _juggleForceSecondary;

	private GameObject _objectPlayer;
	private GameObject _objectBall, _objectSpriteBall;

	ScriptPlayer scriptPlayer;
	ScriptBall scriptBall;
	ScriptSpriteBall scriptSpriteBall;

	/// <summary>
	/// //////////////////////////////////////////////////////////////////////////////////////////////
	/// </summary>


	// Use this for initialization
	void Start ()
	{
		if (this.gameObject.name == "TriggerJuggle")
			_triggerJuggle = true;
		else if (this.gameObject.name == "TriggerJuggleExit")
			_triggerJuggleExit = true;
		else if (this.gameObject.name == "TriggerPlayerHead")
			_triggerPlayerHead = true;


		_objectPlayer = GameObject.Find ("Player");
		scriptPlayer = _objectPlayer.GetComponent <ScriptPlayer> ();

		_objectBall = GameObject.Find ("Ball");
		scriptBall = _objectBall.GetComponent <ScriptBall> ();

		_objectSpriteBall = GameObject.Find ("SpriteBall");
		scriptSpriteBall = _objectSpriteBall.GetComponent <ScriptSpriteBall> ();

		_shootForce = scriptPlayer.shootForce;
		_juggleForce = scriptPlayer.juggleForce;
		_juggleForceSecondary = _juggleForce * 0.5f; // 50% of juggleForce
	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}

	private void OnTriggerEnter2D (Collider2D other)
	{
		
    }

    private void OnTriggerExit2D (Collider2D other)
	{

    }

    private void OnTriggerStay2D (Collider2D other)
	{
		if (_triggerJuggle)
		{
			if (other.name == "SpriteBall")
			{ 	
				if (scriptSpriteBall.ballInAir)
				{
					if (Input.GetKeyDown (KeyCode.F) )
					{
						scriptSpriteBall.JuggleBall (_juggleForceSecondary);
						scriptBall.playerHasBall = true;
					}
				}

				if (Input.GetKeyDown (KeyCode.Return) )
				{
					// PLAYER CAN SHOOT THE BALL AFTER JUGGLING EVEN WHEN BALL DETACHED FROM PLAYER
					JuggleShoot (_shootForce);
				}
			}
		}

		if (_triggerJuggleExit)
		{
			// DETACH BALL FROM PLAYER WHEN PLAYER TURNS THE OTHER WAY WHEN JUGGLING
			if (other.name == "SpriteBall")
			{
				scriptPlayer.SetPlayerHasBall (false, false);
			}
		}

		if (_triggerPlayerHead)
		{
			if (other.name == "SpriteBall")
			{
				// JUGGLE WITH HEAD
				if (Input.GetKeyDown (KeyCode.F) )
				{
					Debug.Log ("JUGGLE WITH HEAD!");

					scriptSpriteBall.JuggleBall (_juggleForceSecondary);
				}

				// SHOOT WITH HEAD
				if (Input.GetKeyDown (KeyCode.Return) )
				{
					Debug.Log ("SHOOT FROM HEAD!");

					JuggleShoot (_shootForce);
				}
			}
		}
		
		return;
    }

	private void JuggleShoot (float sF)
	{
		scriptSpriteBall.playerIsShooting = true;
		scriptBall.playerHasBall = false;
		scriptPlayer.Shoot (sF);
		return;
	}
}
