using SequenceRunner;
using UnityEngine;
using Injection;
using ActionCoroutine;
public class DemodulatorSequence : CoroutineBasedSequenceBase
{
    private PlayerInScene _playerInScene;
    [Inject] private SimpleActionCoroutine _simpleActionCoroutine;
    public override void Start(params object[] parameters)
    {
        _playerInScene = parameters[0] as PlayerInScene;
        //set player to position
        _simpleActionCoroutine.RunSimpleLerpCoroutine(1f, (t) => {
            _playerInScene.transform.position = Vector3.Lerp(_playerInScene.transform.position, new Vector3(0, 0, 0), t);
        }, () => {
            //end
        });
    }

    public override void Update()
    {
        //update visuals
    }

    protected override void Finish()
    {
        //end visuals
    }
}
