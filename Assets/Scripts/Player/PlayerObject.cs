using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerObject : MonoBehaviour
{
    [field: SerializeField] public List<ObjectDataSO> ObjectList {  get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        foreach (var objectData in ObjectList)
            PlayerStatsManager.instance.AddObject(objectData.BaseStats);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddObject(ObjectDataSO objectData)
    {
        ObjectList.Add(objectData);
        PlayerStatsManager.instance.AddObject(objectData.BaseStats);
    }

    public void RemoveObject(ObjectDataSO objectData)
    {
        ObjectList.Remove(objectData);
        PlayerStatsManager.instance.RemoveObjectStats(objectData.BaseStats);
    }

}
