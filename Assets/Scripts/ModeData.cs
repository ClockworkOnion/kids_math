using System;
using System.Collections.Generic;
using Unity;

public static class ModeData
{
    // A list to sort the stages by difficulty
    public static List<Difficulties> stageProgression = new List<Difficulties> {
        Difficulties.addLv1,
        Difficulties.subLv1,
        Difficulties.addSubLv1,

        Difficulties.addLv2,
        Difficulties.subLv2,
        Difficulties.addSubLv2,

        Difficulties.addLv3,
        Difficulties.subLv3,
        Difficulties.addSubLv3,

        Difficulties.addLv4,
        Difficulties.subLv4,
        Difficulties.addSubLv4,

        Difficulties.multLv1,
        Difficulties.divLv1,
        Difficulties.multDivLv1,

        Difficulties.multLv2,
        Difficulties.divLv2,
        Difficulties.multDivLv2,

        Difficulties.multLv3,
        Difficulties.divLv3,
        Difficulties.multDivLv3,

        Difficulties.multLv4,
        Difficulties.divLv4,
        Difficulties.multDivLv4,

        Difficulties.addSubMultDivLv1,
        Difficulties.addSubMultDivLv2,
        Difficulties.addSubMultDivLv3,
        Difficulties.addSubMultDivLv4,
    };

    public static Dictionary<Difficulties, int> scores = new Dictionary<Difficulties, int> {
            {Difficulties.addLv1, 5 },
            {Difficulties.subLv1, 5 },
            {Difficulties.addSubLv1, 5 },

            {Difficulties.addLv2, 10 },
            {Difficulties.subLv2, 10 },
            {Difficulties.addSubLv2, 10},

            {Difficulties.addLv3, 20},
            {Difficulties.subLv3, 20},
            {Difficulties.addSubLv3, 20},

            {Difficulties.addLv4, 40},
            {Difficulties.subLv4, 40},
            {Difficulties.addSubLv4, 40},

            {Difficulties.multLv1, 20},
            {Difficulties.divLv1, 20},
            {Difficulties.multDivLv1, 20},

            {Difficulties.multLv2, 40},
            {Difficulties.divLv2, 40},
            {Difficulties.multDivLv2, 40},

            {Difficulties.multLv3, 80},
            {Difficulties.divLv3, 80},
            {Difficulties.multDivLv3, 80},

            {Difficulties.multLv4, 120},
            {Difficulties.divLv4, 120},
            {Difficulties.multDivLv4, 120},

            {Difficulties.addSubMultDivLv1, 30},
            {Difficulties.addSubMultDivLv2, 60},
            {Difficulties.addSubMultDivLv3, 120},
            {Difficulties.addSubMultDivLv4, 180},
        };


    public enum Difficulties
    {
        addLv1, addLv2, addLv3, addLv4,
        subLv1, subLv2, subLv3, subLv4,
        addSubLv1, addSubLv2, addSubLv3, addSubLv4,
        multLv1, multLv2, multLv3, multLv4,
        divLv1, divLv2, divLv3, divLv4,
        multDivLv1, multDivLv2, multDivLv3, multDivLv4,
        addSubMultDivLv1, addSubMultDivLv2, addSubMultDivLv3, addSubMultDivLv4,
    }

    public static Dictionary<Difficulties, DifficultyInfo> difficultyInfo = new Dictionary<Difficulties, DifficultyInfo> {
        {Difficulties.addLv1, new DifficultyInfo(5, "Addition Level 1") },
        {Difficulties.addLv2, new DifficultyInfo(10, "Addition Level 2") }, 
        {Difficulties.addLv3, new DifficultyInfo(20, "Addition Level 3") },
        {Difficulties.addLv4, new DifficultyInfo(40, "Addition Level 4") },

        {Difficulties.subLv1, new DifficultyInfo(5, "Subtraction Level 1") },
        {Difficulties.subLv2, new DifficultyInfo(10, "Subtraction Level 2") }, 
        {Difficulties.subLv3, new DifficultyInfo(20, "Subtraction Level 3") },
        {Difficulties.subLv4, new DifficultyInfo(40, "Subtraction Level 4") },

        {Difficulties.addSubLv1, new DifficultyInfo(5, "Add & Subtract Level 1") },
        {Difficulties.addSubLv2, new DifficultyInfo(10, "Add & Subtract Level 2") }, 
        {Difficulties.addSubLv3, new DifficultyInfo(20, "Add & Subtract Level 3") },
        {Difficulties.addSubLv4, new DifficultyInfo(40, "Add & Subtract Level 4") },

        {Difficulties.multLv1, new DifficultyInfo(20, "Multiplication Level 1") },
        {Difficulties.multLv2, new DifficultyInfo(40, "Multiplication Level 2") }, 
        {Difficulties.multLv3, new DifficultyInfo(80, "Multiplication Level 3") },
        {Difficulties.multLv4, new DifficultyInfo(120, "Multiplication Level 4") },

        {Difficulties.divLv1, new DifficultyInfo(20, "Division Level 1") },
        {Difficulties.divLv2, new DifficultyInfo(40, "Division Level 2") }, 
        {Difficulties.divLv3, new DifficultyInfo(80, "Division Level 3") },
        {Difficulties.divLv4, new DifficultyInfo(120, "Division Level 4") },

        {Difficulties.multDivLv1, new DifficultyInfo(20, "Multiplication & Division Level 1") },
        {Difficulties.multDivLv2, new DifficultyInfo(40, "Multiplication & Division Level 2") }, 
        {Difficulties.multDivLv3, new DifficultyInfo(80, "Multiplication & Division Level 3") },
        {Difficulties.multDivLv4, new DifficultyInfo(120, "Multiplication & Division Level 4") },

        {Difficulties.addSubMultDivLv1, new DifficultyInfo(30, "Add, Subtract, Multiply & Divide Level 1") },
        {Difficulties.addSubMultDivLv2, new DifficultyInfo(60, "Add & Subtract, Multiply & Divide Level 2") }, 
        {Difficulties.addSubMultDivLv3, new DifficultyInfo(120, "Add & Subtract, Multiply & Divide Level 3") },
        {Difficulties.addSubMultDivLv4, new DifficultyInfo(180, "Add & Subtract, Multiply & Divide Level 4") },
    };

    public struct DifficultyInfo
    {
        int basePoints;
        string longName;

        public DifficultyInfo(int _basePoints, string _longName)
        {
            basePoints = _basePoints;
            longName = _longName;
        }
    }
}

