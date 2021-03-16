using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "DeliveryBox")
        {
            other.gameObject.GetComponent<DeliveryBox>().doorTriggered(transform.GetComponentInParent<Shop>().location);

        }
        if (other.gameObject.GetComponent<Character>() != null)
        {
            other.gameObject.GetComponent<Character>().doorTriggered(transform.GetComponentInParent<Shop>().location);
        }
    }
}
