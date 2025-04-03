using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PrefabLoader 
{
    /// <summary>
    /// Resources�t�H���_�ɂ������̃R���|�[�l���g�������v���n�u���擾
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
   public static GameObject[] LoadPreafabWithComponent<T>()
    {
        GameObject[] allPrefabs=Resources.LoadAll<GameObject>("");
        return allPrefabs.Where(prefab=>prefab.GetComponent<T>()!=null).ToArray();
    }
}
