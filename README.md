<img src="https://github.com/jsonh-org/Jsonh/blob/main/IconUpscaled.png?raw=true" width=180>

[![C#](https://img.shields.io/github/release/jsonh-org/JsonhCli.svg?label=c%23)](https://github.com/jsonh-org/JsonhCli/releases)

**JSON for Humans.**

JSON is great. Until you miss that trailing comma... or want to use comments. What about multiline strings?
JSONH provides a much more elegant way to write JSON that's designed for humans rather than machines.

Since JSONH is compatible with JSON, any JSONH syntax can be represented with equivalent JSON.

## JsonhCli

JsonhCli is a command-line interface for converting [JSONH v1](https://github.com/jsonh-org/Jsonh)
to JSON using [JsonhCs](https://github.com/jsonh-org/Jsonhcs).

## Usage

```
Description:
  The JSONH Command Line Interface

Usage:
  JsonhCli [options]

Options:
  -?, -h, --help              Show help and usage information
  --version                   Show version information
  --input-path                The path of the JSONH file to input
  --input                     The JSONH string to input
  --output-path               The path of the JSON file to output. If null, logs the output
  --pretty                    Whether to indent the outputted JSON
  --lang-version <Latest|V1>  The major version of the JSONH specification to use
```

### Example

Parse JSONH in `example.jsonh` and write indented JSON to `example.json`:
```
JsonhCli --input-path "example.jsonh" --output-path "example.json" --pretty
```

Parse JSONH string and output indented JSON:
```
JsonhCli --input "[hello, world]" --pretty
```