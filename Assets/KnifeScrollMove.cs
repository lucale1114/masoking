using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnifeScrollMove : MonoBehaviour
{
    public SideScroallerKnife SideScroallerKnife;

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector2.down * SideScroallerKnife.currentSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            SideScroallerKnife.GenerateSpike(); 
        }
        if (collision.gameObject.CompareTag("Destroy"))
        {
            Destroy(this.gameObject);
        }

    }


}
