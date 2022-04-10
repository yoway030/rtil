# rtil
regular express api for in-house tool

## Use
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
