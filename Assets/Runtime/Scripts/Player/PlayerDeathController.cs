using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeathController : MonoBehaviour
{
    [SerializeField] PlayerController _playerController;
    [SerializeField] PlayerInputs _playerInputs;
    [SerializeField] PlayerAnimationController _playerAnimation;
    [SerializeField] GameMode _gameMode;

    private bool _playerIsDied;
    public bool PlayerIsDead => _playerIsDied;
    public PlayerController Controller => _playerController;

    public void Die() 
    {
        if(!_playerIsDied)
        {
            _playerController.Die();
            _playerAnimation.EnableAnimations(false);
            _playerInputs.EnableInputs(false);
            _playerIsDied = true;
            _gameMode.GameOver();
        }    
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(!_playerIsDied && other.GetComponent<Pipe>() is Pipe pipe)
        {
            _playerController.Die();
            _playerAnimation.EnableAnimations(false);
            _playerInputs.EnableInputs(false);
            _playerIsDied = true;
        }    
    }
}