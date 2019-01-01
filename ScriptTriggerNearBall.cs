using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptTriggerNearBall : MonoBehaviour
{
	private bool _triggerLayerNearBall, _triggerLayerUp, _triggerLayerDown;

	private GameObject _objectSpriteBall, _objectTriggerBall;

	private SpriteRenderer _ballSpriteLayerOrder;
	private SpriteRenderer _playerSpriteLayerOrder;

	ScriptTriggerBall scriptTriggerBall;
	ScriptSpriteBall scriptSpriteBall;
	
	/// <summary>
	/// //////////////////////////////////////////////////////////////////////////////////////////////
	/// </summary>


	// Use this for initialization
	void Start ()
	{ // CHECK THE NAME OF THE TRIGGER OBJECT TO SET SPRITE DRAWING ORDER
		if (this.gameObject.name == "TriggerNearBall")

			_triggerLayerNearBall = true;
		else if (this.gameObject.name == "TriggerNearBallLayerOrderUp")
			_triggerLayerUp = true;
			
		else if (this.gameObject.name == "TriggerNearBallLayerOrderDown")
			_triggerLayerDown = true;

		
		_objectTriggerBall = GameObject.Find ("TriggerBall");
		scriptTriggerBall = _objectTriggerBall.GetComponent <ScriptTriggerBall> ();

		_objectSpriteBall = GameObject.Find ("SpriteBall");
		scriptSpriteBall = _objectSpriteBall.GetComponent <ScriptSpriteBall> ();

		_ballSpriteLayerOrder = _objectSpriteBall.GetComponent <SpriteRenderer> ();
	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}

	private bool SetPlayerNearBall (bool b)
	{
		return scriptTriggerBall.playerNearBall = scriptSpriteBall.playerNearBall = b;
	}

	private void OnTriggerStay2D (Collider2D other)
	{
		if (other.name == "Player" && _triggerLayerNearBall)
			SetPlayerNearBall (true);


		// SET SPRITE DRAWING ORDER (PRIORITY)
		if (_triggerLayerUp)
		{
			if (other.name == "Player")
			{
				// IF PLAYER ABOVE THE BALL, DRAW SPRITE PLAYER BEHIND SPRITE BALL
				_playerSpriteLayerOrder = other.GetComponentInChildren<SpriteRenderer> ();
				
				if (_playerSpriteLayerOrder.sortingOrder >= _ballSpriteLayerOrder.sortingOrder)
					_ballSpriteLayerOrder.sortingOrder++;
			}
		}

		if (_triggerLayerDown)
		{
			if (other.name == "Player")
			{
				// IF PLAYER BELOW THE BALL, DRAW SPRITE PLAYER IN FRONT OF SPRITE BALL
				_playerSpriteLayerOrder = other.GetComponentInChildren<SpriteRenderer> ();

				if (_playerSpriteLayerOrder.sortingOrder <= _ballSpriteLayerOrder.sortingOrder)
					_ballSpriteLayerOrder.sortingOrder--;
			}
		}

		return;
	}

	private void OnTriggerExit2D (Collider2D other)
	{
		if (_triggerLayerNearBall)
		{
			if (other.name == "Player")
				SetPlayerNearBall (false);
		}

		if (_triggerLayerUp || _triggerLayerDown)
		{
			if (other.name == "Player")
			{	// RESTORE DEFAULT DRAWING ORDER
				if (_ballSpriteLayerOrder != null)
					_ballSpriteLayerOrder.sortingOrder = _playerSpriteLayerOrder.sortingOrder;
			}
		}

		return;
	}
}
