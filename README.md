# Blazor.History
JS History API Wrapper for Blazor WASM

Inspired by [https://github.com/arivera12/BlazorHistory](https://github.com/arivera12/BlazorHistory) but with a few differences:
1) Uses System.Text.Json instead of requiring a dependancy on Newtonsoft.Json
2) Will not throw an error if State<T> is null.
3) Made title/url parameters optional in methods where it's unnecessary (for example, setting the title is not supported in most browsers).

### Usage
1) Install [nuget package](https://www.nuget.org/packages/BlazorBrowserHistory)

2) In Program.cs add the service to the DI builder.
```C#
builder.Services.AddHistoryService();
```

3) Inject the service into pages:
```C#
@inject HistoryService history
```

4) Use injected service:
```C#
var state = await history.State<category_vm>();
history.ReplaceState(coll.selected_item);
