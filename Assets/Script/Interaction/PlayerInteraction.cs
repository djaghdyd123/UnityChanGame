using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField]
    public float interactionDistance = 10.0f;

    UI_Description description;
    private void Start()
    {
        description = Managers.UI.ShowPopupUI<UI_Description>();
        description.gameObject.SetActive(true);
    }
    
    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;

        bool successfulHit = false;

        // player 앞인지?
        // 일인칭으로 바꾸던지, drawRay 가 아닌 다른 방법이 필요.
        // 드래깅상태에서 방향 바꾸는 것도 필요.
        Debug.DrawRay(gameObject.transform.position+ 1.3F* Vector3.up, gameObject.transform.forward * 10.0f, Color.green, interactionDistance);
        if (Physics.Raycast(gameObject.transform.position + 1.3F * Vector3.up, gameObject.transform.forward, out hit, interactionDistance))
        {
           
            Interactable interactable = hit.collider.GetComponent<Interactable>();

            if (interactable != null)
            {   HandleInteraction(interactable);
                // description UI에 text 써서 보이게 하기.
                description.gameObject.SetActive(true);
                description.WriteText(interactable.GetDescription());
                successfulHit = true;
                //interactionHoldGO.SetActive(interactable.interactionType == Interactable.InteractionType.Hold);
            }
        }


        if (!successfulHit)
        {
            
            description.gameObject.SetActive(false);
        }
    }

    void HandleInteraction(Interactable interactable)
    {
        KeyCode key = KeyCode.E;
        switch (interactable.interactionType)
        {
            case Interactable.InteractionType.Click:
                // interaction type is click and we clicked the button -> interact
                if (Input.GetKeyDown(key))
                {
                    interactable.Interact();
                }
                break;
            case Interactable.InteractionType.Hold:
                if (Input.GetKey(key))
                {
                    // we are holding the key, increase the timer until we reach 1f
                    interactable.IncreaseHoldTime();
                    if (interactable.GetHoldTime() > 1f)
                    {
                        interactable.Interact();
                        interactable.ResetHoldTime();
                    }
                }
                else
                {
                    interactable.ResetHoldTime();
                }
                //interactionHoldProgress.fillAmount = interactable.GetHoldTime();
                break;
            // here is started code for your custom interaction :)

            // helpful error for us in the future
            default:
                throw new System.Exception("Unsupported type of interactable.");
        }
    }
}
