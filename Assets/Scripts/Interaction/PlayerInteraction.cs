using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteraction : MonoBehaviour
{
    public PlayerCamera playerCamera;
    [SerializeField] private float interactionRadius = .2f;
    [SerializeField] private float interactionDistance = 8f;
    private IInteractable lookTarget;
    public TMP_Text PickupText;

    void FixedUpdate()
    {
        var direction = playerCamera.CameraRoot.forward;
        var position = playerCamera.CameraRoot.position;
        var sphereCastResults = Physics.SphereCastAll(position, interactionRadius, direction, interactionDistance);
        var interactables = sphereCastResults.Select(hit => (hit, hit.collider.GetComponent<IInteractable>() ?? hit.collider.GetComponentInParent<IInteractable>())).Where(hit => hit.Item2 != null);
        var interactablesPriorityOrdered = interactables.OrderBy(interactable => EvaulatueInteractionPriortiy(interactable.Item1, interactable.Item2, position, direction));
        var highestPrioInteractable = interactablesPriorityOrdered.FirstOrDefault().Item2;
        if (lookTarget == highestPrioInteractable) return;
        lookTarget?.NotifyLookedAway();
        lookTarget = highestPrioInteractable;
        lookTarget?.NotifyLookedAt();
        if (PickupText) PickupText.text = lookTarget?.Message;
    }

    public void Interact()
    {        
        lookTarget?.Execute(this);
    }

    private float EvaulatueInteractionPriortiy(RaycastHit interactableHit, IInteractable interactable, Vector3 cameraPosition, Vector3 cameraDirection)
    {
        var hitPosition = interactableHit.point;
        var delta = (hitPosition - cameraPosition);
        var distance = delta.sqrMagnitude;
        var angle = Vector3.Dot(delta.normalized, cameraDirection.normalized);
        return 1f / (distance * (1 - angle));
    }
}
