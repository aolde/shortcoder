# Shortcoder

> Add support for Wordpress Shortcodes in a .NET application.

## Project Status

- Parsing works in most cases, but more tests need to be added for edge cases.

# Quick Sample Usage

1. Create a Shortcode class.

```csharp
public class NowShortcode : Shortcode
{
    public override string Generate(IShortcodeContext context)
    {
        return DateTime.Now.ToString();
    }
}
```

2. Register the shortcode.

```csharp
ShortcodeFactory.Provider.Add<NowShortcode>(tag: "now");
```

3. Parse content using registered shortcodes


```csharp
var content = "The date and time is: [now].";
var parsedContent = ShortcodeFactory.Parser.Parse(body);
```

Result: 

```
parsedContent == "The Date and Time is: 2015-01-01 10:10:00"
```

# License

See [LICENSE](LICENSE.txt)