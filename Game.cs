using System;
using System.Collections.Generic;

enum Card {
    N0 = 0,
    N1 = 1,
    N2 = 2,
    N3 = 3,
    N4 = 4,
    N5 = 5,
    N6 = 6,
    N7 = 7,
    N8 = 8,
    N9 = 9,
    SA = 10,
    SB = 11,
    SC = 12
}

class Game {
    public List<List<int>> rows = new List<List<int>>();
    public int? grabbed;
    public Random random = new Random();

    public Game() {
        init();
    }
    public void init() {
        rows = new();
        for (int i = 0; i < 6; i++) {
            rows.Add(new List<int>());
        }
        Randomize();        
    }

    public void Randomize() {
        for (int i = 0; i < rows.Count; i++) {
            rows[i].Add(random.Next(0, 13));
            int additionalCards = random.Next(4, 9);

            for (int j = 0; j < additionalCards; j++) {
                int lastCard = rows[i][rows[i].Count - 1];
                List<int> validCards = GetValidCards(lastCard);

                if (validCards.Count > 0) {
                    rows[i].Add(validCards[random.Next(validCards.Count)]);
                }
            }
        }
    }

    public List<int> GetValidCards(int under) {
        List<int> validCards = new List<int>();

        for (int i = 0; i < 13; i++) {
            if (Rules(under, i)) {
                validCards.Add(i);
            }
        }

        return validCards;
    }

    public bool Rules(int under, int upper) {
        if (under == upper) return false;
        if (upper == under - 1 || upper == under + 1) return true;
        if (upper % 4 == 0 && under % 4 == 0) return true;
        return false;
    }

    public bool Grab(int row) {
        if (grabbed.HasValue) return false;
        if (rows[row].Count == 0) return false;

        grabbed = rows[row][rows[row].Count - 1];
        rows[row].RemoveAt(rows[row].Count - 1);
        return true;
    }

    public bool Put(int row) {
        if (!grabbed.HasValue) return false;
        if (rows[row].Count == 0 || !Rules(rows[row][rows[row].Count - 1], grabbed.Value)) {
            return false;
        }

        rows[row].Add(grabbed.Value);
        grabbed = null;
        return true;
    }
}
