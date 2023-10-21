using System;
using System.Collections.Generic;
using UnityEngine;
public class Pool<T> where T : MonoBehaviour,IPoolObject<T>{

        GameObject parent;
        Stack<T> objs;
        Func<T> generator;

        public Pool(Func<T> func) {
            // parent = go;
            objs = new Stack<T>(20);
            generator = func;
        }

        public T Take() {
            T com = objs.Count == 0 ? generator() : objs.Pop();
            com.gameObject.SetActive(true);
            com.InjectPool(this);
            return com;
        }

        public void Return(T obj) {
            objs.Push(obj);
            GameObject go = obj.gameObject;
            go.SetActive(false);
            // go.transform.SetParent(parent.transform);
        }

        public void Clear() {
            objs.Clear();
        }
    }