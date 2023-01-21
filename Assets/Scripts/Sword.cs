using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Sword : MonoBehaviour
{
    private bool Clicking = false;
    private Button LastClicked;
    private IEnumerator coroutine;

    void Start()
    {

    }


    void Update()
    {
        // Bit shift the index of the layer (5) to get a bit mask
        // This casts rays only against colliders in layer 5.
        int layerMask = 1 << 5;


        RaycastHit hit;

        Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.up) * 0.35f, Color.green);
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.up * 0.35f), out hit, Mathf.Infinity, layerMask))
        {
            if (!Clicking)
            {
                Debug.Log("Hit " + hit.collider.name);

                Button btn = hit.collider.GetComponent<Button>();


                if (btn)
                {
                    LastClicked = btn;
                    Clicking = true;
                    coroutine = IsClicking(btn);
                    StartCoroutine(coroutine);
                }
            }
        }
        else
        {
            if (Clicking)
            {
                Clicking = false;
                ColorBlock colors = LastClicked.colors;
                colors.normalColor = Color.red;
                LastClicked.colors = colors;
                StopCoroutine(coroutine);
                Debug.Log("No Hit");
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "UI")
        {
            Debug.Log("Intersecting " + other.name);
        }
    }

    IEnumerator IsClicking(Button btn)
    {
        ColorBlock colors = btn.colors;
        colors.normalColor = Color.green;
        btn.colors = colors;

        yield return new WaitForSeconds(1.0f);

        colors.normalColor = Color.red;
        btn.colors = colors;

        if (Clicking)
            btn.onClick.Invoke();   //invoke click behaviour as if clicking

    }
}
