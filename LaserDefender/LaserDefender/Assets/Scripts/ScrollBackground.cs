using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollBackground : MonoBehaviour
{
	[SerializeField] float scrollSpeedY = 0.2f;
	[SerializeField] float scrollSpeedX = 0.0001f;

	Material material;
	Vector2 offset;

    // Start is called before the first frame update
    void Start()
    {
		material = GetComponent<Renderer>().material;
		offset = new Vector2(scrollSpeedX, scrollSpeedY);
    }

    // Update is called once per frame
    void Update()
    {
		material.mainTextureOffset += offset * Time.deltaTime;
    }
}
