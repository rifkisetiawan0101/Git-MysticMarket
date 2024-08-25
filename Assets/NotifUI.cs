using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotifUI : MonoBehaviour {
    [SerializeField] private GameObject notifUto;
    [SerializeField] private GameObject notifMalam;


    public IEnumerator PlayNotifMalam() {
        notifMalam.SetActive(true);
        yield return new WaitForSeconds(3.5f);
        notifMalam.SetActive(false);
    }

    public IEnumerator PlayNotifUto() {
        notifUto.SetActive(true);
        yield return new WaitForSeconds(3.5f);
        notifUto.SetActive(false);
    }
}
