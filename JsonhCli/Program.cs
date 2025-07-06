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
        Option<string?> InputPathOption = new("--input-path") {
            Description = "The path of the JSONH file to input",
        };
        Option<string?> InputOption = new("--input") {
            Description = "The JSONH string to input",
        };
        Option<string?> OutputPathOption = new("--output-path") {
            Description = "The path of the JSON file to output. If null, logs the output",
        };
        Option<bool> PrettyOption = new("--pretty") {
            Description = "Whether to indent the outputted JSON",
        };

        RootCommand RootCommand = new("The JSONH Command Line Interface") {
            InputPathOption,
            InputOption,
            OutputPathOption,
            PrettyOption,
        };

        RootCommand.SetAction(int (ParseResult ParseResult) => {
            string? InputPath = ParseResult.GetValue(InputPathOption);
            string? Input = ParseResult.GetValue(InputOption);
            string? OutputPath = ParseResult.GetValue(OutputPathOption);
            bool Pretty = ParseResult.GetValue(PrettyOption);

            // JSONH file to JSON file
            if (InputPath is not null || Input is not null) {
                // Ensure unambiguous input
                if (InputPath is not null && Input is not null) {
                    Console.WriteLine("Cannot pass both an input file and an input string.");
                    return 1;
                }

                // Read input path
                if (InputPath is not null) {
                    try {
                        Input = File.ReadAllText(InputPath);
                    }
                    catch (Exception) {
                        Console.WriteLine("The input file is invalid.");
                        return 1;
                    }
                }

                // Parse JSONH
                if (JsonhReader.ParseNode(Input!).TryGetError(out Error Error, out JsonNode? Node)) {
                    Console.WriteLine($"Error parsing JSONH file: \"{Error.Message}\"");
                    return 1;
                }

                // Convert to JSON
                string Json = Node.ToJsonString(Pretty ? JsonhReader.PrettyJson : JsonhReader.MiniJson);

                // Write output
                if (OutputPath is not null) {
                    // Validate output path
                    if (OutputPath is null || !Directory.Exists(Path.GetDirectoryName(OutputPath))) {
                        Console.WriteLine($"The directory of the output file does not exist: \"{OutputPath}\"");
                        return 1;
                    }
                    File.WriteAllText(OutputPath, Json);
                }
                // Log output
                else {
                    Console.WriteLine(Json.ReplaceLineEndings()); // Console doesn't display '\n' properly on Windows
                }
                return 0;
            }
            // Invalid command arguments
            else {
                Console.WriteLine("Invalid arguments.");
                return 1;
            }
        });

        return RootCommand.Parse(Args).Invoke();
    }
}