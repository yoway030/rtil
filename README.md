# rtil
regular express api for in-house tool

## Use
### api /regex_match
Request
```json
{
    "text" : "This is one sentence. This is a second sentence.",
    "pattern" : "This is (?<XXX>\\w+|) (?<YYY>\\w+|)."
}
```

Response
```json
[
    {
        "value": "This is one sentence.",
        "index": 0,
        "groups": [
            {
                "index": 0,
                "length": 21,
                "value": "This is one sentence.",
                "capures": [
                    {
                        "index": 0,
                        "length": 21,
                        "value": "This is one sentence."
                    }
                ]
            },
            {
                "index": 8,
                "length": 3,
                "value": "one",
                "capures": [
                    {
                        "index": 8,
                        "length": 3,
                        "value": "one"
                    }
                ]
            },
            {
                "index": 12,
                "length": 8,
                "value": "sentence",
                "capures": [
                    {
                        "index": 12,
                        "length": 8,
                        "value": "sentence"
                    }
                ]
            }
        ],
        "names": {
            "0": {
                "index": 0,
                "length": 21,
                "value": "This is one sentence.",
                "capures": [
                    {
                        "index": 0,
                        "length": 21,
                        "value": "This is one sentence."
                    }
                ]
            },
            "XXX": {
                "index": 8,
                "length": 3,
                "value": "one",
                "capures": [
                    {
                        "index": 8,
                        "length": 3,
                        "value": "one"
                    }
                ]
            },
            "YYY": {
                "index": 12,
                "length": 8,
                "value": "sentence",
                "capures": [
                    {
                        "index": 12,
                        "length": 8,
                        "value": "sentence"
                    }
                ]
            }
        }
    },
    {
        "value": "This is a second ",
        "index": 22,
        "groups": [
            {
                "index": 22,
                "length": 17,
                "value": "This is a second ",
                "capures": [
                    {
                        "index": 22,
                        "length": 17,
                        "value": "This is a second "
                    }
                ]
            },
            {
                "index": 30,
                "length": 1,
                "value": "a",
                "capures": [
                    {
                        "index": 30,
                        "length": 1,
                        "value": "a"
                    }
                ]
            },
            {
                "index": 32,
                "length": 6,
                "value": "second",
                "capures": [
                    {
                        "index": 32,
                        "length": 6,
                        "value": "second"
                    }
                ]
            }
        ],
        "names": {
            "0": {
                "index": 22,
                "length": 17,
                "value": "This is a second ",
                "capures": [
                    {
                        "index": 22,
                        "length": 17,
                        "value": "This is a second "
                    }
                ]
            },
            "XXX": {
                "index": 30,
                "length": 1,
                "value": "a",
                "capures": [
                    {
                        "index": 30,
                        "length": 1,
                        "value": "a"
                    }
                ]
            },
            "YYY": {
                "index": 32,
                "length": 6,
                "value": "second",
                "capures": [
                    {
                        "index": 32,
                        "length": 6,
                        "value": "second"
                    }
                ]
            }
        }
    }
]
```
### api /regex_capture
Request
```json
{
    "text" : "This is one sentence. This is a second sentence.",
    "pattern" : "This is (?<XXX>\\w+|) (?<YYY>\\w+|)."
}
```

Response
```json
[
    {
        "names": {
            "0": {
                "index": 0,
                "length": 21,
                "value": "This is one sentence.",
                "capures": null
            },
            "XXX": {
                "index": 8,
                "length": 3,
                "value": "one",
                "capures": null
            },
            "YYY": {
                "index": 12,
                "length": 8,
                "value": "sentence",
                "capures": null
            }
        }
    },
    {
        "names": {
            "0": {
                "index": 22,
                "length": 17,
                "value": "This is a second ",
                "capures": null
            },
            "XXX": {
                "index": 30,
                "length": 1,
                "value": "a",
                "capures": null
            },
            "YYY": {
                "index": 32,
                "length": 6,
                "value": "second",
                "capures": null
            }
        }
    }
]
```
### api /regex_replace
Request
```json
{
    "text" : "This is one sentence. This is a second sentence.",
    "pattern" : "This is",
    "to" : "That is"
}
```

Response
```json
{
    "value": "That is one sentence. That is a second sentence."
}
```
