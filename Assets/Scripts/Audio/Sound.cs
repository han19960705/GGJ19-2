//引入命名空间
using UnityEngine;
//表明可以序列化赋值
[System.Serializable]
public class Sound
{
    //声音的名字
    public string name;
    //声音源的片段
    public AudioClip Clip;
    //[HideInInspector]表示在Inspector面板隐藏
    [HideInInspector]
    //声音源
    public AudioSource source;
    //Headr仅仅是在Inspector起提示作用
    [Header("Basic")]
    //[Range(a, b)]表示a到b的取值
    [Range(0f, 1f)]
    //声音音量
    public float Volume;
    //    [Range(-3f, 3f)]
    //    //声音频率
    //    public float Picth;
    //是不是程序一开始就播放
    public bool IsPlayOnAwake;
    //播放是否循环
    public bool IsLoop;
    //    [Range(0f, 1f)]
    //    //3D音效的比例
    //    public float SpatialBlend;
    //    [Header("3d Sound setting")]
    //    [Range(0f, 5f)]
    //    //Doppler音效效果的等级
    //    public float DopplerLevel;
    //    [Range(0f, 360f)]
    //    //传播的角度
    //    public float Spread;
    //    //声音Rolloff的模式
    //    public AudioRolloffMode audioRolloff;
    //    //声音传播递减开始的最小距离
    //    public float MinDistacne;
    //    //声音传播递减结束的最大距离
    //    public float MaxDistacne;
}