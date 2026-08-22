using UnityEngine;
using VictorGame.StateMachine;

public class PlayerStateRun : StateBase
{
    private Player1 player;

    public override void OnStateEnter(object o = null)
    {
        player = o as Player1;
        player.animator.SetBool("Run", true);
        player.animator.speed = player.speedRun;
    }

    public override void OnStateStay()
    {
        player.Move(player.speedRun);

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

        if (!Input.GetKey(player.KeyRun))
        {
            player.stateMachine.SwitchState(Player1.PlayerStates.Move, player);
        }
    }

    public override void OnStateExit()
    {
        player.animator.speed = 1f;
    }
}
