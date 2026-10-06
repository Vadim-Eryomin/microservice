namespace StarshipGame;
using Hwdtech;
using System.Collections.Generic;
using System.IO;
using CollisionTree = Dictionary<int, Dictionary<int, Dictionary<int, HashSet<int>>>>;

public class RegisterIoCDependencyGameCollisionTreeCommand : Hwdtech.ICommand
{
    public void Execute()
    {
        var collisionTree = LoadCollisionTree(IoC.Resolve<string>("Collision.FilePath"));

        IoC.Resolve<Hwdtech.ICommand>(
            "IoC.Register",
            "Game.Collision.Tree",
            (object[] args) => collisionTree
        ).Execute();
    }

    private CollisionTree LoadCollisionTree(string filePath)
    {
        var tree = new CollisionTree();

        using (var reader = new StreamReader(filePath))
        {
            while (!reader.EndOfStream)
            {
                var line = reader.ReadLine().Split(',');
                if (line.Length < 4) continue;

                var dx = int.Parse(line[0]);
                var dy = int.Parse(line[1]);
                var dvx = int.Parse(line[2]);
                var dvy = int.Parse(line[3]);

                if (!tree.TryGetValue(dx, out var dyDict))
                {
                    dyDict = new Dictionary<int, Dictionary<int, HashSet<int>>>();
                    tree[dx] = dyDict;
                }

                if (!dyDict.TryGetValue(dy, out var dvxDict))
                {
                    dvxDict = new Dictionary<int, HashSet<int>>();
                    dyDict[dy] = dvxDict;
                }

                if (!dvxDict.TryGetValue(dvx, out var dvySet))
                {
                    dvySet = new HashSet<int>();
                    dvxDict[dvx] = dvySet;
                }

                dvySet.Add(dvy);
            }
        }

        return tree;
    }
}
