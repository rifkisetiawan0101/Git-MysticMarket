using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MerchantRespawner : MonoBehaviour {
    private void Start() {
        RespawnMerchant();
        RespawnFurnitur();   
        RespawnSpesial();
    }
    private void RespawnMerchant() {
        foreach (var merchantData in PersistentManager.Instance.dataMerchantList) {
            Instantiate(merchantData.merchantTypeSO.merchantPrefab, merchantData.merchantPosition, Quaternion.identity);
        }
    }
    
    private void RespawnFurnitur() {
        foreach (var furniturData in PersistentManager.Instance.dataFurniturList) {
            Instantiate(furniturData.furniturTypeSO.furniturPrefab, furniturData.furniturPosition, Quaternion.identity);
        }
    }

    private void RespawnSpesial() {
        foreach (var spesialData in PersistentManager.Instance.dataSpesialList) {
            Instantiate(spesialData.spesialTypeSO.spesialPrefab, spesialData.spesialPosition, Quaternion.identity);
        }
    }

}
