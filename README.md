# Wit.NET

`Wit.NET` is the C# SDK for [Wit.ai](http://wit.ai).

## Install

[NuGet Package](https://www.nuget.org/packages/MatheusFroes.Wit.NET)

## Usage

See the `examples` folder for examples.

## API

### Versioning

The default API version is `20170107`.
You can target a specific version by setting the variable `WIT_API_VERSION`.

### Overview

`wit.net` provides a Wit class with the following methods:
* `Message` - the Wit [message API](https://wit.ai/docs/http/20160330#get-intent-via-text-link)
* `Converse` - the low-level Wit [converse API](https://wit.ai/docs/http/20160330#converse-link)
* `RunActions` - a higher-level method to the Wit converse API
* `Interactive` - starts an interactive conversation with your bot

### Wit class

The Wit constructor takes the following parameters:
* `accessToken` - the access token of your Wit instance
* `actions` - (optional if you only use `message()`) the dictionary with your actions

`actions` has action names as keys and action implementations as values.

A minimal example looks like this:

```c#
static void Main(string[] args)
{
    var actions = new WitActions();
    actions["send"] = Send;

    Wit client = new Wit(accessToken: accessToken, actions: actions);
}

private static WitContext Send(ConverseRequest request, ConverseResponse response)
{
    // Do something with the Context
    return request.Context;
}
```
### .Message()

The Wit [message API](https://wit.ai/docs/http/20160330#get-intent-via-text-link).

Takes the following parameters:
* `msg` - the text you want Wit.ai to extract the information from
* `verbose` - (optional) if set, calls the API with `verbose=true`

Example:
```c#
var response = client.Message("what is the weather in London?");
Console.WriteLine("Yay, got Wit.ai response: " + response)
```

### .RunActions()

A higher-level method to the Wit converse API.
`RunActions` resets the last turn on new messages and errors.

Takes the following parameters:
* `sessionId` - a unique identifier describing the user session
* `message` - the text received from the user
* `context` - the dict representing the session state
* `maxSteps` - (optional) the maximum number of actions to execute (defaults to 5)
* `verbose` - (optional) if set, calls the API with `verbose=true`

Example:
```c#
string sessionId = "my-user-session-42";
var context0 = new WitContext();
var context1 = client.RunActions(sessionId, "what is the weather in London?", context0);
Console.WriteLine("The session state is now: " + context1);
var context2 = client.RunActions(sessionId, "and in Brussels?", context1);
Console.WriteLine("The session state is now: ' + context2);
```
### .Converse()

The low-level Wit [converse API](https://wit.ai/docs/http/20160330#converse-link).

Takes the following parameters:
* `sessionId` - a unique identifier describing the user session
* `message` - the text received from the user
* `context` - the dict representing the session state
* `reset` - (optional) whether to reset the last turn
* `verbose` - (optional) if set, sets the API parameter `verbose` to `true`

Example:
```c#
var response = client.Converse("my-user-session-42", "what is the weather in London?", new WitContext());
Console.WriteLine("Yay, got Wit.ai response: " + resp)
```

### .Interactive()

Starts an interactive conversation with your bot.

Example:
```c#
client.Interactive()
```

See the [docs](https://wit.ai/docs) for more information.

### Contribute

If you would like to contribute to this project, just send a pull request, open an issue or email me
