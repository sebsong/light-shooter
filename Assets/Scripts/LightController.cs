using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightController : MonoBehaviour
{
    public float FireSpeed;
    public float RotateSpeed;
    public float MoveSpeed;
    public float Distance;
    public Color NeutralColor;

    private Rigidbody2D _rigidbody;
    private int _index;
    private Quaternion _rotation;
    private GameObject _player;
    private Vector2 _fireDirection;

    // Start is called before the first frame update
    void Start(){
        _rigidbody = GetComponent<Rigidbody2D>();
        _fireDirection = Vector2.zero;
    }

    public void Init(int index, Quaternion rotation, GameObject player) {
        _index = index;
        _rotation = rotation;
        Attach(player);
    }

    public void Attach(GameObject player) {
        _player = player;
        SetColor();
    }

    public void Detach() {
        _player = null;
        SetColor();
    }

    public void Fire() {
        _fireDirection = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position);
        _fireDirection.Normalize();
    }

    private bool IsFiring() {
        return _fireDirection != Vector2.zero;
    }

    private void StopFiring() {
        _fireDirection = Vector2.zero;
    }

    // Update is called once per frame
    void Update()
    {
        if (IsFiring()) {
            transform.position += (Vector3) (FireSpeed * _fireDirection * Time.deltaTime);
            // _rigidbody.MovePosition((Vector2) transform.position + (FireSpeed * _fireDirection * Time.deltaTime));
        } else if (_player) {
            Position();
        }
    }

    void Position() {
        _rotation *= Quaternion.AngleAxis(-RotateSpeed * Time.deltaTime, Vector3.forward);
        Vector2 position = _player.transform.position + (_rotation * Vector2.up * Distance);
        transform.position = Vector2.Lerp(transform.position, position, MoveSpeed * Time.deltaTime);
        // _rigidbody.MovePosition((Vector2) transform.position + (position * MoveSpeed * Time.deltaTime));
    }

    void SetColor() {
        Light light = GetComponent<Light>();
        TrailRenderer trailRenderer = GetComponent<TrailRenderer>();
        Color color;
        if (_player) {
            color = _player.GetComponent<PlayerController>().Color;
        } else {
            color = NeutralColor;
        }

        light.color = color;
        trailRenderer.startColor = color;
        trailRenderer.endColor = color;
    }

    void OnCollisionEnter2D(Collision2D other) {
        if (IsFiring()){
            switch(other.gameObject.tag){
                case "Wall":
                    StopFiring();
                    Detach();
                    break;
                case "Player":
                    if (other.gameObject == _player) {
                        break;
                    }
                    StopFiring();
                    PlayerController playerController = _player.GetComponent<PlayerController>();
                    PlayerController otherPlayerController = other.gameObject.GetComponent<PlayerController>();
                    playerController.AddLight(gameObject);
                    playerController.AddLight(otherPlayerController.RemoveLight());
                    break;
                default:
                    break;
            }
        } else {
            switch(other.gameObject.tag){
                case "Player":
                    if (_player == null) {
                        PlayerController otherPlayerController = other.gameObject.GetComponent<PlayerController>();
                        otherPlayerController.AddLight(gameObject);
                    }
                    break;
                default:
                    break;
            }
        }
    }

}
