using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

[AddComponentMenu("Nokobot/Modern Guns/Simple Shoot")]
public class SimpleShoot : MonoBehaviour
{
    [Header("Prefab Refrences")]
    public GameObject bulletPrefab;
    public GameObject casingPrefab;
    public GameObject muzzleFlashPrefab;
    public GameObject bloodParticle;
    

    [Header("Location Refrences")]
    [SerializeField] private Animator gunAnimator;
    [SerializeField] private Transform barrelLocation;
    [SerializeField] private Transform casingExitLocation;

    [Header("Settings")]
    [Tooltip("Specify time to destory the casing object")] [SerializeField] private float destroyTimer = 2f;
    [Tooltip("Bullet Speed")] [SerializeField] private float shotPower = 500f;
    [Tooltip("Casing Ejection Speed")] [SerializeField] private float ejectPower = 150f;
    [SerializeField] float maxDistance = 150f;
    [SerializeField] float damage = 25f;

    [SerializeField] int bulletChambered;

    public AudioSource source;
    public AudioClip fireSound;

    public Magazine magazine;
    public GameObject staticMagazine;
    public XRBaseInteractor socketInteractor;
    [SerializeField] GrabDetection grabDetection;

    [SerializeField] private bool hasSlide = true;

    public HandController[] hc;
    

    void Start()
    {
        if (barrelLocation == null)
            barrelLocation = transform;

        if (gunAnimator == null)
            gunAnimator = GetComponentInChildren<Animator>();

        socketInteractor.onSelectEntered.AddListener(AddMagazine);
        socketInteractor.onSelectExited.AddListener(RemoveMagazine);
        staticMagazine.GetComponentInChildren<MeshRenderer>().enabled = false;
        grabDetection = FindObjectOfType<GrabDetection>();
        magazine.numberOfBullets--;

        //hc = GameObject.FindObjectsOfType<HandController>();

    }

    public void PullTrigger()
    {
        if(magazine && magazine.numberOfBullets > 0 && hasSlide || bulletChambered == 1)
        {           
            gunAnimator.SetTrigger("Fire");
            bulletChambered--;
            magazine.numberOfBullets--;
            if(magazine.numberOfBullets >= 0)
            {
                bulletChambered++;
            }
            
            RaycastHit hit;

            if (Physics.Raycast(barrelLocation.position, barrelLocation.forward, out hit, maxDistance))
            {
                Debug.Log(hit.transform.name);
                if (hit.collider.tag == "Enemy")
                {
                    Enemy enemy = hit.transform.GetComponent<Enemy>();
                    if (enemy != null)
                    {
                        Instantiate(bloodParticle, hit.transform.position, Quaternion.identity);
                        enemy.takeDamage(damage);
                    }
                }
                Debug.DrawRay(barrelLocation.position, barrelLocation.forward * maxDistance, Color.red);
            }
        }
        else
        {
            // play empty gun sound
        }
       
    }


    //This function creates the bullet behavior
    void Shoot()
    {
        if (muzzleFlashPrefab)
        {
            //Create the muzzle flash
            if(grabDetection.handType == "LHand")
            {
                hc[0].SendImpulse("ShootLeft");
            }
            else
            {
                hc[1].SendImpulse("ShootRight");
            }

            GameObject tempFlash;
            tempFlash = Instantiate(muzzleFlashPrefab, barrelLocation.position, barrelLocation.rotation);

            //play sound
            source.PlayOneShot(fireSound);

            //Destroy the muzzle flash effect
            Destroy(tempFlash, destroyTimer);
            
        }

        //cancels if there's no bullet prefeb
        if (!bulletPrefab)
        { return; }

        // Create a bullet and add force on it in direction of the barrel
        Instantiate(bulletPrefab, barrelLocation.position, barrelLocation.rotation).GetComponent<Rigidbody>().AddForce(barrelLocation.forward * shotPower);

    }

    //This function creates a casing at the ejection slot
    void CasingRelease()
    {
        //Cancels function if ejection slot hasn't been set or there's no casing
        if (!casingExitLocation || !casingPrefab)
        { return; }

        //Create the casing
        GameObject tempCasing;
        tempCasing = Instantiate(casingPrefab, casingExitLocation.position, casingExitLocation.rotation) as GameObject;
        //Add force on casing to push it out
        tempCasing.GetComponent<Rigidbody>().AddExplosionForce(Random.Range(ejectPower * 0.7f, ejectPower), (casingExitLocation.position - casingExitLocation.right * 0.3f - casingExitLocation.up * 0.6f), 1f);
        //Add torque to make casing spin in random direction
        tempCasing.GetComponent<Rigidbody>().AddTorque(new Vector3(0, Random.Range(100f, 500f), Random.Range(100f, 1000f)), ForceMode.Impulse);

        //Destroy casing after X seconds
        Destroy(tempCasing, destroyTimer);
    }
    void AddMagazine(XRBaseInteractable interactable)
    {
        magazine = interactable.GetComponent<Magazine>();
        magazine.GetComponentInChildren<MeshRenderer>().enabled = false;
        staticMagazine.GetComponentInChildren<MeshRenderer>().enabled = true;      

        if(bulletChambered != 1)
        {
            hasSlide = false;
        }    
    }

    void RemoveMagazine(XRBaseInteractable interactable)
    {
        magazine.GetComponentInChildren<MeshRenderer>().enabled = true;
        staticMagazine.GetComponentInChildren<MeshRenderer>().enabled = false;
        magazine = null;
        if(bulletChambered == 1)
        {
            hasSlide = true;
        }
        else
        {
            hasSlide = false;
        }
    }

    public void Slide()
    {
        hasSlide = true;
        if(magazine.numberOfBullets < 0 && bulletChambered < 1)
        {
            bulletChambered = 0;
            hasSlide = false;
        }
        else if(magazine.numberOfBullets > 0)
        {
            bulletChambered = 1;
            magazine.numberOfBullets--;
        }
        

    }

}
