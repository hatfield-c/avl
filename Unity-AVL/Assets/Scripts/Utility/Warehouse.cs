using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Warehouse : MonoBehaviour
{
    
    protected Dictionary<string, List<IStorable>> Shelves = new Dictionary<string, List<IStorable>>();

    protected List<IStorable> listBuffer = new List<IStorable>();
    protected IStorable itemBuffer;

    public void InitFromList(List<IStorable> inventory = null){
        if(inventory == null){
            return;
        }

        foreach(IStorable item in inventory){
            this.AddShelf(item);
        }

        this.StockFromList(inventory);
    }

    public void StockFromList(List<IStorable> inventory = null) {
        if (inventory == null) {
            return;
        }

        foreach (IStorable item in inventory) {
            this.StockItem(item);
        }
    }

    public void StockItem(IStorable item){
        if(!this.HasShelf(item.GetArchetype())){
            return;
        }

        item.GetMyGameObject().transform.parent = this.transform;
        item.GetMyGameObject().transform.position = this.transform.position;
        item.GetMyGameObject().transform.rotation = this.transform.rotation;
        this.Shelves[item.GetArchetype()].Add(item);
    }

    public IStorable FetchItem(string archetype){
        if(!this.HasShelf(archetype)){
            return null;
        }

        this.listBuffer = this.Shelves[archetype];

        if(this.listBuffer.Count < 1){
            return null;
        }

        this.itemBuffer = this.listBuffer[0];
        this.listBuffer.RemoveAt(0);
        
        this.itemBuffer.GetMyGameObject().transform.parent = null;

        return this.itemBuffer;
    }

    public bool HasItem(string archetype){
        if(!this.HasShelf(archetype)){
            return false;
        }

        if(this.Shelves[archetype].Count < 1){
            return false;
        }

        return true;
    }

    public int GetItemCount(string archetype){
        if(!this.HasShelf(archetype)){
            return 0;
        }

        return this.Shelves[archetype].Count;
    }

    public void AddShelf(IStorable item){
        if(this.HasShelf(item.GetArchetype())){
            return;
        }

        this.Shelves.Add(item.GetArchetype(), new List<IStorable>());
    }

    public bool HasShelf(string archetype){
        if(!this.Shelves.ContainsKey(archetype)){
            return false;
        }

        return true;
    }

    public int GetTotalCount(){
        int count = 0;
        foreach(KeyValuePair<string, List<IStorable>> pair in this.Shelves){
            count += pair.Value.Count;
        }

        return count;
    }

}
