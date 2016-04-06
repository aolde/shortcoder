Ideas:

- A built in shortcode that outputs all shortcodes that are available and their defined attributes.

- Api to read syntax tree from parsed text. To enable shortcodes to read child content shortcodes.

- Add option to GetShortcodes to return all shortcodes even if they are not defined as classes. Return a "ShortcodeInfo"

- Allow to change the START and END tag to some other character. e.g. "{" and "}".

- If exceptions are thrown in parsing or other, return original content, and an inline HTML comment with exeption message. (clear enough?)

- Pack of popular shortcodes

- Optional attribute on Shortcode:

	[Shortcode(DefaultTag = "name", AutoClose = true)]
	public class NowShortcode : Shortcode {}

	DefaultTag: When registering tags it will use this name as a default.
	AutoClose: If the shortcode doesnt use Content property it can be autoclosed to avoid parsing errors.


- Optional attribute on member:

	[Shortcode(DefaultTag = "name", AutoClose = true)]
	public class NowShortcode : Shortcode {

		[ShortcodeMember(Alias = "your-name", DefaultValue = "Joe")]
		public string Name { get;set; }

	}

	Alias: change the default name to something else. Both names will work but Alias is used first.
	DefaultValue: value used when no value from content has been supplied.

## Shortcodes

- Condition (check some condition to view return Content, e.g. Querystring)