using UnityEngine;
using VictorGame.StateMachine;

public class PlayerStateJump : StateBase
{
    private PlayerMoviment player;

    public override void OnStateEnter(params object[] objs)
    {
        player = objs[0] as PlayerMoviment;
        if (player == null)
        {
            Debug.LogError("PlayerStateJump.OnStateEnter: contexto 'o' é nulo ou não é PlayerMoviment.");
            return;
        }

        player.Jump();
        // player.animator.SetTrigger("Jump");
    }

    public override void OnStateStay()
    {
        if (player == null)
            return;

        
        float speedMultiplier = player.IsRunning ? player.speedRun : 1f;
        player.Move(speedMultiplier);

        // Só sai do estado de pulo quando encostar no chão de novo (e já estiver caindo/parado)
        if (player.IsGrounded && player.VerticalSpeed <= 0f)
        {
            if (player.InputVertical != 0 || player.InputHorizontal != 0)
            {
                var next = player.IsRunning ? PlayerMoviment.PlayerStates.Run : PlayerMoviment.PlayerStates.Move;
                player.stateMachine.SwitchState(next, player);
            }
            else
            {
                player.stateMachine.SwitchState(PlayerMoviment.PlayerStates.Idle, player);
            }
        }
    }

    public override void OnStateExit()
    {
    }
}