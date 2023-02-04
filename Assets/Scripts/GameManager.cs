using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private int _currentPlayerIndex;
    private GameObject[] _players;

    // Start is called before the first frame update
    void Awake()
    {
        if (Instance != null && Instance != this) {
            Destroy(this);
        } else {
            Instance = this;
        }
    }
    void Start() {
        _players = GameObject.FindGameObjectsWithTag("Player");
        if (_players.Length > 0) {
            _currentPlayerIndex = 0;
        }
    }

    void Update()
    {
        if (Input.GetButtonDown("Switch Player")) {
            _currentPlayerIndex += 1;
            _currentPlayerIndex %= _players.Length;
        }
    }

    public bool IsCurrentPlayer(GameObject player) {
        _players = GameObject.FindGameObjectsWithTag("Player");
        return _players[_currentPlayerIndex] == player;
    }
}
