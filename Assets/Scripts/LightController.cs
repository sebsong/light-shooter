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

    private int _index;
    private Quaternion _rotation;
    private GameObject _player;
    private Vector2 _fireDirection;

    // Start is called before the first frame update
    void Start(){}

    public void Init(int index, Quaternion rotation, GameObject player) {
        _index = index;
        _rotation = rotation;
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

    // Update is called once per frame
    void Update()
    {
        if (_player) {
            Position();
        }

        if (_fireDirection != null) {
            transform.position += (Vector3) (FireSpeed * _fireDirection * Time.deltaTime);
        }
    }

    void Position() {
        _rotation *= Quaternion.AngleAxis(-RotateSpeed * Time.deltaTime, Vector3.forward);
        Vector2 position = _player.transform.position + (_rotation * Vector2.up * Distance);
        transform.position = Vector2.Lerp(transform.position, position, MoveSpeed * Time.deltaTime);
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

}
