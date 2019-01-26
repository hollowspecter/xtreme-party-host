using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionQueue : Queue<AbstractAction> {

    public virtual void EnqueueFront(AbstractAction action)
    {
        Queue<AbstractAction> tempQ = new Queue<AbstractAction>();
        while (Count > 0) tempQ.Enqueue(Dequeue());
        Enqueue(action);
        while (tempQ.Count > 0) Enqueue(tempQ.Dequeue());
        tempQ = null;
    }

    public override string ToString() {
        string result = "";
        foreach(AbstractAction action in this)
        {
            result += action.Name + " | ";
        }
        return result;
    }
}
