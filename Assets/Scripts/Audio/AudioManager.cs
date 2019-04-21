using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    //单例
    public static AudioManager Ins { get; private set; }
    //Sound类型的数组，存储所有的Sound
    public Sound[] sounds;

    public NetworkAgent network;

    Sound currentSound;

    private void Awake()
    {
        //单例
        if (Ins == null || network.connID == 0)
            Ins = this;
        else
        {
            Destroy(gameObject);
            return;
        }
    }
    
    private void Start() {
        if (network.connID != 0) return;
        //对每一项Sound进行初始化
        foreach (var item in sounds)
        {
            item.source = gameObject.AddComponent<AudioSource>();
            item.source.clip = item.Clip;

            //item.source.volume = item.Volume;
            //item.source.pitch = item.Picth;
            item.source.loop = item.IsLoop;
            //item.source.spatialBlend = item.SpatialBlend;
            //item.source.dopplerLevel = item.DopplerLevel;
            //item.source.spread = item.Spread;
            //item.source.rolloffMode = item.audioRolloff;
            //item.source.minDistance = item.MinDistacne;
            //item.source.maxDistance = item.MaxDistacne;

            if (item.IsPlayOnAwake)
                item.source.Play();
        }

        currentSound = sounds[0];
    }

    //开放的API，通过声音的名字播放相应的片段
    public void Play(string name, float delay = 0f)
    {
        if (network.connID != 0) return;
        if (name == currentSound.name && currentSound.source.isPlaying)
            return;
        //查找在sounds中名字为name的一个Sound实例
        Sound s = Array.Find(sounds, x => x.name == name);
        if (s == null)
        {
            print(name + " can not found!");
            return;
        }
        //播放，delay表示延迟
        s.source.PlayDelayed(delay);
        currentSound = s;
    }

    public void Stop(string name)
    {
        if (network.connID != 0) return;
        if (name == currentSound.name)
            currentSound.source.Stop();
    }
}
