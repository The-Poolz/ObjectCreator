# DataFormatter

## What is it?
This class contains method for convert `HEX` to `JSON` object.

This class can convert this:

```0x000000000000000000000000000000000000000000000000000000000000442200000000000000000000000077018282fd033daf370337a5367e62d8811bc8850000000000000000000000000000000000000000000000000000000061586f9e00000000000000000000000000000000000000000000019b92c59bdc0df30be6000000000000000000000000a81236c2afe21c0165349b267d5754b6ddcd8300```

to:

```
{
    "PoolId" : 17442,
    "Token" : "0x77018282fd033daf370337a5367e62d8811bc885",
    "FinishTime" : 0.000000001633185694,
    "StartAmount" : 7592.187844964004334566,
    "Owner" : "0xa81236c2afe21c0165349b267d5754b6ddcd8300"
}
```

## Example
**First** we need create `Event` model:
```c#
public class NewPoolCreated
{
    public int PoolId { get; set; }
    public string Token { get; set; }
    public decimal FinishTime { get; set; }
    public decimal StartAmount { get; set; }
    public string Owner { get; set; }
}
```
It's important to know! Depending on the properties type, the conversion will be performed.
```
public string - address
public int - numeric
public decimal - money (or big number)
```
**The second** step is final. Call the function passing the necessary arguments.
```c#
string hex = "0x000000000000000000000000000000000000000000000000000000000000442200000000000000000000000077018282fd033daf370337a5367e62d8811bc8850000000000000000000000000000000000000000000000000000000061586f9e00000000000000000000000000000000000000000000019b92c59bdc0df30be6000000000000000000000000a81236c2afe21c0165349b267d5754b6ddcd8300"
NewPoolCreated pool = new();
string result = DataFormatter.FormatHexToJson<NewPoolCreated>(hex, pool);
```
