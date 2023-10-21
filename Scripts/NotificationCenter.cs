using System;
using System.Collections.Generic;

public static class NotificationCenter{
    static Dictionary<string, Action> dic = new Dictionary<string, Action>();

    public static void Register(string msgName, Action action){
        if(!dic.ContainsKey(msgName)){
            dic.Add(msgName,action);
        }
    }

    public static void Notify(string msgName){
        if(dic.TryGetValue(msgName, out Action action)){
            action.Invoke();
        }
    }
}