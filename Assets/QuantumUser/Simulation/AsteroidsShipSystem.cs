using Photon.Deterministic;
using UnityEngine.Scripting;

namespace Quantum.Asteroids
{
    // Preserve 빌드에서 제외되지 않도록 보존
    // unsafe 키워드를 사용하여 포인터를 사용할 수 있도록 함
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
            var input = f.GetPlayerInput(0);
            
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
