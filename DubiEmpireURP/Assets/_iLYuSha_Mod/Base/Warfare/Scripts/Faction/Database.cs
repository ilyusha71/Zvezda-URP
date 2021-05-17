using UnityEngine;

namespace Warfare.Faction
{
    [CreateAssetMenu(fileName = "Database", menuName = "Warfare/Faction/Create Database")]
    public class Database : Database<int, Model> { }
}