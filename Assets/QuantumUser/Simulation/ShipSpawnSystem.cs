using UnityEngine.Scripting;

namespace Quantum.Asteroids
{
    // SystemSignalsOnly를 상속 : 시스템이 시그널만 사용하도록 함
    // 플레이어가 추가되었다는 신호를 받으면, 그 플레이어를 처리
    [Preserve]
    public unsafe class ShipSpawnSystem : SystemSignalsOnly, ISignalOnPlayerAdded
    {
        public void OnPlayerAdded(Frame f, PlayerRef player, bool firstTime)
        {
            //플레이어 데이터를 가져옴
            //디버그시에는 QuantumDebugRunner의 Local플레이어 데이터를 받아서 채우게 됨
            RuntimePlayer data = f.GetPlayerData(player);

            //엔티티 프로토 타입 에셋을 찾아옴
            var entityPrototypeAsset = f.FindAsset<EntityPrototype>(data.PlayerAvatar);

            //프로토 타입 에셋을 게임 시뮬레이션 안에 생성
            //Ship Entity가 게임에 생성됨
            var shipEntity = f.Create(entityPrototypeAsset);

            //플레이어 링크 컴포넌트를 Ship Entity에 추가
            f.Add(shipEntity, new PlayerLink { PlayerRef = player });
        }
    }
}
