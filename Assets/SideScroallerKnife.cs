using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SideScroallerKnife : MonoBehaviour
{
    public GameObject Knife;

    public float minSpeed;
    public float maxSpeed;
    public float currentSpeed;
    // Start is called before the first frame update
    void Awake()
    {
        currentSpeed = minSpeed;
        GenerateSpike();
    }

    public void GenerateSpike()
    {
        GameObject KnifeIns = Instantiate(Knife, transform.position, transform.rotation);

        KnifeIns.GetComponent<KnifeScrollMove>().SideScroallerKnife = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
