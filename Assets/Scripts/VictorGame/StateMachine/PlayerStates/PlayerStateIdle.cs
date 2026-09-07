using UnityEngine;
using VictorGame.StateMachine;

public class PlayerStateIdle : StateBase
{
    private PlayerMoviment player;

    public override void OnStateEnter(params object[] objs)
    {
        player = objs[0] as PlayerMoviment;
        if (player == null)
        {
            Debug.LogError("PlayerStateIdle.OnStateEnter: contexto 'o' é nulo ou não é PlayerMoviment. Passe a instância do Player ao chamar SwitchState.");
            return;
        }

        player.animator.SetBool("Run", false);
        player.animator.speed = 1f;
    }

    public override void OnStateStay()
    {
        if (player == null)
            return;

        player.Move(1f);

        if (Input.GetKeyDown(KeyCode.Space) && player.IsGrounded)
        {
            player.stateMachine.SwitchState(PlayerMoviment.PlayerStates.Jump, player);
            return;
        }

 

        if (player.InputVertical != 0 || player.InputHorizontal != 0)
        {
            
            var next = player.IsRunning ? PlayerMoviment.PlayerStates.Run : PlayerMoviment.PlayerStates.Move;
            player.stateMachine.SwitchState(next, player);
        }
    }

    public override void OnStateExit()
    {
    }
}