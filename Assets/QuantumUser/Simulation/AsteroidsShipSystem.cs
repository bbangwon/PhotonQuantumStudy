using Photon.Deterministic;
using UnityEngine.Scripting;

namespace Quantum.Asteroids
{
    // Preserve 빌드에서 제외되지 않도록 보존
    // unsafe 키워드를 사용하여 포인터를 사용할 수 있도록 함
    // SystemMainThreadFilter를 상속 : Filter를 가지고 있는 엔티티를 가져와서 매 프레임 업데이트
    [Preserve]  
    public unsafe class AsteriodsShipSystem : SystemMainThreadFilter<AsteriodsShipSystem.Filter>
    {
        // ECS 엔티티들을 필터링하는 데 사용할 구조체
        // 필터링된 엔티티들은 Entity, Transform2D, PhysicsBody2D 컴포넌트를 가지고 있음
        public struct Filter
        {
            public EntityRef Entity;
            public Transform2D* Transform;
            public PhysicsBody2D* Body;
        }

        // 프레임 마다 filter 엔티티를 하나 가져와 업데이트
        public override void Update(Frame f, ref Filter filter)
        {
            Input* input = default;
            //플레이어 링크 컴포넌트를 가져와서 플레이어 입력을 가져옴           

            //아래코드는 값을 읽을 때만 사용함
            //PlayerLink p = f.Get<PlayerLink>(filter.Entity);

            //Unsafe 문법..
            //컴포넌트의 값을 변경하고 싶을때는 포인터로 받아와야 함
            //값을 참조할때도 포인터 참조 문법(->)을 사용해야 함

            //퀀텀은 희소집합 ECS를 사용하므로, TryGetPointer 시간 복잡도가 O(1)이라서 굉장이 빠름
            if (f.Unsafe.TryGetPointer(filter.Entity, out PlayerLink* playerLink))
            {
                input = f.GetPlayerInput(playerLink->PlayerRef);
            }            
            
            UpdateShipMovement(f, ref filter, input);
        }

        // 엔티티의 물리적인 움직임을 업데이트
        private void UpdateShipMovement(Frame f, ref Filter filter, Input* input)
        {
            //float은 부동소수점 타입으로 디바이스마다 달라질 수 있음
            //따라서 퀀텀에서는 FP 타입을 사용하여 결정론적 계산을 수행

            FP shipAcceleration = 7;
            FP turnSpeed = 8;

            if(input->Up)
            {
                filter.Body->AddForce(filter.Transform->Up * shipAcceleration);
            }

            if (input->Left)
            {
                filter.Body->AddTorque(turnSpeed);
            }

            if (input->Right)
            {
                filter.Body->AddTorque(-turnSpeed);
            }

            filter.Body->AngularVelocity = FPMath.Clamp(filter.Body->AngularVelocity, -turnSpeed, turnSpeed);

        }
    }
}
