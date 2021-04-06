using System.Collections.Generic;
using System;

[Serializable]
public class ItemsData 
{
    public List<string> purchasedIDs;

    public ItemsData(List<string> ids)
    {
        if(ids != null)
            purchasedIDs = new List<string>(ids);
        else
            purchasedIDs = new List<string>();
    }

    public bool GetID(string id)
    {
        if(purchasedIDs == null) return false;

        foreach (var item in purchasedIDs)
        {
            if(id == item)
                return true;
        }
        return false;
    }
}
