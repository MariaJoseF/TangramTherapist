using System;

namespace ThalamusTangram
{
    class Program
    {
        static void Main(string[] args)
        {

            string character = "";
            if (args.Length > 0)
            {
                if (args[0] == "help")
                {
                    Console.WriteLine("Usage: " + Environment.GetCommandLineArgs()[0] + " <CharacterName>");
                    return;
                }
                character = args[0];
            }
            ThalamusConnector thalamusCS = new ThalamusConnector("TangramGame",character);
            UnityConnector unityCS = new UnityConnector(thalamusCS);
            thalamusCS.UnityConnector = unityCS;

            Console.ReadLine();
            thalamusCS.Dispose();
        }
    }
}
