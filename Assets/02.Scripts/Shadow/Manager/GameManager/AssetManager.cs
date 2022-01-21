using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Spine.Unity;
using UnityEngine;

public class AssetManager : SingletonBehaviour<AssetManager>
{
    [PreviewField][Header("장비 아이콘")] public List<Sprite> EquipmentIcon;
    [PreviewField][Header("슬롯 아이콘")] public List<Sprite> ItemFrameIcon;
    [PreviewField][Header("슬롯 아이콘")] public List<Sprite> FollowerIcon;
}