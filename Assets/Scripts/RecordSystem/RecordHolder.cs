using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;

namespace RecordSystem
{
    public static class RecordHolder
    {
        private static readonly string _savePath = Path.Combine(Application.persistentDataPath, "SaveData");
        private static Dictionary<GameType, RecordData> _records = new();

        static RecordHolder()
        {
            foreach (GameType gameType in Enum.GetValues(typeof(GameType)))
            {
                _records[gameType] = new RecordData(0, gameType);
            }

            LoadData();
        }

        public static void AddNewRecord(GameType gameType, int value)
        {
            if (_records.TryGetValue(gameType, out var record) && record.BestScore < value)
            {
                record.BestScore = value;
                SaveData();
            }
        }

        public static int GetRecordByType(GameType gameType)
        {
            return _records.TryGetValue(gameType, out var record) ? record.BestScore : default;
        }

        private static void SaveData()
        {
            try
            {
                var wrapper = new RecordDataWrapper(new List<RecordData>(_records.Values));
                var json = JsonConvert.SerializeObject(wrapper, Formatting.Indented);
                File.WriteAllText(_savePath, json);
            }
            catch (Exception e)
            {
                Debug.LogError($"Failed to save data: {e}");
                throw;
            }
        }

        private static void LoadData()
        {
            if (!File.Exists(_savePath))
                return;

            try
            {
                var json = File.ReadAllText(_savePath);
                var wrapper = JsonConvert.DeserializeObject<RecordDataWrapper>(json);

                foreach (var record in wrapper.RecordDatas)
                {
                    if (Enum.IsDefined(typeof(GameType), record.GameType) && _records.ContainsKey(record.GameType))
                    {
                        _records[record.GameType] = record;
                    }
                }
            }
            catch (Exception e)
            {
                Debug.LogError($"Failed to load data: {e}");
            }
        }

        [Serializable]
        private class RecordDataWrapper
        {
            public List<RecordData> RecordDatas;

            public RecordDataWrapper(List<RecordData> records)
            {
                RecordDatas = records;
            }
        }

        [Serializable]
        private class RecordData
        {
            public int BestScore;
            public GameType GameType;

            public RecordData(int bestScore, GameType gameType)
            {
                BestScore = bestScore;
                GameType = gameType;
            }
        }
    }

    public enum GameType
    {
        Balloon,
        ColorSwitch,
        Lighting,
        Path
    }
}