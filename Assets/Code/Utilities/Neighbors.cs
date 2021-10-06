using System.Collections.Generic;
using System.Runtime.InteropServices;
using Code.Components;
using UnityEngine;

namespace Code.Utilities
{
    public class Neighbors
    {
        public TowerComponent neighborN;
        public TowerComponent neighborNE;
        public TowerComponent neighborE;
        public TowerComponent neighborSE;
        public TowerComponent neighborS;
        public TowerComponent neighborSW;
        public TowerComponent neighborW;
        public TowerComponent neighborNW;

        public List<TowerComponent> GetNeighborsInAllDirections()
        {
            var neighbors = new List<TowerComponent>();
            
            CheckNeighborIsNull(neighbors, neighborN);
            CheckNeighborIsNull(neighbors, neighborNE);
            CheckNeighborIsNull(neighbors, neighborE);
            CheckNeighborIsNull(neighbors, neighborSE);
            CheckNeighborIsNull(neighbors, neighborS);
            CheckNeighborIsNull(neighbors, neighborSW);
            CheckNeighborIsNull(neighbors, neighborW);
            CheckNeighborIsNull(neighbors, neighborNW);

            return neighbors;
        }

        private void CheckNeighborIsNull(List<TowerComponent> neighbors, TowerComponent direction)
        {
            if (direction != null)
                neighbors.Add(direction);
        }

        public override string ToString()
        {
            return "North:     " + CheckNull(neighborN) +
                   "\nNortheast: " + CheckNull(neighborNE) +
                   "\nEast:      " + CheckNull(neighborE) +
                   "\nSoutheast: " + CheckNull(neighborSE) +
                   "\nSouth:     " + CheckNull(neighborS) +
                   "\nSouthwest: " + CheckNull(neighborSW) +
                   "\nWest:      " + CheckNull(neighborW) +
                   "\nNorthwest: " + CheckNull(neighborNW);
        }

        private static string CheckNull(TowerComponent tower)
        {
            return tower == null ? "" : tower.name;
        }
    }
}