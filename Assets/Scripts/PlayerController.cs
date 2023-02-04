using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float Speed;
    public Color Color;

    private static Object _lightPrefab;
    public float NumInitialLights;

    private List<GameObject> _lights;


    // Start is called before the first frame update
    void Start()
    {
        _lightPrefab = Resources.Load("Prefabs/Light");
        _lights = new List<GameObject>();
        for (int i = 0; i < NumInitialLights; i++) {
            AddLight();
        }
    }

    void AddLight() {
        GameObject light = Instantiate(_lightPrefab, Vector2.zero, Quaternion.identity) as GameObject;
        AddLight(light);
    }

    void AddLight(GameObject light) {
        _lights.Add(light);
        light.transform.SetParent(transform);

        PositionLights();
    }

    LightController RemoveLight() {
        int lastIndex = _lights.Count - 1;
        GameObject light = _lights[lastIndex];
        LightController lightController = light.GetComponent<LightController>();
        light.transform.parent = null;
        lightController.Detach();
        _lights.RemoveAt(lastIndex);

        PositionLights();
        return lightController;
    }

    void PositionLights() {
        for (int i = 0; i < _lights.Count; i++) {
            LightController lightController = _lights[i].GetComponent<LightController>();
            Quaternion rotation = Quaternion.AngleAxis((360f / _lights.Count) * i, Vector3.forward);
            lightController.Init(i, rotation, gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.IsCurrentPlayer(gameObject)) {
            Move();
            if (Input.GetButtonDown("Fire1") && _lights.Count > 0) {
                Fire();
            }
        }
    }

    void Move() {
        float inputX = Input.GetAxis("Horizontal");
        float inputY = Input.GetAxis("Vertical");

        transform.Translate(new Vector2(inputX * Speed, inputY * Speed) * Time.deltaTime);
    }

    void Fire() {
        LightController lightController = RemoveLight();
        lightController.Fire();
    }

}
