using UnityEngine;
using VictorGame.StateMachine;

public class PlayerStateMove : StateBase
{
    private PlayerMoviment player;

    public override void OnStateEnter(object o = null)
    {
        player = o as PlayerMoviment;
        if (player == null)
        {
            Debug.LogError("PlayerStateMove.OnStateEnter: contexto 'o' é nulo ou não é PlayerMoviment. Passe a instância do Player ao chamar SwitchState.");
            return;
        }

        player.animator.SetBool("Run", true);
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

        

        if (player.InputVertical == 0 && player.InputHorizontal == 0)
        {
            player.stateMachine.SwitchState(PlayerMoviment.PlayerStates.Idle, player);
            return;
        }

        
        if (player.IsRunning)
        {
            player.stateMachine.SwitchState(PlayerMoviment.PlayerStates.Run, player);
        }
    }

    public override void OnStateExit()
    {
    }
}