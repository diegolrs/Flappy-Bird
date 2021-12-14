using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(PipeGroupSpawner))]
[RequireComponent(typeof(PipeGroupDestroyer))]
public class EndlessPipeGenerator : MonoBehaviour
{
    [Header("Manager")]
    [SerializeField] int _spawnOnInit;
    [SerializeField] int _minPipesOnScreen;
    [SerializeField] PipeGroupSpawner _spawner;
    [SerializeField] PipeGroupDestroyer _destroyer;

    private List<PipeGroup> _pipeList;
    private const int DefaultIndexToDestroy = 0;
    private bool _enabled;

    public void StartSpawn()
    {
        _pipeList = new List<PipeGroup>();

        for(int i = 0; i < _spawnOnInit; i++)
            SpawnPipe();

        _enabled = true;
    }

    private void SpawnPipe()
    {
        var pipeGroup = _spawner.SpawnPipes();
        _pipeList.Add(pipeGroup);
    }

    private bool CanUnspawn(int index)
    {
        if(_pipeList.Count <= index)
            return false;

        return _destroyer.CanDestroy(_pipeList[index]);
    }

    private void UnspawnPipe(int index)
    {
        if(_pipeList.Count <= index)
            return;

        _destroyer.Destroy(_pipeList[index]);
        _pipeList.RemoveAt(index);
    }

    private void Update() 
    {
        if(!_enabled)
            return;

        if(_pipeList.Count < _minPipesOnScreen)
            SpawnPipe();
        else if(CanUnspawn(DefaultIndexToDestroy))
            UnspawnPipe(DefaultIndexToDestroy);
    }
}
