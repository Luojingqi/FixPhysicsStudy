using Fix64Physics;
using FrameLogicFramework;
using Google.Protobuf;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using TransformReplace;
using UnityEngine;

/// <summary>
/// ֡ͬ����������mono���
/// ������Ҫʵ��֡ͬ������Ϸ�������Ҫ����
/// </summary>
public abstract class FrameAgentMono : MonoBehaviour, IFrameLogicAgent
{
    [OdinSerialize]
    private int agentID;
    [OdinSerialize]
    private AgentType agentType;
    [OdinSerialize]
    private string playerID;

    protected IFixTransform fixTransform;
    public int AgentID { get => agentID; set => agentID = value; }
    public AgentType AgentType { get => agentType; set => agentType = value; }
    public string PlayerID { get => playerID; set => playerID = value; }
    public IFixTransform FixTransform => fixTransform;


    #region ��¼ת�������һ֡����
    [OdinSerialize]
    protected Vector3 position0;
    [OdinSerialize ]
    protected Quaternion rotation0;
    #endregion
    #region ��¼ת����ĵ�ǰ֡����
    [OdinSerialize ]
    protected Vector3 position1;
    [OdinSerialize ]
    protected Quaternion rotation1;
    #endregion

    public abstract void GameLogicDataUpdata(ByteString data);

    public abstract void GameRenderPosition(float timeTween, float frameTween, float deltaTime);

    public virtual void Init()
    {
        fixTransform = transform.GetComponent<IFixTransform>();
    }

    /// <summary>
    /// ��¼��ǰ�߼�λ��0
    /// </summary>
    public virtual void SaveLogicTransform0()
    {
        position0 = (Vector3)fixTransform.Position;
        rotation0 = (Quaternion)fixTransform.Rotation;
      //  Debug.Log("��¼0");
    }

    /// <summary>
    /// ��¼��ǰ�߼�λ��1
    /// </summary>
    public virtual void SaveLogicTransform1()
    {
        position1 = (Vector3)fixTransform.Position;
        rotation1 = (Quaternion)fixTransform.Rotation;
       // Debug.Log("��¼1");
    }

    /// <summary>
    /// ֻ��Ⱦ��ǰλ�ú���ת
    /// </summary>
    /// <param name="timeTween"></param>
    /// <param name="frameTween"></param>
    /// <param name="deltaTime"></param>
    public void OnlyRenderTransform(float timeTween, float frameTween, float deltaTime)
    {
        transform.position = Vector3.Lerp(position0, position1, frameTween);
        transform.rotation = Quaternion.Lerp(rotation0, rotation1, frameTween);
    }
}
