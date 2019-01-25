using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public enum NeedType { FUN, SOCIAL, BLADDER, HUNGER, ROOM };

public class Needs : MonoBehaviour {

    public TextMeshPro debug_tmp;

    private float[] needs;

    public float funDecayPerSecond = 1;
    public float socialDecayPerSecond = 1;
    public float bladderDecayPerSecond = 1;
    public float hungerDecayPerSecond = 1;
    public float roomDecayPerSecond = 1;

    public float Fun { get { return needs[(int)NeedType.FUN]; } set { needs[(int)NeedType.FUN] = Mathf.Clamp(value, 0f, 100f); } }
    public float Social { get { return needs[(int)NeedType.SOCIAL]; } set { needs[(int)NeedType.SOCIAL] = Mathf.Clamp(value, 0f, 100f); } }
    public float Bladder { get { return needs[(int)NeedType.BLADDER]; } set { needs[(int)NeedType.BLADDER] = Mathf.Clamp(value, 0f, 100f); } }
    public float Hunger { get { return needs[(int)NeedType.HUNGER]; } set { needs[(int)NeedType.HUNGER] = Mathf.Clamp(value, 0f, 100f); } }
    public float Room { get { return needs[(int)NeedType.ROOM]; } set { needs[(int)NeedType.ROOM] = Mathf.Clamp(value, 0f, 100f); } }

    public virtual void Awake()
    {
        DefaultValues();
    }

    public virtual void Update()
    {
        DecayNeeds();

        debug_tmp.text = string.Format("Fun={0}\nSocial={1}\nBladder={2}\nHunger={3}\nRoom={4}", Fun, Social, Bladder, Hunger, Room);
    }

    protected virtual void RewardNeed(NeedType type, float reward)
    {
        needs[(int)type] = Mathf.Clamp(needs[(int)type] + reward, 0f, 100f);
    }

    protected virtual void DecayNeeds()
    {
        Fun = Fun - funDecayPerSecond * Time.deltaTime;
        Social = Social - socialDecayPerSecond * Time.deltaTime;
        Bladder = Bladder - bladderDecayPerSecond * Time.deltaTime;
        Hunger = Hunger - hungerDecayPerSecond * Time.deltaTime;
        //Room = Room - roomDecayPerSecond * Time.deltaTime; Room does not decay like the other things
    }

    protected virtual void DefaultValues()
    {
        needs = new float[System.Enum.GetNames(typeof(NeedType)).Length];
        for (int i = 0; i < needs.Length; ++i) needs[i] = 100f;
    }
}
