using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public enum NeedType { FUN, SOCIAL, BLADDER, HUNGER, ROOM };

public class Needs : MonoBehaviour {

    public float mood;
    public float[] needs;

    [Header("Debug")]
    public TextMeshPro debug_tmp;

    [Header("Attenuation Fun Social Bladder Hunger Room")]
    public AnimationCurve[] curves;
    public bool useAlternateFunction;

    [Header("Decay per Seconds")]
    public float funDecayPerSecond = 1;
    public float socialDecayPerSecond = 1;
    public float bladderDecayPerSecond = 1;
    public float hungerDecayPerSecond = 1;
    public float roomDecayPerSecond = 1;

    [Header("Other")]
    public Gradient colorGradient;
    public MeshRenderer moodColorRenderer;

    public float Fun { get { return needs[(int)NeedType.FUN]; } set { needs[(int)NeedType.FUN] = Mathf.Clamp(value, 0f, 100f); } }
    public float Social { get { return needs[(int)NeedType.SOCIAL]; } set { needs[(int)NeedType.SOCIAL] = Mathf.Clamp(value, 0f, 100f); } }
    public float Bladder { get { return needs[(int)NeedType.BLADDER]; } set { needs[(int)NeedType.BLADDER] = Mathf.Clamp(value, 0f, 100f); } }
    public float Hunger { get { return needs[(int)NeedType.HUNGER]; } set { needs[(int)NeedType.HUNGER] = Mathf.Clamp(value, 0f, 100f); } }
    public float Room { get { return needs[(int)NeedType.ROOM]; } set { needs[(int)NeedType.ROOM] = Mathf.Clamp(value, 0f, 100f); } }

    [Header("icons")]
    public SpriteRenderer sprechblase;
    public SpriteRenderer iconSprite;
    public float showIconNeedValueThreshold = 40f;
    public Sprite[] needIcons;

    public virtual void Awake()
    {
        DefaultValues();
    }

    public virtual void Update()
    {
        DecayNeeds();
        mood = CalculateMood();
        UpdateColorMood();
        UpdateIcon();

        if (debug_tmp)
            debug_tmp.text = string.Format("Fun={0}\nSocial={1}\nBladder={2}\nHunger={3}\nRoom={4}", Fun, Social, Bladder, Hunger, Room);
    }

    public virtual float CalculatePotentialMood(KeyValuePair<NeedType, float>[] advertisedRewards)
    {
        // store needs
        float[] originalNeeds = new float[needs.Length]; for (int i = 0; i < needs.Length; ++i) originalNeeds[i] = needs[i];

        // add rewards if it has any
        if (advertisedRewards != null)
        {
            foreach (var reward in advertisedRewards)
            {
                RewardNeed(reward.Key, reward.Value);
            }
        }

        // calculate mood
        float _mood = CalculateMood();

        // restore needs
        for (int i = 0; i < needs.Length; ++i) needs[i] = originalNeeds[i];

        // return
        return _mood;
    }

    public virtual float CalculateMood()
    {
        float sum = 0f;
        for (int i = 0; i < needs.Length; ++i)
        {
            if (!useAlternateFunction)
                sum += curves[i].Evaluate(needs[i]);
            else
                sum += (10f / needs[i]);
        }
        return sum;
    }

    protected virtual void UpdateIcon()
    {
        int lowestNeedIndex = -1;
        float lowestNeedValue = float.MaxValue;
        for (int i=0; i<needs.Length; ++i)
        {
            if (needs[i] < lowestNeedValue)
            {
                lowestNeedIndex = i;
                lowestNeedValue = needs[i];
            }
        }

        if (lowestNeedValue < showIconNeedValueThreshold)
        {
            // show icon
            iconSprite.sprite = needIcons[lowestNeedIndex];
            sprechblase.enabled = true;
        }
        else
        {
            // hide icon
            iconSprite.sprite = null;
            sprechblase.enabled = false;
        }
    }

    protected virtual void UpdateColorMood()
    {
        if (moodColorRenderer)
        {
            float t = Mathf.Min(needs) / 100f;

            //Set the main Color of the Material to green
            moodColorRenderer.material.color = colorGradient.Evaluate(t);
        }
    }

    public virtual void RewardNeed(NeedType _type, float _reward)
    {
        needs[(int)_type] = Mathf.Clamp(needs[(int)_type] + _reward, 0f, 100f);
    }

    protected virtual void DecayNeeds()
    {
        Fun = Fun - funDecayPerSecond * Time.deltaTime;
        Social = Social - socialDecayPerSecond * Time.deltaTime;
        Bladder = Bladder - bladderDecayPerSecond * Time.deltaTime;
        Hunger = Hunger - hungerDecayPerSecond * Time.deltaTime;

        float totalDisgust = 0f;
        foreach (var rubbish in Rubbish.allRubbish)
        {
            totalDisgust += rubbish.disgustness;
        }
        Room = 100f - totalDisgust;
    }

    protected virtual void DefaultValues()
    {
        needs = new float[System.Enum.GetNames(typeof(NeedType)).Length];
        for (int i = 0; i < needs.Length; ++i) needs[i] = 100f;
    }
}
