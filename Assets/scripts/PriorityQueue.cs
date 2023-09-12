using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PriorityQueue<T>
{
    Dictionary<T, float> _allElements = new Dictionary<T, float>();

    public int Count { get { return _allElements.Count; } }

    public void Enqueue(T elem, float cost)
    {
        if (!_allElements.ContainsKey(elem)) _allElements.Add(elem, cost);
        else _allElements[elem] = cost;
    }

    public T Dequeue()
    {
        //conseguimos el elemento guardado con menor costo
        T elem = default;
        foreach (var item in _allElements)
        {
            elem = elem == null ? item.Key : item.Value < _allElements[elem] ? item.Key : elem;
           
        }
        //lo quitamos de la colección
        _allElements.Remove(elem);

        return elem; //y lo devolvemos
    }

}
