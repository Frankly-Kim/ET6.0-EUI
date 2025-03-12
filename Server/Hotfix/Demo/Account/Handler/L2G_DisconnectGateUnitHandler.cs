using System;

namespace ET
{
    public class L2G_DisconnectGateUnitHandler: AMActorRpcHandler<Scene, L2G_DisconnectGateUnit, G2L_DisConnectGateUnit>
    {
        protected override async ETTask Run(Scene scene, L2G_DisconnectGateUnit request, G2L_DisConnectGateUnit response, Action reply)
        {
            long accountId = request.AccountId;
            using (await CoroutineLockComponent.Instance.Wait(CoroutineLockType.GateLoginLock, accountId.GetHashCode()))
            {
                PlayerComponent playerComponent = scene.GetComponent<PlayerComponent>();
                Player gateUnit = playerComponent.Get(accountId);

                if (gateUnit == null)
                {
                    reply();
                    return;
                }

                playerComponent.Remove(accountId);
                gateUnit.Dispose();
            }

            reply();

            await ETTask.CompletedTask;
        }
    }
}