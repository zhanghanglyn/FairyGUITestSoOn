using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireLight : MonoBehaviour {

    public GameObject m_fire;
    private Material fire_material;

	// Use this for initialization
	void Start () {
		if (m_fire != null)
        {
            fire_material = gameObject.GetComponent<Renderer>().material;
            if (fire_material != null)
            {
                Vector3 fireScreenPos = Camera.main.WorldToScreenPoint(m_fire.transform.position);
                fireScreenPos.x /= Screen.width;
                fireScreenPos.y /= Screen.height;

                fire_material.SetVector("FireScreenPos", fireScreenPos);
                Debug.Log(fireScreenPos);
            }
        }

	}
	
	// Update is called once per frame
	void Update () {
        if (fire_material != null)
        {
            Vector3 fireScreenPos = Camera.main.WorldToScreenPoint(m_fire.transform.position);
            fireScreenPos.x /= Screen.width;
            fireScreenPos.y /= Screen.height;

            fire_material.SetVector("FireScreenPos", fireScreenPos);
        }
    }


}
