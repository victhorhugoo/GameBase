using UnityEngine;
using VictorGame.StateMachine;

public class PlayerStateIdle : StateBase
{
    private PlayerMoviment player;

    public override void OnStateEnter(object o = null)
    {
        player = o as PlayerMoviment;
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
        if (player == null) // proteção extra
            return;

        player.Move(1f);

        if (Input.GetKeyDown(KeyCode.Space) && player.IsGrounded)
        {
            player.stateMachine.SwitchState(PlayerMoviment.PlayerStates.Jump, player);
            return;
        }

        if (Input.GetKeyDown(player.KeyShoot))
        {
            player.stateMachine.SwitchState(PlayerMoviment.PlayerStates.Shoot, player);
            return;
        }

        if (player.InputVertical != 0)
        {
            // CORRIGIDO: usa a flag unificada (teclado + botão touch) em vez de checar só o teclado
            var next = player.IsRunning ? PlayerMoviment.PlayerStates.Run : PlayerMoviment.PlayerStates.Move;
            player.stateMachine.SwitchState(next, player);
        }
    }

    public override void OnStateExit()
    {
    }
}