using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System.Linq;
using UnityEngine;

public class RandomizeInstruments : MonoBehaviour
{
    public List<InstrumentContainer> instruments = new List<InstrumentContainer>();

    private Dictionary<string, int> instrumentSimilarity = new Dictionary<string, int>
    {
        { "Xylophone", 1 },
        { "Piano", 1 },
        { "Guitar", 1 },
        { "Bass", 2 },
        { "Cello", 3 },
        { "Viola", 3 },
        { "Violin", 3 },
        { "Trumpet", 4 },
        { "Trombone", 4 },
        { "Flute", 5 },
        { "Sax", 5 },
        { "Clarinet", 5 },
        { "Drums", 6 }
    };

    private void Start()
    {
        SelectAndRandomizeCards(3, true, true);
    }

    public Tuple<List<string>, List<string>> SelectAndRandomizeCards(int numberOfCards, bool similar, bool sameMelody)
    {

        int numberOfInstruments = numberOfCards / 2;

        // Random seed
        System.Random rnd = new System.Random();

        // Single index melody for "same melody" condition
        int idxMelodies = 0;

        // List that contains melodies indexes
        List<int> melIdxs = new List<int>();

        // Compose the filenames for both sprites and audioclips
        List<string> audioClipArray = new List<string>();
        List<string> hapticClipArray = new List<string>();
        List<string> instrumentGameObjectArray= new List<string>();

        // Container for selected instrument
        List<string> selectedInstruments = new List<string>(numberOfInstruments);
        // Container for selected melodies
        List<string> selectedMelodies = new List<string>(numberOfInstruments);
        List<string> selectedHaptics = new List<string>(numberOfInstruments);
 
        string path = "";
        string tmpInstrument = "";
        List<string> tmpMelody = new List<string>();
        char[] wavChar = { '.', 'w', 'a', 'v' };

        // Load folder content
        string folderPath = "Assets/Resources/MemoryGameSounds/Sounds/Instruments";

        // Check if the folder exists
        if (Directory.Exists(folderPath))
        {
            // Get all files and directories in the folder
            string[] files = Directory.GetFiles(folderPath);

            // For each element in the folder
            foreach (string file in files)
            {
                // If it's a .wav file
                if (!file.Contains(".meta") & file.Contains(".wav"))
                {
                    // Split instrument name and melody name
                    string[] content = file.Split("\\");
                    path = content[0];
                    content = content[1].Split('_');

                    // If it's a new instrument
                    if (tmpInstrument != content[0])
                    {

                        // Clear tmpMelody if it's a new instrument
                        tmpMelody = new List<string>(); ;

                        // Save instrument
                        tmpInstrument = content[0];
                        // Save melody
                        tmpMelody.Add(content[1].TrimEnd(wavChar));

                        // Save the data in a new InstrumentContainer class
                        instruments.Add(new InstrumentContainer(path, tmpInstrument, tmpMelody, instrumentSimilarity[tmpInstrument]));
                    }
                    else // If it's not a new intrument
                    {
                        // Add an element to tmpMelody and save
                        tmpMelody.Add(content[1].TrimEnd(wavChar));
                        // Replace the already stored list of melodies with the new one
                        instruments[instruments.Count - 1].melodies = tmpMelody;
                    }
                }
                else  // Otherwise skip the file
                {
                    continue;
                }

            }

        }
        else
        {
            Debug.LogError("Folder does not exist: " + folderPath);
        }






        // Create a list of instruments for each similarity group
        var instrumentGroups = instrumentSimilarity.GroupBy(x => x.Value).Select(x => x.Select(y => y.Key).ToList()).ToList();


        // Select instruments
        if (similar) // Select similar instruments
        {
            // Select one instrument per group
            foreach (var group in instrumentGroups)
            {
                var instrument = group[rnd.Next(group.Count)];

                if (instrument != "Drums")
                {
                    selectedInstruments.Add(instrument);
                }
            }


            // Generate a random starting index
            int startIndex = rnd.Next(selectedInstruments.Count);

            // Extract the sequence of 4 instruments
            selectedInstruments = selectedInstruments
              .Skip(startIndex)
              .Take(numberOfInstruments)
              .Concat(selectedInstruments.Take(numberOfInstruments - selectedInstruments.Count + startIndex))
              .ToList();

        }
        else // Select different instruments
        {

            // Shuffle the similarity groups
            for (int i = instrumentGroups.Count - 1; i > 0; i--)
            {
                int j = rnd.Next(i + 1);
                var temp = instrumentGroups[i];
                instrumentGroups[i] = instrumentGroups[j];
                instrumentGroups[j] = temp;
            }


            int idx = 0;

            // Select numberOfCards instruments
            foreach (var group in instrumentGroups)
            {
                if (idx == numberOfInstruments)
                {
                    break;
                }

                idx++;

                var instrument = group[rnd.Next(group.Count)];
                selectedInstruments.Add(instrument);
            }

        }


        // Select melodies
        if (sameMelody) // Pick up a random melody for all instruments
        {
            idxMelodies = rnd.Next(instruments[0].melodies.Count);

            // For every instrument
            for (int i = 0; i < numberOfInstruments; i++)
            {
                // Find the corresponding index in the list of all instruments
                int idxInstrument = instruments.FindIndex(x => x.instrument == selectedInstruments[i]);
                // Save the index in a list
                selectedMelodies.Add(instruments[idxInstrument].melodies[idxMelodies]);
            }
        }
        else
        {
            // For all the cards
            while (melIdxs.Count < numberOfInstruments)
            {
                // Generate a random number between 0 and maximum number of melodies
                int randInt = rnd.Next(0, instruments[0].melodies.Count -1);

                // Add the number to the list if it's not already contained
                if (!melIdxs.Contains(randInt))
                {
                    melIdxs.Add(randInt);
                }

            }

            // For every instrument
            for (int i = 0; i < numberOfInstruments; i++)
            {
                // Find the corresponding index in the list of all instruments
                int idxInstrument = instruments.FindIndex(x => x.instrument == selectedInstruments[i]);

                // Save the index in a list
                selectedMelodies.Add(instruments[idxInstrument].melodies[melIdxs[i]]);            
            }

        }

        
        for (int entry = 0; entry < selectedMelodies.Count; entry++)
        {
            string soundPathFile = Path.Combine("Sounds", "Instruments", selectedInstruments[entry] + "_" + selectedMelodies[entry]);
            //string soundPathFile = "Sounds/Instruments/" + selectedInstruments[entry] + "_" + selectedMelodies[entry];
            audioClipArray.Add(soundPathFile);

            string imagePathFile = Path.Combine("Sprites", "Cards", "Card" + selectedInstruments[entry]);
            //string imagePathFile = "Images/Cards/" + "Card" + selectedInstruments[entry];
            instrumentGameObjectArray.Add(imagePathFile);

        }

        // Print all selected audioclips and sprites
        audioClipArray.ForEach(Debug.Log);
        instrumentGameObjectArray.ForEach(Debug.Log);

        return Tuple.Create(audioClipArray, instrumentGameObjectArray);
    }
}
