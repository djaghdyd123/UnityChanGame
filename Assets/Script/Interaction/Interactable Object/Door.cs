using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : Interactable
{
    float targetYRotation;

    public float smooth = 10f;
    public bool autoClose = true;

    

    float defaultYRotation = 0f;
    float timer = 0f;

    bool isOpen;

    void Start()
    {
        SetPlayer(Managers.Game.GetPlayer());
    }

    void Update()
    {
        gameObject.transform.parent.rotation = Quaternion.Lerp(gameObject.transform.parent.rotation, Quaternion.Euler(0f, defaultYRotation + targetYRotation, 0f), smooth * Time.deltaTime);

        timer -= Time.deltaTime;

        if (timer <= 0f && isOpen && autoClose)
        {
            ToggleDoor(_player.transform.position);
        }
    }

    public void ToggleDoor(Vector3 pos)
    {
        isOpen = !isOpen;

        if (isOpen)
        {
            Vector3 dir = (pos - transform.position);
            targetYRotation = -Mathf.Sign(Vector3.Dot(transform.right, dir)) * 90f;
            timer = 5f;
        }
        else
        {
            targetYRotation = 0f;
        }
    }

    public void Open(Vector3 pos)
    {
        if (!isOpen)
        {
            ToggleDoor(pos);
        }
    }
    public void Close(Vector3 pos)
    {
        if (isOpen)
        {
            ToggleDoor(pos);
        }
    }

    public override string GetDescription()
    {
        if (isOpen) return "Close the door";
        return "Open the door";
    }

    public override void Interact()
    {
        ToggleDoor(_player.transform.position);
    }
}
