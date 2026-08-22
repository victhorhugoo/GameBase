using UnityEngine;
using VictorGame.StateMachine;

public class PlayerStateIdle : StateBase
{
    private Player1 player;

    public override void OnStateEnter(object o = null)
    {
        player = o as Player1;
        if (player == null)
        {
            Debug.LogError("PlayerStateIdle.OnStateEnter: contexto 'o' é nulo ou não é Player1. Passe a instância do Player ao chamar SwitchState.");
            return;
        }

        player.animator.SetBool("Run", false);
        player.animator.speed = 1f;
    }

    public override void OnStateStay()
    {
        Debug.Log("player null? " + (player == null));
        Debug.Log("stateMachine null? " + (player != null && player.stateMachine == null));

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

        if (player.InputVertical != 0)
        {
            var next = Input.GetKey(player.KeyRun) ? Player1.PlayerStates.Run : Player1.PlayerStates.Move;
            player.stateMachine.SwitchState(next, player);
        }
    }

    public override void OnStateExit()
    {
    }
}