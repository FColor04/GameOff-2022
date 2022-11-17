using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firearms.Actions;

namespace Firearms
{
    public class PlayerShooting : MonoBehaviour
    {
        [SerializeField] private PlayerEquipment equipment;
        [SerializeField] private Camera playerCamera;
        private float lastShotFired;
        private bool currentlyShooting;
        private bool holdingPrimaryFireButton, holdingSecondaryFireButton;
        private float lastButtonRelease;
        private FirearmData FirearmData { get => equipment?.EquippedFirearm?.FirearmData; }
        public void Update()
        {
            if (holdingPrimaryFireButton && FirearmData && (FirearmData.isAutomatic || lastButtonRelease > lastShotFired))
                FirePrimary();
            if (holdingSecondaryFireButton && FirearmData && (FirearmData.isAutomatic || lastButtonRelease > lastShotFired))
                FireSecondary();
        }

        public void OnPrimaryFireButtonPressed()
        {
            holdingPrimaryFireButton = true;
            if (!equipment || !equipment.EquippedFirearm || !FirearmData) return;
            FirePrimary();
        }
        public void OnPrimaryFireButtonReleased()
        {
            holdingPrimaryFireButton = false;
            lastButtonRelease = Time.time;
        }

        private void FirePrimary()
        {
            if (currentlyShooting || !FirearmData || Time.time < lastShotFired + 1f / FirearmData.fireRate) return;
            currentlyShooting = true;
            StartCoroutine(ExecuteActions(FirearmData.onPrimaryFireActions));
        }

        private IEnumerator ExecuteActions(List<IAction> actions)
        {
            foreach (var action in actions)
            {
                if (action == null) continue;
                var enumerator = action.Execute(equipment.EquippedFirearm, playerCamera);
                while (enumerator.MoveNext())
                {
                    if (enumerator.Current == null) enumerator.MoveNext();
                    else yield return enumerator.Current;
                }
            }
            currentlyShooting = false;
            lastShotFired = Time.time;
        }

        private void FireSecondary()
        {
            if (currentlyShooting || !FirearmData || Time.time < lastShotFired + 1f / FirearmData.fireRate) return;
            currentlyShooting = true;
            StartCoroutine(ExecuteActions(FirearmData.onSecondaryFireActions));
        }


        #region Secondary Fire
        public void OnSecondaryFireButtonPressed()
        {
            holdingSecondaryFireButton = true;
            if (!equipment || !equipment.EquippedFirearm || !FirearmData) return;
            FireSecondary();
        }
        public void OnSecondaryFireButtonReleased()
        {
            holdingSecondaryFireButton = false;
        }

        #endregion
    }
}