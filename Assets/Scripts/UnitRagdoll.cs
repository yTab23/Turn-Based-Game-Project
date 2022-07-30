using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitRagdoll : MonoBehaviour
{
    [SerializeField] private Transform ragdollRootBone;
    [SerializeField] private Transform weapon;
    [SerializeField] private float explosionForce;
    [SerializeField] private float modifiedExplosionForce;
    [SerializeField] private float explosionRange;
    
    private float randomMultiplier;


    public void Setup (Transform _originalRootBone)
    {
    	MatchAllChildTransforms (_originalRootBone, ragdollRootBone);
     
    	randomMultiplier = Random.Range (0.5f, 1.2f);
    	modifiedExplosionForce = randomMultiplier * explosionForce;
     
        // Added code to separate the weapon from the unit
    	weapon.parent = null;
    	Rigidbody weaponRigidbody = weapon.gameObject.AddComponent<Rigidbody> ();
    	weaponRigidbody.collisionDetectionMode = CollisionDetectionMode.Continuous;
     
    	ApplyExplosionToRagdoll (ragdollRootBone, transform.position);
    }

    private void MatchAllChildTransforms(Transform root, Transform clone)
    {
        foreach (Transform child in root)
        {
            Transform cloneChild = clone.Find(child.name);
            if (cloneChild != null) 
            {
                cloneChild.position = child.position;
                cloneChild.rotation = child.rotation;

                MatchAllChildTransforms(child, cloneChild);
            }
        }
    }

    private void ApplyExplosionToRagdoll(Transform root, Vector3 explosionPosition)
    {
        foreach (Transform child in root)
        {
            if(child.TryGetComponent<Rigidbody>(out Rigidbody childRigidbody))
            {
                childRigidbody.AddExplosionForce(explosionForce, explosionPosition, explosionRange);
            }

            ApplyExplosionToRagdoll(child, explosionPosition);
        }
    }
}
