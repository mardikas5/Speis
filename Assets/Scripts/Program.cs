using System;
using System.Collections.Generic;
using System.Linq;
   
class Program
{
#if Console
    static void ClearWrite(string s)
    {
        ClearCurrentConsoleLine();
        Console.WriteLine(s);
    }
    
 
    public static void ClearCurrentConsoleLine()
    {
        int currentLineCursor = Console.CursorTop;
        Console.SetCursorPosition(0, Console.CursorTop);
        Console.Write(new string(' ', Console.WindowWidth)); 
        Console.SetCursorPosition(0, currentLineCursor);
    }

    static bool Exit(ConsoleKeyInfo x)
    {
        if(x.Key == ConsoleKey.Escape)
        {
            return true;
        }
        return false;
    }


    static void ConsoleStart()
    {
        bool VisualsEnabled = false;

        Console.WriteLine("Starting");
        
        Console.WriteLine("");
        
        Position Start = new Position(Console.CursorLeft, Console.CursorTop);

        if (VisualsEnabled)
        {
            Visuals.Draw(World.Size, Start);
            Visuals.DrawEntities(World.Size, p.pos - World.CenterView, entityDB.entities);
        }

        while(true)
        {
            if (Console.ReadKey() != null)
            {
                if (Exit(Console.ReadKey()))
                {
                    break;
                }
                
                p.pos = p.pos + Input.DirectionToOffset(Input.KeyToDirection(Console.ReadKey()));
            }

            sim.Tick(entityDB.entities);
            Console.WriteLine(t.PartsOfType<Storage>().FirstOrDefault().Stored.Count);
            Console.WriteLine(t.PartsOfType<Storage>().FirstOrDefault().Stored[0].Amount);
            //Input.CursorToDirection(Input.KeyToDirection(Console.ReadKey()));
            
            if (VisualsEnabled)
            {
                Visuals.Draw(World.Size, Start);
                Visuals.DrawEntities(World.Size,p.pos - World.CenterView, entityDB.entities);
            }
        }
        
        ConsoleExtentions.WriteBottomLine("Exiting");
    }
#endif

    public static void StandardStart()
    {
        Simulation sim = new Simulation();
        sim.Initialize();

        EntityDatabase entityDB = new EntityDatabase();
        ResourceDatabase resDB = new ResourceDatabase();
        StructureDatabase structDB = new StructureDatabase();
    }

    public static void EnterTestValues()
    {
        ResourceDatabase.Instance.Populate();
        StructureDatabase.Instance.Populate();

        Station t = new Station(new StructureTemplate());
        StorageTemplate storageTemplate = new StorageTemplate();
        t.Register(new Storage(storageTemplate));
        StructureTemplate ore = StructureDatabase.Instance.Structures.Where(name => name.Name == "Ore Processing").FirstOrDefault();
        t.Register(new ResourceProducer(ore as ResourceProducerTemplate));
    }

    static void Main(string[] args)
    {

    }
}

