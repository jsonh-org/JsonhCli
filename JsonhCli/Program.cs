using System.Text.Json.Nodes;
using System.CommandLine;
using JsonhCs;
using ResultZero;

namespace JsonhCli;

/// <summary>
/// The JSONH Command Line Interface.
/// </summary>
public static class Program {
    public static int Main(string[] Args) {
        Option<string?> InputPathOption = new(
            name: "--input-path",
            description: "The path of the JSONH file to input"
        );
        Option<string?> InputOption = new(
            name: "--input",
                description: "The JSONH string to input"
        );
        Option<string?> OutputPathOption = new(
            name: "--output-path",
                description: "The path of the JSON file to output. If null, logs the output"
        );
        Option<bool> PrettyOption = new(
            name: "--pretty",
            description: "Whether to indent the outputted JSON"
        );

        RootCommand RootCommand = new("The JSONH Command Line Interface") {
            InputPathOption,
            InputOption,
            OutputPathOption,
            PrettyOption,
        };

        RootCommand.SetHandler(
            (string? InputPath, string? Input, string? OutputPath, bool Pretty) => {
                // JSONH file to JSON file
                if (InputPath is not null || Input is not null) {
                    // Ensure unambiguous input
                    if (InputPath is not null && Input is not null) {
                        Console.WriteLine("Cannot pass both an input file and an input string.");
                        return;
                    }
                    // Read input path
                    if (InputPath is not null) {
                        try {
                            Input = File.ReadAllText(InputPath);
                        }
                        catch (Exception) {
                            Console.WriteLine("The input file is invalid.");
                            return;
                        }
                    }
                    // Parse JSONH
                    if (JsonhReader.ParseNode(Input!).TryGetError(out Error Error, out JsonNode? Node)) {
                        Console.WriteLine($"Error parsing JSONH file: \"{Error.Message}\"");
                        return;
                    }
                    // Convert to JSON
                    string Json = Node.ToJsonString(Pretty ? JsonhReader.PrettyJson : JsonhReader.MiniJson);
                    // Write output
                    if (OutputPath is not null) {
                        // Validate output path
                        if (OutputPath is null || !Directory.Exists(Path.GetDirectoryName(OutputPath))) {
                            Console.WriteLine($"The directory of the output file does not exist: \"{OutputPath}\"");
                            return;
                        }
                        File.WriteAllText(OutputPath, Json);
                    }
                    // Log output
                    else {
                        Console.WriteLine(Json.ReplaceLineEndings());
                    }
                }
                else {
                    Console.WriteLine("Invalid arguments.");
                }
            },
            InputPathOption,
            InputOption,
            OutputPathOption,
            PrettyOption
        );

        return RootCommand.Invoke(Args);
    }
}