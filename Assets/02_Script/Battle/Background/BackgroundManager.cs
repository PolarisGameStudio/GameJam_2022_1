using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BackgroundManager : SingletonBehaviour<BackgroundManager>
{
    [SerializeField] private List<Texture> _layer1TextureList;
    [SerializeField] private List<Texture> _layer2TextureList;
    [SerializeField] private List<Texture> _layer3TextureList;
    [SerializeField] private List<Texture> _layer4TextureList;
    [SerializeField] private List<Texture> _layer5TextureList;

    public Background Layer1Background;
    public Background Layer2Background;
    public Background Layer3Background;
    public Background Layer4Background;
    public Background Layer5Background;

    private BattleCamera _battleCamera;
    private PlayerObject _playerObject;

    private void Start()
    {
        _battleCamera = BattleCamera.Instance;
        _playerObject = BattleManager.Instance.PlayerObject;
    }

    private void LateUpdate()
    {
        var cameraPosition = _battleCamera.Position;
        var playerPosition = _playerObject.Position;

        cameraPosition.z = 0;
        cameraPosition.y = 0;
        
        transform.position = cameraPosition;
        
        Layer1Background.Refresh(cameraPosition);
        Layer2Background.Refresh(cameraPosition);
        Layer3Background.Refresh(cameraPosition);
        Layer4Background.Refresh(cameraPosition);
        Layer5Background.Refresh(cameraPosition);
    }
    public void SetBackground(int backgroundIndex)
    {
        Layer1Background.SetTexture(_layer1TextureList[backgroundIndex]);
        Layer2Background.SetTexture(_layer2TextureList[backgroundIndex]);
        Layer3Background.SetTexture(_layer3TextureList[backgroundIndex]);
        Layer4Background.SetTexture(_layer4TextureList[backgroundIndex]);
        Layer5Background.SetTexture(_layer5TextureList[backgroundIndex]);
    }
    
    public void SetBackground(BackgroundType backgroundType)
    {
        SetBackground((int) backgroundType);
    }
}
