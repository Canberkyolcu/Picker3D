using System;
using Unity.Mathematics;
using UnityEngine;

[Serializable]
public struct PlayerData
{
    public PlayerMovementData MovementData;
    public PlayerMeshData MeshData;
    public PlayerForceData ForceData;

    [Serializable]
    public struct PlayerMovementData
    {
        public float forwardSpeed;
        public float sidewaySpeed;
    }

    [Serializable]
    public struct PlayerMeshData
    {
        public float scaleCounter;
    }

    [Serializable]
    public struct PlayerForceData
    {
        public float3 forceParameters;
    }
}
