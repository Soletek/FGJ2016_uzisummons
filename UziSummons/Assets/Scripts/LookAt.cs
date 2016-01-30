using UnityEngine;
using System.Collections;

public class LookAt : MonoBehaviour {

	Vector3 mouse_pos;
	Vector3 obj_pos;
	float angle;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		mouse_pos = Input.mousePosition;
		mouse_pos.z = 10;
		obj_pos = Camera.main.WorldToScreenPoint (transform.position);
		mouse_pos.x = mouse_pos.x - obj_pos.x;
		mouse_pos.y = mouse_pos.y - obj_pos.y;
		angle = Mathf.Atan2 (mouse_pos.y, mouse_pos.x) * Mathf.Rad2Deg;
		transform.rotation = Quaternion.Euler (new Vector3 (0, 0, angle - 90));

	}
}
