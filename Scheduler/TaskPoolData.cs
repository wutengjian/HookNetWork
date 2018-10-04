using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Scheduler
{
    public class TaskPoolData
    {
        private static ConcurrentDictionary<string, ConcurrentQueue<TaskPoolInfo>> MapQueue;
        private static Dictionary<string, int> MapCount;
        private static int maxMap = 1000;//默认队列数量1千
        private static int maxQueue = 50000;//每个队列默认长度5万
        public TaskPoolData()
        {
            maxMap = 1000;
            maxQueue = 50000;
            MapQueue = new ConcurrentDictionary<string, ConcurrentQueue<TaskPoolInfo>>();
            MapCount = new Dictionary<string, int>();
            _AutomaticDetection();
        }

        public TaskPoolData(int _maxMap, int _maxQueue)
        {
            maxMap = _maxMap;
            maxQueue = _maxQueue;
            MapQueue = new ConcurrentDictionary<string, ConcurrentQueue<TaskPoolInfo>>();
            MapCount = new Dictionary<string, int>();
            _AutomaticDetection();
        }

        private void _AutomaticDetection()
        {
            Task.Run(() =>
                {
                    while (true)
                    {
                        var list = MapQueue.Keys.ToList();
                        foreach (string key in list)
                        {
                            if (MapQueue.ContainsKey(key))
                            {
                                if (MapCount.ContainsKey(key) == false)
                                {
                                    MapCount.Add(key, MapQueue[key].Count());
                                }
                                else
                                {
                                    MapCount[key] = MapQueue[key].Count;
                                }
                            }
                            else if (MapCount.ContainsKey(key))
                            {
                                MapCount.Remove(key);
                            }
                        }                     
                        Thread.Sleep(1000 * 10);//一分钟
                    }
                });
        }

        public static void Enqueue(string map, TaskPoolInfo info)
        {
            if (MapQueue.ContainsKey(map) == false)
            {
                if (MapCount.Count > maxMap)
                {
                    return;
                }
                MapQueue.TryAdd(map, new ConcurrentQueue<TaskPoolInfo>());
                MapCount.Add(map, 1);
            }
            if (MapCount.ContainsKey(map) && MapCount[map] > maxQueue)
            {
                return;
            }
            MapQueue[map].Enqueue(info);
        }
        public static TaskPoolInfo TryDequeue(string map)
        {
            if (MapQueue.ContainsKey(map) == false)
            {
                MapQueue.TryAdd(map, new ConcurrentQueue<TaskPoolInfo>());
                return null;
            }
            TaskPoolInfo info = new TaskPoolInfo();
            MapQueue[map].TryDequeue(out info);
            return info;
        }
    }
}
