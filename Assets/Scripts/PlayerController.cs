using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;
    private void Awake()
    {
        instance = this;
    }

    public float moveSpeed;

    public Animator anim;

    public float pickupRange = 1.5f;

    //public Weapon activeWeapon;

    public List<Weapon> unassignedWeapons, assignedWeapons;

    public int maxWeapons = 3;

    public List<Image> weaponUISlots = new List<Image>(6);

    [HideInInspector]
    public List<Weapon> fullyLevelledWeapons = new List<Weapon>();

    public GameObject Sprite;

    public Ghost ghost;

    // Start is called before the first frame update
    void Start()
    {
        if (assignedWeapons.Count == 0)
        {
            AddWeapon(Random.Range(0, unassignedWeapons.Count));
        }

        for(int i = 0; i < weaponUISlots.Count; i++)
        {
            weaponUISlots[i].enabled = false;
        }

        UpdateWeaponSlots();

        moveSpeed = PlayerStatController.instance.moveSpeed[0].value;
        pickupRange = PlayerStatController.instance.pickupRange[0].value;
        maxWeapons = Mathf.RoundToInt( PlayerStatController.instance.maxWeapons[0].value);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 moveInput = new Vector3(0f, 0f, 0f);
        moveInput.x = Input.GetAxisRaw("Horizontal");
        moveInput.y = Input.GetAxisRaw("Vertical");

        //Debug.Log(moveInput);

        moveInput.Normalize();

        //Debug.Log(moveInput);

        transform.position += moveInput * moveSpeed * Time.deltaTime;

        if(moveInput != Vector3.zero)
        {
            anim.SetBool("isMoving", true);
            ghost.makeGhost = true;
        }
        else
        {
            anim.SetBool("isMoving", false);
            ghost.makeGhost = false;
        }

        if (moveInput.x > 0)
        {
            Sprite.transform.localScale = new Vector3(2, 2, 2);
        }
        else if (moveInput.x < 0)
        {
            Sprite.transform.localScale = new Vector3(-2, 2, 2);
        }
    }

    public void AddWeapon(int weaponNumber)
    {
        if(weaponNumber < unassignedWeapons.Count)
        {
            assignedWeapons.Add(unassignedWeapons[weaponNumber]);

            unassignedWeapons[weaponNumber].gameObject.SetActive(true);
            unassignedWeapons.RemoveAt(weaponNumber);

            UpdateWeaponSlots();
        }
    }

    public void AddWeapon(Weapon weaponToAdd)
    {
        weaponToAdd.gameObject.SetActive(true);

        assignedWeapons.Add(weaponToAdd);
        unassignedWeapons.Remove(weaponToAdd);

        UpdateWeaponSlots();
    }

    public void UpdateWeaponSlots()
    {
        Debug.Log("Working");

        for(int i = 0; i < assignedWeapons.Count; i++)
        {
            weaponUISlots[i].sprite = assignedWeapons[i].usedIcon;
            weaponUISlots[i].enabled = true;
        }
    }
}
