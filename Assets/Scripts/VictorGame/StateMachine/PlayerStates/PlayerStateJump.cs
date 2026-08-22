using VictorGame.StateMachine;

public class PlayerStateJump : StateBase
{
    private Player1 player;

    public override void OnStateEnter(object o = null)
    {
        player = o as Player1;
        if (player == null)
        {
            UnityEngine.Debug.LogError("PlayerStateJump.OnStateEnter: contexto 'o' é nulo ou não é Player1.");
            return;
        }

        player.Jump();
        // player.animator.SetTrigger("Jump");
    }

    public override void OnStateStay()
    {
        if (player == null)
            return;

        float speedMultiplier = UnityEngine.Input.GetKey(player.KeyRun) ? player.speedRun : 1f;
        player.Move(speedMultiplier);

        // Só sai do estado de pulo quando encostar no chão de novo (e já estiver caindo/parado)
        if (player.IsGrounded && player.VerticalSpeed <= 0f)
        {
            if (player.InputVertical != 0)
            {
                var next = UnityEngine.Input.GetKey(player.KeyRun) ? Player1.PlayerStates.Run : Player1.PlayerStates.Move;
                player.stateMachine.SwitchState(next, player);
            }
            else
            {
                player.stateMachine.SwitchState(Player1.PlayerStates.Idle, player);
            }
        }
    }

    public override void OnStateExit()
    {
    }
}