using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EzySlice;

public class Projectile : MonoBehaviour
{
    public GameManager GManager;

    private Vector3 TopSwordPoint;
    private Vector3 BottomSwordPoint;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Rigidbody>().velocity = new Vector3(0, 0, -3.0f);
        GetComponent<Rigidbody>().angularVelocity = new Vector3(Random.Range(-2.0f, 2.0f), Random.Range(-2.0f, 2.0f), Random.Range(-2.0f, 2.0f));
    }

    // Update is called once per frame
    void Update()
    {
        if(transform.position.z < -10.0f)   //player missed object, reset multiplier and destroy
        {
            GManager.ObjectMissed();
            Destroy(gameObject);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "LongSwordMesh")
        {
            TopSwordPoint = other.transform.position + other.bounds.max;
            BottomSwordPoint = other.transform.position + other.bounds.min;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.name == "LongSwordMesh")
        {
            //play chopping sound
            other.GetComponent<AudioSource>().Play();

            Vector3 exitingTopSwordPoint = other.transform.position + other.bounds.max;

            //Debug.Log("TopIn = " + TopSwordPoint + " Bottom In = " + BottomSwordPoint + " Top Out = " + exitingTopSwordPoint);

            Material mat = GetComponent<MeshRenderer>().material;

            EzySlice.Plane plane = new EzySlice.Plane(TopSwordPoint, BottomSwordPoint, exitingTopSwordPoint);

            SlicedHull hull = gameObject.Slice(plane, mat);

            if (hull == null)
                hull = gameObject.Slice(transform.position, transform.up, mat); //slicing failed with the plane. Just cut it in half

            if (hull == null)   //if still null, there's an issue. Make object drop and give the player the points
            {
                Destroy(gameObject, 5);  //clean up after a few seconds
                gameObject.GetComponent<Rigidbody>().useGravity = true;
            }
            else
            { 
                //create two objects, the initial object cut in half, add colliders with a rigidbody and gravity. Also add force so they jump apart before falling to the ground
                GameObject lowerHull = hull.CreateLowerHull(gameObject, mat);
                GameObject upperHull = hull.CreateUpperHull(gameObject, mat);

                MeshCollider lowerHullMc = lowerHull.AddComponent<MeshCollider>();
                MeshCollider upperHulllMc = upperHull.AddComponent<MeshCollider>();

                lowerHullMc.convex = true;
                upperHulllMc.convex = true;

                Rigidbody lowerHullRb = lowerHull.AddComponent<Rigidbody>();
                Rigidbody upperHullRb = upperHull.AddComponent<Rigidbody>();

                lowerHullRb.AddForce(plane.normal * 100);
                upperHullRb.AddForce(-plane.normal * 100);

                Destroy(lowerHull, 5);  //clean up after a few seconds
                Destroy(upperHull, 5);


                Destroy(gameObject);
            }

            GManager.ObjectHit();

        }
    }
}
