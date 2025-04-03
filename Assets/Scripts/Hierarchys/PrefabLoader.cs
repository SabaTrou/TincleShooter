using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PrefabLoader 
{
    /// <summary>
    /// Resourcesフォルダにある特定のコンポーネントがついたプレハブを取得
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
   public static GameObject[] LoadPreafabWithComponent<T>()
    {
        GameObject[] allPrefabs=Resources.LoadAll<GameObject>("");
        return allPrefabs.Where(prefab=>prefab.GetComponent<T>()!=null).ToArray();
    }
}
