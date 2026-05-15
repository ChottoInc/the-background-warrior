using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Player/Job/Database", fileName = "DatabaseJobData")]
public class JobDatabaseSO : ScriptableObject
{
    public List<AbstractPlayerJobData> jobs;

    private Dictionary<int, AbstractPlayerJobData> lookup;

    public void Initialize()
    {
        lookup = new Dictionary<int, AbstractPlayerJobData>();

        foreach (var job in jobs)
            lookup[(int)job.Id] = job;

        //Debug.Log("databse lentght: " + lookup.Count);
    }

    public T Get<T>(int id) where T : AbstractPlayerJobData
    {
        return (T)lookup[id];
    }
}
