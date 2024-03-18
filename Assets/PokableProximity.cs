using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PokableProximity : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        PokingHand hand = GetComponentInParent<PokingHand>();
        if (tag == "Poke" && other.tag == "Pokable")
        {

            if (!hand.isPoking)
            {
                GetComponent<Collider>().isTrigger = false;
                hand.isPoking = true;
            }


        }
    }

    void OnTriggerExit(Collider other)
    {

        if (tag == "Poke" && other.tag == "Pokable")
        {
            PokingHand hand = GetComponentInParent<PokingHand>();
            if (hand.isPoking)
            {
                GetComponent<Collider>().isTrigger = true;
                hand.isPoking = false;
            }
        }
    }

}