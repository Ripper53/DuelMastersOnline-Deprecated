using UnityEngine;
using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

[Serializable]
public class DeckHolder : ISerializable {
    public static string DeckPath => Path.Combine(Application.persistentDataPath, "Cards");

    public string[] Cards;

    public DeckHolder(string[] cards) {
        Cards = cards;
    }
    public DeckHolder(SerializationInfo info, StreamingContext context) {
        Cards = (string[])info.GetValue("Cards", typeof(string[]));
    }

    public void GetObjectData(SerializationInfo info, StreamingContext context) {
        info.AddValue("Cards", Cards, typeof(string[]));
    }

    public static void SaveDeck(DeckHolder deck, string name) {
        Directory.CreateDirectory(DeckPath);
        using (Stream stream = File.Open(Path.Combine(DeckPath, name + ".dat"), FileMode.OpenOrCreate)) {
            BinaryFormatter bf = new BinaryFormatter();
            bf.Serialize(stream, deck);
        }
    }

    public static DeckHolder LoadDeck(string name) {
        Directory.CreateDirectory(DeckPath);
        DeckHolder deck = null;
        using (Stream stream = File.Open(Path.Combine(DeckPath, name + ".dat"), FileMode.Open)) {
            BinaryFormatter bf = new BinaryFormatter();
            deck = (DeckHolder)bf.Deserialize(stream);
        }
        return deck;
    }

    public static void DeleteDeck(string name) {
        Directory.CreateDirectory(DeckPath);
        File.Delete(Path.Combine(DeckPath, name + ".dat"));
    }

}
