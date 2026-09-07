using UnityEngine;

namespace Boss
{
    public class BossShoot : BossBase
    {
        public GunBase gunBase;

        protected override void OnStateEnter(BossState state)
        {
            if (state == BossState.ATTACK && gunBase != null)
            {
                gunBase.OnShoot += PlayAttackScale; // <- faltava isso
                gunBase.StartShoot();
            }
        }

        protected override void OnStateExit(BossState state)
        {
            if (state == BossState.ATTACK && gunBase != null)
            {
                gunBase.OnShoot -= PlayAttackScale; // <- e isso, pra não acumular assinaturas
                gunBase.StopShoot();
            }
        }

        private void PlayAttackScale()
        {
            PlayAttackScale(1.15f, 0.1f);
        }
    }
}