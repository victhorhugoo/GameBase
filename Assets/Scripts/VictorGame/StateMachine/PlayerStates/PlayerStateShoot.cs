using VictorGame.StateMachine;

public class PlayerStateShoot : StateBase
{
    private const float duration = 0.3f; // tempo que o player fica "travado" atirando
    private Player1 player;
    private float timer;

    public override void OnStateEnter(object o = null)
    {
        player = o as Player1;
        if (player == null)
        {
            UnityEngine.Debug.LogError("PlayerStateShoot.OnStateEnter: contexto 'o' é nulo ou não é Player1.");
            return;
        }

        timer = 0f;
        player.Shoot();
        // player.animator.SetTrigger("Shoot");
    }

    public override void OnStateStay()
    {
        if (player == null)
            return;

        // Se quiser permitir andar enquanto atira, mantenha a linha abaixo.
        // Se quiser travar o player parado, comente a linha abaixo.
        player.Move(1f);

        timer += UnityEngine.Time.deltaTime;
        if (timer >= duration)
        {
            var next = player.InputVertical != 0 ? Player1.PlayerStates.Move : Player1.PlayerStates.Idle;
            player.stateMachine.SwitchState(next, player);
        }
    }

    public override void OnStateExit()
    {
    }
}