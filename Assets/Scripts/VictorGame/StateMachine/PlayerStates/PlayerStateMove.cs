using UnityEngine;
using VictorGame.StateMachine;

public class PlayerStateMove : StateBase
{
    private Player1 player;

    public override void OnStateEnter(object o = null)
    {
        player = o as Player1;
        if (player == null)
        {
            Debug.LogError("PlayerStateMove.OnStateEnter: contexto 'o' é nulo ou não é Player1. Passe a instância do Player ao chamar SwitchState.");
            return;
        }

        player.animator.SetBool("Run", true);
        player.animator.speed = 1f;
    }

    public override void OnStateStay()
    {
        if (player == null) // proteção extra
            return;

        player.Move(1f);

        if (Input.GetKeyDown(KeyCode.Space) && player.IsGrounded)
        {
            player.stateMachine.SwitchState(Player1.PlayerStates.Jump, player);
            return;
        }

        if (Input.GetKeyDown(player.KeyShoot))
        {
            player.stateMachine.SwitchState(Player1.PlayerStates.Shoot, player);
            return;
        }

        if (player.InputVertical == 0)
        {
            player.stateMachine.SwitchState(Player1.PlayerStates.Idle, player);
            return;
        }

        if (Input.GetKey(player.KeyRun))
        {
            player.stateMachine.SwitchState(Player1.PlayerStates.Run, player);
        }
    }

    public override void OnStateExit()
    {
    }
}