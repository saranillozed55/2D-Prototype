using UnityEngine;

public class ParallaxEffect : MonoBehaviour
{

    private float length, startpos;
    private float startPosY;
    public GameObject cam;
    public float parallaxEffect;

    private void Start()
    {
        cam = FindFirstObjectByType<Camera>().gameObject;

        startpos = transform.position.x;
        startPosY = transform.position.y;
        length = GetComponent<SpriteRenderer>().bounds.size.x;
    }
    private void FixedUpdate()
    {
        //float temp = (cam.transform.position.x * (1 - parallaxEffect));

        float dist = (cam.transform.position.x * parallaxEffect);
        transform.position = new Vector3(startpos + dist, startPosY, transform.position.z);

        //if(temp > startpos + length) startpos += length;
        //else if (temp < startpos - length) startpos -= length;
    }
}
