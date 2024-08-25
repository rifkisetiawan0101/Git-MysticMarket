using System;
using UnityEngine;
using UnityEngine.UI;

public class MerchantPlacement : MonoBehaviour {
    private MerchantManager merchantManager;
    private Vector3 placementPosition;

    public void Setup(Vector3 position, MerchantManager manager) {
        placementPosition = position;
        merchantManager = manager;

        // Menemukan tombol dan menambahkan listener
        Button buttonAccept = transform.Find("Canvas/ButtonAccept").GetComponent<Button>();
        Button buttonCancel = transform.Find("Canvas/ButtonCancel").GetComponent<Button>();

        buttonAccept.onClick.AddListener(() => AcceptButtonPlacement());
        buttonCancel.onClick.AddListener(() => CancelButtonPlacement());

        Button buttonFurnitur = GameObject.Find("ButtonFurnitur").GetComponent<Button>(); 
        buttonFurnitur.onClick.AddListener(() => {
            merchantManager.CancelPlacement();
            Destroy(gameObject);
        });

        Button buttonSpesial = GameObject.Find("ButtonSpesial").GetComponent<Button>(); 
        buttonSpesial.onClick.AddListener(() => {
            merchantManager.CancelPlacement();
            Destroy(gameObject);
        });
    }

    private void AcceptButtonPlacement() {
        // Panggil MerchantPlacing di MerchantManager
        merchantManager.MerchantPlacing(placementPosition);
        Destroy(gameObject); // Menghancurkan prefab placement setelah diterima
    }

    private void CancelButtonPlacement() {
        merchantManager.CancelPlacement(); // Reset status placement di MerchantManager
        Destroy(gameObject); // Menghancurkan prefab placement jika dibatalkan
    } 
}
