# wit.net

`wit.net` is the C# SDK for [Wit.ai](http://wit.ai).

## Install


## Usage

See the `examples` folder for examples.

## API

### Versioning

The default API version is `20170107`.
You can target a specific version by setting the variable `WIT_API_VERSION`.

### Overview

`wit.net` provides a Wit class with the following methods:
* `message` - the Wit [message API](https://wit.ai/docs/http/20160330#get-intent-via-text-link)
* `converse` - the low-level Wit [converse API](https://wit.ai/docs/http/20160330#converse-link)
* `run_actions` - a higher-level method to the Wit converse API

### Wit class

The Wit constructor takes the following parameters:
* `accessToken` - the access token of your Wit instance
* `actions` - (optional if you only use `message()`) the dictionary with your actions

`actions` has action names as keys and action implementations as values.

```
Wit client = new Wit(accessToken=accessToken, actions=actions)
```
