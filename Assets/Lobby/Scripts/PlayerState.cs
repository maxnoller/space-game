using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using System;
using UnityEngine;
using Unity.Netcode;

namespace SpaceGame.UI.Lobby{
struct PlayerState : INetworkSerializable, IEquatable<PlayerState>
{
        public ulong ClientId;
        public ForceNetworkSerializeByMemcpy<FixedString64Bytes> PlayerName;
        public bool IsReady;

        public PlayerState(ulong clientId, string playerName, bool isReady){
            ClientId = clientId;
            PlayerName = new FixedString64Bytes(playerName);
            IsReady = isReady;
        }

        public bool Equals(PlayerState other)
        {
            if(ClientId == other.ClientId){
                return true;
            }
            return false;
        }

        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
        {
            serializer.SerializeValue(ref ClientId);
            serializer.SerializeValue(ref PlayerName);
            serializer.SerializeValue(ref IsReady);
        }
}
}
