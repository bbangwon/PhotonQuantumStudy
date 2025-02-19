using UnityEngine;
using Photon.Deterministic;

namespace Quantum.Asteroids
{
    public class AsteroidsInput : MonoBehaviour
    {
        private void OnEnable()
        {
            //콜백을 등록.. 매 프레임마다 호출됨
            QuantumCallback.Subscribe<CallbackPollInput>(this, PollInput);
        }

        public void PollInput(CallbackPollInput callback)
        {
            //퀀텀 시뮬레이션에 입력을 제공
            Input i = new()
            {
                Left = UnityEngine.Input.GetKey(KeyCode.A) || UnityEngine.Input.GetKey(KeyCode.LeftArrow),
                Right = UnityEngine.Input.GetKey(KeyCode.D) || UnityEngine.Input.GetKey(KeyCode.RightArrow),
                Up = UnityEngine.Input.GetKey(KeyCode.W) || UnityEngine.Input.GetKey(KeyCode.UpArrow),
                Fire = UnityEngine.Input.GetKey(KeyCode.Space)
            };

            // 네트워크 시뮬레이션시 예측을 하게 되는데, 똑같은 입력을 사용했다고 가정하라!
            callback.SetInput(i, DeterministicInputFlags.Repeatable);
        }
    }
}
